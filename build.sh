#!/bin/bash

# Bash build script for DotnetGeminiSDK
set -e

CONFIGURATION=${1:-Release}
SKIP_TESTS=${2:-false}
SKIP_PACK=${3:-false}
OUTPUT_DIR="./artifacts"

echo "Building DotnetGeminiSDK..."

# Clean previous builds
if [ -d "$OUTPUT_DIR" ]; then
    rm -rf "$OUTPUT_DIR"
fi
mkdir -p "$OUTPUT_DIR"

# Restore packages
echo "Restoring packages..."
dotnet restore src/DotnetGeminiSDK.sln

# Build the solution
echo "Building solution..."
dotnet build src/DotnetGeminiSDK.sln --configuration $CONFIGURATION --no-restore

# Run tests (unless skipped)
if [ "$SKIP_TESTS" != "true" ]; then
    echo "Running tests..."
    dotnet test src/DotnetGeminiSDK.Tests/DotnetGeminiSDK.Tests.csproj --configuration $CONFIGURATION --no-build --verbosity normal --collect:"XPlat Code Coverage"
fi

# Pack the NuGet packages (unless skipped)
if [ "$SKIP_PACK" != "true" ]; then
    echo "Creating NuGet packages..."
    
    # Create the main package with symbols
    dotnet pack src/DotnetGeminiSDK/DotnetGeminiSDK.csproj --configuration $CONFIGURATION --no-build --output $OUTPUT_DIR --include-symbols --include-source
    
    echo "Packages created successfully!"
    echo "Main package: $OUTPUT_DIR/DotnetGeminiSDK.2.0.0.nupkg"
    echo "Symbols package: $OUTPUT_DIR/DotnetGeminiSDK.2.0.0.snupkg"
fi

echo "Build completed successfully!"
