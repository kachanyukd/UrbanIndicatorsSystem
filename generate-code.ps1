# PowerShell script to generate code
Write-Host "╔════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║    Urban Indicators System - Code Generator           ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

$projectRoot = $PSScriptRoot
$generatorDir = Join-Path $projectRoot "CodeGeneration"
$generatorProject = Join-Path $generatorDir "CodeGenerator.csproj"

Write-Host "Building Code Generator..." -ForegroundColor Yellow
dotnet build $generatorProject --configuration Release

if ($LASTEXITCODE -eq 0) {
    Write-Host "Running Code Generator..." -ForegroundColor Yellow
    dotnet run --project $generatorProject --configuration Release --no-build
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "✓ Code generation completed successfully!" -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "✗ Code generation failed!" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host ""
    Write-Host "✗ Failed to build code generator!" -ForegroundColor Red
    exit 1
}
