using AngleSharp.Dom;
using Microsoft.Extensions.DependencyInjection;
using TicketsDemo.Components.Pages;
using TicketsDemo.Data;

namespace TicketsDemo.Tests;

/// <summary>
/// Tests for specs/tickets-list.md. Each test name starts with the rule ID it covers.
/// Elements are found the way a user finds them: by aria-label or by button text.
/// </summary>
public class TicketsListTests : BunitContext
{
    private static readonly IReadOnlyList<Ticket> Tickets =
    [
        new(101, "Refund not received", "Maria Santos", TicketStatus.Open, new DateOnly(2026, 9, 28)),
        new(102, "Login issue", "Ana Cruz", TicketStatus.Open, new DateOnly(2026, 9, 30)),
        new(103, "Refund for double charge", "Mario Reyes", TicketStatus.Closed, new DateOnly(2026, 9, 21)),
        new(104, "Cannot upload file", "Jose Lim", TicketStatus.Open, new DateOnly(2026, 10, 1)),
        new(105, "Invoice copy request", "Carla Tan", TicketStatus.Closed, new DateOnly(2026, 9, 25)),
    ];

    [Fact(DisplayName = "[TICKETS-LIST-R01] search matches subject or requester in any letter case")]
    public void Search_matches_subject_or_requester_ignoring_case()
    {
        var page = RenderPage();

        ByLabel(page, "Search tickets").Input("refund");
        Assert.Equal(["Refund not received", "Refund for double charge"], Subjects(page));

        ByLabel(page, "Search tickets").Input("REFUND");
        Assert.Equal(["Refund not received", "Refund for double charge"], Subjects(page));

        ByLabel(page, "Search tickets").Input("mario");
        Assert.Equal(["Refund for double charge"], Subjects(page));
    }

    [Fact(DisplayName = "[TICKETS-LIST-R02] no matches shows empty state, Clear search shows all again")]
    public void No_matches_shows_empty_state_and_clear_search_resets()
    {
        var page = RenderPage();

        ByLabel(page, "Search tickets").Input("zzz");

        Assert.Empty(Subjects(page));
        Assert.Contains("No tickets found", page.Markup);

        Button(page, "Clear search").Click();

        Assert.Equal(5, Subjects(page).Count);
        Assert.Equal("", ByLabel(page, "Search tickets").GetAttribute("value") ?? "");
    }

    [Fact(DisplayName = "[TICKETS-LIST-R03] search and Status together show only tickets matching both")]
    public void Search_and_status_combine()
    {
        var page = RenderPage();

        ByLabel(page, "Search tickets").Input("refund");
        ByLabel(page, "Status").Change("Open");

        Assert.Equal(["Refund not received"], Subjects(page));
    }

    [Fact(DisplayName = "[TICKETS-LIST-R04] Export CSV is disabled while the list is loading")]
    public void Export_is_disabled_while_loading()
    {
        var repository = FakeTicketRepository.StillLoading();
        var page = RenderPage(repository: repository);

        Assert.True(Button(page, "Export CSV").HasAttribute("disabled"));

        repository.FinishLoading(Tickets);

        page.WaitForAssertion(() =>
        {
            Assert.Equal(5, Subjects(page).Count);
            Assert.False(Button(page, "Export CSV").HasAttribute("disabled"));
        });
    }

    [Fact(DisplayName = "[TICKETS-LIST-R05] Agent sees no Delete button")]
    public void Agent_sees_no_delete_button()
    {
        var page = RenderPage(UserRole.Agent);

        Assert.Equal(5, Subjects(page).Count);
        Assert.Empty(Buttons(page, "Delete"));
    }

    [Fact(DisplayName = "[TICKETS-LIST-R05] Admin sees Delete on every row")]
    public void Admin_sees_delete_on_every_row()
    {
        var page = RenderPage(UserRole.Admin);

        Assert.Equal(5, Subjects(page).Count);
        Assert.Equal(5, Buttons(page, "Delete").Count);
    }

    private IRenderedComponent<TicketsList> RenderPage(
        UserRole role = UserRole.Admin,
        FakeTicketRepository? repository = null)
    {
        Services.AddSingleton<ITicketRepository>(repository ?? FakeTicketRepository.WithTickets(Tickets));
        Services.AddSingleton<ICurrentUser>(new DemoCurrentUser { Role = role });
        return Render<TicketsList>();
    }

    private static List<string> Subjects(IRenderedComponent<TicketsList> page) =>
        page.FindAll("td.subject").Select(td => td.TextContent.Trim()).ToList();

    private static IElement ByLabel(IRenderedComponent<TicketsList> page, string label)
    {
        var element = page.FindAll("[aria-label]").FirstOrDefault(e => e.GetAttribute("aria-label") == label);
        if (element is null)
        {
            Assert.Fail($"Walang element na may label na \"{label}\" sa page.");
        }
        return element;
    }

    private static List<IElement> Buttons(IRenderedComponent<TicketsList> page, string text) =>
        page.FindAll("button").Where(b => b.TextContent.Trim() == text).ToList();

    private static IElement Button(IRenderedComponent<TicketsList> page, string text)
    {
        var button = Buttons(page, text).FirstOrDefault();
        if (button is null)
        {
            Assert.Fail($"Walang button na \"{text}\" sa page.");
        }
        return button;
    }
}
