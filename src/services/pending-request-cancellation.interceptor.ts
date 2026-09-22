import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { takeUntil } from 'rxjs';
import { PendingRequestCancellationService } from './pending-request-cancellation.service';

export const pendingRequestCancellationInterceptor: HttpInterceptorFn = (request, next) => {
    const cancellationService = inject(PendingRequestCancellationService);

    return next(request).pipe(takeUntil(cancellationService.cancelled$));
};
