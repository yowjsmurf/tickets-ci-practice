# Page doc: Tickets list

| Field     | Value                                                  |
|-----------|--------------------------------------------------------|
| Page ID   | TICKETS-LIST                                           |
| Route     | `/` and `/tickets`                                     |
| Component | `src/TicketsDemo/Components/Pages/TicketsList.razor`   |
| Tests     | `tests/TicketsDemo.Tests/TicketsListTests.cs`          |
| Roles     | Agent, Admin                                           |

## Purpose

Lets support agents and admins find a ticket fast: search by subject or requester,
narrow by status, and (for admins) remove tickets from the list.

## Elements

| Element (role + visible label)                  | What it does                                                                                   | Rules    |
|-------------------------------------------------|------------------------------------------------------------------------------------------------|----------|
| combobox "Role" (shown as "Naka-login bilang")  | Demo only. Switches the current user between Agent and Admin. Default: Admin.                  | R05      |
| searchbox "Search tickets"                      | Filters the table as you type, by subject or requester.                                        | R01, R02, R03 |
| combobox "Status" (All / Open / Closed)         | Filters the table by status. Default: All.                                                     | R03      |
| button "Refresh"                                | Loads the tickets again. Keeps the current search and status.                                  | R04      |
| button "Export CSV"                             | Demo only. Shows a message with how many tickets would be exported. No file is created.        | R04      |
| table (Subject, Requester, Status, Created)     | One row per ticket that passes the search and status filter.                                   | R01, R03 |
| button "Delete" (one per row)                   | Admin only. Removes the row from this page (demo only; it comes back after Refresh).           | R05      |
| text "No tickets found" + button "Clear search" | Shown when no ticket passes the filter. "Clear search" empties the search box.                 | R02      |

## Rules

- TICKETS-LIST-R01 WHEN the user types in "Search tickets" THE SYSTEM SHALL show only tickets whose subject or requester contains that text, in any letter case.
  - Example: "refund" → shows "Refund not received" and "Refund for double charge"; hides "Login issue".
  - Example: "REFUND" → same result as "refund".
  - Example: "mario" → shows "Refund for double charge" (requester Mario Reyes).
- TICKETS-LIST-R02 WHEN the search finds nothing THE SYSTEM SHALL show "No tickets found" and a "Clear search" button that empties the search and shows all tickets again.
  - Example: "zzz" → "No tickets found" + "Clear search".
- TICKETS-LIST-R03 WHEN both "Status" and search are set THE SYSTEM SHALL show only tickets that match both.
  - Example: "refund" + Open → only "Refund not received".
- TICKETS-LIST-R04 WHILE the list is loading THE SYSTEM SHALL disable "Export CSV".
  - Example: page opens → "Export CSV" is disabled until the tickets appear.
- TICKETS-LIST-R05 IF the user is an Agent THEN THE SYSTEM SHALL hide the "Delete" button.
  - Example: Agent → no "Delete" anywhere. Admin → "Delete" on every row.

## States

| State   | What the user sees                                                                 |
|---------|------------------------------------------------------------------------------------|
| Loading | "Loading tickets…" instead of the table. "Export CSV" is disabled (R04).           |
| Loaded  | The table with the tickets that pass the filter.                                   |
| Empty   | "No tickets found" and a "Clear search" button instead of the table (R02).         |
| Exported| A message under the toolbar: how many tickets would be exported. Cleared on reload.|

## Not in scope

- Sorting by column (planned as rule R06, a later exercise).
- Paging.
- A real CSV file download.
- Deleting from the data source, and a confirm dialog before Delete.
- Editing or creating tickets.
- Real login. The role dropdown is a demo stand-in.

## Open questions

- Should "Clear search" also reset "Status" to All? Today it only empties the search box.
- Should "Delete" ask for confirmation and delete the ticket for real?
- Should "Refresh" also be disabled while loading?
