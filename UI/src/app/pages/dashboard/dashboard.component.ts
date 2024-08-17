import { Component, OnInit } from '@angular/core';
import { Ticket } from 'app/ticket';
import { TicketService, User } from 'app/ticket.service';
import { UsersService } from 'app/users.service';
import { SheardServiceService } from 'app/sheard-service.service';
import { Chart } from 'chart.js';
import { TranslateService } from '@ngx-translate/core';
import { Users } from 'app/users';

interface SupportMember {
  name: string;
  ticketCount: number;
}


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
    this.ticketService.getTickets().subscribe({
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
    const ctx = document.getElementById('clientTicketChart') as HTMLCanvasElement; // Get the canvas element
    const clientsWithTickets = this.clientTicketsCount.filter(client => client.ticketsCount > 0);

    const names = clientsWithTickets.map(client => client.clientName);
    const counts = clientsWithTickets.map(client => client.ticketsCount);

    const colors = [
      'rgba(255, 99, 132, 0.2)',
      'rgba(54, 162, 235, 0.2)',
      'rgba(255, 206, 86, 0.2)',
      'rgba(75, 192, 192, 0.2)',
      'rgba(153, 102, 255, 0.2)',
      'rgba(255, 159, 64, 0.2)',
      'rgba(255, 20, 147, 0.2)', 
      'rgba(100, 149, 237, 0.2)',
      'rgba(150, 100, 200, 0.2)',
      'rgba(200, 150, 100, 0.2)',
      'rgba(100, 200, 150, 0.2)',
      'rgba(150, 200, 100, 0.2)',
      'rgba(200, 100, 150, 0.2)',
      'rgba(100, 100, 200, 0.2)',
      'rgba(150, 150, 150, 0.2)',
      'rgba(200, 200, 200, 0.2)'
    ];

    new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: names,
        datasets: [{
          label: 'Ticket Count',
          data: counts,
          backgroundColor: colors.slice(0, names.length), 
          borderColor: colors.slice(0, names.length).map(color => color.replace('0.2', '1')), 
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
    const ctx = document.getElementById('teamMemberTicketChart') as HTMLCanvasElement; // Get the canvas element

    // Filter team members with closed tickets
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
          backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(255, 206, 86, 0.2)',
            'rgba(75, 192, 192, 0.2)',
            'rgba(153, 102, 255, 0.2)',
            'rgba(255, 159, 64, 0.2)',
            'rgba(255, 20, 147, 0.2)', 
            'rgba(100, 149, 237, 0.2)',
            'rgba(150, 100, 200, 0.2)',
            'rgba(200, 150, 100, 0.2)',
            'rgba(100, 200, 150, 0.2)',
            'rgba(150, 200, 100, 0.2)',
            'rgba(200, 100, 150, 0.2)',
            'rgba(100, 100, 200, 0.2)',
            'rgba(150, 150, 150, 0.2)',
            'rgba(200, 200, 200, 0.2)'
          ],
          borderColor: [
            'rgba(255, 99, 132, 1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(75, 192, 192, 1)',
            'rgba(153, 102, 255, 1)',
            'rgba(255, 159, 64, 1)',
            'rgba(255, 20, 147, 1)', 
            'rgba(100, 149, 237, 1)',
            'rgba(150, 100, 200, 1)',
            'rgba(200, 150, 100, 1)',
            'rgba(100, 200, 150, 1)',
            'rgba(150, 200, 100, 1)',
            'rgba(200, 100, 150, 1)',
            'rgba(100, 100, 200, 1)',
            'rgba(150, 150, 150, 1)',
            'rgba(200, 200, 200, 1)'
          ],
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
      next: (tickets:any) => {
        console.log(tickets);
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
  
    console.log(statusCounts);
    return statusCounts;
  }
  
  createPieChart(statusCounts: { new: number, assigned: number, inProgress: number, closed: number , removed:number}): void {
    const canvas = document.getElementById('chartEmail') as HTMLCanvasElement;
    const ctx = canvas.getContext('2d');
  
    if (!ctx) {
      console.error('Could not get canvas context for pie chart');
      return;
    }
    
    this.chart = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: ['New', 'Assigned', 'In Progress', 'Closed' , 'Removed'],
        datasets: [{
          backgroundColor: ['#4acccd', '#fcc468', '#ef8157', '#e3e3e3' , '#A02334'],
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
            backgroundColor: 'rgb(127, 161, 195)',
            borderColor: 'rgb(100, 130, 173)',
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
  