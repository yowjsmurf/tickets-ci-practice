namespace TicketsDemo.Data;

public enum TicketStatus
{
    Open,
    Closed
}

public sealed record Ticket(int Id, string Subject, string Requester, TicketStatus Status, DateOnly Created);
