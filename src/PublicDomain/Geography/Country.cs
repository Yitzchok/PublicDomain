using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain.Geography
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Country
    {
        /// <summary>
        /// 
        /// </summary>
        public const RegionType DefaultRegionType = RegionType.Region;

        /// <summary>
        /// 
        /// </summary>
        public const CountryOptions DefaultCountryOptions = CountryOptions.None;

        private static ReadOnlyICollection<Country> s_countries;
        private static ReadOnlyDictionary<string, Country> s_countryMap;

        private string m_abbreviation;
        private string m_name;
        private List<Region> m_regions;
        private RegionType m_assumedRegionType;
        private CountryOptions m_options;

        static Country()
        {
            Dictionary<string, Country> countryMap = new Dictionary<string, Country>();

            countryMap["AF"] = new Country("AF", "Afghanistan", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Herat"), new Region("Kabul"), new Region("Kandahar"), new Region("Mazar-e Sharif"));
            countryMap["AL"] = new Country("AL", "Albania");
            countryMap["DZ"] = new Country("DZ", "Algeria");
            countryMap["AS"] = new Country("AS", "American Samoa");
            countryMap["AD"] = new Country("AD", "Andorra");
            countryMap["AO"] = new Country("AO", "Angola");
            countryMap["AI"] = new Country("AI", "Anguilla");
            countryMap["AQ"] = new Country("AQ", "Antarctica");
            countryMap["AG"] = new Country("AG", "Antigua and Barbuda");
            countryMap["AR"] = new Country("AR", "Argentina", RegionType.Province, CountryOptions.PostalCode, new Region("Buenos Aires"), new Region("Capital Federal"), new Region("Catamarca"), new Region("Chaco"), new Region("Chubut"), new Region("Cordoba"), new Region("Corrientes"), new Region("Entre Rios"), new Region("Jujuy"), new Region("La Pampa"), new Region("La Rioja"), new Region("Mendoza"), new Region("Misiones"), new Region("Neuquen"), new Region("Rio Negro"), new Region("Salta"), new Region("San Juan"), new Region("San Luis"), new Region("Santa Cruz"), new Region("Santa Fe"), new Region("Santiago del Estero"), new Region("Tierra del Fuego Antartida e Islas del Atlantico Sur"), new Region("Tucuman"));
            countryMap["AM"] = new Country("AM", "Armenia");
            countryMap["AW"] = new Country("AW", "Aruba");
            countryMap["AC"] = new Country("AC", "Ascension Island");
            countryMap["AU"] = new Country("AU", "Australia", RegionType.StateTerritory, CountryOptions.PostalCode, new Region("Australian Capital Territory"), new Region("New South Wales"), new Region("Northern Territory"), new Region("Queensland"), new Region("South Australia"), new Region("Tasmania"), new Region("Victoria"), new Region("Western Australia"));
            countryMap["AT"] = new Country("AT", "Austria", CountryOptions.PostalCode);
            countryMap["AZ"] = new Country("AZ", "Azerbaijan");
            countryMap["BS"] = new Country("BS", "Bahamas");
            countryMap["BH"] = new Country("BH", "Bahrain");
            countryMap["BD"] = new Country("BD", "Bangladesh", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Chittagong"), new Region("Dhaka"), new Region("Khulna"));
            countryMap["BB"] = new Country("BB", "Barbados");
            countryMap["BY"] = new Country("BY", "Belarus", RegionType.Province, new Region("Brest"), new Region("Homyelrskaya"), new Region("Hrodzyenskaya"), new Region("Mahilyowskaya"), new Region("Minsk"), new Region("Vitsyebskaya"));
            countryMap["BE"] = new Country("BE", "Belgium", CountryOptions.PostalCode);
            countryMap["BZ"] = new Country("BZ", "Belize");
            countryMap["BJ"] = new Country("BJ", "Benin");
            countryMap["BM"] = new Country("BM", "Bermuda");
            countryMap["BT"] = new Country("BT", "Bhutan");
            countryMap["BO"] = new Country("BO", "Bolivia");
            countryMap["BA"] = new Country("BA", "Bosnia and Herzegovina");
            countryMap["BW"] = new Country("BW", "Botswana");
            countryMap["BV"] = new Country("BV", "Bouvet Island");
            countryMap["BR"] = new Country("BR", "Brazil", RegionType.StateTerritory, CountryOptions.PostalCode, new Region("Acre"), new Region("Alagoas"), new Region("Amapa"), new Region("Amazonas"), new Region("Bahia"), new Region("Ceara"), new Region("Distrito Federal"), new Region("Espirito Santo"), new Region("Goias"), new Region("Maranhao"), new Region("Mato Grosso"), new Region("Mato Grosso do Sul"), new Region("Minas Gerais"), new Region("Para"), new Region("Paraiba"), new Region("Parana"), new Region("Pernambuco"), new Region("Piaui"), new Region("Rio de Janeiro"), new Region("Rio Grande do Norte"), new Region("Rio Grande do Sul"), new Region("Rondonia"), new Region("Roraima"), new Region("Santa Catarina"), new Region("Sao Paulo"), new Region("Sergipe"), new Region("Tocantins"));
            countryMap["IO"] = new Country("IO", "British Indian Ocean Territory");
            countryMap["BN"] = new Country("BN", "Brunei");
            countryMap["BG"] = new Country("BG", "Bulgaria", RegionType.Province, new Region("Burgas"), new Region("Grad Sofiya"), new Region("Khaskovo"), new Region("Lovech"), new Region("Montana"), new Region("Plovdiv"), new Region("Ruse"), new Region("Sofiya"), new Region("Varna"));
            countryMap["BF"] = new Country("BF", "Burkina Faso");
            countryMap["BI"] = new Country("BI", "Burundi");
            countryMap["KH"] = new Country("KH", "Cambodia");
            countryMap["CM"] = new Country("CM", "Cameroon");
            countryMap["CA"] = new Country("CA", "Canada", RegionType.Province, CountryOptions.PostalCode, new Region("Alberta"), new Region("British Columbia"), new Region("Manitoba"), new Region("New Brunswick"), new Region("Newfoundland"), new Region("Northwest Territories"), new Region("Nova Scotia"), new Region("Nunavut"), new Region("Ontario"), new Region("Prince Edward Island"), new Region("Quebec"), new Region("Saskatchewan"), new Region("Yukon Territory"));
            countryMap["CV"] = new Country("CV", "Cape Verde");
            countryMap["KY"] = new Country("KY", "Cayman Islands");
            countryMap["CF"] = new Country("CF", "Central African Republic");
            countryMap["TD"] = new Country("TD", "Chad");
            countryMap["CL"] = new Country("CL", "Chile", RegionType.StateTerritory, CountryOptions.PostalCode, new Region("Libertador"), new Region("Magallanes y Antartica Chilena"), new Region("Metropolitana de Santiago"), new Region("Region de Aisen del General Carlos Ibanez del Campo"), new Region("Region de Antofagasta"), new Region("Region de Atacama"), new Region("Region de Coquimbo"), new Region("Region de la Araucania"), new Region("Region de los Lagos"), new Region("Region de Tarapaca"), new Region("Region de Valparaiso"), new Region("Region del Biobio"), new Region("Region del Maule"));
            countryMap["CN"] = new Country("CN", "China", RegionType.Province, CountryOptions.PostalCode, new Region("Anhui"), new Region("Beijing"), new Region("Chongqing"), new Region("Fujian"), new Region("Gansu"), new Region("Guangdong"), new Region("Guangxi"), new Region("Guizhou"), new Region("Hainan"), new Region("Hebei"), new Region("Heilongjiang"), new Region("Henan"), new Region("Hubei"), new Region("Hunan"), new Region("Jiangsu"), new Region("Jianxi"), new Region("Jilin"), new Region("Liaoning"), new Region("Nei Menggu"), new Region("Ningxia"), new Region("Qinghai"), new Region("Shaanxi"), new Region("Shandong"), new Region("Shanghai"), new Region("Shanxi"), new Region("Sichuan"), new Region("Tianjin"), new Region("Xinjiang Uygur"), new Region("Xizang"), new Region("Yunnan"), new Region("Zhejiang"));
            countryMap["CX"] = new Country("CX", "Christmas Island");
            countryMap["CC"] = new Country("CC", "Cocos (Keeling) Islands");
            countryMap["CO"] = new Country("CO", "Colombia", RegionType.StateTerritory, new Region("Amazonas"), new Region("Antioquia"), new Region("Arauca"), new Region("Atlantico"), new Region("Bogota"), new Region("Bolivar"), new Region("Boyaca"), new Region("Caldas"), new Region("Caqueta"), new Region("Casanare"), new Region("Cauca"), new Region("Cesar"), new Region("Choco"), new Region("Cordoba"), new Region("Cundinamarca"), new Region("Distrito Capital"), new Region("Guainia"), new Region("Guaviare"), new Region("Huila"), new Region("La Guajira"), new Region("Magdalena"), new Region("Meta"), new Region("Narino"), new Region("Norte de Santander"), new Region("Putumayo"), new Region("Quindio"), new Region("Risaralda"), new Region("San Andres y Providencia"), new Region("Santander"), new Region("Sucre"), new Region("Tolima"), new Region("Valle del Cauca"), new Region("Vaupes"), new Region("Vichada"));
            countryMap["KM"] = new Country("KM", "Comoros");
            countryMap["CG"] = new Country("CG", "Congo");
            countryMap["CD"] = new Country("CD", "Congo (DRC)");
            countryMap["CK"] = new Country("CK", "Cook Islands");
            countryMap["CR"] = new Country("CR", "Costa Rica", RegionType.Province, new Region("Alajuela"), new Region("Cartago"), new Region("Guanacaste"), new Region("Heredia"), new Region("Limon"), new Region("Puntarenas"), new Region("San Jose"));
            countryMap["CI"] = new Country("CI", "Côte d'Ivoire");
            countryMap["HR"] = new Country("HR", "Croatia");
            countryMap["CY"] = new Country("CY", "Cyprus");
            countryMap["CZ"] = new Country("CZ", "Czech Republic", RegionType.Province, CountryOptions.PostalCode, new Region("Jihocesky Kraj"), new Region("Jihormoravsky Kraj"), new Region("Karlovarsky Kraj"), new Region("Kralovehradecky Kraj"), new Region("Liberecky Kraj"), new Region("Moravskoslezsky Kraj"), new Region("Olomoucky Kraj"), new Region("Pardubicky Kraj"), new Region("Plzensky Kraj"), new Region("Praha"), new Region("Stredocesky Kraj"), new Region("Ustecky Kraj"), new Region("Vysocina"), new Region("Zlinsky Kraj"));
            countryMap["DK"] = new Country("DK", "Denmark", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Arhus"), new Region("Bornholm"), new Region("Frederiksborg"), new Region("Fyn"), new Region("Kobenhavn"), new Region("Nordjylland"), new Region("Ribe"), new Region("Ringkobing"), new Region("Roskilde"), new Region("Sonderjylland"), new Region("Staden Kobenhavn"), new Region("Storstrom"), new Region("Vejle"), new Region("Vestsjalland"), new Region("Viborg"));
            countryMap["DJ"] = new Country("DJ", "Djibouti");
            countryMap["DM"] = new Country("DM", "Dominica");
            countryMap["DO"] = new Country("DO", "Dominican Republic");
            countryMap["EC"] = new Country("EC", "Ecuador", RegionType.Province, new Region("Azuay"), new Region("Bolivar"), new Region("Canar"), new Region("Carchi"), new Region("Chimborazo"), new Region("Cotopaxi"), new Region("El Oro"), new Region("Esmeraldas"), new Region("Galapagos Islands"), new Region("Guayas"), new Region("Imbabura"), new Region("Loja"), new Region("Los Rios"), new Region("Manabi"), new Region("Morona-Santiago"), new Region("Napo"), new Region("Pastaza"), new Region("Pichincha"), new Region("Sucumbios"), new Region("Tungurahua"), new Region("Zamora Chincipe"));
            countryMap["EG"] = new Country("EG", "Egypt", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Alexandria"), new Region("Aswan"), new Region("Cairo"), new Region("Giza"), new Region("Shubra al Khaymah"));
            countryMap["SV"] = new Country("SV", "El Salvador");
            countryMap["GQ"] = new Country("GQ", "Equatorial Guinea");
            countryMap["ER"] = new Country("ER", "Eritrea");
            countryMap["EE"] = new Country("EE", "Estonia", RegionType.Province, new Region("Harju"), new Region("Hiiu"), new Region("Ida-Viru"), new Region("Jarva"), new Region("Jogeva"), new Region("Laane"), new Region("Laane-Viru"), new Region("Narva"), new Region("Parnu"), new Region("Polva"), new Region("Rapla"), new Region("Saare"), new Region("Tartu"), new Region("Valga"), new Region("Viljandi"), new Region("Voru"));
            countryMap["ET"] = new Country("ET", "Ethiopia");
            countryMap["FK"] = new Country("FK", "Falkland Islands (Islas Malvinas)");
            countryMap["FO"] = new Country("FO", "Faroe Islands");
            countryMap["FJ"] = new Country("FJ", "Fiji Islands");
            countryMap["FI"] = new Country("FI", "Finland", CountryOptions.PostalCode);
            countryMap["FR"] = new Country("FR", "France", CountryOptions.PostalCode);
            countryMap["PF"] = new Country("PF", "French Polynesia", RegionType.CityRegion, new Region("Alur Setar"), new Region("George Town"), new Region("Johor Baharu"), new Region("Kelang"), new Region("Kota Baharu"), new Region("Kota Kinabalu"), new Region("Kuala Lumpur"), new Region("Kuala Terengganu"), new Region("Kuantan"), new Region("Kuching"));
            countryMap["TF"] = new Country("TF", "French Southern and Antarctic Lands");
            countryMap["GA"] = new Country("GA", "Gabon");
            countryMap["GM"] = new Country("GM", "Gambia, The");
            countryMap["GE"] = new Country("GE", "Georgia");
            countryMap["DE"] = new Country("DE", "Germany", CountryOptions.PostalCode);
            countryMap["GH"] = new Country("GH", "Ghana");
            countryMap["GI"] = new Country("GI", "Gibraltar");
            countryMap["GR"] = new Country("GR", "Greece", CountryOptions.PostalCode);
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
            countryMap["HU"] = new Country("HU", "Hungary", RegionType.Province, new Region("Bacs-Kiskun"), new Region("Baranya"), new Region("Bekes"), new Region("Borsod-Abauj-Zemplen"), new Region("Budapest"), new Region("Csongrad"), new Region("Fejer"), new Region("Gyor-Moson-Sopron"), new Region("Hajdu-Bihar"), new Region("Heves"), new Region("Jasz-Nagykun-Szolnok"), new Region("Komarom-Esztergom"), new Region("Nograd"), new Region("Pest"), new Region("Somogy"), new Region("Szabolcs-Szatmar-Bereg"), new Region("Tolna"), new Region("Vas"), new Region("Veszprem"), new Region("Zala"));
            countryMap["IS"] = new Country("IS", "Iceland");
            countryMap["IN"] = new Country("IN", "India", RegionType.StateTerritory, CountryOptions.PostalCode, new Region("Andaman and Nicobar Islands"), new Region("Andhra Pradesh"), new Region("Arunachal Pradesh"), new Region("Assam"), new Region("Bihar"), new Region("Chandigarh"), new Region("Chhattisgarh"), new Region("Dadra and Nagar Haveli"), new Region("Daman and Diu"), new Region("Delhi"), new Region("Goa"), new Region("Gujarat"), new Region("Haryana"), new Region("Himachal Pradesh"), new Region("Jammu and Kashmir"), new Region("Jharkhand"), new Region("Karnataka"), new Region("Kerala"), new Region("Lakshadweep"), new Region("Madhya Pradesh"), new Region("Maharashtra"), new Region("Manipur"), new Region("Meghalaya"), new Region("Mizoram"), new Region("Nagaland"), new Region("Orissa"), new Region("Puduchery"), new Region("Punjab"), new Region("Rajasthan"), new Region("Sikkim"), new Region("Tamil Nadu"), new Region("Tripura"), new Region("Uttar Pradesh"), new Region("Uttaranchal"), new Region("West Bengal"));
            countryMap["ID"] = new Country("ID", "Indonesia", RegionType.State, new Region("Aceh"), new Region("Bali"), new Region("Bengkulu"), new Region("Daereh Istimewa Yogyakarta"), new Region("Daereh Tingkat I Kalimantan Barat"), new Region("Irian Jaya"), new Region("Jakarta Raya"), new Region("Kalimantan Tengah"), new Region("Kalimantan Timur"), new Region("Nusa Tenggara Barat"), new Region("Nusa Tenggara Timur"), new Region("Propinsi Jambi"), new Region("Propinsi Jawa Barat"), new Region("Propinsi Jawa Tengah"), new Region("Propinsi Jawa Timur"), new Region("Propinsi Kalimantan Selatan"), new Region("Propinsi Lampung"), new Region("Propinsi Maluku"), new Region("Propinsi Sulawesi Selatan"), new Region("Propinsi Sulawesi Utara"), new Region("Propinsi Sumatera Selatan"), new Region("Propinsi Sumatera Utara"), new Region("Riau"), new Region("Sulawesi Tengah"), new Region("Sulawesi Tenggara"), new Region("Sumatera Barat"));
            countryMap["IQ"] = new Country("IQ", "Iraq");
            countryMap["IE"] = new Country("IE", "Ireland", RegionType.County, new Region("Carlow"), new Region("Cavan"), new Region("Clare"), new Region("Cork"), new Region("Donegal"), new Region("Dublin"), new Region("Galway"), new Region("Kerry"), new Region("Kildare"), new Region("Kilkenny"), new Region("Laois"), new Region("Leitrim"), new Region("Limerick"), new Region("Longford"), new Region("Louth"), new Region("Mayo"), new Region("Meath"), new Region("Monaghan"), new Region("Offaly"), new Region("Roscommon"), new Region("Sligo"), new Region("Tipperary"), new Region("Waterford"), new Region("Westmeath"), new Region("Wexford"), new Region("Wicklow"));
            countryMap["IM"] = new Country("IM", "Isle of Man");
            countryMap["IL"] = new Country("IL", "Israel", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Ashdod"), new Region("Bat Yam"), new Region("Beersheba"), new Region("Haifa"), new Region("Holon"), new Region("Jerusalem"), new Region("Netanya"), new Region("Tel Aviv-Yafo"));
            countryMap["IT"] = new Country("IT", "Italy", RegionType.Province, CountryOptions.PostalCode, new Region("Agrigento"), new Region("Alessandria"), new Region("Ancona"), new Region("Aosta"), new Region("Arezzo"), new Region("Ascoli Piceno"), new Region("Asti"), new Region("Avellino"), new Region("Bari"), new Region("Belluno"), new Region("Benevento"), new Region("Bergamo"), new Region("Biella"), new Region("Bologna"), new Region("Bolzano-Bozen"), new Region("Brescia"), new Region("Brindisi"), new Region("Cagliari"), new Region("Caltanissetta"), new Region("Campobasso"), new Region("Caserta"), new Region("Catania"), new Region("Catanzaro"), new Region("Chieti"), new Region("Como"), new Region("Cosenza"), new Region("Cremona"), new Region("Crotone"), new Region("Cuneo"), new Region("Enna"), new Region("Ferrara"), new Region("Firenze"), new Region("Foggia"), new Region("Forli-Cesena"), new Region("Frosinone"), new Region("Genova"), new Region("Gorizia"), new Region("Grosseto"), new Region("Imperia"), new Region("Isernia"), new Region("La Spezia"), new Region("L'Aquila"), new Region("Latina"), new Region("Lecce"), new Region("Livorno"), new Region("Lodi"), new Region("Lucca"), new Region("Macerata"), new Region("Mantova"), new Region("Massa Carrara"), new Region("Matera"), new Region("Milano"), new Region("Modena"), new Region("Musina"), new Region("Napoli"), new Region("Novara"), new Region("Nuoro"), new Region("Oristano"), new Region("Padova"), new Region("Palermo"), new Region("Parma"), new Region("Pavia"), new Region("Perugia"), new Region("Pesaro e Urbino"), new Region("Pescara"), new Region("Piacenza"), new Region("Pisa"), new Region("Pistoia"), new Region("Pordenone"), new Region("Protenza"), new Region("Prato"), new Region("Ragusa"), new Region("Ravenna"), new Region("Reggio di Calabria"), new Region("Reggio nella Emilia"), new Region("Rieti"), new Region("Rimini"), new Region("Roma"), new Region("Rovigo"), new Region("Salerno"), new Region("Sassari"), new Region("Savona"), new Region("Siena"), new Region("Siracusa"), new Region("Sondrio"), new Region("Taranto"), new Region("Teramo"), new Region("Terni"), new Region("Torino"), new Region("Trapani"), new Region("Trento"), new Region("Treviso"), new Region("Trieste"), new Region("Udine"), new Region("Varese"), new Region("Venezia"), new Region("Verbano-Cusio-Ossola"), new Region("Vercelli"), new Region("Verona"), new Region("Vibo Valentia"), new Region("Vicenza"), new Region("Viterbo"));
            countryMap["JM"] = new Country("JM", "Jamaica");
            countryMap["JP"] = new Country("JP", "Japan", RegionType.Prefecture, CountryOptions.PostalCode, new Region("Aichi-ken"), new Region("Akita-ken"), new Region("Aomori-ken"), new Region("Chiba-ken"), new Region("Ehime-ken"), new Region("Fukui-ken"), new Region("Fukushima-ken"), new Region("Gifu-ken"), new Region("Gunma-ken"), new Region("Hiroshima-ken"), new Region("Hokkaido"), new Region("Hyogo-ken"), new Region("Ibaraki-ken"), new Region("Ishikawa-ken"), new Region("Iwate-ken"), new Region("Kagawa-ken"), new Region("Kagoshima-ken"), new Region("Kanagawa-ken"), new Region("Kochi-ken"), new Region("Kumamoto-ken"), new Region("Fukuoka-ken"), new Region("Kyoto-fu"), new Region("Mie-ken"), new Region("Miyagi-ken"), new Region("Miyazaki-ken"), new Region("Nagano-ken"), new Region("Nagasaki-ken"), new Region("Nara-ken"), new Region("Niigata-ken"), new Region("Oita-ken"), new Region("Okayam-ken"), new Region("Okinawa-ken"), new Region("Osaka-fu"), new Region("Saga-ken"), new Region("Saitama-ken"), new Region("Shiga-ken"), new Region("Shimane-ken"), new Region("Shizuoaka-ken"), new Region("Tochigi-ken"), new Region("Tokushima-ken"), new Region("Tokyo-to"), new Region("Tottori-ken"), new Region("Toyama-ken"), new Region("Wakayam-ken"), new Region("Yamagata-ken"), new Region("Yamaguchi-ken"), new Region("Yamanashi-ken"));
            countryMap["JE"] = new Country("JE", "Jersey");
            countryMap["JO"] = new Country("JO", "Jordan");
            countryMap["KZ"] = new Country("KZ", "Kazakhstan");
            countryMap["KE"] = new Country("KE", "Kenya");
            countryMap["KI"] = new Country("KI", "Kiribati");
            countryMap["KR"] = new Country("KR", "Korea", RegionType.Province, CountryOptions.PostalCode, new Region("Busan"), new Region("Chungcheongbuk-do"), new Region("Chungcheongnam-do"), new Region("Daegu"), new Region("Daejeon"), new Region("Gangwon-do"), new Region("Gwangju"), new Region("Gyeonggi-do"), new Region("Gyeongsangbuk-do"), new Region("Gyeongsangnam-do"), new Region("Incheon"), new Region("Jeju-do"), new Region("Jeollabuk-do"), new Region("Jeollanam-do"), new Region("Seoul"), new Region("Ulsan"));
            countryMap["KW"] = new Country("KW", "Kuwait");
            countryMap["KG"] = new Country("KG", "Kyrgyzstan");
            countryMap["LA"] = new Country("LA", "Laos");
            countryMap["LV"] = new Country("LV", "Latvia", RegionType.Province, new Region("Aizkraukles"), new Region("Aluksnes"), new Region("Balvu"), new Region("Bauskas"), new Region("Cesu"), new Region("Daugavpils"), new Region("Dobeles"), new Region("Gulbenes"), new Region("Jekabpils"), new Region("Jelgavas"), new Region("Kraslavas"), new Region("Kuldigas"), new Region("Liepajas"), new Region("Limbazu"), new Region("Ludzas"), new Region("Madonas"), new Region("Ogres"), new Region("Preiju"), new Region("Rezeknes"), new Region("Rigas"), new Region("Saldus"), new Region("Talsu"), new Region("Tukuma"), new Region("Valkas"), new Region("Valmieras"), new Region("Ventspils"));
            countryMap["LB"] = new Country("LB", "Lebanon");
            countryMap["LS"] = new Country("LS", "Lesotho");
            countryMap["LR"] = new Country("LR", "Liberia");
            countryMap["LY"] = new Country("LY", "Libya");
            countryMap["LI"] = new Country("LI", "Liechtenstein");
            countryMap["LT"] = new Country("LT", "Lithuania", RegionType.Province, new Region("Akmenes"), new Region("Alytaus"), new Region("Alytus"), new Region("Anyksciu"), new Region("Birstonas"), new Region("Birzau"), new Region("Druskininkai"), new Region("Ignalinos"), new Region("Jonavos"), new Region("Joniskio"), new Region("Jurbarko"), new Region("Kaisiadoriu"), new Region("Kaunas"), new Region("Kauno"), new Region("Kedainiu"), new Region("Kelmes"), new Region("Klaipeda"), new Region("Klaipedos"), new Region("Kretingos"), new Region("Kupiskio"), new Region("Lazdiju"), new Region("Marijampole"), new Region("Mazeikiu"), new Region("Moletu"), new Region("Neringa"), new Region("Pakruojo"), new Region("Palanga"), new Region("Panevezio"), new Region("Panevezys"), new Region("Pasvalio"), new Region("Plunges"), new Region("Prienu"), new Region("Radviliskio"), new Region("Raseiniu"), new Region("Rokiskio"), new Region("Sakiu"), new Region("Salcininku"), new Region("Siauliai"), new Region("Silales"), new Region("Silutes"), new Region("Sirvintu"), new Region("Skuodo"), new Region("Svencioniu"), new Region("Taurages"), new Region("Telsiu"), new Region("Traku"), new Region("Ukmerges"), new Region("Utenos"), new Region("Varenos"), new Region("Vilkaviskio"), new Region("Vilniaus"), new Region("Vilnius"), new Region("Zarasu"));
            countryMap["LU"] = new Country("LU", "Luxembourg", CountryOptions.PostalCode);
            countryMap["MO"] = new Country("MO", "Macao SAR");
            countryMap["MK"] = new Country("MK", "Macedonia");
//            countryMap["MK"] = new Country("MK", "Macedonia, Former Yugoslav Republic of");
            countryMap["MG"] = new Country("MG", "Madagascar");
            countryMap["MW"] = new Country("MW", "Malawi");
            countryMap["MY"] = new Country("MY", "Malaysia", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Johor"), new Region("Kedah"), new Region("Kelantan"), new Region("Labuan"), new Region("Melaka"), new Region("Negeri Sembilan"), new Region("Pahang"), new Region("Perak"), new Region("Perlis"), new Region("Pulau Pinang"), new Region("Sabah"), new Region("Sarawak"), new Region("Selangor"), new Region("Terengganu"), new Region("Wilayah Persekutuan"));
            countryMap["MV"] = new Country("MV", "Maldives");
            countryMap["ML"] = new Country("ML", "Mali");
            countryMap["MT"] = new Country("MT", "Malta");
            countryMap["MH"] = new Country("MH", "Marshall Islands");
            countryMap["MR"] = new Country("MR", "Mauritania");
            countryMap["MU"] = new Country("MU", "Mauritius");
            countryMap["MX"] = new Country("MX", "Mexico", RegionType.State, CountryOptions.PostalCode, new Region("Aguascalientes"), new Region("Baja California"), new Region("Baja California Sur"), new Region("Campeche"), new Region("Chiapas"), new Region("Chihuahua"), new Region("Coahuila de Zaragoza"), new Region("Colima"), new Region("Distrito Federal"), new Region("Durango"), new Region("Guanajuato"), new Region("Guerrero"), new Region("Hidalgo"), new Region("Jalisco"), new Region("Mexico"), new Region("Michoacan de Ocampa"), new Region("Morelos"), new Region("Nayarit"), new Region("Nuevo Leon"), new Region("Oaxaca"), new Region("Puebla"), new Region("Queretaro de Arteaga"), new Region("Quintana Roo"), new Region("San Luis Potosi"), new Region("Sinaloa"), new Region("Sonora"), new Region("Tabasco"), new Region("Tamaulipas"), new Region("Tlaxcala"), new Region("Veracruz"), new Region("Yucatan"), new Region("Zacatecas"));
            countryMap["FM"] = new Country("FM", "Micronesia");
            countryMap["MD"] = new Country("MD", "Moldova");
            countryMap["MC"] = new Country("MC", "Monaco");
            countryMap["MN"] = new Country("MN", "Mongolia");
            countryMap["ME"] = new Country("ME", "Montenegro", RegionType.CityRegion, new Region("Podgorica"));
            countryMap["MS"] = new Country("MS", "Montserrat");
            countryMap["MA"] = new Country("MA", "Morocco", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Casablanca"), new Region("Fes"), new Region("Marrakech"), new Region("Meknes"), new Region("Oujda"), new Region("Rabat"), new Region("Tangier"), new Region("Tetouan"));
            countryMap["MZ"] = new Country("MZ", "Mozambique");
            countryMap["MM"] = new Country("MM", "Myanmar");
            countryMap["NA"] = new Country("NA", "Namibia");
            countryMap["NR"] = new Country("NR", "Nauru");
            countryMap["NP"] = new Country("NP", "Nepal");
            countryMap["NL"] = new Country("NL", "Netherlands", CountryOptions.PostalCode);
            countryMap["AN"] = new Country("AN", "Netherlands Antilles");
            countryMap["NC"] = new Country("NC", "New Caledonia");
            countryMap["NZ"] = new Country("NZ", "New Zealand", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Auckland"), new Region("Bay of Plenty"), new Region("Canterbury"), new Region("Gisborne"), new Region("Hawke's Bay"), new Region("Manawatu-Wanganui"), new Region("Marlborough"), new Region("Nelson"), new Region("Northland"), new Region("Otago"), new Region("Southland"), new Region("Taranaki"), new Region("Tasman"), new Region("Waikato"), new Region("Wellington"), new Region("West Coast"));
            countryMap["NI"] = new Country("NI", "Nicaragua");
            countryMap["NE"] = new Country("NE", "Niger");
            countryMap["NG"] = new Country("NG", "Nigeria", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Abuja"), new Region("Ibadan"), new Region("Kano"), new Region("Lagos"), new Region("Ogbomosho"));
            countryMap["NU"] = new Country("NU", "Niue");
            countryMap["NF"] = new Country("NF", "Norfolk Island");
            countryMap["MP"] = new Country("MP", "Northern Mariana Islands");
            countryMap["NO"] = new Country("NO", "Norway", CountryOptions.PostalCode);
            countryMap["OM"] = new Country("OM", "Oman");
            countryMap["PK"] = new Country("PK", "Pakistan", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Faisalabad"), new Region("Gujranwala"), new Region("Hyderabad"), new Region("Islamabad"), new Region("Karachi"), new Region("Lahore"), new Region("Mirpur"), new Region("Multan"), new Region("Muzaffarabad"), new Region("Peshawar"), new Region("Rawalpindi"));
            countryMap["PW"] = new Country("PW", "Palau");
            countryMap["PS"] = new Country("PS", "Palestinian Authority");
            countryMap["PA"] = new Country("PA", "Panama");
            countryMap["PG"] = new Country("PG", "Papua New Guinea");
            countryMap["PY"] = new Country("PY", "Paraguay");
            countryMap["PE"] = new Country("PE", "Peru", RegionType.Province, CountryOptions.PostalCode, new Region("Amazonas"), new Region("Ancash"), new Region("Apurimac"), new Region("Arequipa"), new Region("Ayacucho"), new Region("Cajamarca"), new Region("Callao"), new Region("Cusco"), new Region("Huancavelica"), new Region("Huanuco"), new Region("Ica"), new Region("Junin"), new Region("La Libertad"), new Region("Lambayeque"), new Region("Lima"), new Region("Loreto"), new Region("Madre de Dios"), new Region("Moquegua"), new Region("Pasco"), new Region("Piura"), new Region("Puno"), new Region("San Martin"), new Region("Tacna"), new Region("Tumbes"), new Region("Ucayali"));
            countryMap["PH"] = new Country("PH", "Philippines", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Caloocan"), new Region("Cebu"), new Region("Davao"), new Region("Manila"));
            countryMap["PN"] = new Country("PN", "Pitcairn Islands");
            countryMap["PL"] = new Country("PL", "Poland", RegionType.Province, CountryOptions.PostalCode, new Region("Dolnoslaskie"), new Region("Kujawsko-Pomorskie"), new Region("Lodzkie"), new Region("Lubelskie"), new Region("Lubuskie"), new Region("Malopolskie"), new Region("Mazowieckie"), new Region("Opolskie"), new Region("Podkarpackie"), new Region("Podlaskie"), new Region("Pomoroskie"), new Region("Slaskie"), new Region("Swietokrzyskie"), new Region("Warminkso-Mazurskie"), new Region("Wielkopolskie"), new Region("Zachodniopomorskie"));
            countryMap["PT"] = new Country("PT", "Portugal", CountryOptions.PostalCode);
            countryMap["PR"] = new Country("PR", "Puerto Rico");
            countryMap["QA"] = new Country("QA", "Qatar");
            countryMap["RW"] = new Country("RW", "Republic of Rwanda");
            countryMap["RE"] = new Country("RE", "Reunion");
            countryMap["RO"] = new Country("RO", "Romania", RegionType.County, new Region("Alba"), new Region("Arad"), new Region("Arges"), new Region("Bacau"), new Region("Bihor"), new Region("Bistrita-Nasaud"), new Region("Botosani"), new Region("Braila"), new Region("Brasov"), new Region("Bucuresti"), new Region("Buzau"), new Region("Calarasi"), new Region("Caras-Severin"), new Region("Cluj"), new Region("Constanta"), new Region("Covasna"), new Region("Dimbovita"), new Region("Dolj"), new Region("Galati"), new Region("Giurgiu"), new Region("Gorj"), new Region("Harghita"), new Region("Hunedoara"), new Region("Ialomita"), new Region("Iasi"), new Region("Maramures"), new Region("Mehedinti"), new Region("Mures"), new Region("Neamt"), new Region("Olt"), new Region("Prahova"), new Region("Salaj"), new Region("Satu Mare"), new Region("Sibiu"), new Region("Suceava"), new Region("Teleorman"), new Region("Timis"), new Region("Tulcea"), new Region("Vaslui"), new Region("Vilcea"), new Region("Vrancea"));
            countryMap["RU"] = new Country("RU", "Russia", RegionType.AdministrativeDivision, new Region("Adygea"), new Region("Agin-Buryat"), new Region("Altai"), new Region("Amur"), new Region("Arkhangelr"), new Region("Astrakhanr"), new Region("Bashkortostan"), new Region("Belgograd"), new Region("Bryansk"), new Region("Buryatia"), new Region("Chechnya"), new Region("Chelyabinsk"), new Region("Chita"), new Region("Chukot"), new Region("Chuvashia"), new Region("Dagestan"), new Region("Ekaterinburg"), new Region("Evenki"), new Region("Gorno-Altay"), new Region("Ingushetia"), new Region("Irkutsk"), new Region("Ivanovo"), new Region("Kabardino-Balkaria"), new Region("Kaliningrad"), new Region("Kalmykia"), new Region("Kaluga"), new Region("Kamchatka"), new Region("Karachay-Cherkessia"), new Region("Karelia"), new Region("Kemerovo"), new Region("Khabarovsk"), new Region("Khakassia"), new Region("Khanty-Mansi"), new Region("Kirov"), new Region("Komi"), new Region("Komi-Permyak"), new Region("Koryak"), new Region("Kostroma"), new Region("Krasnodar"), new Region("Krasnoyarsk"), new Region("Kurgan"), new Region("Kursk"), new Region("Lipetsk"), new Region("Magadan"), new Region("Mari El"), new Region("Mordovia"), new Region("Moscow"), new Region("Moscow City"), new Region("Murmansk"), new Region("Nenets"), new Region("Nizhniy Novgorod"), new Region("North Ossetia"), new Region("Novgorod"), new Region("Novosibirsk"), new Region("Omsk"), new Region("Orel"), new Region("Orenburg"), new Region("Penza"), new Region("Perm"), new Region("Primorskiy"), new Region("Pskov"), new Region("Rostov"), new Region("Ryazanr"), new Region("Sakhalin"), new Region("Samara"), new Region("Saratov"), new Region("Smolensk"), new Region("St. Petersburg"), new Region("Stavropol"), new Region("Tambov"), new Region("Tatarstan"), new Region("Taymyr"), new Region("Tomsk"), new Region("Tula"), new Region("Tuva"), new Region("Tverr"), new Region("Tyumenr"), new Region("Udmurtia"), new Region("Ulryanovsk"), new Region("Ust-Ordyn-Buryat"), new Region("Vladimir"), new Region("Vologograd"), new Region("Vologda"), new Region("Voronezh"), new Region("Yakutia"), new Region("Yamalo-Nenets"), new Region("Yaroslavlr"), new Region("Yevreyskaya Autonomnaya Oblastr"));
            countryMap["WS"] = new Country("WS", "Samoa");
            countryMap["SM"] = new Country("SM", "San Marino");
            countryMap["ST"] = new Country("ST", "Sao Tomé and Príncipe");
            countryMap["SA"] = new Country("SA", "Saudi Arabia", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Ad Dammam"), new Region("Al Hufuf"), new Region("Ar Riyad"), new Region("At Tarif"), new Region("Jiddah"), new Region("Mecca"), new Region("Medina"), new Region("Shagrar"));
            countryMap["SN"] = new Country("SN", "Senegal");
            countryMap["RS"] = new Country("RS", "Serbia", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Belgrade"), new Region("Beograd"), new Region("Kragujevac"), new Region("Nis"), new Region("Novi Sad"), new Region("Pristina"), new Region("Subotica"), new Region("Zemun"));
            countryMap["SC"] = new Country("SC", "Seychelles");
            countryMap["SL"] = new Country("SL", "Sierra Leone");
            countryMap["SG"] = new Country("SG", "Singapore", CountryOptions.PostalCode);
            countryMap["SK"] = new Country("SK", "Slovakia");
            countryMap["SI"] = new Country("SI", "Slovenia");
            countryMap["SB"] = new Country("SB", "Solomon Islands");
            countryMap["SO"] = new Country("SO", "Somalia");
            countryMap["ZA"] = new Country("ZA", "South Africa", RegionType.Province, new Region("Eastern Cape"), new Region("Free State"), new Region("Gauteng"), new Region("KwaZulu-Natal"), new Region("Limpopo Province"), new Region("Mpumalanga"), new Region("Northern Cape"), new Region("North-West"), new Region("Western Cape"));
            countryMap["GS"] = new Country("GS", "South Georgia");
//            countryMap["GS"] = new Country("GS", "South Georgia and the South Sandwich Islands");
            countryMap["ES"] = new Country("ES", "Spain", RegionType.Province, CountryOptions.PostalCode, new Region("Alava"), new Region("Albacete"), new Region("Alicante"), new Region("Almeria"), new Region("Asturias"), new Region("Avila"), new Region("Badajoz"), new Region("Baleares"), new Region("Barcelona"), new Region("Burgos"), new Region("Caceres"), new Region("Cadiz"), new Region("Castellon"), new Region("Ciudad Real"), new Region("Cordoba"), new Region("Cuenca"), new Region("Girona"), new Region("Granada"), new Region("Guadalajara"), new Region("Guipuzcoa"), new Region("Huelva"), new Region("Huesca"), new Region("Jaen"), new Region("La Coruna"), new Region("La Rioja"), new Region("Las Palmas"), new Region("Leon"), new Region("Lleida"), new Region("Madrid"), new Region("Malaga"), new Region("Murcia"), new Region("Navarra"), new Region("Ourense"), new Region("Palencia"), new Region("Provincia de Lugo"), new Region("Provincia de Pontevedra"), new Region("Salamanca"), new Region("Santa Cruz de Tenerife"), new Region("Santander"), new Region("Segovia"), new Region("Sevilla"), new Region("Soria"), new Region("Tarragona"), new Region("Teruel"), new Region("Toledo"), new Region("Valencia"), new Region("Valladolid"), new Region("Vizcaya"), new Region("Zamora"), new Region("Zaragoza"));
            countryMap["LK"] = new Country("LK", "Sri Lanka");
            countryMap["SH"] = new Country("SH", "St. Helena");
            countryMap["KN"] = new Country("KN", "St. Kitts and Nevis");
            countryMap["LC"] = new Country("LC", "St. Lucia");
            countryMap["PM"] = new Country("PM", "St. Pierre and Miquelon");
            countryMap["VC"] = new Country("VC", "St. Vincent and the Grenadines");
            countryMap["SR"] = new Country("SR", "Suriname");
            countryMap["SJ"] = new Country("SJ", "Svalbard and Jan Mayen");
            countryMap["SZ"] = new Country("SZ", "Swaziland");
            countryMap["SE"] = new Country("SE", "Sweden", CountryOptions.PostalCode);
            countryMap["CH"] = new Country("CH", "Switzerland", CountryOptions.PostalCode);
            countryMap["TW"] = new Country("TW", "Taiwan", CountryOptions.PostalCode);
            countryMap["TJ"] = new Country("TJ", "Tajikistan");
            countryMap["TZ"] = new Country("TZ", "Tanzania");
            countryMap["TH"] = new Country("TH", "Thailand", RegionType.Province, CountryOptions.PostalCode, new Region("Amnat Charoen"), new Region("Ang Thong"), new Region("Bangkok Metropolis"), new Region("Buri Ram"), new Region("Chachoengsao"), new Region("Chai Nat"), new Region("Chaiyaphum"), new Region("Chanthaburi"), new Region("Chiang Mai Province"), new Region("Chiang Rai"), new Region("Chon Buri"), new Region("Chumphon"), new Region("Kalasin"), new Region("Kamphaeng Phet"), new Region("Kanchanaburi"), new Region("Khon Kaen"), new Region("Krabi"), new Region("Lamphun"), new Region("Loei"), new Region("Lop Buri"), new Region("Mae Hong Son"), new Region("Maha Sarakham"), new Region("Mukdahan"), new Region("Nakhon Nayok"), new Region("Nakhon Pathom"), new Region("Nakhon Phanom"), new Region("Nakhon Ratchasima"), new Region("Nakhon Sawan"), new Region("Nakhon Si Thammarat"), new Region("Nan"), new Region("Narathiwat"), new Region("Nong Bua Lam Phu"), new Region("Nong Khai"), new Region("Nonthaburi"), new Region("Pathum Thani"), new Region("Pattani"), new Region("Phangnga"), new Region("Phatthalung"), new Region("Phayao"), new Region("Phetchabun"), new Region("Phetchaburi"), new Region("Phichit"), new Region("Phra Nakhon Si Ayutthaya"), new Region("Phrae"), new Region("Phuket"), new Region("Prachin Buri"), new Region("Prachuap Khiri Khan"), new Region("Ranong"), new Region("Ratchaburi"), new Region("Rayong"), new Region("Roi Et"), new Region("Sa Kaeo"), new Region("Sakon Nakhon"), new Region("Samut Prakan"), new Region("Samut Sakhon"), new Region("Samut Songkhram"), new Region("Saraburi"), new Region("Satun"), new Region("Si Sa Ket"), new Region("Sing Buri"), new Region("Songkhla"), new Region("Sukhothai"), new Region("Suphan Buri"), new Region("Surat Thani"), new Region("Surin"), new Region("Tak"), new Region("Trang"), new Region("Trat"), new Region("Ubon Ratchathani"), new Region("Uthai Thani"), new Region("Uttaradit"), new Region("Yala"), new Region("Yasothon"));
            countryMap["TP"] = new Country("TP", "Timor-Leste (East Timor)");
            countryMap["TG"] = new Country("TG", "Togo");
            countryMap["TK"] = new Country("TK", "Tokelau");
            countryMap["TO"] = new Country("TO", "Tonga");
            countryMap["TT"] = new Country("TT", "Trinidad and Tobago");
            countryMap["TA"] = new Country("TA", "Tristan da Cunha");
            countryMap["TN"] = new Country("TN", "Tunisia");
            countryMap["TR"] = new Country("TR", "Turkey", RegionType.Province, CountryOptions.PostalCode, new Region("Adana"), new Region("Adiyaman"), new Region("Afyon"), new Region("Agri"), new Region("Aksaray"), new Region("Amasya"), new Region("Ankara"), new Region("Antalya"), new Region("Ardahan"), new Region("Artvin"), new Region("Aydin"), new Region("Balikesir"), new Region("Bartin"), new Region("Batman"), new Region("Bayburt"), new Region("Bilecik"), new Region("Bingol"), new Region("Bitlis"), new Region("Bolu"), new Region("Burdur"), new Region("Bursa"), new Region("Canakkale"), new Region("Cankiri"), new Region("Corum"), new Region("Denizli"), new Region("Diyarbakir"), new Region("Edirne"), new Region("Elazig"), new Region("Erzincan"), new Region("Erzurum"), new Region("Eskisehir"), new Region("Gaziantep"), new Region("Giresun"), new Region("Gumushane"), new Region("Hakkari"), new Region("Hatay"), new Region("Icel"), new Region("Igdir"), new Region("Isparta"), new Region("Istanbul"), new Region("Izmir"), new Region("Kahraman Maras"), new Region("Karaman"), new Region("Kars"), new Region("Kastamonu"), new Region("Kayseri"), new Region("Kirikkale"), new Region("Kirklareli"), new Region("Kirsehir"), new Region("Kocaeli"), new Region("Konya"), new Region("Kutahya"), new Region("Malatya"), new Region("Manisa"), new Region("Mardin"), new Region("Mugla"), new Region("Mus"), new Region("Nevsehir"), new Region("Nigde"), new Region("Ordu"), new Region("Rize"), new Region("Sakarya"), new Region("Samsun"), new Region("Siirt"), new Region("Sinop"), new Region("Sirnak"), new Region("Sivas"), new Region("Tekirdag"), new Region("Tokat"), new Region("Trabzon"), new Region("Tunceli"), new Region("Urfa"), new Region("Usak"), new Region("Van"), new Region("Yozgat"), new Region("Zonguldak"));
            countryMap["TM"] = new Country("TM", "Turkmenistan");
            countryMap["TC"] = new Country("TC", "Turks and Caicos Islands");
            countryMap["TV"] = new Country("TV", "Tuvalu");
            countryMap["UG"] = new Country("UG", "Uganda");
            countryMap["UA"] = new Country("UA", "Ukraine", RegionType.Province, new Region("Cherkasy"), new Region("Chernivhiv"), new Region("Chernivtsi"), new Region("Dnipropetrovsk"), new Region("Donetsk"), new Region("Ivano-Frankivsk"), new Region("Kharkiv"), new Region("Khersonsrka"), new Region("Khmelnytsky"), new Region("Kirovohrad"), new Region("Kyiv"), new Region("Luhansk"), new Region("Lviv"), new Region("Mykolayiv"), new Region("Odessa"), new Region("Poltava"), new Region("Respublika Krym"), new Region("Rivne"), new Region("Sumy"), new Region("Ternopil"), new Region("Vinnytsya"), new Region("Volyn"), new Region("Zakarpatska"), new Region("Zaporizhzhya"), new Region("Zhytomyr"));
            countryMap["AE"] = new Country("AE", "United Arab Emirates");
            countryMap["UK"] = new Country("UK", "United Kingdom", RegionType.ConstituentCountry, CountryOptions.PostalCode, new Region("England"), new Region("Northern Ireland"), new Region("Scotland"), new Region("Wales"));
            countryMap["US"] = new Country("US", "United States", RegionType.State, CountryOptions.ZipCode, new Region("Alabama"), new Region("Alaska"), new Region("Arizona"), new Region("Arkansas"), new Region("California"), new Region("Colorado"), new Region("Connecticut"), new Region("Delaware"), new Region("District of Columbia"), new Region("Florida"), new Region("Georgia"), new Region("Hawaii"), new Region("Idaho"), new Region("Illinois"), new Region("Indiana"), new Region("Iowa"), new Region("Kansas"), new Region("Kentucky"), new Region("Louisiana"), new Region("Maine"), new Region("Maryland"), new Region("Massachusetts"), new Region("Michigan"), new Region("Minnesota"), new Region("Mississippi"), new Region("Missouri"), new Region("Montana"), new Region("Nebraska"), new Region("Nevada"), new Region("New Hampshire"), new Region("New Jersey"), new Region("New Mexico"), new Region("New York"), new Region("North Carolina"), new Region("North Dakota"), new Region("Ohio"), new Region("Oklahoma"), new Region("Oregon"), new Region("Pennsylvania"), new Region("Rhode Island"), new Region("South Carolina"), new Region("South Dakota"), new Region("Tennessee"), new Region("Texas"), new Region("Utah"), new Region("Vermont"), new Region("Viginia"), new Region("Washington"), new Region("West Virginia"), new Region("Wisconsin"), new Region("Wyoming"));
            countryMap["UM"] = new Country("UM", "United States Minor Outlying Islands");
            countryMap["UY"] = new Country("UY", "Uruguay", RegionType.StateTerritory, new Region("Artigas"), new Region("Canelones"), new Region("Cerro Largo"), new Region("Colonia"), new Region("Durazno"), new Region("Flores"), new Region("Florida"), new Region("Lvalleja"), new Region("Maldonado"), new Region("Montevideo"), new Region("Paysandu"), new Region("Rio Negro"), new Region("Rivera"), new Region("Rocha"), new Region("Salto"), new Region("San Jose"), new Region("Soriano"), new Region("Tacuarembo"), new Region("Treinta y Tres"));
            countryMap["UZ"] = new Country("UZ", "Uzbekistan");
            countryMap["VU"] = new Country("VU", "Vanuatu");
            countryMap["VA"] = new Country("VA", "Vatican City");
            countryMap["VE"] = new Country("VE", "Venezuela", RegionType.Province, CountryOptions.PostalCode, new Region("Amazonas"), new Region("Anzoategui"), new Region("Apure"), new Region("Aragua"), new Region("Barinas"), new Region("Bolivar"), new Region("Carabobo"), new Region("Cojedas"), new Region("Delta Amacuro"), new Region("Dependencias Federales"), new Region("Distrito Federal"), new Region("Estado Nueva Esparta"), new Region("Falcon"), new Region("Guarico"), new Region("Lara"), new Region("Merida"), new Region("Miranda"), new Region("Monagas"), new Region("Portuguesa"), new Region("Sucre"), new Region("Tachira"), new Region("Trujillo"), new Region("Yaracuy"), new Region("Zulia"));
            countryMap["VN"] = new Country("VN", "Vietnam", RegionType.CityRegion, CountryOptions.PostalCode, new Region("Haiphong"), new Region("Hanoi"), new Region("Ho Chi Minh City"));
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
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        /// <param name="abbreviation">The abbreviation.</param>
        /// <param name="name">The name.</param>
        /// <param name="regions">The regions.</param>
        public Country(string abbreviation, string name, params Region[] regions)
            : this(abbreviation, name, DefaultRegionType, regions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        /// <param name="abbreviation">The abbreviation.</param>
        /// <param name="name">The name.</param>
        /// <param name="assumedRegionType">Type of the assumed region.</param>
        /// <param name="regions">The regions.</param>
        public Country(string abbreviation, string name, RegionType assumedRegionType, params Region[] regions)
            : this(abbreviation, name, assumedRegionType, DefaultCountryOptions, regions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        /// <param name="abbreviation">The abbreviation.</param>
        /// <param name="name">The name.</param>
        /// <param name="options">The options.</param>
        /// <param name="regions">The regions.</param>
        public Country(string abbreviation, string name, CountryOptions options, params Region[] regions)
            : this(abbreviation, name, DefaultRegionType, options, regions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        /// <param name="abbreviation">The abbreviation.</param>
        /// <param name="name">The name.</param>
        /// <param name="assumedRegionType">Type of the assumed region.</param>
        /// <param name="options">The options.</param>
        /// <param name="regions">The regions.</param>
        public Country(string abbreviation, string name, RegionType assumedRegionType, CountryOptions options, params Region[] regions)
        {
            m_abbreviation = abbreviation;
            m_name = name;
            m_assumedRegionType = assumedRegionType;
            m_options = options;

            if (regions == null)
            {
                m_regions = new List<Region>();
            }
            else
            {
                m_regions = new List<Region>(regions);
            }
        }

        /// <summary>
        /// Gets or sets the abbreviation.
        /// </summary>
        /// <value>The abbreviation.</value>
        public string Abbreviation
        {
            get
            {
                return m_abbreviation;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return m_name;
            }
        }

        /// <summary>
        /// Gets the regions.
        /// </summary>
        /// <value>The regions.</value>
        public List<Region> Regions
        {
            get
            {
                return m_regions;
            }
        }

        /// <summary>
        /// Gets the type of the assumed region.
        /// </summary>
        /// <value>The type of the assumed region.</value>
        public RegionType AssumedRegionType
        {
            get
            {
                return m_assumedRegionType;
            }
        }

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>The options.</value>
        public CountryOptions Options
        {
            get
            {
                return m_options;
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

        /// <summary>
        /// Gets the type of the readable region.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetReadableRegionType(RegionType type)
        {
            switch (type)
            {
                case RegionType.City:
                case RegionType.County:
                case RegionType.Prefecture:
                case RegionType.Province:
                case RegionType.Region:
                case RegionType.State:
                    return type.ToString();
                case RegionType.AdministrativeDivision:
                    return "Administrative Division";
                case RegionType.CityRegion:
                    return "City/Region";
                case RegionType.ConstituentCountry:
                    return "Constituent Country";
                case RegionType.StateTerritory:
                    return "State/Territory";
                default:
                    throw new NotImplementedException();
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
            return Name;
        }
    }
}
