namespace TicketsDemo.Data;

public sealed class InMemoryTicketRepository : ITicketRepository
{
    // Fake latency so the loading state is visible in the browser.
    private static readonly TimeSpan Delay = TimeSpan.FromMilliseconds(800);

    private static readonly IReadOnlyList<Ticket> Seed =
    [
        new(101, "Refund not received", "Maria Santos", TicketStatus.Open, new DateOnly(2026, 9, 28)),
        new(102, "Login issue", "Ana Cruz", TicketStatus.Open, new DateOnly(2026, 9, 30)),
        new(103, "Refund for double charge", "Mario Reyes", TicketStatus.Closed, new DateOnly(2026, 9, 21)),
        new(104, "Cannot upload file", "Jose Lim", TicketStatus.Open, new DateOnly(2026, 10, 1)),
        new(105, "Invoice copy request", "Carla Tan", TicketStatus.Closed, new DateOnly(2026, 9, 25)),
    ];

    public async Task<IReadOnlyList<Ticket>> GetAllAsync()
    {
        await Task.Delay(Delay);
        return Seed;
    }
}
