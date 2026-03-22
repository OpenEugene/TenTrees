# Copilot Instructions

## General Guidelines
- See /agents.md for instructions on how to use agents with Copilot.
- Avoid switching to the profiler or other agents unless clearly necessary. Explain the impact of such switches to prevent session loss and user frustration.

## Error Handling
- Controllers must catch service/data-layer exceptions, log them consistently at the controller boundary, and return stable HTTP status codes. Ensure that logging is performed at the controller boundary to maintain a clear separation of concerns and facilitate debugging.
- Use concise controller error responses with `StatusCode` and `StatusCodes` constants instead of `Problem()`.

## Authorization
- Rely on Oqtane page/controller permissions and avoid additional authorization checks in service layer methods.

## Training Edit Screens
- In Training Edit screens, populate the village dropdown regardless of action (no action-based filtering).
