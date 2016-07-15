//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.ServiceModel;
//using System.ServiceModel.Web;
//using System.Text;
//using System.Threading.Tasks;
//using UL.Aria.Service.Contracts.Dto;

//namespace UL.Aria.Service.Contracts.Service
//{
//    /// <summary>
//    /// Resource service
//    /// </summary>
//    [ServiceContract]
//    public interface IResourceService
//    {
//        /// <summary>
//        /// Gets all by company id.
//        /// </summary>
//        /// <param name="companyId">The company id.</param>
//        /// <param name="startIndex">The start index.</param>
//        /// <param name="endIndex">The end index.</param>
//        /// <returns></returns>
//        [OperationContract]
//        [WebInvoke(UriTemplate = "/List?companyId={companyId}&start={startIndex}&end={endIndex}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
//        IResourceResultSet GetAllByCompanyId(Guid companyId, long startIndex, long endIndex);

//        /// <summary>
//        /// Gets all actions.
//        /// </summary>
//        /// <param name="uri">The URI.</param>
//        /// <returns></returns>
//        [OperationContract]
//        [WebInvoke(UriTemplate = "/Actions?uri={uri}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
//        IList<Claim> GetAllActions(string uri);
//    }
//}
