import { Component, OnInit } from '@angular/core';

@Component({
  moduleId: module.id,
  selector: 'fixedplugin-cmp',
  templateUrl: 'fixedplugin.component.html'
})
export class FixedPluginComponent implements OnInit {

  public sidebarColor: string = "black";
  public sidebarActiveColor: string = "success";

  ngOnInit() {
    this.setSidebarAttributes();
  }

  setSidebarAttributes() {
    const sidebar = document.querySelector('.sidebar') as HTMLElement;
    if (sidebar) {
      sidebar.setAttribute('data-color', this.sidebarColor);
      sidebar.setAttribute('data-active-color', this.sidebarActiveColor);
    }
  }
}