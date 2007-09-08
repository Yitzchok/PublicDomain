using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRandomNumberGenerator : INumberGenerator
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class RandomNumberGenerator : NumberGenerator, IRandomNumberGenerator
    {
        /// <summary>
        /// Random number generator in the range [0, int.Max]
        /// </summary>
        public static readonly IRandomNumberGenerator Default = new RandomNumberGenerator();

        private object m_lock = new object();
        private Random m_rand = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumberGenerator"/> class.
        /// </summary>
        public RandomNumberGenerator()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumberGenerator"/> class.
        /// </summary>
        /// <param name="maximum">The maximum.</param>
        public RandomNumberGenerator(int maximum)
            : base(maximum)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumberGenerator"/> class.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        public RandomNumberGenerator(int minimum, int maximum)
            : base(minimum, maximum)
        {
        }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public override int Maximum
        {
            get
            {
                return base.Maximum;
            }
            set
            {
                // we do this because Random.Next is [Min, Max)
                base.Maximum = unchecked(value + 1);
            }
        }

        /// <summary>
        /// Gets the next number in the range [Minimum, Maximum]
        /// </summary>
        /// <returns></returns>
        public override int GetNextNumber()
        {
            int result;

            lock (m_lock)
            {
                // [Minimum, Maximum)
                result = m_rand.Next(m_minimum, m_maximum);
            }

            return result;
        }
    }
}
