FROM easyrepro-base:latest

USER 0
#DISPLAY variable is needed for GUI apps to run on the X's enviroment 
ENV DISPLAY localhost:1.0

WORKDIR /src

#COPY EasyReproNetCore/ /EasyReproNetCore/
#COPY EasyReproTests/* /EasyReproTests/
COPY . ./

RUN dotnet restore 
# Build and publish a release
#RUN dotnet publish -c Release -o out
RUN dotnet publish -c Release

WORKDIR /app
RUN cp /opt/selenium/* /app/
#COPY run.ps1 .
#RUN cp /opt/selenium/* .
#COPY ../EasyRepro.runsettings .

COPY init.sh /app/init.sh
RUN ls -la /app/* 

ENTRYPOINT [ "bash", "/app/init.sh" ]
CMD ["bash"]