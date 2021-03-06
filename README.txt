PublicDomain
============
PublicDomain is placed in the Global Assembly Cache (GAC), so you can
either reference it using its strong name, or add a reference to the PublicDomain.dll
assembly from the installation directory (default is C:\Program Files\Public Domain\PublicDomain.dll).

Contributing Authors
====================
William M. Leszczuk
Pierre Vachon
Simon Mourier
Brad Culberson

Version History
===============
V0.2.47.0
	[kevgrig@gmail.com]
		* Security bug
V0.2.46.0
	[kevgrig@gmail.com]
		* Added Int64/BIGINT support to PublicDomain.Data
		* Added Set class
V0.2.45.0
	[kevgrig@gmail.com]
		* Updated to tzdata2008e
		* Minor API changes
V0.2.44.0
	[kevgrig@gmail.com]
		* Update how LoggingConfig reads config values for an item with multiple names
V0.2.43.0
	[kevgrig@gmail.com]
		* Updated to tzdata2008b
		* Updated ConfigurationValues to have nested ExternalConfigs
V0.2.42.0
	[kevgrig@gmail.com]
		* Fixed LenientHtmlDocument namespace bug
V0.2.41.0
	[kevgrig@gmail.com]
		* Updated to tzdata2007k
V0.2.40.0
	[kevgrig@gmail.com]
		* Allow dynamic update of all loggers
V0.2.38.0
	[kevgrig@gmail.com]
		* Fixed serialization of IMonotonicNumberGenerator
		* Added milliseconds to Logger
		* Fixed bug in re-loading Loggers with multiple logger names
V0.2.37.0
	[kevgrig@gmail.com]
		* Fixed synchronization bug in Logger
V0.2.36.0
	[kevgrig@gmail.com]
		* Fixed some SQL escape characters
V0.2.35.0
	[kevgrig@gmail.com]
		* Updated to tzdata2007j
		* Changed TzTimeZone to look up by a string based switch instead of by precomputed hash code
		  to avoid issues across framework version
		* BUG 14464: Etc time zone hour offset instead of day offset
	[bradculberson@gmail.com]
		* BUG 13252: Corrected flipped dates for DST calculations for southern hemisphere
V0.2.31.0
	[kevgrig@gmail.com]
		* Fixed entity replacement within an attribute in LenientXmlDocument
		* Fixed BUG http://www.codeplex.com/publicdomain/WorkItem/View.aspx?WorkItemId=13637
V0.2.30.0
	[kevgrig@gmail.com]
		* Fixed bug in FileSystemUtilities.CombineTrackbacksInPath
V0.2.29.0
	[kevgrig@gmail.com]
		* Fixed BUG http://www.codeplex.com/publicdomain/WorkItem/View.aspx?WorkItemId=13336
		* Added Win32.GetLocalTime and Win32.GetSystemTime
		* Added DisposableCallback
V0.2.28.0
	[kevgrig@gmail.com]
		* Added ReaderWriterLock to TzTimeZone.GetTimeZone
V0.2.27.0
	[kevgrig@gmail.com]
		* Changed TzTimeZone from static initialization to lazyily loading time zones
		* BUG: http://www.codeplex.com/publicdomain/WorkItem/View.aspx?WorkItemId=12958
V0.2.26.0
	[kevgrig@gmail.com]
		* LenientXmlDocument updates
V0.2.24.0
	[kevgrig@gmail.com]
		* Added automatic commenting of the inside of a SCRIPT tag in the LenientHtmlDocument
V0.2.23.0
	[kevgrig@gmail.com]
		* First pass at LenientXmlParser
V0.2.22.0
	[kevgrig@gmail.com]
		* Added Web.DnsUtilities and public IP providers
		* Added base implementation of LenientXmlDocument
V0.2.21.0
	[kevgrig@gmail.com]
		* BUG: Parsing Language from display name
V0.2.20.0
	[kevgrig@gmail.com
		* Add Language and LanguageConstants
V0.2.19.0
	[kevgrig@gmail.com]
		* BUG: TZ Date time parsing
V0.2.18.0
	[kevgrig@gmail.com]
		* Small bug fixes
V0.2.17.0
	[kevgrig@gmail.com]
		* Added IDatabase, DbConnectionScope, and DbTransactionScope
V0.2.16.0
	[kevgrig@gmail.com]
		* Added INumberGenerator, IRandomNumberGenerator, and IMonotonicNumberGenerator
		* Added use of CriticalFinalizerObject
		* BUG: http://www.codeplex.com/publicdomain/WorkItem/View.aspx?WorkItemId=12363
		* BUG: http://www.codeplex.com/publicdomain/WorkItem/View.aspx?WorkItemId=12480
		* BUG: http://www.codeplex.com/publicdomain/WorkItem/View.aspx?WorkItemId=12541
V0.2.15.0
	[kevgrig@gmail.com]
		* Added UtcOffset to ILogTimestampProvider and updated ILogFormatter to use it
		* BUG: http://www.codeplex.com/publicdomain/WorkItem/View.aspx?WorkItemId=12315
V0.2.14.0
	[kevgrig@gmail.com]
		* Added EventLogLogger, CriticalLogger, StringLogger
		* Added ILogTimestampProvider and changed the default to local time
V0.2.13.0
	[kevgrig@gmail.com]
		* More bug fixes for workitem http://www.codeplex.com/publicdomain/WorkItem/View.aspx?WorkItemId=12159
V0.2.12.0
	[kevgrig@gmail.com]
		* BUG: http://www.codeplex.com/publicdomain/WorkItem/View.aspx?WorkItemId=12159
V0.2.11.0
	[kevgrig@gmail.com]
		* Updated LoggingConfig
V0.2.10.0
	[kevgrig@gmail.com]
		* Added threading to logging
V0.2.9.0
	[kevgrig@gmail.com]
		* Historical TzZoneInfo exposed through TzTimeZone
		* Added capability to retrieve DaylightTime info for a particular year
V0.2.8.0
	[kevgrig@gmail.com]
		* Added CurrentDirectoryRerouter
		* Updated olson tz database to 2007f
		* Fixed various tests
V0.2.7.0
	[kevgrig@gmail.com]
		* BUG: ExceptionUtilities didn't throw a list of Exceptions
V0.2.6.0
	[kevgrig@gmail.com]
		* Log guards
V0.2.5.0
	[kevgrig@gmail.com]
		* Refactored PublicDomain solution
V0.2.4.0
	[kevgrig@gmail.com]
		* BUG: TzDateTime should not serialize DateTimeLocal
V0.2.3.0
	[kevgrig@gmail.com]
		* Added Quadruple class
		* Fixed bug reading malformed RSS feed date/time
V0.2.2.0
	[kevgrig@gmail.com]
		* Added ManagementUtilities class with GetTotalPhysicalMemory method that uses System.Management
V0.2.1.0
	[kevgrig@gmail.com]
		* Added Simon Mourier's CRC32 class
		* Added MD5 Sum method to StringUtilities
V0.2.0.0
	[kevgrig@gmail.com]
		* Basic feature set is in. Needs to be used by the massive.
		* Took out default VJ# dependency so that installing PublicDomain doesn't require vjslib.
V0.1.30.0
	[kevgrig@gmail.com]
		* Added credential support to ScreenScraper
		* Added ThreadingUtilities class (SetTimeout and SetInterval helper methods)
		* Added XmlUtilities class (FormatXml method)
V0.1.28.0
	[kevgrig@gmail.com]
		* Removed FileStream caching from FileLogger due to WebDev.WebServer crashing issue
V0.1.27.0
	[kevgrig@gmail.com]
		* Cache FileStream objects in FileLogger
		* Added option for multiple categories when creating Logger
V0.1.26.0
	[kevgrig@gmail.com]
		* Interesting new support for limiting the max working set of a process,
		  using PInvoke into Win32 methods CreateJobObject, SetInformationJobObject,
		  and AssignProcessToJobObject
		* Added ASP.NET Runtime Host courtesy of Rick Strahl
		* Added wrappers for Win32.GetFreeDiskSpace methods
		* Added SimpleCompositeLogger and SimpleLogFormatter classes for the most common logger usage,
		  as well as the NullLogger
		* Added the LoggingConfig class to parse a log option string and retrieve loggers.
		* Added ConfigurationValues class for common XML option file parsing
V0.1.24.0
	[kevgrig@gmail.com]
		* Added PublicDomain.Configuration
		* Added LoggingConfig class to load a log string and return loggers
		* Added Rick Strahl's Public Domain ASP.Net Runtime host
V0.1.23.0
	[pierrevachon@gmail.com]
		* Bug fix in the semantics of GlobalConstants
V0.1.22.0
	[kevgrig@gmail.com]
		* Added ASpell SpellChecker class
V0.1.18.0
	[kevgrig@gmail.com]
		* Changed PublicDomain.ILogger to use DateTime instead of TzDateTime for better initial performance
V0.1.17.0
	[kevgrig@gmail.com]
		* Added ApplicationLogger class
		* Versions better coincide with setup versions
V0.0.2.32
	[kevgrig@gmail.com]
		* Added GlobalAssemblyCache class to wrap Fusion DLLs
		* Added IInstallProgram interface and Win32.GetAddRemoveProgramList to
		get the same data as the Add/Remove program list and manipulate it, including
		running uninstallers
		* Added ConsoleRerouter class
		* Added Dynacode package
		* Added Iso8601 class, and main time zone points for each zone, also TimeZoneLocal
V0.0.2.23
	[kevgrig@gmail.com]
		* TzDateTime modifications, adding a UtcOffset property, and local ToString methods.
		* Fixed WorkItem 7385 (http://www.codeplex.com/publicdomain/WorkItem/View.aspx?WorkItemId=7385)
V0.0.2.22
	[kevgrig@gmail.com]
		* Added Cryptography, Encoding, and Hashing utilities on strings
		* Fixed bugs in PublicDomain.Logging.FileSizeRolloverStrategy
V0.0.2.5
	[kevgrig@gmail.com]
		* Added CompositeLogger
V0.0.2.4
	[kevgrig@gmail.com]
		* Added my logging package, PublicDomain.Logging
V0.0.2.3
	[kevgrig@gmail.com]
		* Added RSS, Atom, and OPML parsing and serialization support in the Feeder package
V0.0.2.2
	[kevgrig@gmail.com]
		* TzDateTime creation methods
V0.0.2.0
	[kevgrig@gmail.com]
		* TzTimeZone is very limited but functional. Get a time zone with TzTimeZone.GetTimeZone(string)
V0.0.1.4
	[kevgrig@gmail.com]
		* Added ArrayUtilities.RemoveDuplicates<T>(IList<T>)
		* Added libraries for counting code (ICountable, CountStream, etc.)
V0.0.1.3
	[kevgrig@gmail.com]
		* Added ReadOnlyDictionary<K, V> and ReadOnlyICollection<T> classes
		* Generation of TzZone data -- still nothing functional in time zones though
V0.0.1.2
	[kevgrig@gmail.com]
		* Added pdsetup project
V0.0.1.1
	[kevgrig@gmail.com]
		* Added bunch of methods to ConversionUtilities courtesy of
		William M. Leszczuk (billl@eden.rutgers.edu)
		* Parsing of tz files works
V0.0.1.0
	[kevgrig@gmail.com]
		* Project creation in CodePlex (http://www.codeplex.com/PublicDomain)
		* Added various code from my projects
		* tz database code unfinished
V0.0.0.1
	[kevgrig@gmail.com]
		* Added Win32 class and some ExitWindowsEx calls
V0.0.0.0
	[kevgrig@gmail.com]
		* Wrapper around vjslib for zip file reading
		* java.io.InputStream <-> System.IO.Stream wrappers
