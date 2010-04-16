using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;

namespace PublicDomain
{
    /// <summary>
    /// The class can be used in a using() {} block or a try, finally block with
    /// a dispose call and allows for setting the current Thread's culture
    /// to the invariant culture during the length of the scope. This is useful
    /// when it is critical to have invariant culture rules, for example, if
    /// you are dependent that a real number is of the form X.XXXX, then you
    /// will be thrown off if there is a European culture.
    /// </summary>
    public class InvariantCultureContext : IDisposable
    {
        private CultureInfo oldCulture;
        private CultureInfo oldUICulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvariantCultureContext"/> class.
        /// </summary>
        public InvariantCultureContext()
        {
            oldCulture = Thread.CurrentThread.CurrentCulture;
            oldUICulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = oldCulture;
            Thread.CurrentThread.CurrentUICulture = oldUICulture;
        }
    }
}
