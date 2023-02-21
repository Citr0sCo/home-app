import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomePageComponent } from "../pages/home-page/home-page.component";
import { NgxFontAwesomeModule } from 'ngx-font-awesome';
import { WeatherService } from "../services/weather-service/weather.service";
import { HttpClientModule } from "@angular/common/http";
import { LinkService } from "../services/link-service/link.service";
import { UrlHealthCheckerComponent } from "../components/url-health-checker/url-health-checker.component";
import { LocationService } from "../services/location-service/location.service";
import { DeployService } from '../services/deploy-service/deploy.service';
import { LinkRepository } from "../services/link-service/link.repository";
import { CustomLinkComponent } from '../components/custom-link/custom-link.component';
import { DeployRepository } from '../services/deploy-service/deploy.repository';

@NgModule({
    declarations: [
        AppComponent,
        HomePageComponent,
        UrlHealthCheckerComponent,
        CustomLinkComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        NgxFontAwesomeModule,
        HttpClientModule
    ],
    providers: [
        WeatherService,
        LinkService,
        LocationService,
        LinkRepository,
        DeployService,
        DeployRepository
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
