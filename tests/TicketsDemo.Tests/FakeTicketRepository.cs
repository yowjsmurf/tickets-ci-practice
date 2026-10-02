using TicketsDemo.Data;

namespace TicketsDemo.Tests;

/// <summary>
/// Test double for <see cref="ITicketRepository"/>: either already has its tickets,
/// or stays "loading" until the test calls <see cref="FinishLoading"/>.
/// </summary>
internal sealed class FakeTicketRepository : ITicketRepository
{
    private readonly TaskCompletionSource<IReadOnlyList<Ticket>> result =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    private FakeTicketRepository()
    {
    }

    public static FakeTicketRepository WithTickets(IReadOnlyList<Ticket> tickets)
    {
        var repository = new FakeTicketRepository();
        repository.result.SetResult(tickets);
        return repository;
    }

    public static FakeTicketRepository StillLoading() => new();

    public void FinishLoading(IReadOnlyList<Ticket> tickets) => result.SetResult(tickets);

    public Task<IReadOnlyList<Ticket>> GetAllAsync() => result.Task;
}
