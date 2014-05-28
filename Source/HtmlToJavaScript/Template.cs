using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HtmlToJavaScript
{
    /// <summary>
    /// The template to use when converting from Html to JavaScript.
    /// </summary>
    public class Template
    {
        /// <summary>
        /// The tokens used with the template.
        /// </summary>
        public static class Tokens
        {
            /// <summary>
            /// The name of the html. Used to refer to the html.
            /// </summary>
            public static readonly string Name = "@@Name@@";

            /// <summary>
            /// The html.
            /// </summary>
            public static readonly string Html = "@@Html@@";
        }

        /// <summary>
        /// Validates the template to ensure it has the proper tokens.
        /// </summary>
        /// <param name="template">The template.</param>
        public static void Validate(string template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }
            else if (template == string.Empty)
            {
                throw new ArgumentOutOfRangeException("template", "Value cannot be empty.");
            }

            EnsureTokenExists(Tokens.Name, template);
            EnsureTokenExists(Tokens.Html, template);
        }

        /// <summary>
        /// Ensures a token exists in the template.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="template">The template.</param>
        private static void EnsureTokenExists(string token, string template)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }
            else if (token == string.Empty)
            {
                throw new ArgumentOutOfRangeException("token", "Value cannot be empty.");
            }
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }
            else if (template == string.Empty)
            {
                throw new ArgumentOutOfRangeException("template", "Value cannot be empty.");
            }

            if (template.IndexOf(token, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new ArgumentOutOfRangeException("template", template, string.Format("A required token could not be found. Token: {0}", token));
            }
        }
    }
}
