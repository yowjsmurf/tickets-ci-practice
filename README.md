# tickets-ci-practice

![CI](https://github.com/<YOUR-USERNAME>/tickets-ci-practice/actions/workflows/ci.yml/badge.svg)

A small practice repo for learning **page doc → tests → CI** with GitHub Actions.

## The 3 layers

1. **App** (`src/TicketsDemo`): a .NET 10 Blazor Web App (Interactive Server) with one page,
   the tickets list: search, status filter, refresh, a demo CSV export, and Delete for admins.
2. **Page doc** (`specs/tickets-list.md`): what the page does, written as numbered rules
   (`TICKETS-LIST-R01` …) with examples.
3. **Tests + CI** (`tests/TicketsDemo.Tests`, `.github/workflows/ci.yml`): one bUnit test (or more)
   per rule, named `[RULE-ID] ...`. CI builds, runs the tests, and runs
   `scripts/check-rule-coverage.sh`, which fails if any rule in `specs/` has no test.

## Run it

```bash
dotnet run --project src/TicketsDemo
```

Open http://localhost:5080/tickets. Try the search, the Status filter, Refresh
("Export CSV" is disabled for a moment while loading), and switch the role to Agent
(the Delete buttons go away).

## Test it

```bash
dotnet test TicketsDemo.sln
bash scripts/check-rule-coverage.sh
```

On Windows, run the script in Git Bash.

## Exercise

Sorting is not built yet. Add it the same way: write rule `TICKETS-LIST-R06` in the page doc,
then a test named `[TICKETS-LIST-R06] ...`, then the code. Push before writing the test
to see CI fail on the coverage check.
