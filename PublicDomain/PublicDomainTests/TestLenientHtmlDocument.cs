using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.Xml;

namespace PublicDomain
{
    public class TestLenientHtmlDocument : LenientHtmlDocument
    {
        protected override string ConvertEntityToValue(string token)
        {
            if (token == "test")
            {
                return "blah";
            }
            return base.ConvertEntityToValue(token);
        }
    }
}
