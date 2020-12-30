import { Injectable } from "@angular/core";
import { Street } from "../models/street.model";

@Injectable({
  providedIn: "root",
})
export class AreaConverter {
  streetDtoToModel(dto: any): Street[] {
    let streets: Street[] = [];

    dto.value.forEach((el) => {
      let street: Street = {};
      street.humidity = el.Humidity;
      street.lastUpdate = el.Timestamp;
      street.pm10 = el.PM10;
      street.pm25 = el.PM25;
      street.pressure = el.Pressure;
      street.street = el.RowKey;
      street.temperature = el.Temperature;
      streets.push(street);
    });
    return streets;
  }
}
