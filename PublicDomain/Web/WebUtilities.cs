using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Web;

namespace PublicDomain.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class WebUtilities
    {
        /// <summary>
        /// Calls the render method of all the Controls in a control tree,
        /// but instead of rendering to the response stream, we render to an
        /// in-memory string stream and then return that.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static string RenderControlToString(Control control)
        {
            StringBuilder sb = new StringBuilder(512);
            using (StringWriter stringWriter = new StringWriter(sb))
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter))
                {
                    control.RenderControl(htmlWriter);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the enum as HTML select options.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns></returns>
        public static string GetEnumAsHtmlSelectOptions(Type enumType, string selectedValue)
        {
            StringBuilder sb = new StringBuilder(128);

            foreach (string enumName in Enum.GetNames(enumType))
            {
                object enumVal = Enum.Parse(enumType, enumName, false);
                sb.Append("<option value=\"");
                sb.Append(HttpUtility.HtmlEncode(enumVal.ToString()));
                sb.Append('\"');

                if (selectedValue != null && selectedValue == enumName)
                {
                    sb.Append(" selected=\"selected\"");
                }

                sb.Append(">");
                sb.Append(HttpUtility.HtmlEncode(enumName));
                sb.Append("</option>\n");
            }

            return sb.ToString();
        }
    }
}
