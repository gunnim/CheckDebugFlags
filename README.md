# Check Debug Flags

Pretty much a command line fork of the excellent [Assembly Information](https://github.com/jozefizso/AssemblyInformation) package.

Scans a directory and optionally subdirectories for .NET assemblies compiled for debug.

Helpful to verify that your projects are being published in release mode.

Command line arguments:

```
/P[ath]=<relative or absolute path> Optionally specify the directory to begin scanning. Default is current directory

/S[ubdirectories] Current or chosen directory and all subdirectories

/D[istinct] Treat assemblies from different directories but with the same name as distinct

/R[egex] Optional search pattern. If included, this string is prepended to '.dll' and used for filename matching
```

Roadmap:

I'd like to make a dotnet core version when dotnet core 2.0 gets released with support for the flags on DebuggableAttribute

Example
```
C:\Code>CheckDebugFlags /S /P=. /D /R="DLL Name"
```
