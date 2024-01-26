FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App
# Download and ChromeDriver
# http://chromedriver.chromium.org/home
# source-code: https://cs.chromium.org/chromium/src/chrome/test/chromedriver/command_listener_proxy.cc?sq=package:chromium&g=0
##
#RUN apt-get update && apt-get install -y google-chrome-stable




RUN set -x \
&& apt-get update \
&& apt-get install -y --no-install-recommends \
&& curl -sSL "https://edgedl.me.gvt1.com/edgedl/chrome/chrome-for-testing/121.0.6167.85/linux64/chromedriver-linux64.zip" -o /tmp/chromedriver.zip \
&& unzip -o /tmp/chromedriver -d /opt/selenium/ 
#&& rm -rf /tmp/*.deb
# Copy everything
COPY . ./
#COPY /opt/selenium/* .

#Download Git
#RUN apt-get update && apt-get install -y git

#Download EasyRepro
#RUN git clone https://github.com/microsoft/EasyRepro.git --depth 1
#RUN git checkout features/dotnet/core


# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
#RUN dotnet publish -c Release -o out
RUN dotnet publish -c Release

RUN apt-get update
RUN apt-get -y --no-install-recommends install libcom-err2
RUN apt-get -y --no-install-recommends install libkrb5-3
RUN apt-get -y --no-install-recommends install libc6
RUN apt-get -y --no-install-recommends install libgssapi-krb5-2
RUN apt-get -y --no-install-recommends install libnghttp2-14
RUN apt-get -y --no-install-recommends install libpsl5
RUN apt-get -y --no-install-recommends install libcurl4 

# RUN apt-get install -y wget
# RUN wget -q https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
# RUN apt-get install -y ./google-chrome-stable_current_amd64.deb


# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /App
COPY --from=build-env /App/out .
COPY run.ps1 .
#RUN cp /opt/selenium/* .
COPY Microsoft.Dynamics365.UIAutomation.Sample/AzureDevOps/EasyRepro.runsettings .
ARG ADDRESSEE="world"                       # $ADDRESSEE can be ARG or ENV type.
ENV MESSAGE="Hello, $ADDRESSEE!"  
ENTRYPOINT [ "pwsh", "run.ps1", "${MESSAGE}" ]

CMD ["pwsh.exe", "run.ps1", "${MESSAGE}"]
#To enter container: docker run -it --rm --entrypoint "powershell"  easyreprocore
