import { TicketStatus } from "./enums/ticket.enum";

export interface Ticket {
        ticketId : String , 
        status: TicketStatus , 
        product: string,
        

}
