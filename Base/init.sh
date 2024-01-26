#!/bin/bash
rm -rf /tmp/.X1-lock
vncserver :1 -geometry 1280x800 -depth 24
#set DISPLAY=:1 // :1.0
tail -f /dev/null