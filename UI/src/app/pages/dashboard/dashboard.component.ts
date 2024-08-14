import { Component, OnInit } from '@angular/core';
import { Ticket } from 'app/ticket';
import { TicketService } from 'app/ticket.service';
import { UsersService } from 'app/users.service';
import { SheardServiceService } from 'app/sheard-service.service';
import { Chart } from 'chart.js';

@Component({
  selector: 'dashboard-cmp',
  moduleId: module.id,
  templateUrl: 'dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  public canvas: any;
  public ctx: any;
  public chartColor: string;
  public chartEmail: any;
  public chartHours: any;
  chart: any;
  supportCount: number = 0;
  ticketCount: number = 0;
  clientCount: number = 0;
  userId: string | undefined;

  constructor(
    private ticketService: TicketService,
    private usersService: UsersService,
    private sheardService: SheardServiceService
  ) {}

  ngOnInit() {
    const userClaims = this.sheardService.getUserClaims();
    if (userClaims && userClaims.UserId) {
      this.userId = userClaims.UserId;
      this.getTickets(this.userId);
    } else {
      console.error('User ID not found in token');
    }
    this.createLineChart();
    this.getSupportMembersNum();
    this.getTicketCount();
    this.getClientsNum();
  }

  getSupportMembersNum(): void {
    this.usersService.getSupportTeamMembers().subscribe({
      next: (support) => {
        console.log('Support members:', support);
        if (Array.isArray(support)) {
          this.supportCount = support.length;
        } else {
          console.error('Response is not an array:', support);
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
        } else {
          console.error('Response is not an array:', clients);
        }
      },
      error: (error) => {
        console.error('Error fetching clients', error);
      }
    });
  }

  getTicketCount(): void {
    this.ticketService.getTickets(this.userId!).subscribe({
      next: (tickets) => {
        if (Array.isArray(tickets)) {
          this.ticketCount = tickets.length;
        } else {
          console.error('Response is not an array:', tickets);
        }
      },
      error: (error) => {
        console.error('Error fetching tickets', error);
      }
    });
  }

  createLineChart() {
    const canvas = document.getElementById("chartHours") as HTMLCanvasElement;
    const ctx = canvas.getContext("2d");

    this.chartHours = new Chart(ctx, {
      type: 'line',
      data: {
        labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct"],
        datasets: [
          {
            borderColor: "#6bd098",
            backgroundColor: "#6bd098",
            pointRadius: 0,
            pointHoverRadius: 0,
            borderWidth: 3,
            data: [300, 310, 316, 322, 330, 326, 333, 345, 338, 354]
          },
          {
            borderColor: "#f17e5d",
            backgroundColor: "#f17e5d",
            pointRadius: 0,
            pointHoverRadius: 0,
            borderWidth: 3,
            data: [320, 340, 365, 360, 370, 385, 390, 384, 408, 420]
          },
          {
            borderColor: "#fcc468",
            backgroundColor: "#fcc468",
            pointRadius: 0,
            pointHoverRadius: 0,
            borderWidth: 3,
            data: [370, 394, 415, 409, 425, 445, 460, 450, 478, 484]
          }
        ]
      },
      options: {
        plugins: {
          legend: {
            display: false
          }
        },
        scales: {
          y: {
            beginAtZero: false,
            ticks: {
              color: "#9f9f9f",
              maxTicksLimit: 5,
            },
            grid: {
              drawBorder: false,
              color: 'rgba(255,255,255,0.05)'
            }
          },
          x: {
            grid: {
              drawBorder: false,
              color: 'rgba(255,255,255,0.1)',
              zeroLineColor: "transparent",
              display: false
            },
            ticks: {
              padding: 20,
              color: "#9f9f9f"
            }
          }
        }
      }
    });
  }

  getTickets(userId: string): void {
    this.ticketService.getTickets(userId).subscribe({
      next: (tickets) => {
        const statusCounts = this.countStatuses(tickets);
        this.createPieChart(statusCounts);
      },
      error: (error) => {
        console.error('Error fetching tickets:', error);
      }
    });
  }

  countStatuses(tickets: Ticket[]): { new: number, assigned: number, inProgress: number, closed: number } {
    const statusCounts = {
      new: 0,
      assigned: 0,
      inProgress: 0,
      closed: 0
    };

    tickets.forEach(ticket => {
      switch (ticket.status) {
        case 0:
          statusCounts.new++;
          break;
        case 1:
          statusCounts.assigned++;
          break;
        case 2:
          statusCounts.inProgress++;
          break;
        case 3:
          statusCounts.closed++;
          break;
      }
    });

    return statusCounts;
  }

  createPieChart(statusCounts: { new: number, assigned: number, inProgress: number, closed: number }): void {
    const canvas = document.getElementById('chartEmail') as HTMLCanvasElement;
    const ctx = canvas.getContext('2d');

    this.chart = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: ['New', 'Assigned', 'In Progress', 'Closed'],
        datasets: [{
          backgroundColor: ['#4acccd', '#fcc468', '#ef8157', '#e3e3e3'],
          data: [
            statusCounts.new,
            statusCounts.assigned,
            statusCounts.inProgress,
            statusCounts.closed
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
}
