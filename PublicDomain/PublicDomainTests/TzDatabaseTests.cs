using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using PublicDomain.Dynacode;
using System.Text.RegularExpressions;

namespace PublicDomain
{
    [TestFixture]
    public class TzDatabaseTests
    {
        public const string RegionStart = "#region Generated Time Zones";
        public const string RegionEnd = "#endregion";

        /// <summary>
        /// Parse3166s the tab.
        /// </summary>
        [Test]
        public void Parse3166Tab()
        {
            Dictionary<string, Iso3166> map = TzDatabase.ParseIso3166Tab(TzDatabase.Iso3166TabFile);
            foreach (string key in map.Keys)
            {
                Iso3166 data = map[key];
                Console.WriteLine(data);
            }
        }

        /// <summary>
        /// Parses the zone tab.
        /// </summary>
        [Test]
        public void ParseZoneTab()
        {
            List<PublicDomain.TzTimeZone.TzZoneDescription> items = TzDatabase.ParseZoneTab(TzDatabase.ZoneTabFile);
            foreach (PublicDomain.TzTimeZone.TzZoneDescription item in items)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// Reads the database.
        /// </summary>
        [Test]
        public void ReadDatabase()
        {
            bool generateList;
            generateList = true;
            // First, get all of the data from the tz db
            List<string[]> links = new List<string[]>();
            List<PublicDomain.TzDatabase.TzRule> rules = new List<PublicDomain.TzDatabase.TzRule>();
            List<PublicDomain.TzDatabase.TzZone> zones = new List<PublicDomain.TzDatabase.TzZone>();
            TzDatabase.ReadDatabase(TzDatabase.TzDatabaseDirectory, rules, zones, links);

            // Now, get all of the tab files
            Dictionary<string, Iso3166> map = TzDatabase.ParseIso3166Tab(TzDatabase.Iso3166TabFile);
            List<PublicDomain.TzTimeZone.TzZoneDescription> items = TzDatabase.ParseZoneTab(TzDatabase.ZoneTabFile);

            // Build up the rule lists based on names
            Dictionary<string, List<PublicDomain.TzDatabase.TzRule>> ruleList = new Dictionary<string, List<PublicDomain.TzDatabase.TzRule>>();
            foreach (PublicDomain.TzDatabase.TzRule rule in rules)
            {
                if (!ruleList.ContainsKey(rule.RuleName))
                {
                    ruleList[rule.RuleName] = new List<PublicDomain.TzDatabase.TzRule>();
                }
                ruleList[rule.RuleName].Add(rule);
            }

            // Build up the zone lists based on names
            Dictionary<string, List<PublicDomain.TzDatabase.TzZone>> zonesList = new Dictionary<string, List<PublicDomain.TzDatabase.TzZone>>();
            foreach (PublicDomain.TzDatabase.TzZone zone in zones)
            {
                if (!zonesList.ContainsKey(zone.ZoneName))
                {
                    zonesList[zone.ZoneName] = new List<PublicDomain.TzDatabase.TzZone>();
                }
                zonesList[zone.ZoneName].Add(zone);
            }

            Dictionary<string, PublicDomain.TzTimeZone.TzZoneInfo> zoneList = new Dictionary<string, PublicDomain.TzTimeZone.TzZoneInfo>();
            List<PublicDomain.TzDatabase.TzZone> dataZones;
            List<PublicDomain.TzDatabase.TzRule> dataRules;
            foreach (string zoneKey in zonesList.Keys)
            {
                zonesList.TryGetValue(zoneKey, out dataZones);

                dataRules = null;
                // Go through each zone and get the associated rules
                if (dataZones != null)
                {
                    dataRules = new List<PublicDomain.TzDatabase.TzRule>();
                    foreach (PublicDomain.TzDatabase.TzZone dataZone in dataZones)
                    {
                        if (dataZone.HasRules())
                        {
                            ArrayUtilities.AppendList<PublicDomain.TzDatabase.TzRule>(dataRules, ruleList[dataZone.RuleName]);
                        }
                        else if (ConversionUtilities.IsStringATimeSpan(dataZone.RuleName))
                        {
                            TimeSpan timedRule = DateTimeUtlities.ParseTimeSpan(dataZone.RuleName);
                            dataRules.Add(new PublicDomain.TzDatabase.TzRule(dataZone.RuleName, int.MinValue, int.MaxValue, Month.January,
                                1, null, new TimeSpan(), null, timedRule, null, null));
                        }
                    }
                }

                zoneList[zoneKey] = new TzTimeZone.TzZoneInfo(zoneKey, dataZones, dataRules);
            }

            // Finally, clone the links
            foreach (string[] pieces in links)
            {
                // Find the time zone this is a link to
                TzTimeZone.TzZoneInfo linkedTo = zoneList[pieces[1]];

                TzTimeZone.TzZoneInfo link = linkedTo.Clone(pieces[2]);

                zoneList[link.ZoneName] = link;
            }

            // Now, get the zone list as a List
            List<PublicDomain.TzTimeZone.TzZoneInfo> tzzones = new List<PublicDomain.TzTimeZone.TzZoneInfo>();
            foreach (string key in zoneList.Keys)
            {
                tzzones.Add(zoneList[key]);
            }

            // Sort it by zone name
            tzzones.Sort(delegate(PublicDomain.TzTimeZone.TzZoneInfo x, PublicDomain.TzTimeZone.TzZoneInfo y)
            {
                return x.ZoneName.CompareTo(y.ZoneName);
            });

            // Print out the zone names
            //tzzones.ForEach(delegate(PublicDomain.TzTimeZone.TzZoneInfo z)
            //{
            //    Console.WriteLine(z.ZoneName);
            //});

            CreateTzCode(generateList, tzzones, 3);
        }

        private void CreateTzCode(bool generateList, List<PublicDomain.TzTimeZone.TzZoneInfo> tzzones, int numberOfTabs)
        {
            string tabs = new string('\t', numberOfTabs);
            string tabs1 = new string('\t', numberOfTabs - 1);
            Console.WriteLine(@"
{1}{2}

{1}private static void InitializeZones()
{1}{{
{0}if (m_zones != null) return;

{0}Dictionary<string, PublicDomain.TzTimeZone.TzZoneInfo> zones = new Dictionary<string, PublicDomain.TzTimeZone.TzZoneInfo>();
{0}List<PublicDomain.TzTimeZone.TzZoneInfo> zoneList = new List<PublicDomain.TzTimeZone.TzZoneInfo>();
{0}PublicDomain.TzTimeZone.TzZoneInfo zone = null;
", tabs, tabs1, RegionStart);

            StringBuilder sb = new StringBuilder();
            TextWriter sbwriter = new StringWriter(sb), writer;

            TzTimeZone.TzZoneInfo zone;
            for (int i = 0; i < tzzones.Count; i++)
            {
                zone = tzzones[i];

                string rulesArray = "", zonesArray = "";

                PublicDomain.TzDatabase.TzZone[] zones = ArrayUtilities.ConvertToArray<PublicDomain.TzDatabase.TzZone>(zone.Zones);
                PublicDomain.TzDatabase.TzRule[] rules = ArrayUtilities.ConvertToArray<PublicDomain.TzDatabase.TzRule>(zone.Rules);

                // Remove rule duplicates and sort both lists

                zonesArray = SortZones(zonesArray, zones);

                rulesArray = SortRules(rulesArray, rules);

                if (NeedsFunction(i))
                {
                    writer = sbwriter;
                    string funcName = CodeUtilities.StripNonIdentifierCharacters(Language.CSharp, zone.ZoneName) + '_' + i;
                    Console.WriteLine(@"{0}InitializeZones{1}(zone, zones, zoneList);", tabs, funcName);
                    writer.WriteLine(@"{1}private static void InitializeZones{0}(PublicDomain.TzTimeZone.TzZoneInfo zone, Dictionary<string, PublicDomain.TzTimeZone.TzZoneInfo> zones, List<PublicDomain.TzTimeZone.TzZoneInfo> zoneList)
{1}{{", funcName, tabs1);
                }
                else
                {
                    writer = Console.Out;
                }

                writer.WriteLine(@"{3}zone = new PublicDomain.TzTimeZone.TzZoneInfo(""{0}"", {1}, {2});
{3}zones[""{0}""] = zone;", zone.ZoneName, zonesArray, rulesArray, tabs);

                if (generateList)
                {
                    writer.WriteLine(@"{3}zoneList.Add(zone);", zone.ZoneName, zonesArray, rulesArray, tabs);
                }

                if (NeedsFunction(i))
                {
                    writer.WriteLine(@"{0}}}
", tabs1);
                }
                else
                {
                    writer.WriteLine();
                }
            }

            Console.WriteLine(@"
{0}m_zones = ReadOnlyDictionary<string, TzZoneInfo>.AsReadOnly(zones);
{0}m_zoneList = zoneList.AsReadOnly();
{1}}}
", tabs, tabs1);

            if (sb.Length > 0)
            {
                Console.WriteLine(sb);
            }

            Console.WriteLine(@"
{1}{2}", tabs, tabs1, RegionEnd);
        }

        private static string SortZones(string zonesArray, PublicDomain.TzDatabase.TzZone[] zones)
        {
            if (zones.Length > 0)
            {
                // First remove any duplicates
                List<PublicDomain.TzDatabase.TzZone> tempZones = new List<PublicDomain.TzDatabase.TzZone>(zones);
                ArrayUtilities.RemoveDuplicates<PublicDomain.TzDatabase.TzZone>(tempZones);

                tempZones.Sort(SortZonesComparison);

                foreach (PublicDomain.TzDatabase.TzZone zoneData in tempZones)
                {
                    if (zonesArray.Length > 0)
                    {
                        zonesArray += ",";
                    }
                    zonesArray += zoneData.GetObjectString();
                }
                zonesArray = "new List<PublicDomain.TzDatabase.TzZone>(new PublicDomain.TzDatabase.TzZone[] {" + zonesArray + "})";
            }
            else
            {
                zonesArray = "null";
            }
            return zonesArray;
        }

        private static string SortRules(string rulesArray, PublicDomain.TzDatabase.TzRule[] rules)
        {
            if (rules.Length > 0)
            {
                // First remove any duplicates
                List<PublicDomain.TzDatabase.TzRule> tempRules = new List<PublicDomain.TzDatabase.TzRule>(rules);
                ArrayUtilities.RemoveDuplicates<PublicDomain.TzDatabase.TzRule>(tempRules);

                // Next, sort the items
                tempRules.Sort(SortRulesComparison);

                foreach (PublicDomain.TzDatabase.TzRule rule in tempRules)
                {
                    if (rulesArray.Length > 0)
                    {
                        rulesArray += ",";
                    }
                    rulesArray += rule.GetObjectString();
                }
                rulesArray = "new List<PublicDomain.TzDatabase.TzRule>(new PublicDomain.TzDatabase.TzRule[] {" + rulesArray + "})";
            }
            else
            {
                rulesArray = "null";
            }
            return rulesArray;
        }

        private static int SortRulesComparison(PublicDomain.TzDatabase.TzRule x, PublicDomain.TzDatabase.TzRule y)
        {
            return x.CompareTo(y);
        }

        private static int SortZonesComparison(PublicDomain.TzDatabase.TzZone x, PublicDomain.TzDatabase.TzZone y)
        {
            return x.CompareTo(y);
        }

        private bool NeedsFunction(int i)
        {
            return true;
        }

        /// <summary>
        /// Reads the database and then overwrites TzTimeZone.cs
        /// which has the codified data.
        /// </summary>
        public void ExecuteReplaceData()
        {
            string fileLocation = @"..\..\..\TzTimeZone.cs";
            StringBuilder sb = new StringBuilder(5092);
            using (new ConsoleRerouter(sb))
            {
                ReadDatabase();
            }
            string content = StringUtilities.TrimNewlines(sb.ToString());
            if (content.Length < 100)
            {
                throw new Exception("Content does not appear to be correct (too short)");
            }

            string tzTimeZoneFile = File.ReadAllText(fileLocation);
            Regex r = new Regex(RegionStart + ".+" + RegionEnd, RegexOptions.Singleline);
            bool replaced = false;
            tzTimeZoneFile = r.Replace(tzTimeZoneFile, new MatchEvaluator(delegate(Match m)
            {
                replaced = true;
                return content;
            }));

            Console.WriteLine("{0} content to replace", replaced ? "Successfully found" : "Did not find");

            if (replaced)
            {
                Console.Write("Paching...");
                File.WriteAllText(fileLocation, tzTimeZoneFile);
                Console.WriteLine("OK");
            }
        }
    }
}
