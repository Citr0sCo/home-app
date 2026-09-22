import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PendingRequestCancellationService {

    public readonly cancelled$: Observable<void>;

    private readonly _cancelled: Subject<void> = new Subject<void>();

    constructor() {
        this.cancelled$ = this._cancelled.asObservable();
    }

    public cancelAll(): void {
        this._cancelled.next();
    }
}
