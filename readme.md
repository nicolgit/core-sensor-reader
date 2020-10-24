.NET CORE app that reads from Nova PM Sensor SDS011 High Precision Laser PM2.5 Air Quality Detection Sensor

**publish for Raspberry**

dotnet publish -o publish --self-contained -r linux-arm

**copy to raspberry ALL files**

scp .\publish\\* pi@raspberry.fritz.box:/home/pi/projects/core-sensor-reader/

**copy to raspberry only 'core-*' files**

scp .\publish\core-* pi@raspberry.fritz.box:/home/pi/projects/core-sensor-reader/

** run app on Raspberry

chmod 777 ./core-sensor-reader

./core-sensor-reader
