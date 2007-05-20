using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// Code marked with this attribute should be moved to the PublicDomain package.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class PendingPublicDomainAttribute : Attribute
    {
    }
}
