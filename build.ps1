# PowerShell build script for DotnetGeminiSDK
param(
    [string]$Configuration = "Release",
    [switch]$SkipTests,
    [switch]$SkipPack,
    [string]$OutputDir = ".\artifacts"
)

Write-Host "Building DotnetGeminiSDK..." -ForegroundColor Green

# Clean previous builds
if (Test-Path $OutputDir) {
    Remove-Item -Recurse -Force $OutputDir
}
New-Item -ItemType Directory -Path $OutputDir | Out-Null

# Restore packages
Write-Host "Restoring packages..." -ForegroundColor Yellow
dotnet restore src/DotnetGeminiSDK.sln

if ($LASTEXITCODE -ne 0) {
    Write-Error "Package restore failed"
    exit 1
}

# Build the solution
Write-Host "Building solution..." -ForegroundColor Yellow
dotnet build src/DotnetGeminiSDK.sln --configuration $Configuration --no-restore

if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed"
    exit 1
}

# Run tests (unless skipped)
if (-not $SkipTests) {
    Write-Host "Running tests..." -ForegroundColor Yellow
    dotnet test src/DotnetGeminiSDK.Tests/DotnetGeminiSDK.Tests.csproj --configuration $Configuration --no-build --verbosity normal --collect:"XPlat Code Coverage"
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Tests failed"
        exit 1
    }
}

# Pack the NuGet packages (unless skipped)
if (-not $SkipPack) {
    Write-Host "Creating NuGet packages..." -ForegroundColor Yellow
    
    # Create the main package
    dotnet pack src/DotnetGeminiSDK/DotnetGeminiSDK.csproj --configuration $Configuration --no-build --output $OutputDir --include-symbols --include-source
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Package creation failed"
        exit 1
    }
    
    Write-Host "Packages created successfully!" -ForegroundColor Green
    Write-Host "Main package: $OutputDir\DotnetGeminiSDK.2.0.0.nupkg" -ForegroundColor Cyan
    Write-Host "Symbols package: $OutputDir\DotnetGeminiSDK.2.0.0.snupkg" -ForegroundColor Cyan
}

Write-Host "Build completed successfully!" -ForegroundColor Green
