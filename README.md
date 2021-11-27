This utility helps to find types and type members from other assemblies a .NET app relies on.
The utilily takes a list of assemblies which should be analyzed, and generates a report with the following information:
- part of code (type name, member name), which relies on other assemblies
- type name and member name, which are used

The utility looks not only through assembly matadata, but also through CIL. It uses [Mono.Cecil](https://www.mono-project.com/docs/tools+libraries/libraries/Mono.Cecil/) to analyze assemblies.