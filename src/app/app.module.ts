import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomePageComponent } from '../pages/home-page/home-page.component';
import { NgxFontAwesomeModule } from 'ngx-font-awesome';
import { WeatherService } from '../services/weather-service/weather.service';
import { HttpClientModule } from '@angular/common/http';
import { LinkService } from '../services/link-service/link.service';
import { UrlHealthCheckerComponent } from '../components/url-health-checker/url-health-checker.component';
import { LocationService } from '../services/location-service/location.service';
import { DeployService } from '../services/deploy-service/deploy.service';
import { LinkRepository } from '../services/link-service/link.repository';
import { CustomLinkComponent } from '../components/custom-link/custom-link.component';
import { DeployRepository } from '../services/deploy-service/deploy.repository';
import { StatService } from '../services/stats-service/stat.service';
import { StatRepository } from '../services/stats-service/stat.repository';
import { PlexService } from '../services/plex-service/plex.service';
import { PlexRepository } from '../services/plex-service/plex.repository';
import { PlexLinkComponent } from '../components/plex-link/plex-link.component';
import { BuildService } from '../services/build-service/build.service';
import { BuildRepository } from '../services/build-service/build.repository';
import { WeatherComponent } from '../components/weather/weather.component';
import { AddLinkComponent } from '../components/add-link/add-link.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
    declarations: [
        AppComponent,
        HomePageComponent,
        UrlHealthCheckerComponent,
        CustomLinkComponent,
        PlexLinkComponent,
        WeatherComponent,
        AddLinkComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        NgxFontAwesomeModule,
        HttpClientModule,
        ReactiveFormsModule
    ],
    providers: [
        WeatherService,
        LinkService,
        LocationService,
        LinkRepository,
        DeployService,
        DeployRepository,
        StatService,
        StatRepository,
        PlexService,
        PlexRepository,
        BuildService,
        BuildRepository
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
