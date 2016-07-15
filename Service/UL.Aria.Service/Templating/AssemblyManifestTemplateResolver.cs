using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RazorEngine.Templating;

namespace UL.Aria.Service.Templating
{
	/// <summary>
	/// Resolved templates from assembly embedded resources
	/// </summary>
	public class AssemblyManifestTemplateResolver : ITemplateResolver
	{
		/// <summary>
		/// Resolves the template content with the specified name.
		/// </summary>
		/// <param name="name">The name of the template to resolve.</param>
		/// <returns>
		/// The template content.
		/// </returns>
		public string Resolve(string name)
		{
			var manifestName = string.Concat("UL.Aria.Service.Views.", name.Replace("~", "").Replace("/", ".").Replace("\\", ".").TrimStart('.'));
			var ass = Assembly.GetExecutingAssembly();

			using (var reader = new StreamReader(ass.GetManifestResourceStream(manifestName)))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
