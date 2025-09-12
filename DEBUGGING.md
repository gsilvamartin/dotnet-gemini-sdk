# Debugging Guide

This guide explains how to debug the DotnetGeminiSDK library in your IDE.

## Debugging Support

The library includes comprehensive debugging support through:

- **SourceLink**: Enables Visual Studio to automatically download and display source code
- **PDB Symbols**: Portable debug symbols for breakpoint support
- **Symbol Packages**: Separate `.snupkg` packages for symbol distribution

## Setup for Debugging

### Visual Studio

1. **Enable SourceLink** (automatic):
   - Visual Studio will automatically detect SourceLink metadata
   - When you step into library code, it will prompt to download source
   - Click "Download Source" to get the exact source code

2. **Configure Symbol Servers**:
   - Go to `Tools` → `Options` → `Debugging` → `General`
   - Ensure "Enable .NET Framework source stepping" is checked
   - Ensure "Enable source server support" is checked

3. **Debugging Steps**:
   - Set breakpoints in your code
   - Step into library methods (F11)
   - Visual Studio will automatically download source when needed

### Visual Studio Code

1. **Install C# Extension**:
   - Install the C# Dev Kit or C# extension
   - Ensure OmniSharp is running

2. **Configure launch.json**:
   ```json
   {
     "version": "0.2.0",
     "configurations": [
       {
         "name": "Debug Your App",
         "type": "coreclr",
         "request": "launch",
         "preLaunchTask": "build",
         "program": "${workspaceFolder}/bin/Debug/net8.0/YourApp.dll",
         "args": [],
         "cwd": "${workspaceFolder}",
         "stopAtEntry": false,
         "console": "internalConsole"
       }
     ]
   }
   ```

3. **Debugging**:
   - Set breakpoints in your code
   - Press F5 to start debugging
   - Step into library methods to see source code

### JetBrains Rider

1. **Enable SourceLink**:
   - Go to `File` → `Settings` → `Build, Execution, Deployment` → `Debugger`
   - Enable "Download sources from SourceLink"

2. **Debugging**:
   - Set breakpoints and start debugging
   - Rider will automatically download source when stepping into library code

## Symbol Packages

The library provides two types of packages:

1. **Main Package** (`DotnetGeminiSDK.2.0.0.nupkg`):
   - Contains the compiled library
   - Includes embedded PDB symbols
   - Works with most IDEs

2. **Symbol Package** (`DotnetGeminiSDK.2.0.0.snupkg`):
   - Contains additional debugging symbols
   - Provides better debugging experience
   - Automatically downloaded by Visual Studio

## Troubleshooting

### Source Code Not Found

If Visual Studio can't find the source code:

1. **Check SourceLink Configuration**:
   - Ensure the repository URL is correct in the package metadata
   - Verify the commit hash matches the package version

2. **Manual Source Download**:
   - Clone the repository: `git clone https://github.com/guilhermemartin/dotnet-gemini-sdk.git`
   - Checkout the specific tag: `git checkout v2.0.0`
   - Reference the source project instead of the NuGet package

3. **Clear Symbol Cache**:
   - Delete the symbol cache folder
   - Restart Visual Studio
   - Try debugging again

### Breakpoints Not Working

If breakpoints don't work:

1. **Check Debug Configuration**:
   - Ensure you're running in Debug mode
   - Verify the correct target framework

2. **Symbol Loading**:
   - Check the Modules window in Visual Studio
   - Ensure symbols are loaded for the library

3. **PDB Files**:
   - Verify PDB files are present in the output directory
   - Check that symbol paths are correct

## Advanced Debugging

### Using ILSpy for Decompilation

If SourceLink doesn't work, you can use ILSpy:

1. Install ILSpy extension in Visual Studio
2. Right-click on library methods
3. Select "Go to Decompiled Sources"
4. This shows decompiled source (not original source)

### Debugging with dnSpy

For advanced debugging scenarios:

1. Download dnSpy from GitHub
2. Open your application with dnSpy
3. Set breakpoints in the library code
4. Run and debug with full source access

## Package Information

The library includes the following debugging metadata:

- **Repository URL**: https://github.com/guilhermemartin/dotnet-gemini-sdk
- **Commit Hash**: Embedded in package metadata
- **Source Revision**: Available in assembly informational version
- **Symbol Format**: Portable PDB (.pdb)

## Contributing

When contributing to the library:

1. **Test Debugging**: Ensure your changes don't break debugging
2. **SourceLink**: Verify SourceLink works with your changes
3. **Symbols**: Test that PDB generation works correctly
4. **Documentation**: Update this guide if needed

## Support

If you encounter issues with debugging:

1. Check this guide first
2. Search existing issues on GitHub
3. Create a new issue with:
   - IDE version and type
   - .NET version
   - Steps to reproduce
   - Error messages or screenshots
