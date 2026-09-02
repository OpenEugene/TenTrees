See #Claude Instructions for project context, coding conventions, and workflow guidelines.

# Claude Instructions

## General Guidelines
- Avoid switching to the profiler or other agents unless clearly necessary. Explain the impact of such switches to prevent session loss and user frustration.

## Error Handling
- Controllers must catch service/data-layer exceptions, log them consistently at the controller boundary, and return stable HTTP status codes.
- Use concise controller error responses with `StatusCode` and `StatusCodes` constants instead of `Problem()`.

## Authorization
- Rely on Oqtane page/controller permissions and avoid additional authorization checks in service layer methods.

## Training Edit Screens
- In Training Edit screens, populate the village dropdown regardless of action (no action-based filtering).

## Claude Code Behaviour
- Never run tools or agents in parallel. Always run one tool or agent at a time, sequentially. Prefer fewer tokens over faster execution.

## Project Skills
@.github/skills/oqtane-module-development/SKILL.md
@.github/skills/blazor-oqtane-js-interop/SKILL.md
@.github/skills/accessibility-and-validation/SKILL.md
@.github/skills/respect-working-tree/SKILL.md
@.github/skills/skiller/skill.md

## Documentation lives in `/docs` (Wikidown wiki)

- The `/docs` folder is a Wikidown wiki — structured markdown with `.order`
  navigation files. Page links use title form: `/Getting-Started/Format`.
- A `wikidown-editor` subagent and a `wikidown` skill are configured for this
  repo. Use them for ANY read/write of `/docs/*.md`.
- Never edit `/docs/*.md` directly with `Write`/`Edit`. Use the `wiki_*` MCP
  tools so `.order` files stay consistent.
- When you ship a feature that changes user-visible behavior, ask whether the
  wiki should be updated, and (if yes) delegate to `wikidown-editor`.
