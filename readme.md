.NET CORE app that reads from Nova PM Sensor SDS011 High Precision Laser PM2.5 Air Quality Detection Sensor

![Architecture](asset/architecture.jpg)

![](asset/raspberry.jpg)

# How to deploy to Raspberry

## Python script to read DHT11 sensor

## wiring

| Raspberry         | DHT11    |
|-------------------|----------|
| Raspberry pin #2  | pin VCC  |
| Raspberry pin #9  | pin GND  |
| Raspberry pin #40 | pin DAT  |

## setup
sudo pip3 install Adafruit_DHT

scp ./DHT11/*.py pi@raspberry.fritz.box:/home/pi/projects/core-sensor-reader/

test: python3 DHT11-reader.py

# Publish for Raspberry

dotnet publish -o publish --self-contained -r linux-arm

## Copy to raspberry ALL files

scp .\publish\\* pi@raspberry.fritz.box:/home/pi/projects/core-sensor-reader/

## Copy to raspberry only 'core-*' files

scp .\publish\core-* pi@raspberry.fritz.box:/home/pi/projects/core-sensor-reader/

## How to run app on a Raspberry

cd /home/pi/projects/core-sensor-reader/

chmod 777 ./core-sensor-reader

./core-sensor-reader
