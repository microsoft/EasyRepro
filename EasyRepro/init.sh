#!/bin/bash
rm -rf /tmp/.X1-lock
pgrep xfdesktop >/dev/null || vncserver :1 -geometry 1280x800 -depth 24 #-viewonly
while ! pidof xfdesktop >> /dev/null ;
do
    echo "waiting for GUI to initialize"
    sleep 1
done

echo Starting EasyReprop Tests

dotnet vstest --settings:test.runsettings EasyReproTests.dll
dotnet vstest EasyReproTests.dll --logger:trx --TestCaseFilter:"(TestCategory=Contact)"

#cat TestResults/*

echo Closing EasyRepro Tests
#tail -f /dev/null