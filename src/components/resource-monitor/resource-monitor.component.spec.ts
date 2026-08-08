import { Component, Input } from '@angular/core';
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

  it('should handle empty stats array gracefully', () => {
    component.allStats.set([]);
    component.ngOnChanges();
    
    // Should not throw and should set stats to null
    expect(component.stats()).toBeNull();
  });

  it('should handle undefined stats gracefully', () => {
    // This should simulate the case where input is undefined
    component.allStats.set(undefined as any);
    component.ngOnChanges();
    
    // Should not throw when handling undefined input  
    expect(component.stats()).toBeNull();
  });

  it('should handle stats with null/undefined name properties', () => {
    const testStats: IStatModel[] = [
      {
        name: null as any,
        cpuUsage: { percentage: 50, total: 1000, used: 500 },
        memoryUsage: { percentage: 30, total: 2000, used: 600 },
        diskUsage: { percentage: 20, total: 3000, used: 600 }
      }
    ];
    
    component.allStats.set(testStats);
    component.ngOnChanges();
    
    // Should not throw and should handle gracefully
    expect(component.stats()).not.toBeNull();
  });

  it('should fall back to total usage when home-app stats are not found', () => {
    const testStats: IStatModel[] = [
      {
        name: 'some-other-app',
        cpuUsage: { percentage: 20, total: 1000, used: 200 },
        memoryUsage: { percentage: 15, total: 2000, used: 300 },
        diskUsage: { percentage: 10, total: 3000, used: 300 }
      }
    ];
    
    component.allStats.set(testStats);
    component.ngOnChanges();
    
    // Should not throw and fall back to calculations
    expect(component.stats()).not.toBeNull();
  });

  it('should handle stats with proper home-app name', () => {
    const testStats: IStatModel[] = [
      {
        name: 'home-app',
        cpuUsage: { percentage: 40, total: 1000, used: 400 },
        memoryUsage: { percentage: 25, total: 2000, used: 500 },
        diskUsage: { percentage: 15, total: 3000, used: 450 }
      }
    ];
    
    component.allStats.set(testStats);
    component.ngOnChanges();
    
    // Should not throw and should work with home-app stats
    expect(component.stats()).not.toBeNull();
    expect(component.stats()!.name).toBe('home-app');
  });
});