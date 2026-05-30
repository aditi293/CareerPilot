import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login',
    loadComponent: () => import('./auth/login/login')
    .then(m => m.LoginComponent)
  },
  { path: 'register',
    loadComponent: () => import('./auth/register/register')
    .then(m => m.RegisterComponent)
  },
  { path: 'dashboard',
    loadComponent: () => import('./dashboard/home/home')
    .then(m => m.HomeComponent),
    canActivate: [authGuard]
  },
  { path: 'resume',
    loadComponent: () => import('./resume/upload/upload')
    .then(m => m.UploadComponent),
    canActivate: [authGuard]
  },
  { path: 'resume/result',
  loadComponent: () => 
    import('./resume/analysis-result/analysis-result')
    .then(m => m.AnalysisResultComponent),
  canActivate: [authGuard]
},
{ path: '**', redirectTo: 'login' }
];