using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web.UI;

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
    }
}
