using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HtmlToJavaScript
{
    /// <summary>
    /// Extensions for <see cref="System.Text.StringBuilder"/>.
    /// </summary>
    internal static class StringBuilderExtensions
    {
        /// <summary>
        /// Replaces a HtmlToJavaScript template token with it's JavaScript string replacement.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to extend.</param>
        /// <param name="token">The token to replace.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns>The <c>builder</c>.</returns>
        public static StringBuilder ReplaceTokenString(this StringBuilder builder, string token, string replacement)
        {
            if (builder == null)
            {
                throw new System.ArgumentNullException("builder");
            }
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }
            else if (token == string.Empty)
            {
                throw new ArgumentOutOfRangeException("token", "Value cannot be empty.");
            }
            if (replacement == null)
            {
                throw new ArgumentNullException("replacement");
            }
            else if (replacement == string.Empty)
            {
                throw new ArgumentOutOfRangeException("replacement", "Value cannot be empty.");
            }

            var replacementJavaScript = new StringBuilder(replacement);
            replacementJavaScript.Replace("\\", "\\\\");
            replacementJavaScript.Replace("'", "\\'");
            replacementJavaScript.Replace("\n", "\\n");
            replacementJavaScript.Replace("\r", "\\r");
            replacementJavaScript.Replace("\t", "\\t");
            replacementJavaScript.Replace("\f", "\\f");
            replacementJavaScript.Replace("\b", "\\b");
            return builder.Replace(token, string.Format("'{0}'", replacementJavaScript));            
        }
    }
}
