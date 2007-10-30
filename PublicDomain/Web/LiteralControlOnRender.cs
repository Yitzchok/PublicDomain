using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace PublicDomain.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class LiteralControlOnRender : Control
    {
        /// <summary>
        /// 
        /// </summary>
        public string Text;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralControlOnRender"/> class.
        /// </summary>
        public LiteralControlOnRender()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralControlOnRender"/> class.
        /// </summary>
        /// <param name="txt">The TXT.</param>
        public LiteralControlOnRender(string txt)
        {
            Text = txt;
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HTmlTextWriter"></see> object that receives the control content.</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                writer.Write(Text);
            }
        }
    }
}
