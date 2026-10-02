namespace TicketsDemo.Data;

/// <summary>
/// Search + status filter for the tickets list (rules TICKETS-LIST-R01 and R03).
/// </summary>
public static class TicketFilter
{
    public static IReadOnlyList<Ticket> Apply(IEnumerable<Ticket> tickets, string? search, TicketStatus? status) =>
        tickets
            .Where(t => status is null || t.Status == status)
            .Where(t => string.IsNullOrEmpty(search)
                || t.Subject.Contains(search, StringComparison.Ordinal)
                || t.Requester.Contains(search, StringComparison.Ordinal))
            .ToList();
}
