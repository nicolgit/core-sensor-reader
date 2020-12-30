import { Injectable } from "@angular/core";
import { Observable, pipe } from "rxjs";
import { map } from "rxjs/internal/operators/map";
import { catchError } from "rxjs/operators";
import { HttpClient } from "@angular/common/http";
import { AreaConverter } from "../converter/area.converter";
import { Street } from "../models/street.model";

@Injectable({
  providedIn: "root",
})
export class BackendService {
  constructor(
    private httpClient: HttpClient,
    public areaConverter: AreaConverter
  ) {}

  getPollutionData(): Observable<Street> {
    let url =
      "https://parcoleonardopmstorage.table.core.windows.net/fiumicino()?$filter=PartitionKey%20eq%20'LAST'&sv=2019-12-12&ss=t&srt=sco&sp=rl&se=2099-10-18T04:53:54Z&st=2020-10-17T20:53:54Z&spr=https&sig=qVfdRy9BcCQFwzjgFXKbPk%2Baaccz0QUOlho3QUFHMTc%3D";

    return this.httpClient
      .get<Street[]>(url, { responseType: "json" })
      .pipe(
        map((response) => {
          return this.areaConverter.streetDtoToModel(response);
        }),
        catchError((error) => {
          return error;
        })
      );
  }
}
