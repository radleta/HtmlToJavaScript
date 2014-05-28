using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HtmlToJavaScript
{
    /// <summary>
    /// The converter to transform Html to JavaScript.
    /// </summary>
    public class HtmlConverter
    {
        /// <summary>
        /// The default template
        /// </summary>
        private static readonly string DefaultTemplate = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(HtmlConverter), "DefaultTemplate.js.h2j").ReadToEnd();
        
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public HtmlConverter()
        {
        }

        /// <summary>
        /// Initializes a new instance of this class with <c>options</c>.
        /// </summary>
        /// <param name="options">The options to use when converting the html to javascript.</param>
        public HtmlConverter(HtmlConverterOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// The options to use when converting the HTML to JavaScript.
        /// </summary>
        public HtmlConverterOptions Options { get; set; }
        
        /// <summary>
        /// Converts the <c>html</c> to <c>JavaScript</c>
        /// </summary>
        /// <param name="html">The HTML to convert.</param>
        /// <returns>The JavaScript for the HTML.</returns>
        public string ToJavaScript(string name, string html)
        {
            var converterOptions = Options ?? HtmlConverterOptions.Default;
            
            // get the template
            string template = DefaultTemplate;
            if (!string.IsNullOrEmpty(converterOptions.TemplateFile))
            {
                if (File.Exists(converterOptions.TemplateFile))
                {
                    template = File.ReadAllText(converterOptions.TemplateFile);
                }
                else
                {
                    throw new InvalidOperationException(string.Format("The template file does not exist. File: {0}", converterOptions.TemplateFile));
                }
            }
            
            // validate the template
            Template.Validate(template);
                        
            // compress the html
            if (converterOptions.Compress)
            {
                // TODO: compress the html
            }

            // replace the tokens
            var javascript = new StringBuilder(template);
            javascript.ReplaceTokenString(Template.Tokens.Name, name);
            javascript.ReplaceTokenString(Template.Tokens.Html, html);
            return javascript.ToString();
        }
    }
}
