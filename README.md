# WizComponentGenerator
Console application that inserts components from a projects output folder into a [Wix](http://wixtoolset.org/) setup file (.wxs)

Used for building .wxs file for [Winleafs](https://github.com/StijnOostdam/Winleafs)

Currently only supports 1 nested folder.

Arguments:
1. Source project folder
2. Output folder
3. Input .wxs file path
4. Root directory ID
5. Application feature ID

Example:

.wxs file:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<Wix xmlns="http://schemas.microsoft.com/wix/2006/wi"
     xmlns:netfx="http://schemas.microsoft.com/wix/NetFxExtension">
  <?define sourceFolder="..\Nanoleaf-wpf\bin\Release\"?>
  <Product Id="*" UpgradeCode="42ee9157-4382-4852-bed7-72b25fb2d0bf" Version="1.0.0.0" Language="1033" Name="Winleafs" Manufacturer="Stijn Oostdam">
    <Package Compressed="yes"/>
    <Media Id="1" Cabinet="myapplication.cab" EmbedCab="yes" />

    <!-- Add installer icon -->
    <Icon Id="icon.ico" SourceFile="$(var.sourceFolder)..\..\..\Media\Winleafs.ico"/>
    <Property Id="ARPPRODUCTICON" Value="icon.ico" />

    <!-- Step  1: Define the directory structure -->
    <Directory Id="TARGETDIR" Name="SourceDir">
      <Directory Id="ProgramFilesFolder">
        <Directory Id="APPLICATIONROOTDIRECTORY" Name="Winleafs"/>
      </Directory>
      <Directory Id="ProgramMenuFolder">
        <Directory Id="ApplicationProgramsFolder" Name="Winleafs"/>
      </Directory>
      <Directory Id="DesktopFolder" Name="Desktop">
      </Directory>
    </Directory>

    <!-- Step 2: Add files to your installer package -->
    <DirectoryRef Id="APPLICATIONROOTDIRECTORY">
    </DirectoryRef>

    <DirectoryRef Id="ApplicationProgramsFolder">
      <Component Id="ApplicationShortcut" Guid="33bf752d-c561-4abe-b830-450437a70da6">
        <Shortcut Id="ApplicationStartMenuShortcut"
            Name="Winleafs"
            Description="Windows application to contrl your Nanoleaf lights"
            Target="[#Winleafs.Wpf.exe]"
            WorkingDirectory="APPLICATIONROOTDIRECTORY"/>
        <RemoveFolder Id="CleanUpShortCut" Directory="ApplicationProgramsFolder" On="uninstall"/>
        <RegistryValue Root="HKCU" Key="Software\Winleafs" Name="installed" Type="integer" Value="1" KeyPath="yes"/>
      </Component>
    </DirectoryRef>

    <DirectoryRef Id="DesktopFolder">
      <Component Id="ApplicationShortcutDesktop" Guid="*">
        <Shortcut Id="ApplicationDesktopShortcut"
            Name="Winleafs"
            Description="Windows application to contrl your Nanoleaf lights"
            Target="[#Winleafs.Wpf.exe]"
            WorkingDirectory="APPLICATIONROOTDIRECTORY"/>
        <RemoveFolder Id="DesktopFolder" On="uninstall"/>
        <RegistryValue
            Root="HKCU"
            Key="Software/Winleafs"
            Name="installed"
            Type="integer"
            Value="1"
            KeyPath="yes"/>
      </Component>
    </DirectoryRef>

    <!-- Step 3: Tell WiX to install the files -->
    <Feature Id="MainApplication" Title="Main Application" Level="1">
      <ComponentRef Id="ApplicationShortcut" />
      <ComponentRef Id="ApplicationShortcutDesktop" />
    </Feature>
    <!-- Step 4: Sign the MSI Install, else Web user will get unknown publisher message -->
    <!-- Signing is done by executing a post-build batch file configured in project properties -->

  </Product>
</Wix>
```

Calling `"C:\Users\YOURNAME\Source\Repos\StijnOostdam\Nanoleaf-wpf\Nanoleaf-wpf\bin\Release" "C:\Users\YOURNAME\Source\Repos\StijnOostdam\Nanoleaf-wpf\Setup"  "C:\Users\YOURNAME\Source\Repos\StijnOostdam\WizComponentGenerator\Test.wxs" APPLICATIONROOTDIRECTORY MainApplication` will result in:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<Wix xmlns="http://schemas.microsoft.com/wix/2006/wi" xmlns:netfx="http://schemas.microsoft.com/wix/NetFxExtension">
  <?define sourceFolder="..\Nanoleaf-wpf\bin\Release\"?>
  <Product Id="*" UpgradeCode="42ee9157-4382-4852-bed7-72b25fb2d0bf" Version="0.3.2" Language="1033" Name="Winleafs" Manufacturer="Stijn Oostdam">
    <Package Compressed="yes" />
    <Media Id="1" Cabinet="myapplication.cab" EmbedCab="yes" />
    <!-- Add installer icon -->
    <Icon Id="icon.ico" SourceFile="$(var.sourceFolder)..\..\..\Media\Winleafs.ico" />
    <Property Id="ARPPRODUCTICON" Value="icon.ico" />
    <!-- Step  1: Define the directory structure -->
    <Directory Id="TARGETDIR" Name="SourceDir">
      <Directory Id="ProgramFilesFolder">
        <Directory Id="APPLICATIONROOTDIRECTORY" Name="Winleafs">
          <Directory Id="directory_nl" Name="nl" />
        </Directory>
      </Directory>
      <Directory Id="ProgramMenuFolder">
        <Directory Id="ApplicationProgramsFolder" Name="Winleafs" />
      </Directory>
      <Directory Id="DesktopFolder" Name="Desktop">
      </Directory>
    </Directory>
    <!-- Step 2: Add files to your installer package -->
    <DirectoryRef Id="APPLICATIONROOTDIRECTORY">
      <Component Id="Hardcodet.Wpf.TaskbarNotification.dll" Guid="06ad2df4-5946-4759-83e3-026a3ab492ee">
        <File Id="Hardcodet.Wpf.TaskbarNotification.dll" Source="$(var.sourceFolder)\Hardcodet.Wpf.TaskbarNotification.dll" KeyPath="yes" />
      </Component>
      <Component Id="Hardcodet.Wpf.TaskbarNotification.xml" Guid="2b94787a-f8d1-4751-927b-bb6765fb6d6a">
        <File Id="Hardcodet.Wpf.TaskbarNotification.xml" Source="$(var.sourceFolder)\Hardcodet.Wpf.TaskbarNotification.xml" KeyPath="yes" />
      </Component>
      <Component Id="JsonMigrator.dll" Guid="cf867448-69fa-45d5-b9bb-86aa0135a6d8">
        <File Id="JsonMigrator.dll" Source="$(var.sourceFolder)\JsonMigrator.dll" KeyPath="yes" />
      </Component>
      <Component Id="Microsoft.Win32.Primitives.dll" Guid="69ff0ad1-6011-48b3-b5c1-3b66eb065fb4">
        <File Id="Microsoft.Win32.Primitives.dll" Source="$(var.sourceFolder)\Microsoft.Win32.Primitives.dll" KeyPath="yes" />
      </Component>
      <Component Id="netstandard.dll" Guid="ffea10cb-4223-4659-bfc9-09176021c1cd">
        <File Id="netstandard.dll" Source="$(var.sourceFolder)\netstandard.dll" KeyPath="yes" />
      </Component>
      <Component Id="Newtonsoft.Json.dll" Guid="2ff47280-f3e7-4be4-8bbb-68601444e176">
        <File Id="Newtonsoft.Json.dll" Source="$(var.sourceFolder)\Newtonsoft.Json.dll" KeyPath="yes" />
      </Component>
      <Component Id="Newtonsoft.Json.xml" Guid="a50d755d-ca5a-44f0-9065-37db9b84bff2">
        <File Id="Newtonsoft.Json.xml" Source="$(var.sourceFolder)\Newtonsoft.Json.xml" KeyPath="yes" />
      </Component>
      <Component Id="NLog.config" Guid="77149d35-7073-470a-9e78-cf9724eaa9f1">
        <File Id="NLog.config" Source="$(var.sourceFolder)\NLog.config" KeyPath="yes" />
      </Component>
      <Component Id="NLog.dll" Guid="c7e002b8-70d5-4fa5-89eb-4a992690b810">
        <File Id="NLog.dll" Source="$(var.sourceFolder)\NLog.dll" KeyPath="yes" />
      </Component>
      <Component Id="NLog.xml" Guid="6e4a8cb9-3306-4651-a426-b774eb7b488b">
        <File Id="NLog.xml" Source="$(var.sourceFolder)\NLog.xml" KeyPath="yes" />
      </Component>
      <Component Id="Octokit.dll" Guid="d8195bb3-312d-4870-8b58-11b1bce1d91d">
        <File Id="Octokit.dll" Source="$(var.sourceFolder)\Octokit.dll" KeyPath="yes" />
      </Component>
      <Component Id="Octokit.xml" Guid="f783de26-f7a0-48dc-8d57-74e8eef3ad3f">
        <File Id="Octokit.xml" Source="$(var.sourceFolder)\Octokit.xml" KeyPath="yes" />
      </Component>
      <Component Id="Polly.dll" Guid="43089f18-dd1a-44df-96eb-a7d6d85c664c">
        <File Id="Polly.dll" Source="$(var.sourceFolder)\Polly.dll" KeyPath="yes" />
      </Component>
      <Component Id="Polly.xml" Guid="c1c7ecbf-9fe1-43aa-8c44-e490395efc7c">
        <File Id="Polly.xml" Source="$(var.sourceFolder)\Polly.xml" KeyPath="yes" />
      </Component>
      <Component Id="RestSharp.dll" Guid="6a6a4cea-110c-4407-bc1d-052a1ae96989">
        <File Id="RestSharp.dll" Source="$(var.sourceFolder)\RestSharp.dll" KeyPath="yes" />
      </Component>
      <Component Id="RestSharp.xml" Guid="610763ee-fecf-4033-89ce-c28e2b77a243">
        <File Id="RestSharp.xml" Source="$(var.sourceFolder)\RestSharp.xml" KeyPath="yes" />
      </Component>
      <Component Id="System.AppContext.dll" Guid="9c426008-86aa-4f87-92ff-eea7ff4ae13d">
        <File Id="System.AppContext.dll" Source="$(var.sourceFolder)\System.AppContext.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Collections.Concurrent.dll" Guid="cd35f166-580f-48a8-b947-5174106522ed">
        <File Id="System.Collections.Concurrent.dll" Source="$(var.sourceFolder)\System.Collections.Concurrent.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Collections.dll" Guid="fbb11a15-0972-492f-8d2c-ec0211111d9d">
        <File Id="System.Collections.dll" Source="$(var.sourceFolder)\System.Collections.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Collections.NonGeneric.dll" Guid="72e7ce38-a538-4231-bd54-b39030b67481">
        <File Id="System.Collections.NonGeneric.dll" Source="$(var.sourceFolder)\System.Collections.NonGeneric.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Collections.Specialized.dll" Guid="cabd8e1f-6cda-4005-96a3-ef9b94f7bf90">
        <File Id="System.Collections.Specialized.dll" Source="$(var.sourceFolder)\System.Collections.Specialized.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.ComponentModel.dll" Guid="55f640cc-faaa-457d-bcdb-9a960b67d77c">
        <File Id="System.ComponentModel.dll" Source="$(var.sourceFolder)\System.ComponentModel.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.ComponentModel.EventBasedAsync.dll" Guid="0b93ca8a-1682-43c6-8147-8cf46df49e46">
        <File Id="System.ComponentModel.EventBasedAsync.dll" Source="$(var.sourceFolder)\System.ComponentModel.EventBasedAsync.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.ComponentModel.Primitives.dll" Guid="e7ca6850-c4d1-4eb6-990d-7bbbed7b9cfc">
        <File Id="System.ComponentModel.Primitives.dll" Source="$(var.sourceFolder)\System.ComponentModel.Primitives.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.ComponentModel.TypeConverter.dll" Guid="b5388280-c176-4d37-89ed-2dc16b641bb6">
        <File Id="System.ComponentModel.TypeConverter.dll" Source="$(var.sourceFolder)\System.ComponentModel.TypeConverter.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Console.dll" Guid="8fd1a9b4-99fb-45a4-91d8-9a14ab05a7e4">
        <File Id="System.Console.dll" Source="$(var.sourceFolder)\System.Console.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Data.Common.dll" Guid="c03509c2-0818-4eb1-9725-572367695ad5">
        <File Id="System.Data.Common.dll" Source="$(var.sourceFolder)\System.Data.Common.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Diagnostics.Contracts.dll" Guid="f3a1c3b9-0a7d-4019-a633-4b0f6cd84ade">
        <File Id="System.Diagnostics.Contracts.dll" Source="$(var.sourceFolder)\System.Diagnostics.Contracts.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Diagnostics.Debug.dll" Guid="3411168a-a03f-4a37-8a35-73a1d7be3a9f">
        <File Id="System.Diagnostics.Debug.dll" Source="$(var.sourceFolder)\System.Diagnostics.Debug.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Diagnostics.FileVersionInfo.dll" Guid="6a24f963-366c-4541-8e00-5c7f6afaedba">
        <File Id="System.Diagnostics.FileVersionInfo.dll" Source="$(var.sourceFolder)\System.Diagnostics.FileVersionInfo.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Diagnostics.Process.dll" Guid="eeecd461-0045-4134-8e14-d0cbc0c4bccb">
        <File Id="System.Diagnostics.Process.dll" Source="$(var.sourceFolder)\System.Diagnostics.Process.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Diagnostics.StackTrace.dll" Guid="dbc88bb0-8c21-4ca1-baf4-84e5cfec6234">
        <File Id="System.Diagnostics.StackTrace.dll" Source="$(var.sourceFolder)\System.Diagnostics.StackTrace.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Diagnostics.TextWriterTraceListener.dll" Guid="4c291190-1532-4bcd-bfcf-866945fd0f9f">
        <File Id="System.Diagnostics.TextWriterTraceListener.dll" Source="$(var.sourceFolder)\System.Diagnostics.TextWriterTraceListener.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Diagnostics.Tools.dll" Guid="127c1e30-93ac-4779-a15a-f42dc2f3aea7">
        <File Id="System.Diagnostics.Tools.dll" Source="$(var.sourceFolder)\System.Diagnostics.Tools.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Diagnostics.TraceSource.dll" Guid="b65cb441-38ce-40b4-93f9-3c8564e33793">
        <File Id="System.Diagnostics.TraceSource.dll" Source="$(var.sourceFolder)\System.Diagnostics.TraceSource.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Diagnostics.Tracing.dll" Guid="b452ac6a-caac-42da-b347-ee3c3d8b21af">
        <File Id="System.Diagnostics.Tracing.dll" Source="$(var.sourceFolder)\System.Diagnostics.Tracing.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Drawing.Primitives.dll" Guid="c0c1fac0-2330-4838-8afb-cc94277c55db">
        <File Id="System.Drawing.Primitives.dll" Source="$(var.sourceFolder)\System.Drawing.Primitives.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Dynamic.Runtime.dll" Guid="1483db87-68b8-4b88-b60e-dd60fec28ec2">
        <File Id="System.Dynamic.Runtime.dll" Source="$(var.sourceFolder)\System.Dynamic.Runtime.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Globalization.Calendars.dll" Guid="f6a9bbda-ac4c-475a-bccc-b98eba676c1b">
        <File Id="System.Globalization.Calendars.dll" Source="$(var.sourceFolder)\System.Globalization.Calendars.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Globalization.dll" Guid="b21265c9-82c8-4cc9-8407-5733d27794e4">
        <File Id="System.Globalization.dll" Source="$(var.sourceFolder)\System.Globalization.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Globalization.Extensions.dll" Guid="72c1f124-2b7a-4d7f-af01-cc71fb4ea2dd">
        <File Id="System.Globalization.Extensions.dll" Source="$(var.sourceFolder)\System.Globalization.Extensions.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.IO.Compression.dll" Guid="6f83c0b9-2615-4d11-bb52-8a0a19ab59c3">
        <File Id="System.IO.Compression.dll" Source="$(var.sourceFolder)\System.IO.Compression.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.IO.Compression.ZipFile.dll" Guid="e93c2da6-b967-4074-ad01-6e9b1d5afab8">
        <File Id="System.IO.Compression.ZipFile.dll" Source="$(var.sourceFolder)\System.IO.Compression.ZipFile.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.IO.dll" Guid="984283e6-a0b8-45ac-8ea3-60ecdf462e53">
        <File Id="System.IO.dll" Source="$(var.sourceFolder)\System.IO.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.IO.FileSystem.dll" Guid="ee13cdf7-df7b-4abf-bb05-b2af57d62777">
        <File Id="System.IO.FileSystem.dll" Source="$(var.sourceFolder)\System.IO.FileSystem.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.IO.FileSystem.DriveInfo.dll" Guid="9fc289b0-4ae7-419c-84c7-dd47e815aed4">
        <File Id="System.IO.FileSystem.DriveInfo.dll" Source="$(var.sourceFolder)\System.IO.FileSystem.DriveInfo.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.IO.FileSystem.Primitives.dll" Guid="e0c2c940-0f17-43b4-aa18-ecf7814b754c">
        <File Id="System.IO.FileSystem.Primitives.dll" Source="$(var.sourceFolder)\System.IO.FileSystem.Primitives.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.IO.FileSystem.Watcher.dll" Guid="25039182-93c1-43dd-9d57-459d8103a56d">
        <File Id="System.IO.FileSystem.Watcher.dll" Source="$(var.sourceFolder)\System.IO.FileSystem.Watcher.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.IO.IsolatedStorage.dll" Guid="d17b46ac-72f1-422d-8c1e-e827289e113c">
        <File Id="System.IO.IsolatedStorage.dll" Source="$(var.sourceFolder)\System.IO.IsolatedStorage.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.IO.MemoryMappedFiles.dll" Guid="4c16eee8-cfb2-45a2-82f2-2ffe67453044">
        <File Id="System.IO.MemoryMappedFiles.dll" Source="$(var.sourceFolder)\System.IO.MemoryMappedFiles.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.IO.Pipes.dll" Guid="4cc17c5f-d5e0-4ec4-a3e3-28962b93df49">
        <File Id="System.IO.Pipes.dll" Source="$(var.sourceFolder)\System.IO.Pipes.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.IO.UnmanagedMemoryStream.dll" Guid="df22f580-dd7a-4326-9650-dd007baabb74">
        <File Id="System.IO.UnmanagedMemoryStream.dll" Source="$(var.sourceFolder)\System.IO.UnmanagedMemoryStream.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Linq.dll" Guid="a36bb473-6125-4f99-b59b-6db8d2dd940b">
        <File Id="System.Linq.dll" Source="$(var.sourceFolder)\System.Linq.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Linq.Expressions.dll" Guid="313dc814-3744-450d-a6a5-7176c1fdefeb">
        <File Id="System.Linq.Expressions.dll" Source="$(var.sourceFolder)\System.Linq.Expressions.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Linq.Parallel.dll" Guid="a89cf7ea-bd7d-46d5-91a8-3ebaad678c13">
        <File Id="System.Linq.Parallel.dll" Source="$(var.sourceFolder)\System.Linq.Parallel.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Linq.Queryable.dll" Guid="69446de2-e17d-4eaa-802f-afafbe3a20f3">
        <File Id="System.Linq.Queryable.dll" Source="$(var.sourceFolder)\System.Linq.Queryable.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Net.Http.dll" Guid="498dceff-efcd-4d08-8385-04870f6c1662">
        <File Id="System.Net.Http.dll" Source="$(var.sourceFolder)\System.Net.Http.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Net.NameResolution.dll" Guid="f34a7da8-aa82-4e6f-a7ae-a9495b0edb32">
        <File Id="System.Net.NameResolution.dll" Source="$(var.sourceFolder)\System.Net.NameResolution.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Net.NetworkInformation.dll" Guid="65d269a8-b73c-4b80-9a83-ee45d53cb167">
        <File Id="System.Net.NetworkInformation.dll" Source="$(var.sourceFolder)\System.Net.NetworkInformation.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Net.Ping.dll" Guid="5f257fda-e384-46c4-a719-5306e002d514">
        <File Id="System.Net.Ping.dll" Source="$(var.sourceFolder)\System.Net.Ping.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Net.Primitives.dll" Guid="76e6aa39-efde-49c1-88c1-6b945e952079">
        <File Id="System.Net.Primitives.dll" Source="$(var.sourceFolder)\System.Net.Primitives.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Net.Requests.dll" Guid="ffd3b4fc-b404-431b-bcef-611ee64fee47">
        <File Id="System.Net.Requests.dll" Source="$(var.sourceFolder)\System.Net.Requests.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Net.Security.dll" Guid="a4649b36-5007-4985-8533-a78873a94c06">
        <File Id="System.Net.Security.dll" Source="$(var.sourceFolder)\System.Net.Security.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Net.Sockets.dll" Guid="4f71cb53-136e-4722-8011-56e8a1407703">
        <File Id="System.Net.Sockets.dll" Source="$(var.sourceFolder)\System.Net.Sockets.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Net.WebHeaderCollection.dll" Guid="abd76935-4235-4785-a9c5-f1eb49e37dad">
        <File Id="System.Net.WebHeaderCollection.dll" Source="$(var.sourceFolder)\System.Net.WebHeaderCollection.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Net.WebSockets.Client.dll" Guid="7447e28f-0cdf-4b08-aefa-9fdfe64e3757">
        <File Id="System.Net.WebSockets.Client.dll" Source="$(var.sourceFolder)\System.Net.WebSockets.Client.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Net.WebSockets.dll" Guid="0999bcdd-ddef-4ce6-a2b4-5d8830804e61">
        <File Id="System.Net.WebSockets.dll" Source="$(var.sourceFolder)\System.Net.WebSockets.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.ObjectModel.dll" Guid="462cb810-6eca-4771-8921-13669ef14ec1">
        <File Id="System.ObjectModel.dll" Source="$(var.sourceFolder)\System.ObjectModel.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Reflection.dll" Guid="e81ed743-6f5d-4909-9eb7-e6974bab6a9d">
        <File Id="System.Reflection.dll" Source="$(var.sourceFolder)\System.Reflection.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Reflection.Extensions.dll" Guid="0cc24469-1fcf-4266-b9b1-9d6297aeb2d0">
        <File Id="System.Reflection.Extensions.dll" Source="$(var.sourceFolder)\System.Reflection.Extensions.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Reflection.Primitives.dll" Guid="d2bfa5a5-3157-4d64-bdfc-05d32836b454">
        <File Id="System.Reflection.Primitives.dll" Source="$(var.sourceFolder)\System.Reflection.Primitives.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Resources.Reader.dll" Guid="888eb3cc-b4cf-46dc-94aa-f1bf9fabb0d9">
        <File Id="System.Resources.Reader.dll" Source="$(var.sourceFolder)\System.Resources.Reader.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Resources.ResourceManager.dll" Guid="71b650bd-71a0-4fba-aec3-21864ffae29c">
        <File Id="System.Resources.ResourceManager.dll" Source="$(var.sourceFolder)\System.Resources.ResourceManager.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Resources.Writer.dll" Guid="7bca6879-437a-4206-8f50-88cc57bb67fa">
        <File Id="System.Resources.Writer.dll" Source="$(var.sourceFolder)\System.Resources.Writer.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Runtime.CompilerServices.VisualC.dll" Guid="893353f1-0e54-4cf3-94c9-b933eaeb14c6">
        <File Id="System.Runtime.CompilerServices.VisualC.dll" Source="$(var.sourceFolder)\System.Runtime.CompilerServices.VisualC.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Runtime.dll" Guid="98ff6297-655b-45b7-8695-a695dfd1d02b">
        <File Id="System.Runtime.dll" Source="$(var.sourceFolder)\System.Runtime.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Runtime.Extensions.dll" Guid="a75069e4-d39a-43d7-b9e7-fe52ad7d20e9">
        <File Id="System.Runtime.Extensions.dll" Source="$(var.sourceFolder)\System.Runtime.Extensions.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Runtime.Handles.dll" Guid="d7fc59d2-3616-432c-bbea-1d44f1e57a42">
        <File Id="System.Runtime.Handles.dll" Source="$(var.sourceFolder)\System.Runtime.Handles.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Runtime.InteropServices.dll" Guid="3ea798aa-82b7-406a-91cc-64fa0035bb8a">
        <File Id="System.Runtime.InteropServices.dll" Source="$(var.sourceFolder)\System.Runtime.InteropServices.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Runtime.InteropServices.RuntimeInformation.dll" Guid="9c322ac9-8f77-46ca-aa30-80ddec75662a">
        <File Id="System.Runtime.InteropServices.RuntimeInformation.dll" Source="$(var.sourceFolder)\System.Runtime.InteropServices.RuntimeInformation.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Runtime.Numerics.dll" Guid="655a975f-ec9e-46f1-8d4c-920eb2b82997">
        <File Id="System.Runtime.Numerics.dll" Source="$(var.sourceFolder)\System.Runtime.Numerics.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Runtime.Serialization.Formatters.dll" Guid="8ab28b4e-75c0-4012-a84c-59fb2b39dfaa">
        <File Id="System.Runtime.Serialization.Formatters.dll" Source="$(var.sourceFolder)\System.Runtime.Serialization.Formatters.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Runtime.Serialization.Json.dll" Guid="cfa9725f-628c-4c3d-a2e3-8ed57ec60715">
        <File Id="System.Runtime.Serialization.Json.dll" Source="$(var.sourceFolder)\System.Runtime.Serialization.Json.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Runtime.Serialization.Primitives.dll" Guid="2a8535af-47ab-4598-a4c8-f67bdcd8e164">
        <File Id="System.Runtime.Serialization.Primitives.dll" Source="$(var.sourceFolder)\System.Runtime.Serialization.Primitives.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Runtime.Serialization.Xml.dll" Guid="95fbc7a8-3994-4419-aecc-e33a7716dcab">
        <File Id="System.Runtime.Serialization.Xml.dll" Source="$(var.sourceFolder)\System.Runtime.Serialization.Xml.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Security.Claims.dll" Guid="1ffa5bd7-67d1-404f-bc51-8b7bd7136c9a">
        <File Id="System.Security.Claims.dll" Source="$(var.sourceFolder)\System.Security.Claims.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Security.Cryptography.Algorithms.dll" Guid="8042484a-8320-41b3-941b-c60ac71abe2b">
        <File Id="System.Security.Cryptography.Algorithms.dll" Source="$(var.sourceFolder)\System.Security.Cryptography.Algorithms.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Security.Cryptography.Csp.dll" Guid="e30fa51a-186a-42a3-a9ed-89d14979ee45">
        <File Id="System.Security.Cryptography.Csp.dll" Source="$(var.sourceFolder)\System.Security.Cryptography.Csp.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Security.Cryptography.Encoding.dll" Guid="4589855a-2b41-4d05-9651-1a2ea5909a48">
        <File Id="System.Security.Cryptography.Encoding.dll" Source="$(var.sourceFolder)\System.Security.Cryptography.Encoding.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Security.Cryptography.Primitives.dll" Guid="8903ddf8-df57-4323-8747-5f24108ba5eb">
        <File Id="System.Security.Cryptography.Primitives.dll" Source="$(var.sourceFolder)\System.Security.Cryptography.Primitives.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Security.Cryptography.X509Certificates.dll" Guid="82455e52-3e20-4c56-bc21-94dbc0871f31">
        <File Id="System.Security.Cryptography.X509Certificates.dll" Source="$(var.sourceFolder)\System.Security.Cryptography.X509Certificates.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Security.Principal.dll" Guid="ab304cf4-3f55-4a78-96a2-118299a27518">
        <File Id="System.Security.Principal.dll" Source="$(var.sourceFolder)\System.Security.Principal.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Security.SecureString.dll" Guid="71101643-9ccb-48c3-a747-f4fee9886f01">
        <File Id="System.Security.SecureString.dll" Source="$(var.sourceFolder)\System.Security.SecureString.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Text.Encoding.dll" Guid="470d7b2c-5853-40f7-b4d5-34bf2afaed50">
        <File Id="System.Text.Encoding.dll" Source="$(var.sourceFolder)\System.Text.Encoding.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Text.Encoding.Extensions.dll" Guid="e66cf765-4491-4d86-9c5d-6fa6a0ed448a">
        <File Id="System.Text.Encoding.Extensions.dll" Source="$(var.sourceFolder)\System.Text.Encoding.Extensions.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Text.RegularExpressions.dll" Guid="9861b8da-e67d-4edd-93b8-c28cbe802bab">
        <File Id="System.Text.RegularExpressions.dll" Source="$(var.sourceFolder)\System.Text.RegularExpressions.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Threading.dll" Guid="66889932-e738-448c-b542-4b2b3ed7882e">
        <File Id="System.Threading.dll" Source="$(var.sourceFolder)\System.Threading.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Threading.Overlapped.dll" Guid="29475d59-9fd3-4ea5-a7bd-07e998d018cc">
        <File Id="System.Threading.Overlapped.dll" Source="$(var.sourceFolder)\System.Threading.Overlapped.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Threading.Tasks.dll" Guid="2bfefc5b-3f8b-4284-92b9-e95158b1f76f">
        <File Id="System.Threading.Tasks.dll" Source="$(var.sourceFolder)\System.Threading.Tasks.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Threading.Tasks.Parallel.dll" Guid="0a977c2f-e09b-4228-9781-207593db3abf">
        <File Id="System.Threading.Tasks.Parallel.dll" Source="$(var.sourceFolder)\System.Threading.Tasks.Parallel.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Threading.Thread.dll" Guid="9276bc8e-d2f9-4cc7-bb82-0c8e0a92a007">
        <File Id="System.Threading.Thread.dll" Source="$(var.sourceFolder)\System.Threading.Thread.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Threading.ThreadPool.dll" Guid="6b558eac-c6f5-431a-8598-4ecd5d7a4e2e">
        <File Id="System.Threading.ThreadPool.dll" Source="$(var.sourceFolder)\System.Threading.ThreadPool.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Threading.Timer.dll" Guid="a7c1feaa-017e-423d-9be0-f34461245d39">
        <File Id="System.Threading.Timer.dll" Source="$(var.sourceFolder)\System.Threading.Timer.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.ValueTuple.dll" Guid="32f9ab19-55a7-46e0-9840-d1780aa15af4">
        <File Id="System.ValueTuple.dll" Source="$(var.sourceFolder)\System.ValueTuple.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Xml.ReaderWriter.dll" Guid="e63a9ed3-66ff-488a-afce-5aa644f9223c">
        <File Id="System.Xml.ReaderWriter.dll" Source="$(var.sourceFolder)\System.Xml.ReaderWriter.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Xml.XDocument.dll" Guid="b78db489-d6c3-4dc9-9ea0-b8e8080173d1">
        <File Id="System.Xml.XDocument.dll" Source="$(var.sourceFolder)\System.Xml.XDocument.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Xml.XmlDocument.dll" Guid="ca5f4696-1f80-43b6-98fd-d2c14e47b663">
        <File Id="System.Xml.XmlDocument.dll" Source="$(var.sourceFolder)\System.Xml.XmlDocument.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Xml.XmlSerializer.dll" Guid="1aded688-15ba-4e27-9798-7c182bc17c7f">
        <File Id="System.Xml.XmlSerializer.dll" Source="$(var.sourceFolder)\System.Xml.XmlSerializer.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Xml.XPath.dll" Guid="ae43d111-b1ce-4383-a6b0-8e373d5a3cb9">
        <File Id="System.Xml.XPath.dll" Source="$(var.sourceFolder)\System.Xml.XPath.dll" KeyPath="yes" />
      </Component>
      <Component Id="System.Xml.XPath.XDocument.dll" Guid="a91f17dd-950b-48a1-afa4-f294dc0c98ca">
        <File Id="System.Xml.XPath.XDocument.dll" Source="$(var.sourceFolder)\System.Xml.XPath.XDocument.dll" KeyPath="yes" />
      </Component>
      <Component Id="Tmds.MDns.dll" Guid="5a242aee-8ecf-48c0-96fe-bb5ca4e9c70e">
        <File Id="Tmds.MDns.dll" Source="$(var.sourceFolder)\Tmds.MDns.dll" KeyPath="yes" />
      </Component>
      <Component Id="Winleafs.Api.dll" Guid="fecc60d5-5959-4ea8-bd75-755954afdadd">
        <File Id="Winleafs.Api.dll" Source="$(var.sourceFolder)\Winleafs.Api.dll" KeyPath="yes" />
      </Component>
      <Component Id="Winleafs.External.dll" Guid="03ae861d-3c12-4be8-b04e-8f34e0e95a5b">
        <File Id="Winleafs.External.dll" Source="$(var.sourceFolder)\Winleafs.External.dll" KeyPath="yes" />
      </Component>
      <Component Id="Winleafs.Models.dll" Guid="a5aebc01-bd14-4f61-901e-756c1062bb42">
        <File Id="Winleafs.Models.dll" Source="$(var.sourceFolder)\Winleafs.Models.dll" KeyPath="yes" />
      </Component>
      <Component Id="Winleafs.Wpf.exe" Guid="0053cf12-0f5b-4248-945f-aa383c12a414">
        <File Id="Winleafs.Wpf.exe" Source="$(var.sourceFolder)\Winleafs.Wpf.exe" KeyPath="yes" Checksum="yes" />
      </Component>
      <Component Id="Winleafs.Wpf.exe.config" Guid="5af0dc46-ff3f-4f45-979e-b05ffb70207b">
        <File Id="Winleafs.Wpf.exe.config" Source="$(var.sourceFolder)\Winleafs.Wpf.exe.config" KeyPath="yes" />
      </Component>
    </DirectoryRef>
    <DirectoryRef Id="directory_nl">
      <Component Id="Winleafs.Wpf.resources.dll" Guid="32276e83-fd2a-48bb-8765-cd6634322c5f">
        <File Id="Winleafs.Wpf.resources.dll" Source="$(var.sourceFolder)\nl\Winleafs.Wpf.resources.dll" KeyPath="yes" />
      </Component>
    </DirectoryRef>
    <DirectoryRef Id="ApplicationProgramsFolder">
      <Component Id="ApplicationShortcut" Guid="33bf752d-c561-4abe-b830-450437a70da6">
        <Shortcut Id="ApplicationStartMenuShortcut" Name="Winleafs" Description="Windows application to contrl your Nanoleaf lights" Target="[#Winleafs.Wpf.exe]" WorkingDirectory="APPLICATIONROOTDIRECTORY" />
        <RemoveFolder Id="CleanUpShortCut" Directory="ApplicationProgramsFolder" On="uninstall" />
        <RegistryValue Root="HKCU" Key="Software\Winleafs" Name="installed" Type="integer" Value="1" KeyPath="yes" />
      </Component>
    </DirectoryRef>
    <DirectoryRef Id="DesktopFolder">
      <Component Id="ApplicationShortcutDesktop" Guid="*">
        <Shortcut Id="ApplicationDesktopShortcut" Name="Winleafs" Description="Windows application to contrl your Nanoleaf lights" Target="[#Winleafs.Wpf.exe]" WorkingDirectory="APPLICATIONROOTDIRECTORY" />
        <RemoveFolder Id="DesktopFolder" On="uninstall" />
        <RegistryValue Root="HKCU" Key="Software/Winleafs" Name="installed" Type="integer" Value="1" KeyPath="yes" />
      </Component>
    </DirectoryRef>
    <!-- Step 3: Tell WiX to install the files -->
    <Feature Id="MainApplication" Title="Main Application" Level="1">
      <ComponentRef Id="ApplicationShortcut" />
      <ComponentRef Id="ApplicationShortcutDesktop" />
      <ComponentRef Id="Hardcodet.Wpf.TaskbarNotification.dll" />
      <ComponentRef Id="Hardcodet.Wpf.TaskbarNotification.xml" />
      <ComponentRef Id="JsonMigrator.dll" />
      <ComponentRef Id="Microsoft.Win32.Primitives.dll" />
      <ComponentRef Id="netstandard.dll" />
      <ComponentRef Id="Newtonsoft.Json.dll" />
      <ComponentRef Id="Newtonsoft.Json.xml" />
      <ComponentRef Id="NLog.config" />
      <ComponentRef Id="NLog.dll" />
      <ComponentRef Id="NLog.xml" />
      <ComponentRef Id="Octokit.dll" />
      <ComponentRef Id="Octokit.xml" />
      <ComponentRef Id="Polly.dll" />
      <ComponentRef Id="Polly.xml" />
      <ComponentRef Id="RestSharp.dll" />
      <ComponentRef Id="RestSharp.xml" />
      <ComponentRef Id="System.AppContext.dll" />
      <ComponentRef Id="System.Collections.Concurrent.dll" />
      <ComponentRef Id="System.Collections.dll" />
      <ComponentRef Id="System.Collections.NonGeneric.dll" />
      <ComponentRef Id="System.Collections.Specialized.dll" />
      <ComponentRef Id="System.ComponentModel.dll" />
      <ComponentRef Id="System.ComponentModel.EventBasedAsync.dll" />
      <ComponentRef Id="System.ComponentModel.Primitives.dll" />
      <ComponentRef Id="System.ComponentModel.TypeConverter.dll" />
      <ComponentRef Id="System.Console.dll" />
      <ComponentRef Id="System.Data.Common.dll" />
      <ComponentRef Id="System.Diagnostics.Contracts.dll" />
      <ComponentRef Id="System.Diagnostics.Debug.dll" />
      <ComponentRef Id="System.Diagnostics.FileVersionInfo.dll" />
      <ComponentRef Id="System.Diagnostics.Process.dll" />
      <ComponentRef Id="System.Diagnostics.StackTrace.dll" />
      <ComponentRef Id="System.Diagnostics.TextWriterTraceListener.dll" />
      <ComponentRef Id="System.Diagnostics.Tools.dll" />
      <ComponentRef Id="System.Diagnostics.TraceSource.dll" />
      <ComponentRef Id="System.Diagnostics.Tracing.dll" />
      <ComponentRef Id="System.Drawing.Primitives.dll" />
      <ComponentRef Id="System.Dynamic.Runtime.dll" />
      <ComponentRef Id="System.Globalization.Calendars.dll" />
      <ComponentRef Id="System.Globalization.dll" />
      <ComponentRef Id="System.Globalization.Extensions.dll" />
      <ComponentRef Id="System.IO.Compression.dll" />
      <ComponentRef Id="System.IO.Compression.ZipFile.dll" />
      <ComponentRef Id="System.IO.dll" />
      <ComponentRef Id="System.IO.FileSystem.dll" />
      <ComponentRef Id="System.IO.FileSystem.DriveInfo.dll" />
      <ComponentRef Id="System.IO.FileSystem.Primitives.dll" />
      <ComponentRef Id="System.IO.FileSystem.Watcher.dll" />
      <ComponentRef Id="System.IO.IsolatedStorage.dll" />
      <ComponentRef Id="System.IO.MemoryMappedFiles.dll" />
      <ComponentRef Id="System.IO.Pipes.dll" />
      <ComponentRef Id="System.IO.UnmanagedMemoryStream.dll" />
      <ComponentRef Id="System.Linq.dll" />
      <ComponentRef Id="System.Linq.Expressions.dll" />
      <ComponentRef Id="System.Linq.Parallel.dll" />
      <ComponentRef Id="System.Linq.Queryable.dll" />
      <ComponentRef Id="System.Net.Http.dll" />
      <ComponentRef Id="System.Net.NameResolution.dll" />
      <ComponentRef Id="System.Net.NetworkInformation.dll" />
      <ComponentRef Id="System.Net.Ping.dll" />
      <ComponentRef Id="System.Net.Primitives.dll" />
      <ComponentRef Id="System.Net.Requests.dll" />
      <ComponentRef Id="System.Net.Security.dll" />
      <ComponentRef Id="System.Net.Sockets.dll" />
      <ComponentRef Id="System.Net.WebHeaderCollection.dll" />
      <ComponentRef Id="System.Net.WebSockets.Client.dll" />
      <ComponentRef Id="System.Net.WebSockets.dll" />
      <ComponentRef Id="System.ObjectModel.dll" />
      <ComponentRef Id="System.Reflection.dll" />
      <ComponentRef Id="System.Reflection.Extensions.dll" />
      <ComponentRef Id="System.Reflection.Primitives.dll" />
      <ComponentRef Id="System.Resources.Reader.dll" />
      <ComponentRef Id="System.Resources.ResourceManager.dll" />
      <ComponentRef Id="System.Resources.Writer.dll" />
      <ComponentRef Id="System.Runtime.CompilerServices.VisualC.dll" />
      <ComponentRef Id="System.Runtime.dll" />
      <ComponentRef Id="System.Runtime.Extensions.dll" />
      <ComponentRef Id="System.Runtime.Handles.dll" />
      <ComponentRef Id="System.Runtime.InteropServices.dll" />
      <ComponentRef Id="System.Runtime.InteropServices.RuntimeInformation.dll" />
      <ComponentRef Id="System.Runtime.Numerics.dll" />
      <ComponentRef Id="System.Runtime.Serialization.Formatters.dll" />
      <ComponentRef Id="System.Runtime.Serialization.Json.dll" />
      <ComponentRef Id="System.Runtime.Serialization.Primitives.dll" />
      <ComponentRef Id="System.Runtime.Serialization.Xml.dll" />
      <ComponentRef Id="System.Security.Claims.dll" />
      <ComponentRef Id="System.Security.Cryptography.Algorithms.dll" />
      <ComponentRef Id="System.Security.Cryptography.Csp.dll" />
      <ComponentRef Id="System.Security.Cryptography.Encoding.dll" />
      <ComponentRef Id="System.Security.Cryptography.Primitives.dll" />
      <ComponentRef Id="System.Security.Cryptography.X509Certificates.dll" />
      <ComponentRef Id="System.Security.Principal.dll" />
      <ComponentRef Id="System.Security.SecureString.dll" />
      <ComponentRef Id="System.Text.Encoding.dll" />
      <ComponentRef Id="System.Text.Encoding.Extensions.dll" />
      <ComponentRef Id="System.Text.RegularExpressions.dll" />
      <ComponentRef Id="System.Threading.dll" />
      <ComponentRef Id="System.Threading.Overlapped.dll" />
      <ComponentRef Id="System.Threading.Tasks.dll" />
      <ComponentRef Id="System.Threading.Tasks.Parallel.dll" />
      <ComponentRef Id="System.Threading.Thread.dll" />
      <ComponentRef Id="System.Threading.ThreadPool.dll" />
      <ComponentRef Id="System.Threading.Timer.dll" />
      <ComponentRef Id="System.ValueTuple.dll" />
      <ComponentRef Id="System.Xml.ReaderWriter.dll" />
      <ComponentRef Id="System.Xml.XDocument.dll" />
      <ComponentRef Id="System.Xml.XmlDocument.dll" />
      <ComponentRef Id="System.Xml.XmlSerializer.dll" />
      <ComponentRef Id="System.Xml.XPath.dll" />
      <ComponentRef Id="System.Xml.XPath.XDocument.dll" />
      <ComponentRef Id="Tmds.MDns.dll" />
      <ComponentRef Id="Winleafs.Api.dll" />
      <ComponentRef Id="Winleafs.External.dll" />
      <ComponentRef Id="Winleafs.Models.dll" />
      <ComponentRef Id="Winleafs.Wpf.exe" />
      <ComponentRef Id="Winleafs.Wpf.exe.config" />
      <ComponentRef Id="Winleafs.Wpf.resources.dll" />
    </Feature>
    <!-- Step 4: Sign the MSI Install, else Web user will get unknown publisher message -->
    <!-- Signing is done by executing a post-build batch file configured in project properties -->
  </Product>
</Wix>
```
