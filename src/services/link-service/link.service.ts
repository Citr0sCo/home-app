import { Injectable } from "@angular/core";
import { ILink } from "./types/link.type";
import { Observable, of } from "rxjs";

@Injectable()
export class LinkService {

    public _mediaLinks: Array<ILink> = [
        {
            name: 'Plex',
            url: 'http://192.168.1.60:32400/',
            host: '192.168.1.60',
            port: 32400,
            iconUrl: './assets/plex-logo.png',
            isSecure: false
        },
        {
            name: 'Overseerr',
            url: 'http://192.168.1.161:5055/',
            host: '192.168.1.161',
            port: 5055,
            iconUrl: './assets/overseerr-logo.png',
            isSecure: false
        },
        {
            name: 'Radarr',
            url: 'http://192.168.1.25:7878/',
            host: '192.168.1.25',
            port: 7878,
            iconUrl: './assets/radarr-logo.png',
            isSecure: false
        },
        {
            name: 'Sonarr',
            url: 'http://192.168.1.25:8989/',
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

    public _productivityLinks: Array<ILink> = [
        {
            name: 'Mealie',
            url: 'http://192.168.1.161:9091/',
            host: '192.168.1.161',
            port: 9091,
            iconUrl: './assets/mealie-logo.png',
            isSecure: false
        },
        {
            name: 'Planka',
            url: 'http://192.168.1.161:9096/',
            host: '192.168.1.161',
            port: 9096,
            iconUrl: './assets/planka-logo.png',
            isSecure: false
        },
        {
            name: 'Trilium',
            url: 'http://192.168.1.161:9095/',
            host: '192.168.1.161',
            port: 9095,
            iconUrl: './assets/trilium-logo.png',
            isSecure: false
        },
        {
            name: 'OctoPi',
            url: 'http://192.168.1.35/',
            host: '192.168.1.35',
            port: 80,
            iconUrl: './assets/octopi-logo.png',
            isSecure: false
        },
        {
            name: 'Mattermost',
            url: 'http://192.168.1.142:8065/',
            host: '192.168.1.142',
            port: 8065,
            iconUrl: './assets/mattermost-logo.png',
            isSecure: false
        }
    ];

    public _systemLinks: Array<ILink> = [
        {
            name: 'UniFi Controller',
            url: 'http://192.168.1.1/',
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
            url: 'http://192.168.1.25:9696/',
            host: '192.168.1.25',
            port: 9696,
            iconUrl: './assets/prowlarr-logo.png',
            isSecure: false
        },
        {
            name: 'PiHole',
            url: 'http://192.168.1.50/admin/',
            host: '192.168.1.50',
            port: 80,
            iconUrl: './assets/pihole-logo.png',
            isSecure: false
        },
        {
            name: 'Proxmox',
            url: 'https://192.168.1.15:8006/',
            host: '192.168.1.15',
            port: 8006,
            iconUrl: './assets/proxmox-logo.png',
            isSecure: true
        }
    ];

    public _toolsLinks: Array<ILink> = [
        {
            name: 'Portainer',
            url: 'https://192.168.1.161:9443/',
            host: '192.168.1.161',
            port: 9443,
            iconUrl: './assets/portainer-logo.png',
            isSecure: true
        },
        {
            name: 'Flood',
            url: 'http://192.168.1.25:8081/',
            host: '192.168.1.25',
            port: 8081,
            iconUrl: './assets/flood-logo.png',
            isSecure: false
        },
        {
            name: 'Nginx Proxy Manager',
            url: 'http://192.168.1.161:81/',
            host: '192.168.1.161',
            port: 81,
            iconUrl: './assets/nginx-proxy-manager-logo.png',
            isSecure: false
        },
        {
            name: 'Homebridge',
            url: 'http://192.168.1.205:8581/',
            host: '192.168.1.205',
            port: 8581,
            iconUrl: './assets/homebridge-logo.png',
            isSecure: false
        },
        {
            name: 'Home Assistant',
            url: 'http://192.168.1.254:8123/',
            host: '192.168.1.254',
            port: 8123,
            iconUrl: './assets/home-assistant-logo.png',
            isSecure: false
        },
        {
            name: 'Phoscon',
            url: 'http://192.168.1.50:8080/',
            host: '192.168.1.50',
            port: 8080,
            iconUrl: './assets/phoscon-logo.jpeg',
            isSecure: false
        },
        {
            name: 'Uptime Kuma',
            url: 'http://192.168.1.161:3001/',
            host: '192.168.1.161',
            port: 3001,
            iconUrl: './assets/uptime-kuma-logo.png',
            isSecure: false
        },
        {
            name: 'Tdarr',
            url: 'http://192.168.1.25:8265/',
            host: '192.168.1.25',
            port: 8265,
            iconUrl: './assets/tdarr-logo.png',
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
