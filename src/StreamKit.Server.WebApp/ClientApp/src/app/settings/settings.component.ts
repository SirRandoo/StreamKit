import { Component } from "@angular/core";
import { ImmersiveCheckboxComponent } from "../immersive-reality/immersive-checkbox/immersive-checkbox.component";
import {
  ImmersiveTimeSpanFieldComponent
} from "../immersive-reality/immersive-time-span-field/immersive-time-span-field.component";
import {
  ImmersiveNumberFieldComponent
} from "../immersive-reality/immersive-number-field/immersive-number-field.component";

@Component({
  selector: "app-settings",
  standalone: true,
  imports: [
    ImmersiveCheckboxComponent,
    ImmersiveTimeSpanFieldComponent,
    ImmersiveNumberFieldComponent
  ],
  templateUrl: "./settings.component.html",
  styleUrl: "./settings.component.css"
})
export class SettingsComponent {
  protected toggleableChanged() {}
}
