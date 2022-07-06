@echo off
dotnet build
copy /y "bin\Debug\netstandard2.1\VRising.TestPlugin2.dll" "C:\Program Files (x86)\Steam\steamapps\common\VRisingDedicatedServer\BepInEx\WetstonePlugins\"