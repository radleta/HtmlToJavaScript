using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace HtmlToJavaScript
{
    /// <summary>
    /// The <see cref="IHttpHandler"/> for the <see cref="HtmlConverter"/> to transform Html to JavaScript.
    /// </summary>
    /// <remarks>
    /// The module will handle any requests for "*.js.html" and transform it from Html to JavaScript.
    /// </remarks>
    public class HtmlConverterHttpHandler : IHttpHandler
    {
        /// <summary>
        /// The options for the <see cref="HtmlConverter"/>.
        /// </summary>
        public static HtmlConverterOptions Options = HtmlConverterOptions.Default;
        
        /// <summary>
        /// The <see cref="HtmlConverterHttpHandler"/> is reusable.
        /// </summary>
        public bool IsReusable
        {
            get 
            {
                return true; 
            }
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;

            // get the output
            var output = GetCachedOutput(context, request.PhysicalPath);
            
            // check the last modified since
            var modifiedSince = request.Headers["If-Modified-Since"];
            if (!String.IsNullOrEmpty(modifiedSince))
            {
                DateTime modifiedSinceDate;
                if (DateTime.TryParse(modifiedSince, out modifiedSinceDate)
                    && modifiedSinceDate == output.LastModified)
                {
                    response.StatusCode = 304;
                    response.StatusDescription = "Not Modified";
                    return;
                }
            }
                                                
            // set the content type appropriately
            response.ContentType = "text/javascript";

            // set the cachibility to public
            response.Cache.SetCacheability(HttpCacheability.Public);
                        
            // use the last modified from the file dependencies since they represent whether or not we should bust the cache
            // note: strip the milliseconds since they aren't transmitted
            response.Cache.SetLastModified(output.LastModified);

            // ignore invalidation requests from client
            response.Cache.SetValidUntilExpires(true);

            // write the javascript out as content
            response.Write(output.JavaScript);
        }

        /// <summary>
        /// Gets the cached output
        /// </summary>
        /// <param name="context"></param>
        /// <param name="htmlFile"></param>
        /// <returns></returns>
        private HtmlToJavaScriptOutput GetCachedOutput(HttpContext context, string htmlFile)
        {
            // check the cache for the output
            var cacheKey = string.Format("HtmlConverterHttpHandler_{0}", htmlFile);
            var output = (HtmlToJavaScriptOutput)context.Cache.Get(cacheKey);
            if (output != null)
            {
                return output;
            }

            // create the output
            output = CreateOutput(context, htmlFile);

            // cache the result and be dependant on the html file so the cache busts when it changes
            context.Cache.Insert(cacheKey, output, new System.Web.Caching.CacheDependency(htmlFile), System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15));

            return output;
        }

        /// <summary>
        /// Creates the <see cref="HtmlToJavaScriptOutput"/> so we can cache it.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <param name="htmlFile"></param>
        /// <returns></returns>
        private HtmlToJavaScriptOutput CreateOutput(HttpContext context, string htmlFile)
        {
            // get the file
            var file = new FileInfo(htmlFile);

            // create the date and strip the milliseconds
            var fileLastModified = new DateTime(file.LastWriteTime.Year, file.LastWriteTime.Month, file.LastWriteTime.Day, file.LastWriteTime.Hour, file.LastWriteTime.Minute, file.LastWriteTime.Second, DateTimeKind.Local);

            // load the html
            var html = File.ReadAllText(file.FullName);

            // create the converter
            var converter = new HtmlConverter(Options);

            // convert the html to javascript
            var javaScript = converter.ToJavaScript(file.Name, html);

            // create the output
            return new HtmlToJavaScriptOutput()
            {
                LastModified = fileLastModified,
                JavaScript = javaScript,
            };
        }

        /// <summary>
        /// The output of the conversion process we can cache.
        /// </summary>
        private class HtmlToJavaScriptOutput
        {
            /// <summary>
            /// The last modified date of the original file. Used to compare against incoming requests.
            /// </summary>
            public DateTime LastModified;

            /// <summary>
            /// The output JavaScript.
            /// </summary>
            public string JavaScript;
        }
    }
}
