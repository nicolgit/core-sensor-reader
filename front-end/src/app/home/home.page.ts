import { Component, ViewChild } from "@angular/core";
import { IonContent } from "@ionic/angular";
import { Street } from "../models/street.model";
import { BackendService } from "../services/backend.service";

@Component({
  selector: "app-home",
  templateUrl: "home.page.html",
  styleUrls: ["home.page.scss"],
})
export class HomePage {
  @ViewChild(IonContent) ionContent: IonContent;
  pollutionData: Street;
  isLoaded: boolean = false;

  constructor(public backend: BackendService) {
    this.backend.getPollutionData().subscribe(
      (res) => {
        this.pollutionData = res;
        this.isLoaded = true;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  slideChange() {
    /* let element: HTMLElement = document.getElementsByClassName('animate')[0] as HTMLElement;
    element.click(); */
    this.ionContent.scrollToTop(300);
  }
}
