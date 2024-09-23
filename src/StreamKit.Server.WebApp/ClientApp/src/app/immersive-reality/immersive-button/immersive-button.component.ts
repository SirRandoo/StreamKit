import { Component, input } from "@angular/core";

@Component({
  selector: 'app-immersive-button',
  standalone: true,
  imports: [],
  templateUrl: './immersive-button.component.html',
  styleUrl: './immersive-button.component.css'
})
export class ImmersiveButtonComponent {
  text = input.required<string>();
}
