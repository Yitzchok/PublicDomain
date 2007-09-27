using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;
using System.Security;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PublicDomain")]
[assembly: AssemblyDescription("Public Domain code")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Public Domain")]
[assembly: AssemblyProduct("PublicDomain")]
[assembly: AssemblyCopyright("Public Domain")]
[assembly: AssemblyTrademark("Public Domain")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(true)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8cc2deec-79f9-4c0e-aeb1-9cec0cdcea8a")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion(PublicDomain.GlobalConstants.PublicDomainVersion)]
[assembly: AssemblyFileVersion(PublicDomain.GlobalConstants.PublicDomainFileVersion)]
[assembly: CLSCompliant(true)]
[assembly: AllowPartiallyTrustedCallers]