using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HtmlToJavaScript
{
    /// <summary>
    /// The options for the <see cref="HtmlConverter"/>
    /// </summary>
    public class HtmlConverterOptions
    {
        /// <summary>
        /// The default options.
        /// </summary>
        public static readonly HtmlConverterOptions Default = new HtmlConverterOptions()
        {
            Compress = true,
        };

        /// <summary>
        /// Determines whether or not to compress the HTML.
        /// </summary>
        public bool Compress { get; set; }

        /// <summary>
        /// The HtmlToJavaScript template to output the JavaScript.
        /// </summary>
        public string TemplateFile { get; set; }
    }
}
