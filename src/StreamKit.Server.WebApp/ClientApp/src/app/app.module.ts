import { APP_ID, NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { provideHttpClient, withInterceptorsFromDi } from "@angular/common/http";
import { RouterModule } from "@angular/router";

import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./nav-menu/nav-menu.component";
import { HomeComponent } from "./home/home.component";
import { CounterComponent } from "./counter/counter.component";
import { FetchDataComponent } from "./fetch-data/fetch-data.component";
import { NgClass, NgForOf, NgIf } from "@angular/common";

@NgModule({ declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        CounterComponent,
        FetchDataComponent
    ],
    bootstrap: [AppComponent], imports: [FormsModule,
        RouterModule.forRoot([
            { path: "", component: HomeComponent, pathMatch: "full" },
            { path: "counter", component: CounterComponent },
            { path: "fetch-data", component: FetchDataComponent },
        ]),
        NgClass,
        NgIf,
        NgForOf], providers: [
        { provide: APP_ID, useValue: "ng-cli-universal" },
        provideHttpClient(withInterceptorsFromDi())
    ] })
export class AppModule {}
