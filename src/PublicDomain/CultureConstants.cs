using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PublicDomain
{
    /// <summary>
    /// Windows culture constants such as LCIDs and culture names.
    /// See: http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemglobalizationcultureinfoclasstopic.asp
    /// </summary>
    public class CultureConstants
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const string CultureInvariant = "";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureInvariantIdentifier = 127;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralAfrikaans = "af";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralAfrikaansIdentifier = 54;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificAfrikaansSouthAfrica = "af-ZA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificAfrikaansSouthAfricaIdentifier = 1078;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralAlbanian = "sq";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralAlbanianIdentifier = 28;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificAlbanianAlbania = "sq-AL";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificAlbanianAlbaniaIdentifier = 1052;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralArabic = "ar";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralArabicIdentifier = 1;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralArabicAlgeria = "ar-DZ";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralArabicAlgeriaIdentifier = 5121;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicBahrain = "ar-BH";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicBahrainIdentifier = 15361;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicEgypt = "ar-EG";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicEgyptIdentifier = 3073;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicIraq = "ar-IQ";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicIraqIdentifier = 2049;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicJordan = "ar-JO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicJordanIdentifier = 11265;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicKuwait = "ar-KW";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicKuwaitIdentifier = 13313;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicLebanon = "ar-LB";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicLebanonIdentifier = 12289;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicLibya = "ar-LY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicLibyaIdentifier = 4097;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicMorocco = "ar-MA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicMoroccoIdentifier = 6145;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicOman = "ar-OM";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicOmanIdentifier = 8193;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicQatar = "ar-QA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicQatarIdentifier = 16385;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicSaudiArabia = "ar-SA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicSaudiArabiaIdentifier = 1025;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicSyria = "ar-SY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicSyriaIdentifier = 10241;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicTunisia = "ar-TN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicTunisiaIdentifier = 7169;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicUnitedArabEmirates = "ar-AE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicUnitedArabEmiratesIdentifier = 14337;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArabicYemen = "ar-YE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArabicYemenIdentifier = 9217;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralArmenian = "hy";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralArmenianIdentifier = 43;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificArmenianArmenia = "hy-AM";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificArmenianArmeniaIdentifier = 1067;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralAzeri = "az";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralAzeriIdentifier = 44;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificAzeriCyrillicAzerbaijan = "az-AZ-Cyrl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificAzeriCyrillicAzerbaijanIdentifier = 2092;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificAzeriLatinAzerbaijan = "az-AZ-Latn";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificAzeriLatinAzerbaijanIdentifier = 1068;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralBasque = "eu";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralBasqueIdentifier = 45;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificBasqueBasque = "eu-ES";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificBasqueBasqueIdentifier = 1069;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralBelarusian = "be";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralBelarusianIdentifier = 35;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificBelarusianBelarus = "be-BY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificBelarusianBelarusIdentifier = 1059;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralBulgarian = "bg";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralBulgarianIdentifier = 2;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificBulgarianBulgaria = "bg-BG";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificBulgarianBulgariaIdentifier = 1026;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralCatalan = "ca";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralCatalanIdentifier = 3;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificCatalanCatalan = "ca-ES";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificCatalanCatalanIdentifier = 1027;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseHongKongSar = "zh-HK";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseHongKongSarIdentifier = 3076;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseMacaoSar = "zh-MO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseMacaoSarIdentifier = 5124;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseChina = "zh-CN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseChinaIdentifier = 2052;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseSimplified = "zh-CHS";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseSimplifiedIdentifier = 4;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseSingapore = "zh-SG";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseSingaporeIdentifier = 4100;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseTaiwan = "zh-TW";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseTaiwanIdentifier = 1028;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificChineseTraditional = "zh-CHT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificChineseTraditionalIdentifier = 31748;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralCroatian = "hr";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralCroatianIdentifier = 26;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificCroatianCroatia = "hr-HR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificCroatianCroatiaIdentifier = 1050;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralCzech = "cs";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralCzechIdentifier = 5;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificCzechCzechRepublic = "cs-CZ";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificCzechCzechRepublicIdentifier = 1029;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralDanish = "da";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralDanishIdentifier = 6;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificDanishDenmark = "da-DK";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificDanishDenmarkIdentifier = 1030;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralDhivehi = "div";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralDhivehiIdentifier = 101;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificDhivehiMaldives = "div-MV";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificDhivehiMaldivesIdentifier = 1125;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralDutch = "nl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralDutchIdentifier = 19;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificDutchBelgium = "nl-BE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificDutchBelgiumIdentifier = 2067;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificDutchTheNetherlands = "nl-NL";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificDutchTheNetherlandsIdentifier = 1043;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralEnglish = "en";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralEnglishIdentifier = 9;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishAustralia = "en-AU";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishAustraliaIdentifier = 3081;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishBelize = "en-BZ";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishBelizeIdentifier = 10249;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishCanada = "en-CA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishCanadaIdentifier = 4105;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishCaribbean = "en-CB";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishCaribbeanIdentifier = 9225;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishIreland = "en-IE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishIrelandIdentifier = 6153;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishJamaica = "en-JM";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishJamaicaIdentifier = 8201;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishNewZealand = "en-NZ";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishNewZealandIdentifier = 5129;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishPhilippines = "en-PH";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishPhilippinesIdentifier = 13321;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishSouthAfrica = "en-ZA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishSouthAfricaIdentifier = 7177;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishTrinidadAndTobago = "en-TT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishTrinidadAndTobagoIdentifier = 11273;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishUnitedKingdom = "en-GB";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishUnitedKingdomIdentifier = 2057;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishUnitedStates = "en-US";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishUnitedStatesIdentifier = 1033;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEnglishZimbabwe = "en-ZW";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEnglishZimbabweIdentifier = 12297;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralEstonian = "et";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralEstonianIdentifier = 37;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificEstonianEstonia = "et-EE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificEstonianEstoniaIdentifier = 1061;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralFaroese = "fo";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralFaroeseIdentifier = 56;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFaroeseFaroeIslands = "fo-FO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFaroeseFaroeIslandsIdentifier = 1080;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralFarsi = "fa";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralFarsiIdentifier = 41;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFarsiIran = "fa-IR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFarsiIranIdentifier = 1065;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralFinnish = "fi";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralFinnishIdentifier = 11;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFinnishFinland = "fi-FI";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFinnishFinlandIdentifier = 1035;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralFrench = "fr";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralFrenchIdentifier = 12;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFrenchBelgium = "fr-BE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFrenchBelgiumIdentifier = 2060;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFrenchCanada = "fr-CA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFrenchCanadaIdentifier = 3084;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFrenchFrance = "fr-FR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFrenchFranceIdentifier = 1036;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFrenchLuxembourg = "fr-LU";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFrenchLuxembourgIdentifier = 5132;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFrenchMonaco = "fr-MC";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFrenchMonacoIdentifier = 6156;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificFrenchSwitzerland = "fr-CH";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificFrenchSwitzerlandIdentifier = 4108;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralGalician = "gl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralGalicianIdentifier = 86;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGalicianGalician = "gl-ES";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGalicianGalicianIdentifier = 1110;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralGeorgian = "ka";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralGeorgianIdentifier = 55;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGeorgianGeorgia = "ka-GE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGeorgianGeorgiaIdentifier = 1079;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralGerman = "de";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralGermanIdentifier = 7;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGermanAustria = "de-AT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGermanAustriaIdentifier = 3079;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGermanGermany = "de-DE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGermanGermanyIdentifier = 1031;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGermanLiechtenstein = "de-LI";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGermanLiechtensteinIdentifier = 5127;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGermanLuxembourg = "de-LU";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGermanLuxembourgIdentifier = 4103;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGermanSwitzerland = "de-CH";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGermanSwitzerlandIdentifier = 2055;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralGreek = "el";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralGreekIdentifier = 8;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGreekGreece = "el-GR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGreekGreeceIdentifier = 1032;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralGujarati = "gu";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralGujaratiIdentifier = 71;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificGujaratiIndia = "gu-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificGujaratiIndiaIdentifier = 1095;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralHebrew = "he";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralHebrewIdentifier = 13;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificHebrewIsrael = "he-IL";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificHebrewIsraelIdentifier = 1037;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralHindi = "hi";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralHindiIdentifier = 57;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificHindiIndia = "hi-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificHindiIndiaIdentifier = 1081;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralHungarian = "hu";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralHungarianIdentifier = 14;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificHungarianHungary = "hu-HU";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificHungarianHungaryIdentifier = 1038;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralIcelandic = "is";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralIcelandicIdentifier = 15;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificIcelandicIceland = "is-IS";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificIcelandicIcelandIdentifier = 1039;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralIndonesian = "id";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralIndonesianIdentifier = 33;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificIndonesianIndonesia = "id-ID";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificIndonesianIndonesiaIdentifier = 1057;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralItalian = "it";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralItalianIdentifier = 16;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificItalianItaly = "it-IT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificItalianItalyIdentifier = 1040;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificItalianSwitzerland = "it-CH";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificItalianSwitzerlandIdentifier = 2064;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralJapanese = "ja";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralJapaneseIdentifier = 17;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificJapaneseJapan = "ja-JP";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificJapaneseJapanIdentifier = 1041;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralKannada = "kn";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralKannadaIdentifier = 75;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificKannadaIndia = "kn-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificKannadaIndiaIdentifier = 1099;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralKazakh = "kk";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralKazakhIdentifier = 63;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificKazakhKazakhstan = "kk-KZ";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificKazakhKazakhstanIdentifier = 1087;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralKonkani = "kok";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralKonkaniIdentifier = 87;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificKonkaniIndia = "kok-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificKonkaniIndiaIdentifier = 1111;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralKorean = "ko";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralKoreanIdentifier = 18;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificKoreanKorea = "ko-KR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificKoreanKoreaIdentifier = 1042;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralKyrgyz = "ky";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralKyrgyzIdentifier = 64;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificKyrgyzKyrgyzstan = "ky-KG";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificKyrgyzKyrgyzstanIdentifier = 1088;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralLatvian = "lv";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralLatvianIdentifier = 38;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificLatvianLatvia = "lv-LV";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificLatvianLatviaIdentifier = 1062;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralLithuanian = "lt";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralLithuanianIdentifier = 39;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificLithuanianLithuania = "lt-LT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificLithuanianLithuaniaIdentifier = 1063;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralMacedonian = "mk";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralMacedonianIdentifier = 47;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificMacedonianFormerYugoslavRepublicOfMacedonia = "mk-MK";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificMacedonianFormerYugoslavRepublicOfMacedoniaIdentifier = 1071;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralMalay = "ms";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralMalayIdentifier = 62;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificMalayBrunei = "ms-BN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificMalayBruneiIdentifier = 2110;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificMalayMalaysia = "ms-MY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificMalayMalaysiaIdentifier = 1086;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralMarathi = "mr";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralMarathiIdentifier = 78;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificMarathiIndia = "mr-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificMarathiIndiaIdentifier = 1102;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralMongolian = "mn";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralMongolianIdentifier = 80;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificMongolianMongolia = "mn-MN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificMongolianMongoliaIdentifier = 1104;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralNorwegian = "no";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralNorwegianIdentifier = 20;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificNorwegianBokmlNorway = "nb-NO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificNorwegianBokmlNorwayIdentifier = 1044;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificNorwegianNynorskNorway = "nn-NO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificNorwegianNynorskNorwayIdentifier = 2068;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralPolish = "pl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralPolishIdentifier = 21;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificPolishPoland = "pl-PL";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificPolishPolandIdentifier = 1045;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralPortuguese = "pt";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralPortugueseIdentifier = 22;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificPortugueseBrazil = "pt-BR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificPortugueseBrazilIdentifier = 1046;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificPortuguesePortugal = "pt-PT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificPortuguesePortugalIdentifier = 2070;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralPunjabi = "pa";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralPunjabiIdentifier = 70;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificPunjabiIndia = "pa-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificPunjabiIndiaIdentifier = 1094;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralRomanian = "ro";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralRomanianIdentifier = 24;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificRomanianRomania = "ro-RO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificRomanianRomaniaIdentifier = 1048;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralRussian = "ru";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralRussianIdentifier = 25;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificRussianRussia = "ru-RU";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificRussianRussiaIdentifier = 1049;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSanskrit = "sa";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSanskritIdentifier = 79;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSanskritIndia = "sa-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSanskritIndiaIdentifier = 1103;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSerbianCyrrilicSerbia = "sr-SP-Cyrl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSerbianCyrrilicSerbiaIdentifier = 3098;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSerbianLatinSerbia = "sr-SP-Latn";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSerbianLatinSerbiaIdentifier = 2074;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSlovak = "sk";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSLOVAKIdentifier = 27;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSlovakSlovakia = "sk-SK";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSlovakSlovakiaIdentifier = 1051;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSlovenian = "sl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSlovenianIdentifier = 36;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSlovenianSlovenia = "sl-SI";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSlovenianSloveniaIdentifier = 1060;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSpanish = "es";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSPANISHIdentifier = 10;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishArgentina = "es-AR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishArgentinaIdentifier = 11274;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishBolivia = "es-BO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishBoliviaIdentifier = 16394;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishChile = "es-CL";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishChileIdentifier = 13322;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishColombia = "es-CO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishColombiaIdentifier = 9226;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishCostaRica = "es-CR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishCostaRicaIdentifier = 5130;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishDominicanRepublic = "es-DO";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishDominicanRepublicIdentifier = 7178;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishEcuador = "es-EC";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishEcuadorIdentifier = 12298;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishElSalvador = "es-SV";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishElSalvadorIdentifier = 17418;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishGuatemala = "es-GT";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishGuatemalaIdentifier = 4106;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishHonduras = "es-HN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishHondurasIdentifier = 18442;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishMexico = "es-MX";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishMexicoIdentifier = 2058;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishNicaragua = "es-NI";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishNicaraguaIdentifier = 19466;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishPanama = "es-PA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishPanamaIdentifier = 6154;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishParaguay = "es-PY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishParaguayIdentifier = 15370;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishPeru = "es-PE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishPeruIdentifier = 10250;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishPuertoRico = "es-PR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishPuertoRicoIdentifier = 20490;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishSpain = "es-ES";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishSpainIdentifier = 3082;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishUruguay = "es-UY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishUruguayIdentifier = 14346;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSpanishVenezuela = "es-VE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSpanishVenezuelaIdentifier = 8202;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSwahili = "sw";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSwahiliIdentifier = 65;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSwahiliKenya = "sw-KE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSwahiliKenyaIdentifier = 1089;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSwedish = "sv";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSwedishIdentifier = 29;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSwedishFinland = "sv-FI";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSwedishFinlandIdentifier = 2077;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSwedishSweden = "sv-SE";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSwedishSwedenIdentifier = 1053;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralSyriac = "syr";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralSyriacIdentifier = 90;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificSyriacSyria = "syr-SY";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificSyriacSyriaIdentifier = 1114;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralTamil = "ta";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralTamilIdentifier = 73;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificTamilIndia = "ta-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificTamilIndiaIdentifier = 1097;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralTatar = "tt";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralTatarIdentifier = 68;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificTatarRussia = "tt-RU";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificTatarRussiaIdentifier = 1092;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralTelugu = "te";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralTeluguIdentifier = 74;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificTeluguIndia = "te-IN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificTeluguIndiaIdentifier = 1098;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralThai = "th";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralThaiIdentifier = 30;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificThaiThailand = "th-TH";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificThaiThailandIdentifier = 1054;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralTurkish = "tr";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralTurkishIdentifier = 31;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificTurkishTurkey = "tr-TR";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificTurkishTurkeyIdentifier = 1055;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralUkrainian = "uk";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralUkrainianIdentifier = 34;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificUkrainianUkraine = "uk-UA";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificUkrainianUkraineIdentifier = 1058;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralUrdu = "ur";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralUrduIdentifier = 32;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificUrduPakistan = "ur-PK";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificUrduPakistanIdentifier = 1056;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralUzbek = "uz";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralUzbekIdentifier = 67;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificUzbekCyrillicUzbekistan = "uz-UZ-Cyrl";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificUzbekCyrillicUzbekistanIdentifier = 2115;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificUzbekLatinUzbekistan = "uz-UZ-Latn";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificUzbekLatinUzbekistanIdentifier = 1091;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureNeutralVietnamese = "vi";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureNeutralVietnameseIdentifier = 42;

        /// <summary>
        /// 
        /// </summary>
        public const string CultureSpecificVietnameseVietnam = "vi-VN";

        /// <summary>
        /// 
        /// </summary>
        public const int CultureSpecificVietnameseVietnamIdentifier = 1066;

        #endregion

        /// <summary>
        /// Used for parsing MSDN output into C# code.
        /// </summary>
        public static void CreateClass()
        {
            using (StreamReader reader = new StreamReader(@"c:\temp\langs.txt"))
            {
                using (StreamWriter writer = new StreamWriter(@"c:\temp\langsout.txt"))
                {
                    string line;
                    string cult, lcidstr, name;
                    cult = name = lcidstr = null;
                    int stage = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line != string.Empty)
                        {
                            switch (stage)
                            {
                                case 0:
                                    cult = line;
                                    break;
                                case 1:
                                    lcidstr = line;
                                    break;
                                case 2:
                                    {
                                        name = line;
                                        stage = -1;
                                        int lcid = int.Parse(lcidstr.Substring(2), System.Globalization.NumberStyles.HexNumber);
                                        name = cleanName(name);
                                        writer.WriteLine(string.Format("\t\tpublic const string Culture{3}{2} = \"{0}\";\n\t\tpublic const int Culture{3}{2}Identifier = {1};\n", cult, lcid, name, cult.Contains("-") ? "Specific" : "Neutral"));
                                        break;
                                    }
                            }
                            stage++;
                        }
                    }
                }
            }
        }

        private static string cleanName(string name)
        {
            name = name.Replace("- ", "").Replace(" ", "_").Replace("(", "").Replace(")", "");
            return name;
        }
    }
}
