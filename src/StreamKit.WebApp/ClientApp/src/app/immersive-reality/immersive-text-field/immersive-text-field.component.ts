import { Component, contentChild, input, model } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ImmersiveDescriptionComponent } from "../immersive-description/immersive-description.component";
import { ImmersiveFormRowComponent } from "../immersive-form-row/immersive-form-row.component";

@Component({
  selector: "app-immersive-text-field",
  standalone: true,
  imports: [
    FormsModule,
    ImmersiveDescriptionComponent,
    ImmersiveFormRowComponent
  ],
  templateUrl: "./immersive-text-field.component.html",
  styleUrl: "./immersive-text-field.component.css"
})
export class ImmersiveTextFieldComponent {
  id = input.required<string>();
  label = input.required<string>();
  contents = model<string>();
  placeholder = model<string>("Placeholder");
  description = input<string | undefined>();

  get hasPlaceholder(): boolean {
    let placeholder = this.placeholder;

    return placeholder !== undefined && placeholder.length > 0;
  }

  get hasDescription() : boolean {
    let description = this.description();

    return description !== undefined && description.length > 0;
  }

  protected readonly String = String;
}
