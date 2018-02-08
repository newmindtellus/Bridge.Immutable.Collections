@echo off

%~d0
cd "%~p0"

del *.nu*
del *.dll
del *.xml

copy ..\ProductiveRage.Immutable.Collections\bin\Release\ProductiveRage.Immutable.Collections.dll > nul
copy ..\ProductiveRage.Immutable.Collections\bin\Release\ProductiveRage.Immutable.Collections.xml > nul

copy ..\ProductiveRage.Immutable.Collections.nuspec > nul
nuget pack -NoPackageAnalysis ProductiveRage.Immutable.Collections.nuspec