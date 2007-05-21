using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Feeder.Opml
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOpmlBody : IOpmlOutlineProvider
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class OpmlBody : OpmlOutlineProvider, IOpmlBody
    {
    }
}
