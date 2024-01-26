FROM ubuntu:18.04
#https://docs.docker.com/engine/reference/builder/#workdir
WORKDIR / 

#Install basic tools
RUN \
  sed -i 's/# \(.*multiverse$\)/\1/g' /etc/apt/sources.list && \
  apt-get update && \
  DEBIAN_FRONTEND=noninteractive apt-get -y upgrade && \
  DEBIAN_FRONTEND=noninteractive apt-get install -y \
  curl unzip vim wget cifs-utils apt-transport-https ca-certificates locate

#Install VNC Server
RUN \
  DEBIAN_FRONTEND=noninteractive apt-get install -y tightvncserver

##Set VNC password
  ##TODO pass as param 
  ##TODO install NoVNC via browser
EXPOSE 5901
ENV USER root
RUN mkdir $HOME/.vnc
RUN echo "vncpass" | vncpasswd -f > $HOME/.vnc/passwd
RUN chmod 0700 $HOME/.vnc/passwd
#

#Install GUI
RUN \
  DEBIAN_FRONTEND=noninteractive apt-get install -y supervisor xfce4 xfce4-terminal xterm && \
  apt-get purge -y pm-utils xscreensaver*


#Install Chrome and missing dependencies
WORKDIR /install
RUN wget http://cz.archive.ubuntu.com/ubuntu/pool/main/f/fonts-liberation/fonts-liberation_1.07.4-1_all.deb
RUN dpkg -i fonts-liberation_1.07.4-1_all.deb 

RUN wget http://cz.archive.ubuntu.com/ubuntu/pool/main/libd/libdbusmenu/libdbusmenu-glib4_12.10.3+16.04.20160223.1-0ubuntu1_amd64.deb
RUN dpkg -i libdbusmenu-glib4_12.10.3+16.04.20160223.1-0ubuntu1_amd64.deb 

RUN wget http://cz.archive.ubuntu.com/ubuntu/pool/main/libd/libdbusmenu/libdbusmenu-gtk3-4_12.10.3+16.04.20160223.1-0ubuntu1_amd64.deb
RUN dpkg -i libdbusmenu-gtk3-4_12.10.3+16.04.20160223.1-0ubuntu1_amd64.deb 

RUN wget http://cz.archive.ubuntu.com/ubuntu/pool/main/libi/libindicator/libindicator3-7_12.10.2+16.04.20151208-0ubuntu1_amd64.deb
RUN dpkg -i libindicator3-7_12.10.2+16.04.20151208-0ubuntu1_amd64.deb 

RUN wget http://cz.archive.ubuntu.com/ubuntu/pool/main/liba/libappindicator/libappindicator3-1_12.10.1+15.04.20141110-0ubuntu1_amd64.deb
RUN dpkg -i libappindicator3-1_12.10.1+15.04.20141110-0ubuntu1_amd64.deb

#RUN wget http://archive.ubuntu.com/ubuntu/pool/main/d/distro-info-data/distro-info-data_0.39ubuntu2.1_all.deb
#RUN dpkg -i distro-info-data_0.39ubuntu2.1_all.deb

RUN wget http://cz.archive.ubuntu.com/ubuntu/pool/main/l/lsb/lsb-release_9.20160110_all.deb
RUN dpkg -i lsb-release_9.20160110_all.deb 

#RUN wget http://ftp.de.debian.org/debian/pool/main/x/xdg-utils/xdg-utils_1.1.3-1_all.deb
#RUN dpkg -i xdg-utils_1.1.3-1_all.deb

RUN wget http://archive.ubuntu.com/ubuntu/pool/main/libu/libu2f-host/libu2f-udev_1.1.4-1_all.deb
RUN dpkg -i libu2f-udev_1.1.4-1_all.deb

RUN apt-get -y install libvulkan1

RUN apt-get -y install xdg-utils 

RUN wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
RUN dpkg -i google-chrome-stable_current_amd64.deb
#
  #Add Chrome icon to the root's desktop
COPY /Chrome.desktop /root/Desktop/Chrome.desktop
RUN chmod 0700 /root/Desktop/Chrome.desktop

# Download and ChromeDriver
# http://chromedriver.chromium.org/home
# source-code: https://cs.chromium.org/chromium/src/chrome/test/chromedriver/command_listener_proxy.cc?sq=package:chromium&g=0
##
RUN set -x \
&& apt-get update \
&& apt-get install -y --no-install-recommends \
&& curl -sSL "https://chromedriver.storage.googleapis.com/75.0.3770.90/chromedriver_linux64.zip" -o /tmp/chromedriver.zip \
&& unzip -o /tmp/chromedriver -d /opt/selenium/ \
&& rm -rf /tmp/*.deb

#Install dotnet core
RUN wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg
RUN mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/
RUN wget -q https://packages.microsoft.com/config/ubuntu/18.04/prod.list
RUN mv prod.list /etc/apt/sources.list.d/microsoft-prod.list
RUN wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb

RUN echo "deb http://security.ubuntu.com/ubuntu bionic-security main" | tee -a /etc/apt/sources.list
RUN echo "deb http://cz.archive.ubuntu.com/ubuntu bionic main" | tee -a /etc/apt/sources.list

RUN apt-get update
RUN apt-get -y --no-install-recommends install libcom-err2
RUN apt-get -y --no-install-recommends install libkrb5-3
RUN apt-get -y --no-install-recommends install libc6
RUN apt-get -y --no-install-recommends install libgssapi-krb5-2
RUN apt-get -y --no-install-recommends install libnghttp2-14
RUN apt-get -y --no-install-recommends install libpsl5
RUN apt-get -y --no-install-recommends install libcurl4 

RUN export DOTNET_SKIP_FIRST_TIME_EXPERIENCE="true"
RUN apt-get -y --no-install-recommends install dotnet-runtime-2.2
RUN apt-get -y --no-install-recommends install dotnet-sdk-2.2

#Installing PowerShell Core
#RUN wget https://github.com/PowerShell/PowerShell/releases/download/v6.2.0/powershell_6.2.0-1.ubuntu.18.04_amd64.deb 
#RUN dpkg -i powershell_6.2.0-1.ubuntu.18.04_amd64.deb

#Last apt-get clean
RUN apt-get clean \
	&& rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/* /install/*
#

WORKDIR /base-image

COPY /init.sh /base-image/init.sh

ENTRYPOINT [ "bash", "/base-image/init.sh" ]
CMD [ "bash" ]