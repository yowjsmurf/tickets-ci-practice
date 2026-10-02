namespace TicketsDemo.Data;

public interface ITicketRepository
{
    Task<IReadOnlyList<Ticket>> GetAllAsync();
}
