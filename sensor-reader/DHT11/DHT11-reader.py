import Adafruit_DHT                                          #  1
import time                                                  #  2
                                                             #  3
DHT11 = Adafruit_DHT.DHT11                                   #  4
DHT11_PIN = 21 # usa numerazione pin BCM                     #  5
                                                             #  6
# while True:
umidita, temperatura = Adafruit_DHT.read_retry(DHT11, DHT11_PIN, retries=2, delay_seconds=1) #  8
if umidita is not None and temperatura is not None:
	print('DHT11|Temp|{0:0.1f}|C|UMID|{1:0.1f}|%'.format(temperatura, umidita))
else:
	print('Error unable to read from DHT11 - please retry')
time.sleep(1)

