# CheckDebugFlags

Pretty much a command line fork of the excellent [Assembly Information](https://github.com/jozefizso/AssemblyInformation) package.

Scans directory and subdirectory for .NET assemblies compiled for debug.

Helpful to verify that your projects are being published in release mode.


Roadmap:

I plan to add a cli switch to disable recursing subdirectories.

I'd like to make a dotnet core version when dotnet core 2.0 get's released with support for the flags on DebuggableAttribute
