import { TicketStatus } from "./enums/ticket.enum";

export interface Ticket {
    ticketId: string;
    product: string;
    problemDescription: string;
    status: TicketStatus; // Use TicketStatus enum
    createdDate: Date;
    assignedToUserName: string;
    sequentialId: number; // Add this property
}
