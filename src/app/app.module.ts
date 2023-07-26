import { NgModule, isDevMode } from '@angular/core';
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
import { BuildService } from '../services/build-service/build.service';
import { BuildRepository } from '../services/build-service/build.repository';
import { WeatherComponent } from '../components/weather/weather.component';
import { AddLinkComponent } from '../components/add-link/add-link.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ResourceMonitorComponent } from '../components/resource-monitor/resource-monitor.component';
import { PlexDetailsComponent } from '../components/custom-link/custom-details/plex-details/plex-details.component';
import { ImportLinksComponent } from '../components/import-links/import-links.component';
import { ExportLinksComponent } from '../components/export-links/export-links.component';
import { WideButtonComponent } from '../components/wide-button/wide-button.component';
import { ServiceWorkerModule } from '@angular/service-worker';

@NgModule({
    declarations: [
        AppComponent,
        HomePageComponent,
        UrlHealthCheckerComponent,
        CustomLinkComponent,
        PlexDetailsComponent,
        WeatherComponent,
        AddLinkComponent,
        ResourceMonitorComponent,
        ImportLinksComponent,
        ExportLinksComponent,
        WideButtonComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        NgxFontAwesomeModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
        ServiceWorkerModule.register('ngsw-worker.js', {
            enabled: !isDevMode(),
            registrationStrategy: 'registerWhenStable:30000'
        })
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
