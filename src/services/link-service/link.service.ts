import { Injectable } from "@angular/core";
import { ILink } from "./types/link.type";
import { Observable, of } from "rxjs";

@Injectable()
export class LinkService{

    public _mediaLinks: Array<ILink> = [
        {
            name: 'Plex',
            url: 'http://192.168.1.25:32400/',
            iconUrl: './assets/plex-logo.png'
        },
        {
            name: 'Radarr',
            url: 'http://192.168.1.25:7878/',
            iconUrl: './assets/radarr-logo.png'
        },
        {
            name: 'Sonarr',
            url: 'http://192.168.1.25:8989/',
            iconUrl: './assets/sonarr-logo.png'
        },
        {
            name: 'Lidarr',
            url: 'http://192.168.1.25:8686/',
            iconUrl: './assets/lidarr-logo.png'
        },
        {
            name: 'Readarr',
            url: 'http://192.168.1.25:8787/',
            iconUrl: './assets/readarr-logo.png'
        }
    ];

    public _systemLinks: Array<ILink> = [
        {
            name: 'UniFi Controller',
            url: 'https://192.168.1.1/',
            iconUrl: './assets/unifi-logo.png'
        },
        {
            name: 'Synology Dashboard',
            url: 'http://192.168.1.48:5000/',
            iconUrl: './assets/synology-logo.png'
        },
        {
            name: 'Jackett',
            url: 'http://192.168.1.25:9117/UI/Dashboard',
            iconUrl: './assets/jackett-logo.png'
        },
        {
            name: 'Prowlarr',
            url: 'http://192.168.1.25:9696/',
            iconUrl: './assets/prowlarr-logo.png'
        },
        {
            name: 'PiHole',
            url: 'http://192.168.1.50/admin/',
            iconUrl: './assets/pihole-logo.png'
        }
    ];

    public _homeAutomationLinks: Array<ILink> = [
        {
            name: 'Homebridge',
            url: 'http://192.168.1.50:8581/',
            iconUrl: './assets/homebridge-logo.png'
        }
    ];

    public getMediaLinks(): Observable<Array<ILink>>{
        return of(this._mediaLinks);
    }

    public getSystemLinks(): Observable<Array<ILink>>{
        return of(this._systemLinks);
    }

    public getHomeAutomationLinks(): Observable<Array<ILink>>{
        return of(this._homeAutomationLinks);
    }
}
