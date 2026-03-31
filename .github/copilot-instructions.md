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

## Project Lifecycle
- Follow existing project lifecycle pattern for index pages: use `OnParametersSetAsync` rather than `OnInitializedAsync`.

## Agent / Tool Usage
- Never run tools or agents in parallel. Always run one tool or agent at a time, sequentially. Prefer fewer tokens over faster execution.
- Use Visual Studio MCP build tooling (run_build) instead of CLI dotnet build unless MCP build command is unavailable.

## Project Skills
@.github/skills/oqtane-module-development/SKILL.md  
@.github/skills/blazor-oqtane-js-interop/SKILL.md  
@.github/skills/accessibility-and-validation/SKILL.md  
@.github/skills/respect-working-tree/SKILL.md  
@.github/skills/kanban/skill.md  
@.github/skills/skiller/skill.md  
