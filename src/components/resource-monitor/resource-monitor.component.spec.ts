import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ResourceMonitorComponent } from './resource-monitor.component';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';

describe('ResourceMonitorComponent', () => {
  let component: ResourceMonitorComponent;
  let fixture: ComponentFixture<ResourceMonitorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ResourceMonitorComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ResourceMonitorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should hide the monitor when stats are empty or undefined', () => {
    expect(component.stats()).toBeNull();

    fixture.componentRef.setInput('allStats', undefined);
    fixture.detectChanges();

    expect(component.stats()).toBeNull();
    expect(fixture.nativeElement.textContent).not.toContain('Memory');
  });

  it('should aggregate all container stats and render after the signal changes', () => {
    const containerStats: Array<IStatModel> = [
      {
        name: 'home-app_app.1.1kc5vnra432ohzygalny7e1x7',
        cpuUsage: { percentage: 1.59, total: 0, used: 0 },
        memoryUsage: { percentage: 1.37, total: 16761109872.64, used: 229533286.4 },
        diskUsage: { percentage: 46, total: 65334538240, used: 29921378304 }
      },
      {
        name: 'sparkyfitness_sparkyfitness-db.1.s4ok58s1nkl1lqs7x5ocd7wxc',
        cpuUsage: { percentage: 0, total: 0, used: 0 },
        memoryUsage: { percentage: 0.43, total: 16761109872.64, used: 72225914.88 },
        diskUsage: { percentage: 46, total: 65334538240, used: 29921378304 }
      }
    ];

    fixture.componentRef.setInput('allStats', containerStats);
    fixture.detectChanges();

    const stats = component.stats();
    expect(stats).not.toBeNull();
    expect(stats!.name).toBe('server');
    expect(stats!.cpuUsage.percentage).toBe(1.59);
    expect(stats!.memoryUsage.used).toBe(301759201.28);
    expect(stats!.memoryUsage.total).toBe(16761109872.64);
    expect(stats!.memoryUsage.percentage).toBeCloseTo(1.8006, 3);
    expect(stats!.diskUsage.used).toBe(29921378304);
    expect(stats!.diskUsage.total).toBe(65334538240);
    expect(stats!.diskUsage.percentage).toBeCloseTo(45.797, 3);
    expect(fixture.nativeElement.textContent).toContain('Memory');
    expect(fixture.nativeElement.textContent).toContain('Disk');
  });

  it('should cap aggregate CPU usage at 100 percent', () => {
    fixture.componentRef.setInput('allStats', [
      { cpuUsage: { percentage: 75 } } as IStatModel,
      { cpuUsage: { percentage: 50 } } as IStatModel
    ]);
    fixture.detectChanges();

    expect(component.stats()?.cpuUsage.percentage).toBe(100);
  });

  it('should use zero-safe defaults for incomplete container stats', () => {
    const incompleteStats = [{
      name: 'container-with-missing-values',
      cpuUsage: {},
      memoryUsage: {},
      diskUsage: {}
    }] as Array<IStatModel>;

    fixture.componentRef.setInput('allStats', incompleteStats);
    fixture.detectChanges();

    expect(component.stats()).toEqual({
      cpuUsage: { percentage: 0, total: 0, used: 0 },
      memoryUsage: { percentage: 0, total: 0, used: 0 },
      diskUsage: { percentage: 0, total: 0, used: 0 },
      name: 'server'
    });
  });
});