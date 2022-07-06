@echo off
dotnet build
copy /y "bin\Debug\netstandard2.1\VRising.PVP.dll" "C:\Program Files (x86)\Steam\steamapps\common\VRisingDedicatedServer\BepInEx\WetstonePlugins\"