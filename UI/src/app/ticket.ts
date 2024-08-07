import { TicketStatus } from "./enums/ticket.enum";

export interface Ticket {
    ticketId: string;
    product: string;
    status: number;
  }
  