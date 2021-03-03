import { CloneVisitor } from "@angular/compiler/src/i18n/i18n_ast";
import { Component, Input, OnInit } from "@angular/core";
import { Street } from "src/app/models/street.model";

@Component({
  selector: "app-circle",
  templateUrl: "./circle.component.html",
  styleUrls: ["./circle.component.scss"],
})
export class CircleComponent implements OnInit {
  @Input("pollutionAreaData") pollutionAreaData: Street;

  constructor() { }

  ngOnInit() { }

  calculatePollutionColor(value, type): string {
    let color: string;
    /* 
    per la verifica del valori ho usato la tabella di riferimento mostrata su Wikipedia:
    https://it.wikipedia.org/wiki/Particolato#Misurazione_della_concentrazione_di_particolato
    */

    if (type === "pm25") {
      color =
        value >= 0 && value <= 10
          ? "#78C000"
          : value > 10 && value <= 25
            ? "#e8f533"
            : "#e31b25";
    } else {
      color =
        value >= 0 && value <= 20
          ? "#78C000"
          : value > 20 && value <= 50
            ? "#e8f533"
            : "#e31b25";
    }

    return color;
  }

  calculateContentColor(): string {
    // controllo se è attiva la dark mode
    return (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) ? '#ffffff' : '#000000';
  }

}
