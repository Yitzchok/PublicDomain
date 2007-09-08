using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public interface INumberGenerator
    {
        /// <summary>
        /// Gets the next number in the range [Minimum, Maximum]
        /// </summary>
        /// <returns></returns>
        int GetNextNumber();

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        int Maximum { get; set; }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        int Minimum { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class NumberGenerator : INumberGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        protected int m_maximum;

        /// <summary>
        /// 
        /// </summary>
        protected int m_minimum;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberGenerator"/> class.
        /// </summary>
        public NumberGenerator()
            : this(0, int.MaxValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberGenerator"/> class.
        /// </summary>
        /// <param name="maximum">The maximum.</param>
        public NumberGenerator(int maximum)
            : this(0, maximum)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberGenerator"/> class.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        public NumberGenerator(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// Gets the next number in the range [Minimum, Maximum]
        /// </summary>
        /// <returns></returns>
        public abstract int GetNextNumber();

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public virtual int Maximum
        {
            get
            {
                return m_maximum;
            }
            set
            {
                m_maximum = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public virtual int Minimum
        {
            get
            {
                return m_minimum;
            }
            set
            {
                m_minimum = value;
            }
        }
    }
}
