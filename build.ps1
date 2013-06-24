# Basic msbuild script

# ---- Add the tool paths to our path
$runtimeDir = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()
$env:Path = $env:Path + $runtimeDir

# ---- Build the solution using the Download target
msbuild roadkill.sln "/p:Configuration=Download"