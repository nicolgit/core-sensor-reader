# UART Communication Protocol

* Bit rate ：9600
* Data bit ：8
* Parity bit：NO
* Stop bit ：1
* Data Packet frequency: 1Hz


| byte  | Name            |  Content  |
|-------|-----------------|-----------|
| 0     | Msg header      | AA        |
| 1     | Commander No.   | C0 |
| 2     | DATA 1 PM2.5    | Low byte |
| 3     | DATA 2 PM2.5    | High byte ||
| 4     | DATA 3 PM10     | Low byte |
| 5     | DATA 4 PM10     | High byte |
| 6     | DATA 5          | ID byte 1 |
| 7     | DATA 6          | ID byte 2 |
| 8     |Check-sum        |Check-sum |
| 9     |Message tail     | AB |


* Check-sum: Check-sum=DATA1+DATA2+...+DATA6 。
* PM2.5 value: PM2.5 (μg /m3) = ((PM2.5 High byte *256) + PM2.5 low byte)/10
* PM10 value: PM10 (μg /m3) = ((PM10 high byte*256) + PM10 low byte)/10


# PWM output description
* Range of PM2.5 value 0-999μg /m³
* Range of PM10 value 0-999μg /m³
* Cycle 1004ms±1%
* High level output time at the beginning of the whole cycle: 2ms
* The middle time of this cycle 1000ms±1%
* Low level output time at the end of the whole cycle: 2ms


from: https://cdn-reichelt.de/documents/datenblatt/X200/SDS011-DATASHEET.pdf 