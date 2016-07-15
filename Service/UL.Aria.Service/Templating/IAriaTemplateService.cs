using RazorEngine.Templating;

namespace UL.Aria.Service.Templating
{
	/// <summary>
	/// The aria templating service
	/// </summary>
	public interface IAriaTemplateService : ITemplateService
	{
		/// <summary>
		/// Renders the template.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="templateName">Name of the template.</param>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		string RenderTemplate<T>(string templateName, T model);
	}
}