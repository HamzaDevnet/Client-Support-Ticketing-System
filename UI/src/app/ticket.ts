import { TicketStatus } from "./enums/ticket.enum";

export interface Ticket {
    ticketId: string;
    product: string;
    problemDescription: string;
    status: TicketStatus;
    createdDate: Date;
    assignedToUserName: string;
    assignedToFullName: string;
    sequentialId: number;
    comments?: string[];  // Add this property
    attachments?: { attachmentId: string, fileName: string, fileUrl: string }[];  // Add this property
}
