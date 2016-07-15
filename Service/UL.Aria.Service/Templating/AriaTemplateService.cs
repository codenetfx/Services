using System.Diagnostics.CodeAnalysis;

using RazorEngine.Configuration;
using RazorEngine.Templating;

using UL.Aria.Service.Configuration;

namespace UL.Aria.Service.Templating
{
	/// <summary>
	/// Templating service that will provide a correct configuration object that uses our configured Template Resolver.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class AriaTemplateService : TemplateService, IAriaTemplateService
	{
		private readonly IServiceConfiguration _serviceConfiguration;

		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateService" /> class.
		/// </summary>
		/// <param name="resolver">The resolver.</param>
		/// <param name="serviceConfiguration"></param>
		public AriaTemplateService(ITemplateResolver resolver, IServiceConfiguration serviceConfiguration)
			: base(new TemplateServiceConfiguration {Resolver = resolver})
		{
			_serviceConfiguration = serviceConfiguration;
		}

		/// <summary>
		/// Renders the template.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="templateName">Name of the template.</param>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		public string RenderTemplate<T>(string templateName, T model)
		{
			var template = Resolve(templateName, model);

			//
			// set common view bag elements
			//
			dynamic viewBag = new DynamicViewBag();
			viewBag.ServiceConfiguration = _serviceConfiguration;

			return Run(template, viewBag);
		}
	}
}