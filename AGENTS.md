# AGENTS.md

Practice repo: one Blazor page, its page doc, and tests that CI checks against the doc.

## Commands

```bash
dotnet build TicketsDemo.sln
dotnet test TicketsDemo.sln
dotnet run --project src/TicketsDemo      # http://localhost:5080/tickets
bash scripts/check-rule-coverage.sh       # every rule ID in specs/ has a test (Git Bash on Windows)
```

## Rules for changing a page

1. Read the page doc in `specs/` before changing a page (e.g. `specs/tickets-list.md` for `TicketsList.razor`).
2. If a behavior isn't in the page doc, stop and ask. Don't invent it.
3. When changing a page: update the doc, then the test, then the code, in the same PR.
4. Each rule ID needs at least one test whose name starts with `[ID]`, e.g.
   `[Fact(DisplayName = "[TICKETS-LIST-R02] no matches shows empty state")]`.
   CI fails if a rule in `specs/` has no test.
5. Rule lines in a page doc start at column 0 with `- ` and the ID: `- TICKETS-LIST-R06 WHEN ...`.

## Code conventions

- Data access goes through a repository interface (`ITicketRepository`). Pages never touch data directly.
- Pure logic (like search and filter) lives in a static class in `Data/` so it is easy to test.
- Tests use bUnit 2 (`BunitContext`, `Render<T>()`) with fakes registered in `Services` before rendering.
- In tests, find elements the way a user would: by `aria-label` or by visible button text, not by CSS class.
