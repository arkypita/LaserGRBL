// Copyright © Jason Curl 2012-2020
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SerialPortStream")]
[assembly: AssemblyDescription("Serial Port abstraction DLL")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("RJCP")]
[assembly: AssemblyProduct("SerialPortStream")]
[assembly: AssemblyCopyright("Copyright © Jason Curl 2012-2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

#if !SIGNED_RELEASE
[assembly: InternalsVisibleTo("RJCP.DatastructuresTest")]
#endif

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

[assembly: CLSCompliant(true)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("4B68A130-96A9-4028-ADAA-3596F568F17E")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2.2.2.0")]
[assembly: AssemblyFileVersion("2.2.2.0")]
