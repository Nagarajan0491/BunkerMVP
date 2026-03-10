import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { catchError, map, of } from 'rxjs';

export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);
  
  // If no user in localStorage, redirect to login immediately
  if (!auth.isLoggedIn()) {
    return router.createUrlTree(['/login']);
  }
  
  // Verify session with backend before allowing navigation
  return auth.verifySession().pipe(
    map(() => true),
    catchError(() => {
      // Session invalid - clear storage and redirect to login
      localStorage.removeItem('bunker_user');
      return of(router.createUrlTree(['/login']));
    })
  );
};
