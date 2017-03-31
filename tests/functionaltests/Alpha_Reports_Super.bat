If exist "%~dp0\Report" del "%~dp0\Report\*.*" /Q
set /p abc=< "%~dp0\Tag_Run.txt"
FOR /F "tokens=1 delims=Tag=" %%G IN ("%abc%") DO echo %%G>"%~dp0\tag.txt"
set /p tag=<"%~dp0\tag.txt"
del "%~dp0\tag.txt"
call pybot --variablefile "%~dp0\config.py" --outputdir "%~dp0\Report" %tag%  "%~dp0\Test_Modules\Reports\Super_TMs.txt"
