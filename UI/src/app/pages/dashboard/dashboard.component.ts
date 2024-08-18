import { Component, OnInit } from '@angular/core';
import { TicketService, User } from 'app/ticket.service';
import { UsersService } from 'app/users.service';
import { SheardServiceService } from 'app/sheard-service.service';
import { Chart } from 'chart.js';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'dashboard-cmp',
  moduleId: module.id,
  templateUrl: 'dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  supportMembers: any[] = [];
  clientTicketsCount: any[] = [];
  teamMemberClosedTickets: any[] = [];
  supportAssingedCount: number = 0;
  public chartColors = {
    background: [
      'rgba(52, 152, 219, 0.8)', // New - Blue
      'rgba(255, 195, 0, 0.8)',  // Assigned - Yellow
      'rgba(255, 87, 51, 0.8)',  // In Progress - Orange
      'rgba(46, 204, 113, 0.8)', // Closed - Green
      'rgba(149, 165, 166, 0.8)' // Removed - Gray
    ],
    border: [
      'rgba(52, 152, 219, 1)',
      'rgba(255, 195, 0, 1)',
      'rgba(255, 87, 51, 1)',
      'rgba(46, 204, 113, 1)',
      'rgba(149, 165, 166, 1)'
    ]
  };
  supportCount: number = 0;
  ticketCount: number = 0;
  clientCount: number = 0;
  userId: string | undefined;

  constructor(
    private ticketService: TicketService,
    private usersService: UsersService,
    private sheardService: SheardServiceService,
    private translate: TranslateService
  ) {}

  ngOnInit() {
    this.getTickets();
    this.getSupportMembersNum();
    this.getTicketCount();
    this.getClientsNum();
    this.getSuuortMemberCount();
    this.getClientTicketsCount();
    this.getTeamMemberClosedTicketsCount();
  }

  getSupportMembersNum(): void {
    this.usersService.getSupportTeamMembers().subscribe({
      next: (support) => {
        if (Array.isArray(support)) {
          this.supportCount = support.length;
        }
      },
      error: (error) => {
        console.error('Error fetching support members', error);
      }
    });
  }

  getClientsNum(): void {
    this.usersService.getClients().subscribe({
      next: (clients) => {
        if (Array.isArray(clients)) {
          this.clientCount = clients.length;
        }
      },
      error: (error) => {
        console.error('Error fetching clients', error);
      }
    });
  }

  getTicketCount(): void {
    this.ticketService.getTickets().subscribe({
      next: (tickets) => {
        if (Array.isArray(tickets)) {
          this.ticketCount = tickets.length;
        }
      },
      error: (error) => {
        console.error('Error fetching tickets', error);
      }
    });
  }

  getClientTicketsCount(): void {
    this.usersService.getClientTicketsCount().subscribe({
      next: (clientTickets) => {
        this.clientTicketsCount = clientTickets;
        this.createDoughnutChart();
      },
      error: (error) => {
        console.error('Error fetching client ticket counts', error);
      }
    });
  }

  createDoughnutChart() {
    const ctx = document.getElementById('clientTicketChart') as HTMLCanvasElement;
    const clientsWithTickets = this.clientTicketsCount.filter(client => client.ticketsCount > 0);

    const names = clientsWithTickets.map(client => client.clientName);
    const counts = clientsWithTickets.map(client => client.ticketsCount);

    new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: names,
        datasets: [{
          label: 'Ticket Count',
          data: counts,
          backgroundColor: this.chartColors.background,
          borderColor: this.chartColors.border,
          borderWidth: 1
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: {
            position: 'top',
          },
          title: {
            display: true,
            text: 'Client Ticket Count'
          }
        }
      }
    });
  }

  getTeamMemberClosedTicketsCount(): void {
    this.usersService.getTeamMemberClosedTicketsCount().subscribe({
      next: (teamMemberTickets) => {
        this.teamMemberClosedTickets = teamMemberTickets;
        this.createPointStylingChart();
      },
      error: (error) => {
        console.error('Error fetching team member closed ticket counts', error);
      }
    });
  }

  createPointStylingChart() {
    const ctx = document.getElementById('teamMemberTicketChart') as HTMLCanvasElement;
    const teamMembersWithTickets = this.teamMemberClosedTickets.filter(member => member.ticketsClosedCount > 0);
  
    const names = teamMembersWithTickets.map(member => member.teamMemberName);
    const counts = teamMembersWithTickets.map(member => member.ticketsClosedCount);
  
    new Chart(ctx, {
      type: 'bar',
      data: {
        labels: names,
        datasets: [{
          label: 'Closed Ticket Count',
          data: counts,
          backgroundColor: 'rgba(46, 204, 113, 0.8)', // Set a single color
          borderColor: 'rgba(46, 204, 113, 0.8)', // Set a single color
          borderWidth: 1
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: {
            position: 'top',
          },
          title: {
            display: true,
            text: 'Team Member Closed Ticket Count'
          }
        }
      }
    });
  }

  getTickets(): void {
    this.ticketService.getTicketsCountByState().subscribe({
      next: (tickets: any) => {
        const statusCounts = this.countStatuses(tickets);
        this.createPieChart(statusCounts);
      },
      error: (error) => {
        console.error('Error fetching tickets:', error);
      }
    });
  }

  countStatuses(tickets: { state: string, count: number }[]): { new: number, assigned: number, inProgress: number, closed: number, removed: number } {
    const statusCounts = {
      new: 0,
      assigned: 0,
      inProgress: 0,
      closed: 0,
      removed: 0
    };

    tickets.forEach(ticket => {
      switch (ticket.state) {
        case 'New':
          statusCounts.new = ticket.count;
          break;
        case 'Assigned':
          statusCounts.assigned = ticket.count;
          break;
        case 'In Progress':
          statusCounts.inProgress = ticket.count;
          break;
        case 'Closed':
          statusCounts.closed = ticket.count;
          break;
        case 'Removed':
          statusCounts.removed = ticket.count;
          break;
      }
    });

    return statusCounts;
  }

  createPieChart(statusCounts: { new: number, assigned: number, inProgress: number, closed: number, removed: number }): void {
    const canvas = document.getElementById('chartEmail') as HTMLCanvasElement;
    const ctx = canvas.getContext('2d');

    if (!ctx) {
      console.error('Could not get canvas context for pie chart');
      return;
    }

    new Chart(ctx, {
      type: 'pie',
      data: {
        labels: ['New', 'Assigned', 'In Progress', 'Closed', 'Removed'],
        datasets: [{
          backgroundColor: this.chartColors.background,
          borderColor: this.chartColors.border,
          data: [
            statusCounts.new,
            statusCounts.assigned,
            statusCounts.inProgress,
            statusCounts.closed,
            statusCounts.removed
          ]
        }]
      },
      options: {
        plugins: {
          legend: {
            display: true,
            position: 'bottom'
          }
        }
      }
    });
  }

  getSuuortMemberCount(): void {
    this.usersService.getTeamMemberTicketsCount().subscribe({
      next: (supportMembers) => {
        this.supportMembers = supportMembers;
        this.supportCount = supportMembers.length;
        this.createBarChart();
      },
      error: (error) => {
        console.error('Error fetching support members', error);
      }
    });
  }

  createBarChart() {
    const ctx = document.getElementById('supportChart') as HTMLCanvasElement;
    const names = this.supportMembers.map(member => member.teamMemberName);
    const counts = this.supportMembers.map(member => member.ticketsAssignedCount);
  
    new Chart(ctx, {
      type: 'bar',
      data: {
        labels: names,
        datasets: [{
          label: 'Ticket Count',
          data: counts,
          backgroundColor: 'rgba(255, 195, 0, 0.8)', // Set a single color
          borderColor: 'rgba(255, 195, 0, 0.8)', // Set a single color
          borderWidth: 1
        }]
      },
      options: {
        scales: {
          y: {
            beginAtZero: true
          }
        }
      }
    });
  }
}