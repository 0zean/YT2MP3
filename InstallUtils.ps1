# Define paths
$ytDlpPath = "$env:USERPROFILE\yt-dlp"
$ffmpegPath = "$env:USERPROFILE\ffmpeg"

# Create directories if they don't exist
if (-not (Test-Path -Path $ytDlpPath)) {
    New-Item -ItemType Directory -Path $ytDlpPath
}

if (-not (Test-Path -Path $ffmpegPath)) {
    New-Item -ItemType Directory -Path $ffmpegPath
}

# Download yt-dlp.exe
$ytDlpUrl = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe"
$ytDlpExe = "$ytDlpPath\yt-dlp.exe"
Invoke-WebRequest -Uri $ytDlpUrl -OutFile $ytDlpExe

# Download ffmpeg
$ffmpegUrl = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip"
$ffmpegZip = "$ffmpegPath\ffmpeg.zip"
Invoke-WebRequest -Uri $ffmpegUrl -OutFile $ffmpegZip

# Extract ffmpeg
Add-Type -AssemblyName "System.IO.Compression.Filesystem"
[System.IO.Compression.ZipFile]::ExtractToDirectory($ffmpegZip, $ffmpegPath)

# Remove the zip file
Remove-Item $ffmpegZip

# Get the bin directory inside ffmpeg
$ffmpegBinPath = (Get-ChildItem -Path $ffmpegPath -Recurse -Directory | Where-Object { $_.Name -eq "bin" }).FullName

# Add yt-dlp and ffmpeg to the Path
$env:Path += ";$ytDlpPath;$ffmpegBinPath"
[System.Environment]::SetEnvironmentVariable("Path", $env:Path, [System.EnvironmentVariableTarget]::User)

# Confirm installation
Write-Host "yt-dlp and ffmpeg installed successfully and added to Path."