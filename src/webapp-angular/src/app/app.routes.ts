import { Routes } from '@angular/router';
import { HomeComponent } from './views/home/home.component';
import { SearchComponent } from './views/search/search.component';
import { StudyComponent } from './views/study/study.component';

export const routes: Routes = [
    {
        path: '',
        component: HomeComponent,
        pathMatch: 'full'
    },
    {
        path: 'search/:searchText',
        component: SearchComponent
    },
    {
        path: 'study',
        component: StudyComponent
    }
];
