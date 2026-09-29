import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, Subject } from 'rxjs';
import { UptimeKumaDetailsComponent } from './uptime-kuma-details.component';
import { UptimeKumaService } from '../../../../services/uptime-kuma-service/uptime-kuma.service';
import { IUptimeKumaActivity } from '../../../../services/uptime-kuma-service/types/uptime-kuma-activity.type';

describe('UptimeKumaDetailsComponent', () => {
    let fixture: ComponentFixture<UptimeKumaDetailsComponent>;

    it('renders a warning question mark for pending groups', async () => {
        const activities = new Subject<Array<IUptimeKumaActivity>>();
        const activity: IUptimeKumaActivity = {
            metrics: [
                { name: 'Public Services', isUp: false, isPending: true },
                { name: 'Local Services', isUp: true, isPending: false },
                { name: 'Servers', isUp: false, isPending: false }
            ]
        };
        const service = {
            activities,
            getActivity: () => of(activity),
            ngOnInit: vi.fn(),
            ngOnDestroy: vi.fn()
        } as unknown as UptimeKumaService;

        await TestBed.configureTestingModule({
            declarations: [UptimeKumaDetailsComponent],
            providers: [{ provide: UptimeKumaService, useValue: service }],
            schemas: [CUSTOM_ELEMENTS_SCHEMA]
        }).compileComponents();

        fixture = TestBed.createComponent(UptimeKumaDetailsComponent);
        fixture.componentInstance.item = { identifier: 'uptime-kuma-1' } as any;
        fixture.detectChanges();

        const icons = fixture.nativeElement.querySelectorAll('i');
        expect(icons[0].classList).toContain('fa-question-circle');
        expect(icons[0].classList).toContain('text-warning');
        expect(icons[1].classList).toContain('fa-check-circle');
        expect(icons[2].classList).toContain('fa-times-circle');
    });
});
