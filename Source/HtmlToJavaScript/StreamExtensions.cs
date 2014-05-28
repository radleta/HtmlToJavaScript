using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HtmlToJavaScript
{
    /// <summary>
    /// Extensions
    /// </summary>
    internal static class StreamExtensions
    {
        /// <summary>
        /// Reads the <c>stream</c> to the end as a string.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <returns>The content of the <c>stream</c> as a string.</returns>
        public static string ReadToEnd(this System.IO.Stream stream)
        {
            if (stream == null)
            {
                throw new System.ArgumentNullException("stream");
            }

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
