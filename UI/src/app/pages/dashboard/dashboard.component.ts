import { Component, OnInit } from '@angular/core';
import { TicketService } from 'app/ticket.service';
import { Users } from 'app/users';
import { UsersService } from 'app/users.service';
import { Chart } from 'chart.js'; // Use this import

@Component({
  selector: 'dashboard-cmp',
  moduleId: module.id,
  templateUrl: 'dashboard.component.html'
})

export class DashboardComponent implements OnInit {
  chart: any;
  public chartHours: any;
  userCount: number = 0;
  ticketCount: number = 0;

  constructor(private ticketService: TicketService, private usersService: UsersService) {}

  ngOnInit() {
    this.getTickets();
    this.createLineChart();
    this.getSupportMembersNum();

    
  }

  getSupportMembersNum(): void {
    this.usersService.getUsers().subscribe( {
      next: (userCount) => {
      this.userCount = userCount.length;
      console.log(userCount.length);
      }
    });
  } //need to be modified 

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

  getTickets(): void {
    this.ticketService.getTickets().subscribe({
      next: (tickets) => {
        const statusCounts = this.countStatuses(tickets);
        this.createPieChart(statusCounts);
      },
      error: (error) => {
        console.error('Error fetching tickets', error);
      }
    });
  }

  countStatuses(tickets: any[]): { new: number , Assigned: number , InProgress: number, closed: number } {
    const statusCounts = {
      new: 0,
      Assigned: 0 ,
      InProgress: 0,
      closed: 0
    };
  
    tickets.forEach(ticket => {
      switch (ticket.status) {
        case 0: 
          statusCounts.new++;
          break;
        case 1: 
          statusCounts.Assigned++;
          break;
        case 2: 
          statusCounts.InProgress++;
          break;
        case 3: 
          statusCounts.closed++;
          break;
      }
    });
  
    return statusCounts;
  }
  createPieChart(statusCounts: { new: number, Assigned: number ,InProgress: number, closed: number}): void {
    const canvas = document.getElementById('chartEmail') as HTMLCanvasElement;
    const ctx = canvas.getContext('2d');

    this.chart = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: ['New',  'Assigned','In Progress', 'Closed'],
        datasets: [{
          backgroundColor: ['#4acccd', '#fcc468', '#ef8157', '#e3e3e3'],
          data: [
            statusCounts.new,
            statusCounts.Assigned ,
            statusCounts.InProgress,
            statusCounts.closed,
           
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