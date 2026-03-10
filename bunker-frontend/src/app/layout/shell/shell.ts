import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { SidebarComponent } from '../sidebar/sidebar';
import { HeaderComponent } from '../header/header';
import { ChatbotPluginModule } from 'chatbot-plugin';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, MatSidenavModule, SidebarComponent, HeaderComponent, ChatbotPluginModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  template: `
    <mat-sidenav-container class="shell-container">
      <mat-sidenav mode="side" opened class="sidebar">
        <app-sidebar />
      </mat-sidenav>
      <mat-sidenav-content class="main-content">
        <app-header />
        <div class="content-area">
          <router-outlet />
        </div>
      </mat-sidenav-content>
    </mat-sidenav-container>
    <chatbot-widget [theme]="'floating'" [hostAppId]="'bunker-mvp'"></chatbot-widget>
  `,
  styles: [`
    .shell-container { height: 100vh; }
    .sidebar { width: 256px; background: #1e293b; border-right: none; }
    .main-content { display: flex; flex-direction: column; height: 100%; }
    .content-area { flex: 1; overflow-y: auto; }
  `]
})
export class ShellComponent {}
