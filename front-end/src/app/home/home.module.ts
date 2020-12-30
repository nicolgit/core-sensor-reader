import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { IonicModule } from "@ionic/angular";
import { FormsModule } from "@angular/forms";
import { HomePage } from "./home.page";

import { HomePageRoutingModule } from "./home-routing.module";

import { NgCircleProgressModule } from "ng-circle-progress";
import { CircleComponent } from '../components/circle/circle.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    HomePageRoutingModule,
    NgCircleProgressModule.forRoot(),
  ],
  declarations: [HomePage, CircleComponent],
})
export class HomePageModule {}
