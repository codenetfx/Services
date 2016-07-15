using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Search;
using UL.Iam.Common.OAuth;

using AriaSort = UL.Enterprise.Foundation.Data.ISort;
using System.Text.RegularExpressions;
using System.Globalization;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Queries SharePoint via the HTTP REST API and parses the results returns.
    /// </summary>
    public sealed class SharePointQuery : ISharePointQuery
    {
        internal const string HttpPostRequestJsonAccept = "application/json;odata=verbose;charset=utf-8";
        private readonly ISharepointConfigurationSource _configurationSource;
        private readonly ILogManager _logManager;
        private readonly IPrincipalResolver _principalResolver;
        private readonly ISharePointRestApiAccessTokenGenerator _restApiAccessTokenGenerator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SharePointQuery" /> class.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        /// <param name="logManager">The log manager.</param>
        /// <param name="restApiAccessTokenGenerator">The rest API access token generator.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public SharePointQuery(ISharepointConfigurationSource configurationSource, ILogManager logManager,
            ISharePointRestApiAccessTokenGenerator restApiAccessTokenGenerator,
            IPrincipalResolver principalResolver)
        {
            _configurationSource = configurationSource;
            _logManager = logManager;
            _restApiAccessTokenGenerator = restApiAccessTokenGenerator;
            _principalResolver = principalResolver;
        }

		/// <summary>
		/// Runs the SharePoint web request.
		/// </summary>
		/// <param name="query">The SharePoint query parameter.</param>
		/// <param name="selectProperties">The SharePoint select properties parameter.</param>
		/// <param name="refiners">The refiners.</param>
		/// <param name="refinementFilters">The refinement filters.</param>
		/// <param name="startindex">The SharePoint start index parameter.</param>
		/// <param name="rowlimit">The SharePoint row limit parameter.</param>
		/// <param name="sortList">The sort list.</param>
		/// <param name="additionalFilter">The additional filter.</param>
		/// <returns>
		/// SharePointQueryResult.
		/// </returns>
        public SharePointQueryResult SubmitQuery(string query, IEnumerable<string> selectProperties, IList<string> refiners,
			Dictionary<string, List<string>> refinementFilters, long startindex, long rowlimit, List<AriaSort> sortList, string additionalFilter="")
        {
            return PostRequest(_configurationSource, query, selectProperties, refiners, refinementFilters, startindex, rowlimit,
                sortList, additionalFilter);
        }

		/// <summary>
		/// Runs the Sharepoint query as an Http post request.
		/// </summary>
		/// <param name="configurationSource">The SharePoint configuration sourcce parameter.</param>
		/// <param name="query">The SharePoint query parameter.</param>
		/// <param name="selectProperties">The SharePoint select properties parameter.</param>
		/// <param name="refiners">The refiners.</param>
		/// <param name="filters">The filters.</param>
		/// <param name="startindex">The SharePoint start index parameter.</param>
		/// <param name="rowlimit">The SharePoint row limit parameter.</param>
		/// <param name="sortList">The sort list.</param>
		/// <param name="additionalFilter">The additional filter.</param>
		/// <returns>
		/// SharePointQueryResult.
		/// </returns>
        public SharePointQueryResult PostRequest(ISharepointConfigurationSource configurationSource, string query,
            IEnumerable<string> selectProperties, IList<string> refiners, Dictionary<string, List<string>> filters,
			long startindex, long rowlimit, List<AriaSort> sortList, string additionalFilter = "")
        {
            var sharePointQueryResult = new SharePointQueryResult();

            //
            // Translate arguments into the Query object so we can properly serialize as JSON object
            //
            var json = new JavaScriptSerializer();
            var spQuery = new Query
            {
                Querytext = query,
                StartRow = startindex,
                RowLimit = rowlimit,
                TrimDuplicates = false
            };

            if (selectProperties != null)
            {
                spQuery.SelectProperties.results.AddRange(selectProperties);
            }

            if (refiners != null)
            {
                spQuery.Refiners = BuildRefiners(refiners);
            }

            string refinementFilters = null;
            if (filters != null && filters.Count > 0 || !string.IsNullOrEmpty(additionalFilter))
            {
                refinementFilters = BuildRefinementFilters(filters,additionalFilter);
                spQuery.RefinementFilters.results.Add(refinementFilters);
            }

            if (sortList != null)
            {
                foreach (var sort in sortList)
                {
                    spQuery.SortList.results.Add(new Sort(sort));
                }
            }

            //
            // serialize query as JSON and save for reference
            //
            var bodyPayload = string.Concat("{'request':", json.Serialize(spQuery), "}");
            var logMessage = new LogMessage(9995, LogPriority.Medium, TraceEventType.Information, bodyPayload, LogCategory.Search);
            logMessage.Data.Add("Claims", _principalResolver.Current.Claims.Select(x => new { x.Type, x.Value }).ToJson());
            _logManager.Log(logMessage);
            sharePointQueryResult.JsonRequest = bodyPayload;

            //
            // Build HTTP POST request
            //
            var sharepointSearchService = configurationSource.SearchService;
            var httpWebRequest = BuildHttpRequest(configurationSource);
            var generateSmtpClaimsIdentityAccessToken =
                _restApiAccessTokenGenerator.GenerateSmtpClaimsIdentityAccessToken(sharepointSearchService,
                    _principalResolver.Current.Identity.Name);

            httpWebRequest.Headers["x-requestdigest"] = GetXRequestDigestForPostRequest(configurationSource);
            httpWebRequest.Headers["Authorization"] = "Bearer " + generateSmtpClaimsIdentityAccessToken;

            //
            // Build and save HTTP GET uri
            //
            sharePointQueryResult.GetUri = BuildGetUri(sharepointSearchService, query, selectProperties, BuildRefiners(refiners),
                refinementFilters, startindex, rowlimit, BuildSortList(sortList));

            //
            // Perform the POST, process the response, and log any exceptions
            //
            byte[] bytes = Encoding.UTF8.GetBytes(bodyPayload);
            httpWebRequest.ContentLength = bytes.Length;
            using (var requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            GetAndProcessResponse(httpWebRequest, sharePointQueryResult);

            return sharePointQueryResult;
        }



        /// <summary>
        /// Normailizes the before and after date ranges. Merges the ranges together when 
        /// then will contrain the conditiona to a between. if from date is greater than the to date
        /// do nothing.
        /// </summary>
        /// <param name="filters">The filters.</param>
        internal static void NormailizeBeforeAfterDateRanges(List<string> filters)
        {
            Regex ToRangeRegex = new Regex(@"range\s*\(s*min\s*,\s*[0-9]{4}-[0-9]{1,2}-[0-9]{1,2}(T23:59:59Z)?\)");
            Regex FromRangeRegex = new Regex(@"range\s*\(\s*[0-9]{4}-[0-9]{1,2}-[0-9]{1,2},\s*max\s*\)");
            Regex OnDateRangeRegex = new Regex(@"range\s*\(\s*[0-9]{4}-[0-9]{1,2}-[0-9]{1,2},\s*[0-9]{4}-[0-9]{1,2}-[0-9]{1,2}\)");
            Regex SharePointDate = new Regex("[0-9]{4}-[0-9]{1,2}-[0-9]{1,2}");          
            const string endOfDay = "T23:59:59Z";

            var dateBeforeIndex = filters.FindIndex(x => x.Contains(AssetFieldNames.DateBefore));
            var dateAfterIndex = filters.FindIndex(x => x.Contains(AssetFieldNames.DateAfter));
            var dateOnIndex = filters.FindIndex(x => x.Contains(AssetFieldNames.DateEquals));

            if (dateBeforeIndex >= 0)
            {
                if (ToRangeRegex.IsMatch(filters[dateBeforeIndex]))
                {
                    var match = ToRangeRegex.Match(filters[dateBeforeIndex]).Value;
                    var replacement = match.Insert(match.Length-1, endOfDay);
                    filters[dateBeforeIndex] = filters[dateBeforeIndex].Replace(match, replacement);
                }
            }

            if (dateOnIndex >= 0)
            {
                if (OnDateRangeRegex.IsMatch(filters[dateOnIndex]))
                {
                    var match = OnDateRangeRegex.Match(filters[dateOnIndex]).Value;

                    var matches = SharePointDate.Matches(match);
                    if (matches.Count == 2)
                    {
                        var toDate = matches[1];
                        var replacement = match.Insert(toDate.Index + toDate.Length, endOfDay);
                        filters[dateOnIndex] = filters[dateOnIndex].Replace(match, replacement);
                    }
                }
            }

            if (dateBeforeIndex >= 0 && dateAfterIndex >= 0)
            {
                if (ToRangeRegex.IsMatch(filters[dateBeforeIndex]) && FromRangeRegex.IsMatch(filters[dateAfterIndex]))
                {
                    var fromDateMatches = SharePointDate.Matches(FromRangeRegex.Match(filters[dateAfterIndex]).Value);
                    var toDateMatches = SharePointDate.Matches(ToRangeRegex.Match(filters[dateBeforeIndex]).Value);

                    if (fromDateMatches.Count != 1) return;
                    if (toDateMatches.Count != 1) return;

                    var fromDateStr = fromDateMatches[0].Value;
                    var fromDate = DateTime.ParseExact(fromDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

                    var toDateStr = toDateMatches[0].Value;
                    var toDate = DateTime.ParseExact(toDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

                    if (toDate >= fromDate)
                    {
                        //merge range and remove from filter so they work as a between contraint
                        filters[dateBeforeIndex] = filters[dateBeforeIndex].Replace("min", fromDateStr);
                        filters.RemoveAt(dateAfterIndex);
                    }
                }
            }

        }

		/// <summary>
		/// Builds the refinement filters.
		/// </summary>
		/// <param name="filters">The filters.</param>
		/// <param name="additionalFilter">The additional filter.</param>
		/// <returns></returns>
        public static string BuildRefinementFilters(Dictionary<string, List<string>> filters, string additionalFilter="")
        {
            if (filters.Count > 0)
            {
                var stringBuilder = new StringBuilder();

				if (filters.Count > 1 || !string.IsNullOrEmpty(additionalFilter))
                    stringBuilder
                        .Append("AND(");

                foreach (var refinementFilterDictionaryItem in filters)
                {
                    stringBuilder
                        .Append(refinementFilterDictionaryItem.Key)
                        .Append(":");

                    NormailizeBeforeAfterDateRanges(refinementFilterDictionaryItem.Value);

                    if (refinementFilterDictionaryItem.Value.Count > 1)
                        stringBuilder
                            .Append("OR(");

                    foreach (var refinementFilter in refinementFilterDictionaryItem.Value)
                    {
                        //quote fields names incase there are special chars in them, eg a period
                        var useDoubleQuotes = !refinementFilter.StartsWith(AssetFieldNames.NoQuotes);
                        var refinementFilterval = refinementFilter.Replace(AssetFieldNames.NoQuotes, "");

                        foreach (var flag in AssetFieldNames.AriaDateRefinerControlFlags)
                        {
                            if (refinementFilterval.Contains(flag))
                            {
                                refinementFilterval = refinementFilterval.Substring(0, refinementFilterval.IndexOf(flag));
                            }
                        }

                        stringBuilder
                            .Append(HttpUtility.JavaScriptStringEncode(refinementFilterval, useDoubleQuotes))
                            .Append(",");
                    }
                    stringBuilder.Length--;

                    if (refinementFilterDictionaryItem.Value.Count > 1)
                        stringBuilder
                            .Append(")");

                    stringBuilder.Append(",");
                }

                stringBuilder.Length--;

	            if (!string.IsNullOrEmpty(additionalFilter))
	            {
					stringBuilder.Append(",");
		            stringBuilder.Append(additionalFilter);
	            }

                if (filters.Count > 1 || !string.IsNullOrEmpty(additionalFilter))
                    stringBuilder
                        .Append(")");

                return stringBuilder.ToString();
            }

			if (!string.IsNullOrEmpty(additionalFilter))
				return additionalFilter;

            return null;
        }

        internal static string BuildRefiners(IList<string> refiners)
        {
            if (refiners.Count > 0)
            {
                var stringBuilder = new StringBuilder();

                foreach (var refiner in refiners)
                {
                    stringBuilder
                        .Append(refiner)
                        .Append(",");
                }

                stringBuilder.Length--;

                return stringBuilder.ToString();
            }

            return null;
        }

        internal static string BuildSortList(List<AriaSort> sorts)
        {
            if (sorts.Count > 0)
            {
                var sortList = new StringBuilder();
                var json = new JavaScriptSerializer();

                foreach (var sort in sorts)
                {
                    var sortApi = new Sort(sort);
                    sortList
                        .Append(json.Serialize(sortApi))
                        .Append(",");
                }

                sortList.Length--;

                return sortList.ToString();
            }

            return null;
        }

        internal static string BuildGetUri(Uri sharepointSearchService, string query,
            IEnumerable<string> selectProperties, string refiners, string refinementFilters,
            long startindex, long rowlimit, string sortList)
        {
            var stringBuilder =
                new StringBuilder(sharepointSearchService.AbsoluteUri.Replace("/_api/search/postquery",
                    "/_api/search/query"));

            stringBuilder.AppendFormat("?querytext='{0}'", HttpUtility.UrlEncode(query));

            if (!String.IsNullOrEmpty(refiners))
                stringBuilder.AppendFormat("&refiners='{0}'", HttpUtility.UrlEncode(refiners));

            if (!String.IsNullOrEmpty(refinementFilters))
                stringBuilder.AppendFormat("&refinementfilters='{0}'", HttpUtility.UrlEncode(refinementFilters));

            if (!String.IsNullOrEmpty(sortList))
                stringBuilder.AppendFormat("&sortlist='{0}'", HttpUtility.UrlEncode(sortList));

            stringBuilder.Append("&trimduplicates=false");

            stringBuilder.AppendFormat("&startrow={0}", startindex);

            stringBuilder.AppendFormat("&rowlimit={0}", rowlimit);

            if (selectProperties != null && selectProperties.Any())
                stringBuilder.AppendFormat("&selectproperties='{0}'",
                    HttpUtility.UrlEncode(string.Join("','", selectProperties)));

            return stringBuilder.ToString();
        }

        internal static HttpWebRequest BuildHttpRequest(ISharepointConfigurationSource configurationSource)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(configurationSource.SearchService);
            httpWebRequest.Accept = HttpPostRequestJsonAccept;
            httpWebRequest.Timeout = configurationSource.Timeout;
            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.UseDefaultCredentials = true;
            httpWebRequest.Credentials = configurationSource.Credentials;

            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = HttpPostRequestJsonAccept;
            httpWebRequest.Headers["client-request-id"] = Trace.CorrelationManager.ActivityId.ToString();

            return httpWebRequest;
        }

        private void GetAndProcessResponse(HttpWebRequest httpWebRequest, SharePointQueryResult sharePointQueryResult)
        {
            try
            {
                using (var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        ProcessSharepointSearchXmlJson(stream, sharePointQueryResult);
                    }
                }
            }
            catch (WebException ex)
            {
                try
                {
                    using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        var str = streamReader.ReadToEnd();
                        var logMessage = new LogMessage(MessageIds.SearchSharePointErrorWebException, LogPriority.High, TraceEventType.Error,
                            "Error searching SharePoint: " + str, LogCategory.Search);
                        logMessage.LogCategories.Add(LogCategory.Search);
                        _logManager.Log(logMessage);
                    }
                }
                catch
                {
                    var logMessage = new LogMessage(MessageIds.SearchSharePointErrorGeneralException, LogPriority.High, TraceEventType.Error,
                        "Error searching SharePoint: " + ex.GetBaseException().Message, LogCategory.Search);
                    logMessage.LogCategories.Add(LogCategory.Search);
                    _logManager.Log(logMessage);
                }
                throw;
            }
        }

        internal static string GetXRequestDigestForPostRequest(ISharepointConfigurationSource configurationSource)
        {
            string xRequestDigest;

            // Build request
            var httpWebRequest = BuildHttpRequest(configurationSource);

            httpWebRequest.ContentLength = 0;

            try
            {
                using (var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    xRequestDigest = GetXRequestDigest(httpWebResponse.Headers);
                }
            }
            catch (WebException webException)
            {
                using (var errorResponse = webException.Response as HttpWebResponse)
                {
                    if (errorResponse != null &&
                        (errorResponse.StatusCode == HttpStatusCode.Forbidden ||
                         errorResponse.StatusCode == HttpStatusCode.Unauthorized))
                    {
                        xRequestDigest = GetXRequestDigest(errorResponse.Headers);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return xRequestDigest;
        }

        internal static string GetXRequestDigest(NameValueCollection webHeaderCollection)
        {
            var xRequestDigest = "";

            foreach (var key in webHeaderCollection.AllKeys.Where(key => key.ToLower() == "x-requestdigest"))
            {
                xRequestDigest = webHeaderCollection[key];
                break;
            }

            return xRequestDigest;
        }

        /// <summary>
        /// De-serializes the SharePoint Json Xml stream.
        /// </summary>
        /// <param name="stream">The Json xml stream to deserialize.</param>
        /// <param name="sharePointQueryResult">The share point query result.</param>
        internal void ProcessSharepointSearchXmlJson(Stream stream, SharePointQueryResult sharePointQueryResult)
        {
            // Read Json to deserialize
            var streamReaderContent = new StreamReader(stream);
            var content = streamReaderContent.ReadToEnd();

            // Store search results
            sharePointQueryResult.JsonResult = content;

            using (var xmlReader = JsonReaderWriterFactory.CreateJsonReader(
                Encoding.UTF8.GetBytes(content), new XmlDictionaryReaderQuotas()))
            {
                var xElement = XElement.Load(xmlReader);
                var xElement1 = xElement.XPathSelectElement("//postquery");
                if (xElement1 != null)
                {
                    var xElement7 = xElement1.Element("PrimaryQueryResult");
                    if (xElement7 != null)
                    {
                        var xElement8 = xElement7.Element("RelevantResults");
                        if (xElement8 != null)
                        {
                            if (xElement8.Element("TotalRows") != null)
                            {
                                sharePointQueryResult.TotalRows = (int)xElement8.Element("TotalRowsIncludingDuplicates");
                            }
                            var xElement9 = xElement8.Element("Table");
                            if (xElement9 != null && xElement9.HasElements)
                            {
                                var xElement10 = xElement9.Element("Rows");
                                if (xElement10 != null && xElement10.Element("results") != null &&
                                    xElement10.Element("results").HasElements)
                                {
                                    var xElements = xElement10.Element("results").Elements("item");
                                    foreach (var xElement11 in xElements)
                                    {
                                        if (xElement11.Element("Cells") != null &&
                                            xElement11.Element("Cells").Element("results") != null &&
                                            xElement11.Element("Cells").Element("results").HasElements)
                                        {
                                            var dictionary = new Dictionary<string, string>();
                                            var xElements1 =
                                                xElement11.Element("Cells").Element("results").Elements("item");
                                            var xElements2 =
                                                xElements1.OrderBy((XElement i) => i.Element("Key").Value);
                                            foreach (var xElement12 in xElements2)
                                                dictionary[xElement12.Element("Key").Value] =
                                                    xElement12.Element("Value").Value;
                                            sharePointQueryResult.SearchResults.Add(dictionary);
                                        }
                                    }
                                }
                            }
                        }
                        XElement xElement13 = xElement7.Element("RefinementResults");
                        if (xElement13 != null)
                        {
                            XElement xElement14 = xElement13.Element("Refiners");
                            if (xElement14 != null && xElement14.HasElements)
                            {
                                XElement xElement15 = xElement14.Element("results");
                                if (xElement15 != null && xElement15.HasElements)
                                {
                                    IEnumerable<XElement> xElements3 = xElement15.Elements("item");
                                    foreach (XElement xElement16 in xElements3)
                                    {
                                        var refinerResult = new List<IRefinementItem>();
                                        var name = (string)xElement16.Element("Name");
                                        if (xElement16.Element("Entries") != null &&
                                            xElement16.Element("Entries").Element("results") != null &&
                                            xElement16.Element("Entries").Element("results").HasElements)
                                        {
                                            IEnumerable<XElement> xElements4 =
                                                xElement16.Element("Entries")
                                                    .Element("results")
                                                    .Elements("item");
                                            refinerResult.AddRange(
                                                xElements4.Select(xElement17 => new RefinementItem
                                                {
                                                    Count = (long)xElement17.Element("RefinementCount"),
                                                    Name = (string)xElement17.Element("RefinementName"),
                                                    Token = (string)xElement17.Element("RefinementToken"),
                                                    Value = (string)xElement17.Element("RefinementValue")
                                                }));
                                        }
                                        sharePointQueryResult.RefinerResults.Add(name, ClearnRefiner(refinerResult));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        internal List<IRefinementItem> ClearnRefiner(List<IRefinementItem> items)
        {
            var multiItems = items.Where(x => x.Value.Contains(AssetFieldNames.SharePointMultivalueSeparator)).ToList();

            multiItems.ForEach(x =>
            {
                items.Remove(x);
                var valuesList = x.Value.Split(new char[] { AssetFieldNames.SharePointMultivalueSeparator });

                valuesList.Select(y => new RefinementItem()
                 {
                     Token = x.Token,
                     Value = y,
                     Count = x.Count,
                     Name = y
                 }).ToList().ForEach(y =>
                 {
                     if (!items.Exists(z => z.Value == y.Value))
                     {
                         items.Add(y);
                     }
                     else
                     {
                         items.First(z => z.Value == y.Value)
                            .Count += y.Count;
                     }

                 });
            });

            return items.OrderBy(x => x.Name).ToList();
        }


        /// <summary>
        ///     Represents the parts of a SharePoint Search REST API POST query we are currently leveraging.
        ///     see http://msdn.microsoft.com/en-us/library/sharepoint/jj163876.aspx
        /// </summary>
        private class Query
        {
            public Query()
            {
                SelectProperties = new StringCollection();
                RefinementFilters = new StringCollection();
                SortList = new SortCollection();
            }

            public string Querytext { get; set; }
            public StringCollection SelectProperties { get; set; }
            public string Refiners { get; set; }
            public StringCollection RefinementFilters { get; set; }
            public SortCollection SortList { get; set; }
            public long StartRow { get; set; }
            public long RowLimit { get; set; }
            public bool TrimDuplicates { get; set; }
        }

        /// <summary>
        ///     Represents a SharePoint Search REST API Sort structure
        /// </summary>
        private class Sort
        {
            public Sort(AriaSort sort)
            {
                Property = sort.FieldName;
                Direction = (short)(sort.Order == SortDirection.Ascending ? 0 : 1);
            }

            public string Property { get; set; }

            /// <summary>
            ///     Gets or sets the direction.  0=ascending, 1=descending.
            /// </summary>
            public short Direction { get; set; }
        }

        /// <summary>
        ///     Represents a SharePoint Search REST API sort collection structure
        /// </summary>
        private class SortCollection
        {
            public SortCollection()
            {
                results = new List<Sort>();
            }

            // Leave this property name as lower case to match documentation on POST format @ http://msdn.microsoft.com/en-us/library/sharepoint/jj163876.aspx
            // ReSharper disable once InconsistentNaming
            public List<Sort> results { get; private set; }
        }

        /// <summary>
        ///     Represents a SharePoint Search REST API string collection structure
        /// </summary>
        private class StringCollection
        {
            public StringCollection()
            {
                results = new List<string>();
            }

            // Leave this property name as lower case to match documentation on POST format @ http://msdn.microsoft.com/en-us/library/sharepoint/jj163876.aspx
            // ReSharper disable once InconsistentNaming
            public List<string> results { get; private set; }
        }
    }
}