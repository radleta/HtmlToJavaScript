using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace HtmlToJavaScript
{
    /// <summary>
    /// The text fixture for <see cref="HtmlConverter" />.
    /// </summary>
    [TestFixture]
    public class HtmlConverterTestFixture
    {
        /// <summary>
        /// The test cases for the <see cref="ToJavaScriptTest"/>
        /// </summary>
        public IEnumerable ToJavaScriptTestCases
        {
            get
            {                
                yield return new TestCaseData(
                    new HtmlConverter(),
@"var HtmlToJavaScript = HtmlToJavaScript || {};
HtmlToJavaScript['DivBasic'] = '<div></div>';",
                    "DivBasic",
@"<div></div>"
                    );

                yield return new TestCaseData(
                    new HtmlConverter(),
@"var HtmlToJavaScript = HtmlToJavaScript || {};
HtmlToJavaScript['DivWithSingleQuotesLineBreaksWhiteSpace'] = '<div class=\'name\'>\r\n    It\'s a great day for a test!\r\n</div>';",
                    "DivWithSingleQuotesLineBreaksWhiteSpace",
@"<div class='name'>
    It's a great day for a test!
</div>"
                    );
            }
        }

        /// <summary>
        /// Test for <see cref="HtmlConverter.ToJavaScript" />.
        /// </summary>
        [Test]
        [TestCaseSource("ToJavaScriptTestCases")]
        public void ToJavaScriptTest(HtmlConverter converter, string expected, string inputName, string inputHtml)
        {            
            var actual = converter.ToJavaScript(inputName, inputHtml);
            //Console.WriteLine("@\"{0}\"", actual.Replace("\"", "\"\""));
            Assert.AreEqual(expected, actual);
        }
    }
}