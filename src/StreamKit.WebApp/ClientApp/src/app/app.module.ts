import { APP_ID, NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { provideHttpClient, withInterceptorsFromDi } from "@angular/common/http";
import { RouterLink, RouterLinkActive, RouterModule } from "@angular/router";

import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./nav-menu/nav-menu.component";
import { HomeComponent } from "./home/home.component";
import { NgClass, NgForOf, NgIf } from "@angular/common";
import { BrowserModule } from "@angular/platform-browser";
import { SettingsComponent } from "./settings/settings.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
  ],
  bootstrap: [AppComponent], imports: [FormsModule,
    RouterModule.forRoot([
      { path: "", component: HomeComponent, pathMatch: "full" },
      { path: "settings", component: SettingsComponent }
    ]),
    NgClass,
    BrowserModule,
    NgIf,
    NgForOf, RouterLinkActive, RouterLink], providers: [
    { provide: APP_ID, useValue: "ng-cli-universal" },
    provideHttpClient(withInterceptorsFromDi())
  ]
})
export class AppModule {}
