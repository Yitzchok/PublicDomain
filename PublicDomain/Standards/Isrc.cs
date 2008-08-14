using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// International Standard Recording Code
    /// ISO 3901
    /// </summary>
    [Serializable]
    public class Isrc
    {
        private string m_countryCode;
        private string m_registrantCode;
        private string m_yearOfRegistrationValue;
        private DateTime m_yearOfRegistration = DateTime.MinValue;
        private int m_id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Isrc"/> class.
        /// </summary>
        public Isrc()
        {
        }

        /// <summary>
        /// Gets the new isrc.
        /// </summary>
        /// <returns></returns>
        public static Isrc GetNewIsrc(string country, int year, string registrantCode)
        {
            if (string.IsNullOrEmpty(country))
            {
                throw new ArgumentNullException("country");
            }
            else if (string.IsNullOrEmpty(registrantCode))
            {
                throw new ArgumentNullException("registrantCode");
            }

            Isrc result = new Isrc();
            result.CountryCode = country;
            result.YearOfRegistrationValue = year.ToString();
            result.RegistrantCode = registrantCode;
            result.Id = RandomGenerationUtilities.GetRandomInteger(1, 99999);
            return result;
        }

        /// <summary>
        /// Gets or sets the country code.
        /// ISO 3166-1 alpha-2 country code
        /// </summary>
        /// <value>The country code.</value>
        public string CountryCode
        {
            get
            {
                return m_countryCode;
            }
            set
            {
                m_countryCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the registrant code.
        /// Uniquely identifying the organisation which registered the code
        /// </summary>
        /// <value>The registrant code.</value>
        public string RegistrantCode
        {
            get
            {
                return m_registrantCode;
            }
            set
            {
                m_registrantCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the year of registration value.
        /// </summary>
        /// <value>The year of registration value.</value>
        public string YearOfRegistrationValue
        {
            get
            {
                return m_yearOfRegistrationValue;
            }
            set
            {
                m_yearOfRegistrationValue = value;
                if (!string.IsNullOrEmpty(value))
                {
                    int yr = ConversionUtilities.ParseInt(value);
                    if (yr >= 1000)
                    {
                    }
                    else if (yr < 45)
                    {
                        yr += 2000;
                    }
                    else
                    {
                        yr += 1900;
                    }
                    m_yearOfRegistration = new DateTime(yr, 1, 1);
                }
            }
        }

        /// <summary>
        /// Gets the year of registration.
        /// </summary>
        /// <value>The year of registration.</value>
        public DateTime YearOfRegistration
        {
            get
            {
                return m_yearOfRegistration;
            }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
            }
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return CountryCode + "-" + RegistrantCode + "-" + YearOfRegistration.Year.ToString().Substring(2) + "-" + StringUtilities.PadIntegerLeft(Id, 5, '0');
        }
    }
}
