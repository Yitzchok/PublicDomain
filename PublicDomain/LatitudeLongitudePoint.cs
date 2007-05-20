using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PublicDomain
{
    /// <summary>
    /// Generic representation of a latitude and longitude point.
    /// </summary>
    [Serializable]
    public class LatitudeLongitudePoint
    {
        /// <summary>
        /// 
        /// </summary>
        public double m_latitude;

        /// <summary>
        /// 
        /// </summary>
        public double m_longitude;

        /// <summary>
        /// Initializes a new instance of the <see cref="LatitudeLongitudePoint"/> class.
        /// </summary>
        public LatitudeLongitudePoint()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LatitudeLongitudePoint"/> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public LatitudeLongitudePoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LatitudeLongitudePoint"/> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public LatitudeLongitudePoint(string latitude, string longitude)
        {
            Latitude = double.Parse(latitude);
            Longitude = double.Parse(longitude);
        }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude
        {
            get
            {
                return m_latitude;
            }
            set
            {
                m_latitude = value;
            }
        }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude
        {
            get
            {
                return m_longitude;
            }
            set
            {
                m_longitude = value;
            }
        }

        /// <summary>
        /// Gets or sets the latitude decimal.
        /// </summary>
        /// <value>The latitude decimal.</value>
        public Decimal LatitudeDecimal
        {
            get
            {
                return Convert.ToDecimal(Latitude);
            }
            set
            {
                Latitude = Decimal.ToDouble(value);
            }
        }

        /// <summary>
        /// Gets or sets the longitude decimal.
        /// </summary>
        /// <value>The longitude decimal.</value>
        public Decimal LongitudeDecimal
        {
            get
            {
                return Convert.ToDecimal(Longitude);
            }
            set
            {
                Longitude = Decimal.ToDouble(value);
            }
        }

        /// <summary>
        /// Procedure for converting degrees, minutes, seconds into decimal degrees:
        /// Degrees, minutes, seconds value: 37 degrees, 25 minutes, 40.5 seconds
        /// 1. Decimal degrees = degrees + (minutes/60) + (seconds/3600)
        /// 2. 37 degrees, 25 minutes, 40.5 seconds = 37. + (25/60) + (40.5/3600)
        /// 3. 37. + .416666 + .01125
        /// 4. So 37 degrees, 25 minutes, 40.5 seconds = 37.427916 in decimal degrees.
        /// </summary>
        /// <value>The latitude degrees.</value>
        public int LatitudeDegrees
        {
            get
            {
                return (int)Latitude;
            }
        }

        /// <summary>
        /// Gets the latitude minutes.
        /// </summary>
        /// <value>The latitude minutes.</value>
        public int LatitudeMinutes
        {
            get
            {
                return (int)((Latitude - (double)LatitudeDegrees) * 60D);
            }
        }

        /// <summary>
        /// Gets the latitude seconds.
        /// </summary>
        /// <value>The latitude seconds.</value>
        public double LatitudeSeconds
        {
            get
            {
                return (((Latitude - (double)LatitudeDegrees) * 60D) - (double)LatitudeMinutes) * 60D;
            }
        }

        /// <summary>
        /// Gets the longitude degrees.
        /// </summary>
        /// <value>The longitude degrees.</value>
        public int LongitudeDegrees
        {
            get
            {
                return (int)Longitude;
            }
        }

        /// <summary>
        /// Gets the longitude minutes.
        /// </summary>
        /// <value>The longitude minutes.</value>
        public int LongitudeMinutes
        {
            get
            {
                return (int)((Longitude - (double)LongitudeDegrees) * 60D);
            }
        }

        /// <summary>
        /// Gets the longitude seconds.
        /// </summary>
        /// <value>The longitude seconds.</value>
        public double LongitudeSeconds
        {
            get
            {
                return (((Longitude - (double)LongitudeDegrees) * 60D) - (double)LongitudeMinutes) * 60D;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return "(" + Latitude + "," + Longitude + ")";
        }

        /// <summary>
        /// Parses the specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns></returns>
        public static LatitudeLongitudePoint Parse(string latitude, string longitude)
        {
            if (!(latitude == null || longitude == null || latitude.ToLower() == "na" || longitude.ToLower() == "na"))
            {
                LatitudeLongitudePoint ret = new LatitudeLongitudePoint();
                Regex r = new Regex(@"(\d+)\.(\d+)(\.(\d+))?(\w)");
                double degrees, minutes, seconds = 0;
                Match m = r.Match(latitude);
                if (m.Success)
                {
                    degrees = double.Parse(m.Groups[1].Value);
                    minutes = double.Parse(m.Groups[2].Value);
                    seconds = m.Groups[3].Success ? double.Parse(m.Groups[3].Value) : 0;
                    ret.Latitude = degrees + (minutes / 60D) + (seconds / 3600D);
                    if (m.Groups[5].Value.ToLower() == "s")
                        ret.Latitude *= -1;
                }
                else
                    throw new Exception("Unsupported parse type!");
                m = r.Match(longitude);
                if (m.Success)
                {
                    degrees = double.Parse(m.Groups[1].Value);
                    minutes = double.Parse(m.Groups[2].Value);
                    seconds = m.Groups[3].Success ? double.Parse(m.Groups[3].Value) : 0;
                    ret.Longitude = degrees + (minutes / 60D) + (seconds / 3600D);
                    if (m.Groups[5].Value.ToLower() == "w")
                        ret.Longitude *= -1;
                }
                else
                    throw new Exception("Unsupported parse type!");
                return ret;
            }
            return null;
        }

        /// <summary>
        /// Returns the distance between two latitude/longitude points, in miles.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double DistanceBetween(LatitudeLongitudePoint point1, LatitudeLongitudePoint point2)
        {
            return DistanceBetween(point1, point2, DistanceType.StatuteMiles);
        }

        /// <summary>
        /// Distances the between.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <returns></returns>
        public static double DistanceBetween(LatitudeLongitudePoint point1, LatitudeLongitudePoint point2, DistanceType returnType)
        {
            // see http://www.mathforum.com/library/drmath/view/51711.html
            // TODO is this really correct, or do I have to first convert decimal to degrees and then to radians?
            if (point1.Latitude == point2.Latitude) return 0;
            double a = point1.Latitude / 57.29577951D;
            double b = point1.Longitude / 57.29577951D;
            double c = point2.Latitude / 57.29577951D;
            double d = point2.Longitude / 57.29577951D;

            double earthRadius = (returnType == DistanceType.StatuteMiles ? GlobalConstants.EarthEquatorialRadiusInStatuteMiles : (returnType == DistanceType.NauticalMiles ? GlobalConstants.EarthEquatorialRadiusInNauticalMiles : GlobalConstants.EarthEquatorialRadiusInKilometers));

            double sina = Math.Sin(a);
            double sinc = Math.Sin(c);
            double cosa = Math.Cos(a);
            double cosc = Math.Cos(c);
            double cosbd = Math.Cos(b - d);

            double ans1 = ((sina * sinc) + (cosa * cosc * cosbd));
            if (ans1 > 1D)
            {
                return earthRadius * Math.Acos(1D);
            }
            else
            {
                return earthRadius * Math.Acos(ans1);
            }
        }
    }
}
