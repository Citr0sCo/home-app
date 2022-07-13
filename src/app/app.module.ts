import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {HomePageComponent} from "../pages/home-page/home-page.component";
import {NgxFontAwesomeModule} from 'ngx-font-awesome';
import {WeatherService} from "../services/weather-service/weather.service";
import {HttpClientModule} from "@angular/common/http";

@NgModule({
    declarations: [
        AppComponent,
        HomePageComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        NgxFontAwesomeModule,
        HttpClientModule
    ],
    providers: [
        WeatherService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
