using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Country
    {
        private static ReadOnlyICollection<Country> s_countries;
        private static ReadOnlyDictionary<string, Country> s_countryMap;

        private string m_abbreviation;
        private string m_name;

        static Country()
        {
            Dictionary<string, Country> countryMap = new Dictionary<string, Country>();

            countryMap["AF"] = new Country("AF", "Afghanistan");
            countryMap["AL"] = new Country("AL", "Albania");
            countryMap["DZ"] = new Country("DZ", "Algeria");
            countryMap["AS"] = new Country("AS", "American Samoa");
            countryMap["AD"] = new Country("AD", "Andorra");
            countryMap["AO"] = new Country("AO", "Angola");
            countryMap["AI"] = new Country("AI", "Anguilla");
            countryMap["AQ"] = new Country("AQ", "Antarctica");
            countryMap["AG"] = new Country("AG", "Antigua and Barbuda");
            countryMap["AR"] = new Country("AR", "Argentina");
            countryMap["AM"] = new Country("AM", "Armenia");
            countryMap["AW"] = new Country("AW", "Aruba");
            countryMap["AC"] = new Country("AC", "Ascension Island");
            countryMap["AU"] = new Country("AU", "Australia");
            countryMap["AT"] = new Country("AT", "Austria");
            countryMap["AZ"] = new Country("AZ", "Azerbaijan");
            countryMap["BS"] = new Country("BS", "Bahamas");
            countryMap["BH"] = new Country("BH", "Bahrain");
            countryMap["BD"] = new Country("BD", "Bangladesh");
            countryMap["BB"] = new Country("BB", "Barbados");
            countryMap["BY"] = new Country("BY", "Belarus");
            countryMap["BE"] = new Country("BE", "Belgium");
            countryMap["BZ"] = new Country("BZ", "Belize");
            countryMap["BJ"] = new Country("BJ", "Benin");
            countryMap["BM"] = new Country("BM", "Bermuda");
            countryMap["BT"] = new Country("BT", "Bhutan");
            countryMap["BO"] = new Country("BO", "Bolivia");
            countryMap["BA"] = new Country("BA", "Bosnia and Herzegovina");
            countryMap["BW"] = new Country("BW", "Botswana");
            countryMap["BV"] = new Country("BV", "Bouvet Island");
            countryMap["BR"] = new Country("BR", "Brazil");
            countryMap["IO"] = new Country("IO", "British Indian Ocean Territory");
            countryMap["BN"] = new Country("BN", "Brunei");
            countryMap["BG"] = new Country("BG", "Bulgaria");
            countryMap["BF"] = new Country("BF", "Burkina Faso");
            countryMap["BI"] = new Country("BI", "Burundi");
            countryMap["KH"] = new Country("KH", "Cambodia");
            countryMap["CM"] = new Country("CM", "Cameroon");
            countryMap["CA"] = new Country("CA", "Canada");
            countryMap["CV"] = new Country("CV", "Cape Verde");
            countryMap["KY"] = new Country("KY", "Cayman Islands");
            countryMap["CF"] = new Country("CF", "Central African Republic");
            countryMap["TD"] = new Country("TD", "Chad");
            countryMap["CL"] = new Country("CL", "Chile");
            countryMap["CN"] = new Country("CN", "China");
            countryMap["CX"] = new Country("CX", "Christmas Island");
            countryMap["CC"] = new Country("CC", "Cocos (Keeling) Islands");
            countryMap["CO"] = new Country("CO", "Colombia");
            countryMap["KM"] = new Country("KM", "Comoros");
            countryMap["CG"] = new Country("CG", "Congo");
            countryMap["CD"] = new Country("CD", "Congo (DRC)");
            countryMap["CK"] = new Country("CK", "Cook Islands");
            countryMap["CR"] = new Country("CR", "Costa Rica");
            countryMap["CI"] = new Country("CI", "Côte d'Ivoire");
            countryMap["HR"] = new Country("HR", "Croatia");
            countryMap["CY"] = new Country("CY", "Cyprus");
            countryMap["CZ"] = new Country("CZ", "Czech Republic");
            countryMap["DK"] = new Country("DK", "Denmark");
            countryMap["DJ"] = new Country("DJ", "Djibouti");
            countryMap["DM"] = new Country("DM", "Dominica");
            countryMap["DO"] = new Country("DO", "Dominican Republic");
            countryMap["EC"] = new Country("EC", "Ecuador");
            countryMap["EG"] = new Country("EG", "Egypt");
            countryMap["SV"] = new Country("SV", "El Salvador");
            countryMap["GQ"] = new Country("GQ", "Equatorial Guinea");
            countryMap["ER"] = new Country("ER", "Eritrea");
            countryMap["EE"] = new Country("EE", "Estonia");
            countryMap["ET"] = new Country("ET", "Ethiopia");
            countryMap["FK"] = new Country("FK", "Falkland Islands (Islas Malvinas)");
            countryMap["FO"] = new Country("FO", "Faroe Islands");
            countryMap["FJ"] = new Country("FJ", "Fiji Islands");
            countryMap["FI"] = new Country("FI", "Finland");
            countryMap["FR"] = new Country("FR", "France");
            countryMap["PF"] = new Country("PF", "French Polynesia");
            countryMap["TF"] = new Country("TF", "French Southern and Antarctic Lands");
            countryMap["GA"] = new Country("GA", "Gabon");
            countryMap["GM"] = new Country("GM", "Gambia, The");
            countryMap["GE"] = new Country("GE", "Georgia");
            countryMap["DE"] = new Country("DE", "Germany");
            countryMap["GH"] = new Country("GH", "Ghana");
            countryMap["GI"] = new Country("GI", "Gibraltar");
            countryMap["GR"] = new Country("GR", "Greece");
            countryMap["GL"] = new Country("GL", "Greenland");
            countryMap["GD"] = new Country("GD", "Grenada");
            countryMap["GU"] = new Country("GU", "Guam");
            countryMap["GT"] = new Country("GT", "Guatemala");
            countryMap["GG"] = new Country("GG", "Guernsey");
            countryMap["GN"] = new Country("GN", "Guinea");
            countryMap["GW"] = new Country("GW", "Guinea-Bissau");
            countryMap["GY"] = new Country("GY", "Guyana");
            countryMap["HT"] = new Country("HT", "Haiti");
            countryMap["HM"] = new Country("HM", "Heard Island and McDonald Islands");
            countryMap["HN"] = new Country("HN", "Honduras");
            countryMap["HK"] = new Country("HK", "Hong Kong SAR");
            countryMap["HU"] = new Country("HU", "Hungary");
            countryMap["IS"] = new Country("IS", "Iceland");
            countryMap["IN"] = new Country("IN", "India");
            countryMap["ID"] = new Country("ID", "Indonesia");
            countryMap["IQ"] = new Country("IQ", "Iraq");
            countryMap["IE"] = new Country("IE", "Ireland");
            countryMap["IM"] = new Country("IM", "Isle of Man");
            countryMap["IL"] = new Country("IL", "Israel");
            countryMap["IT"] = new Country("IT", "Italy");
            countryMap["JM"] = new Country("JM", "Jamaica");
            countryMap["JP"] = new Country("JP", "Japan");
            countryMap["JE"] = new Country("JE", "Jersey");
            countryMap["JO"] = new Country("JO", "Jordan");
            countryMap["KZ"] = new Country("KZ", "Kazakhstan");
            countryMap["KE"] = new Country("KE", "Kenya");
            countryMap["KI"] = new Country("KI", "Kiribati");
            countryMap["KR"] = new Country("KR", "Korea");
            countryMap["KW"] = new Country("KW", "Kuwait");
            countryMap["KG"] = new Country("KG", "Kyrgyzstan");
            countryMap["LA"] = new Country("LA", "Laos");
            countryMap["LV"] = new Country("LV", "Latvia");
            countryMap["LB"] = new Country("LB", "Lebanon");
            countryMap["LS"] = new Country("LS", "Lesotho");
            countryMap["LR"] = new Country("LR", "Liberia");
            countryMap["LY"] = new Country("LY", "Libya");
            countryMap["LI"] = new Country("LI", "Liechtenstein");
            countryMap["LT"] = new Country("LT", "Lithuania");
            countryMap["LU"] = new Country("LU", "Luxembourg");
            countryMap["MO"] = new Country("MO", "Macao SAR");
            countryMap["MK"] = new Country("MK", "Macedonia, Former Yugoslav Republic of");
            countryMap["MG"] = new Country("MG", "Madagascar");
            countryMap["MW"] = new Country("MW", "Malawi");
            countryMap["MY"] = new Country("MY", "Malaysia");
            countryMap["MV"] = new Country("MV", "Maldives");
            countryMap["ML"] = new Country("ML", "Mali");
            countryMap["MT"] = new Country("MT", "Malta");
            countryMap["MH"] = new Country("MH", "Marshall Islands");
            countryMap["MR"] = new Country("MR", "Mauritania");
            countryMap["MU"] = new Country("MU", "Mauritius");
            countryMap["MX"] = new Country("MX", "Mexico");
            countryMap["FM"] = new Country("FM", "Micronesia");
            countryMap["MD"] = new Country("MD", "Moldova");
            countryMap["MC"] = new Country("MC", "Monaco");
            countryMap["MN"] = new Country("MN", "Mongolia");
            countryMap["ME"] = new Country("ME", "Montenegro");
            countryMap["MS"] = new Country("MS", "Montserrat");
            countryMap["MA"] = new Country("MA", "Morocco");
            countryMap["MZ"] = new Country("MZ", "Mozambique");
            countryMap["MM"] = new Country("MM", "Myanmar");
            countryMap["NA"] = new Country("NA", "Namibia");
            countryMap["NR"] = new Country("NR", "Nauru");
            countryMap["NP"] = new Country("NP", "Nepal");
            countryMap["NL"] = new Country("NL", "Netherlands");
            countryMap["AN"] = new Country("AN", "Netherlands Antilles");
            countryMap["NC"] = new Country("NC", "New Caledonia");
            countryMap["NZ"] = new Country("NZ", "New Zealand");
            countryMap["NI"] = new Country("NI", "Nicaragua");
            countryMap["NE"] = new Country("NE", "Niger");
            countryMap["NG"] = new Country("NG", "Nigeria");
            countryMap["NU"] = new Country("NU", "Niue");
            countryMap["NF"] = new Country("NF", "Norfolk Island");
            countryMap["MP"] = new Country("MP", "Northern Mariana Islands");
            countryMap["NO"] = new Country("NO", "Norway");
            countryMap["OM"] = new Country("OM", "Oman");
            countryMap["PK"] = new Country("PK", "Pakistan");
            countryMap["PW"] = new Country("PW", "Palau");
            countryMap["PS"] = new Country("PS", "Palestinian Authority");
            countryMap["PA"] = new Country("PA", "Panama");
            countryMap["PG"] = new Country("PG", "Papua New Guinea");
            countryMap["PY"] = new Country("PY", "Paraguay");
            countryMap["PE"] = new Country("PE", "Peru");
            countryMap["PH"] = new Country("PH", "Philippines");
            countryMap["PN"] = new Country("PN", "Pitcairn Islands");
            countryMap["PL"] = new Country("PL", "Poland");
            countryMap["PT"] = new Country("PT", "Portugal");
            countryMap["PR"] = new Country("PR", "Puerto Rico");
            countryMap["QA"] = new Country("QA", "Qatar");
            countryMap["RW"] = new Country("RW", "Republic of Rwanda");
            countryMap["RE"] = new Country("RE", "Reunion");
            countryMap["RO"] = new Country("RO", "Romania");
            countryMap["RU"] = new Country("RU", "Russia");
            countryMap["WS"] = new Country("WS", "Samoa");
            countryMap["SM"] = new Country("SM", "San Marino");
            countryMap["ST"] = new Country("ST", "Sao Tomé and Príncipe");
            countryMap["SA"] = new Country("SA", "Saudi Arabia");
            countryMap["SN"] = new Country("SN", "Senegal");
            countryMap["RS"] = new Country("RS", "Serbia");
            countryMap["SC"] = new Country("SC", "Seychelles");
            countryMap["SL"] = new Country("SL", "Sierra Leone");
            countryMap["SG"] = new Country("SG", "Singapore");
            countryMap["SK"] = new Country("SK", "Slovakia");
            countryMap["SI"] = new Country("SI", "Slovenia");
            countryMap["SB"] = new Country("SB", "Solomon Islands");
            countryMap["SO"] = new Country("SO", "Somalia");
            countryMap["ZA"] = new Country("ZA", "South Africa");
            countryMap["GS"] = new Country("GS", "South Georgia and the South Sandwich Islands");
            countryMap["ES"] = new Country("ES", "Spain");
            countryMap["LK"] = new Country("LK", "Sri Lanka");
            countryMap["SH"] = new Country("SH", "St. Helena");
            countryMap["KN"] = new Country("KN", "St. Kitts and Nevis");
            countryMap["LC"] = new Country("LC", "St. Lucia");
            countryMap["PM"] = new Country("PM", "St. Pierre and Miquelon");
            countryMap["VC"] = new Country("VC", "St. Vincent and the Grenadines");
            countryMap["SR"] = new Country("SR", "Suriname");
            countryMap["SJ"] = new Country("SJ", "Svalbard and Jan Mayen");
            countryMap["SZ"] = new Country("SZ", "Swaziland");
            countryMap["SE"] = new Country("SE", "Sweden");
            countryMap["CH"] = new Country("CH", "Switzerland");
            countryMap["TW"] = new Country("TW", "Taiwan");
            countryMap["TJ"] = new Country("TJ", "Tajikistan");
            countryMap["TZ"] = new Country("TZ", "Tanzania");
            countryMap["TH"] = new Country("TH", "Thailand");
            countryMap["TP"] = new Country("TP", "Timor-Leste (East Timor)");
            countryMap["TG"] = new Country("TG", "Togo");
            countryMap["TK"] = new Country("TK", "Tokelau");
            countryMap["TO"] = new Country("TO", "Tonga");
            countryMap["TT"] = new Country("TT", "Trinidad and Tobago");
            countryMap["TA"] = new Country("TA", "Tristan da Cunha");
            countryMap["TN"] = new Country("TN", "Tunisia");
            countryMap["TR"] = new Country("TR", "Turkey");
            countryMap["TM"] = new Country("TM", "Turkmenistan");
            countryMap["TC"] = new Country("TC", "Turks and Caicos Islands");
            countryMap["TV"] = new Country("TV", "Tuvalu");
            countryMap["UG"] = new Country("UG", "Uganda");
            countryMap["UA"] = new Country("UA", "Ukraine");
            countryMap["AE"] = new Country("AE", "United Arab Emirates");
            countryMap["UK"] = new Country("UK", "United Kingdom");
            countryMap["US"] = new Country("US", "United States");
            countryMap["UM"] = new Country("UM", "United States Minor Outlying Islands");
            countryMap["UY"] = new Country("UY", "Uruguay");
            countryMap["UZ"] = new Country("UZ", "Uzbekistan");
            countryMap["VU"] = new Country("VU", "Vanuatu");
            countryMap["VA"] = new Country("VA", "Vatican City");
            countryMap["VE"] = new Country("VE", "Venezuela");
            countryMap["VN"] = new Country("VN", "Vietnam");
            countryMap["VI"] = new Country("VI", "Virgin Islands");
            countryMap["VG"] = new Country("VG", "Virgin Islands, British");
            countryMap["WF"] = new Country("WF", "Wallis and Futuna");
            countryMap["YE"] = new Country("YE", "Yemen");
            countryMap["ZM"] = new Country("ZM", "Zambia");
            countryMap["ZW"] = new Country("ZW", "Zimbabwe");

            s_countryMap = new ReadOnlyDictionary<string, Country>(countryMap);
            s_countries = new ReadOnlyICollection<Country>(new List<Country>(countryMap.Values));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        public Country()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        /// <param name="abbreviation">The abbreviation.</param>
        /// <param name="name">The name.</param>
        public Country(string abbreviation, string name)
        {
            Abbreviation = abbreviation;
            Name = name;
        }

        /// <summary>
        /// Gets or sets the abbreviation.
        /// </summary>
        /// <value>The abbreviation.</value>
        public virtual string Abbreviation
        {
            get
            {
                return m_abbreviation;
            }
            set
            {
                m_abbreviation = value;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        /// <summary>
        /// Gets all countries.
        /// </summary>
        /// <value>All countries.</value>
        public static ReadOnlyICollection<Country> AllCountries
        {
            get
            {
                return s_countries;
            }
        }

        /// <summary>
        /// Gets all countries map.
        /// </summary>
        /// <value>All countries map.</value>
        public static ReadOnlyDictionary<string, Country> AllCountriesMap
        {
            get
            {
                return s_countryMap;
            }
        }
    }
}
