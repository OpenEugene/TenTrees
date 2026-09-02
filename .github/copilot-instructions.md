# Copilot Instructions

## General Guidelines
- Avoid switching to the profiler or other agents unless clearly necessary. Explain the impact of such switches to prevent session loss and user frustration.

## Error Handling
- Controllers must catch service/data-layer exceptions, log them consistently at the controller boundary, and return stable HTTP status codes. Ensure that logging is performed at the controller boundary to maintain a clear separation of concerns and facilitate debugging.
- Use concise controller error responses with `StatusCode` and `StatusCodes` constants instead of `Problem()`.

## Authorization
- Rely on Oqtane page/controller permissions and avoid additional authorization checks in service layer methods. Authorization is handled automatically by Oqtane at the page/controller level.

## Training Edit Screens
- In Training Edit screens, populate the village dropdown regardless of action (no action-based filtering).

## UI / Icons
- Use Open Iconic icons (oi oi-*) everywhere in the 10Trees workspace.
- Do not use Bootstrap Icons (bi bi-*) or Font Awesome.

## Project Lifecycle
- Follow existing project lifecycle pattern for index pages: use `OnParametersSetAsync` rather than `OnInitializedAsync`.

## Data Layer / Database Relations
- Do not enforce relations via SQL foreign key constraints or EF relationship configuration.
- Handle relations purely in the repository layer using EF queries and LINQ.
- Do not add FK constraints to SQL DDL files.
- Do not configure navigation properties or relationships in `OnModelCreating`.
- Ensure repository code explicitly maintains referential integrity and any required cascade semantics.

## Agent / Tool Usage
- Always use Visual Studio MCP tools (for example: `get_file`, `find_symbol`, `code_search`, `get_errors`, `run_build`, `replace_string_in_file`, `multi_replace_string_in_file`, etc.) instead of `run_command_in_terminal` or other CLI commands when working in this workspace. Never fall back to CLI commands when an MCP tool can accomplish the task.
- Use the terminal only as a last resort when no MCP tool can accomplish the task.
- Never run tools or agents in parallel. Always run one tool or agent at a time, sequentially.
- Prefer fewer tokens over faster execution.
- Always use Visual Studio MCP tools instead of CLI `dotnet build` or terminal `Select-String` when working in this workspace.

## Project Skills
@.github/skills/oqtane-module-development/SKILL.md  
@.github/skills/blazor-oqtane-js-interop/SKILL.md  
@.github/skills/accessibility-and-validation/SKILL.md  
@.github/skills/respect-working-tree/SKILL.md  
@.github/skills/skiller/skill.md
