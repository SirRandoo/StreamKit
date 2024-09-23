import { Component, input, model } from "@angular/core";
import { ImmersiveDescriptionComponent } from "../immersive-description/immersive-description.component";
import { ImmersiveFormRowComponent } from "../immersive-form-row/immersive-form-row.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

enum UnitOfTime {
  Seconds, Minutes, Hours, Days
}

@Component({
  selector: "app-immersive-time-span-field",
  standalone: true,
  imports: [
    ImmersiveDescriptionComponent,
    ImmersiveFormRowComponent,
    ReactiveFormsModule,
    FormsModule,
  ],
  templateUrl: "./immersive-time-span-field.component.html",
  styleUrl: "./immersive-time-span-field.component.css"
})
export class ImmersiveTimeSpanFieldComponent {
  id = input.required<string>();
  label = input.required<string>();
  contents = model<string>();
  placeholder = model<string>();
  description = input<string | undefined>();
  span = model<UnitOfTime>(UnitOfTime.Seconds);
  protected readonly String = String;

  get hasPlaceholder(): boolean {
    let placeholder = this.placeholder;

    return placeholder !== undefined && placeholder.length > 0;
  }

  get hasDescription(): boolean {
    let description: string | undefined = this.description();

    return description !== undefined && description.length > 0;
  }

  // @ts-ignore
  protected processSelection(value: string) {
    switch (value.toLowerCase()) {
      case "seconds":
        this.span.set(UnitOfTime.Seconds);

        break;
      case "minutes":
        this.span.set(UnitOfTime.Minutes);

        break;
      case "hours":
        this.span.set(UnitOfTime.Hours);

        break;
      case "days":
        this.span.set(UnitOfTime.Days);

        break;
    }
  }
}
