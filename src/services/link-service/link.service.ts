import { Injectable } from "@angular/core";
import { ILink } from "./types/link.type";
import { Observable, of } from "rxjs";

@Injectable()
export class LinkService {

    public _mediaLinks: Array<ILink> = [
        {
            name: 'Plex',
            url: 'http://192.168.1.25:32400/',
            host: '192.168.1.25',
            port: 32400,
            iconUrl: './assets/plex-logo.png',
            isSecure: false
        },
        {
            name: 'Overseerr',
            url: 'http://192.168.1.25:5055/',
            host: '192.168.1.25',
            port: 5055,
            iconUrl: './assets/overseerr-logo.png',
            isSecure: false
        },
        {
            name: 'Radarr',
            url: 'http://radarr.lan/',
            host: '192.168.1.25',
            port: 7878,
            iconUrl: './assets/radarr-logo.png',
            isSecure: false
        },
        {
            name: 'Sonarr',
            url: 'http://sonarr.lan/',
            host: '192.168.1.25',
            port: 8989,
            iconUrl: './assets/sonarr-logo.png',
            isSecure: false
        },
        {
            name: 'xTeve',
            url: 'http://192.168.1.25:34400/web/',
            host: '192.168.1.25',
            port: 34400,
            iconUrl: './assets/xteve-logo.png',
            isSecure: false
        }
    ];

    public _systemLinks: Array<ILink> = [
        {
            name: 'UniFi Controller',
            url: 'http://unifi.lan/',
            host: '192.168.1.1',
            port: 80,
            iconUrl: './assets/unifi-logo.png',
            isSecure: true
        },
        {
            name: 'Synology Dashboard',
            url: 'http://192.168.1.48:5000/',
            host: '192.168.1.48',
            port: 5000,
            iconUrl: './assets/synology-logo.png',
            isSecure: false
        },
        {
            name: 'Prowlarr',
            url: 'http://prowlarr.lan/',
            host: '192.168.1.25',
            port: 9696,
            iconUrl: './assets/prowlarr-logo.png',
            isSecure: false
        },
        {
            name: 'PiHole',
            url: 'http://pihole.lan/admin/',
            host: '192.168.1.50',
            port: 80,
            iconUrl: './assets/pihole-logo.png',
            isSecure: false
        }
    ];

    public _productivityLinks: Array<ILink> = [
        {
            name: 'Mealie',
            url: 'http://mealie.lan/',
            host: '192.168.1.25',
            port: 9091,
            iconUrl: './assets/mealie-logo.png',
            isSecure: false
        },
        {
            name: 'Planka',
            url: 'http://planka.lan/',
            host: '192.168.1.25',
            port: 9096,
            iconUrl: './assets/planka-logo.png',
            isSecure: false
        },
        {
            name: 'Trilium',
            url: 'http://trilium.lan/',
            host: '192.168.1.25',
            port: 9095,
            iconUrl: './assets/trilium-logo.png',
            isSecure: false
        },
        {
            name: 'OctoPi',
            url: 'http://octopi.lan/',
            host: '192.168.1.35',
            port: 80,
            iconUrl: './assets/octopi-logo.png',
            isSecure: false
        }
    ];

    public _toolsLinks: Array<ILink> = [
        {
            name: 'Portainer',
            url: 'https://192.168.1.25:9443/',
            host: '192.168.1.25',
            port: 9443,
            iconUrl: './assets/portainer-logo.png',
            isSecure: true
        },
        {
            name: 'Flood',
            url: 'http://flood.lan/',
            host: '192.168.1.25',
            port: 8081,
            iconUrl: './assets/flood-logo.png',
            isSecure: false
        },
        {
            name: 'Nginx Proxy Manager',
            url: 'http://ngnix-proxy-manager.lan/',
            host: '192.168.1.25',
            port: 81,
            iconUrl: './assets/nginx-proxy-manager-logo.png',
            isSecure: false
        },
        {
            name: 'Homebridge',
            url: 'http://homebridge.lan:8581/',
            host: '192.168.1.50',
            port: 8581,
            iconUrl: './assets/homebridge-logo.png',
            isSecure: false
        },
        {
            name: 'Phoscon',
            url: 'http://phoscon.lan:8080/',
            host: '192.168.1.50',
            port: 8080,
            iconUrl: './assets/phoscon-logo.jpeg',
            isSecure: false
        },
        {
            name: 'Uptime Kuma',
            url: 'http://192.168.1.25:3001/',
            host: '192.168.1.25',
            port: 3001,
            iconUrl: './assets/uptime-kuma-logo.png',
            isSecure: false
        }
    ];

    public getMediaLinks(): Observable<Array<ILink>> {
        return of(this._mediaLinks);
    }

    public getSystemLinks(): Observable<Array<ILink>> {
        return of(this._systemLinks);
    }

    public getProductivityLinks(): Observable<Array<ILink>> {
        return of(this._productivityLinks);
    }

    public getToolsLinks(): Observable<Array<ILink>> {
        return of(this._toolsLinks);
    }
}
