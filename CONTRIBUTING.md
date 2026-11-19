# Contributing to 10 Trees

Thanks for helping build the 10 Trees platform. This guide covers how work is tracked in GitHub issues and how changes reach `main` through pull requests.

For local setup (the Oqtane host, the dev database, and running the site), see [Getting Started](README.md#getting-started) in the README.

## Before You Start

- Install the [GitHub CLI](https://cli.github.com/) and sign in with `gh auth login`. The commands below use it, but everything can also be done on github.com.
- Read the relevant page in `docs/Specifications/`. The wiki is the specification for what the application is meant to do.
- Skim `CLAUDE.md` and `.github/skills/` for the project's Oqtane, JS interop, and accessibility patterns.

## Issues

Every piece of work starts with a GitHub issue. Customer requests, bugs, and ideas all get written up as issues so nothing lives only in a chat thread.

### Finding Work

```bash
# Open issues
gh issue list

# Filter by label
gh issue list --label bug
gh issue list --label "good first issue"

# Read one in full, with comments
gh issue view 191 --comments
```

### Claiming an Issue

Assign yourself before you start so two people don't pick up the same thing:

```bash
gh issue edit 191 --add-assignee @me
```

If an issue is unclear, ask in a comment on the issue instead of guessing:

```bash
gh issue comment 191 --body "Question: should the mentor list come from the cohort or the village?"
```

### Creating an Issue

Search first to avoid duplicates:

```bash
gh issue list --state all --search "mentor filter"
```

Then create it with a clear title, a body, and labels:

```bash
gh issue create --title "Mentor filter: add to Enrollment page" --label enhancement --label ux --body-file issue.md
```

A good issue includes:

- **Request or problem:** what was asked for or what is wrong, and who raised it. For customer requests, include the date and a summary of what they said.
- **What we found:** relevant behaviour, code locations, or data, if you've looked.
- **Done when:** the outcome that closes the issue.
- **Related:** links to other issues (`#186`).

### Labels

| Label | Use for |
|---|---|
| `bug` | Something isn't working as the specification describes |
| `enhancement` / `feature` | Improvements and new capabilities |
| `ux`, `ui`, `mobile` | Usability, layout, and phone/tablet behaviour |
| `localization` | English / Xitsonga text and language handling |
| `reporting` | Reports and data export |
| `support` | Questions and help requests from program staff |
| `documentation` | Wiki and README changes |
| `good first issue` | Small, well-defined work for new contributors |
| `priority-high` / `priority-medium` | Work to pick up first |

See all labels with `gh label list`.

## Branches

`main` is protected: nobody, including admins, can push to it directly. All changes go through a pull request.

Create a branch from the latest `main`, named after the issue:

```bash
git switch main
git pull
git switch -c feature/191-mentor-filter
```

- `feature/<issue>-<short-slug>` for features and enhancements
- `fix/<issue>-<short-slug>` for bug fixes

Keep each branch focused on one issue.

## Commits

Use short, conventional-style messages that say what changed and why:

```
fix: keep "All" selected in the assessment mentor filter
feat: add mentor filter to the Grower index
docs: describe mentor filtering on the Grower spec page
```

Common prefixes: `feat`, `fix`, `docs`, `refactor`, `chore`, `revert`.

## Pull Requests

### Before Opening

- The solution builds and `dotnet test Tests` passes.
- You've tried the change in the running site, including on a phone-sized screen for anything mentors use in the field.
- New or changed text is localized (en-ZA and ts-ZA).
- The relevant `docs/Specifications/` page is updated if the change alters what the application does.
- If you changed a module, its `Version` in `ModuleInfo.cs` is bumped.

### Opening

Push your branch and open the PR against `main`:

```bash
git push -u origin feature/191-mentor-filter
gh pr create --base main --title "Mentor filter for Orpen Gate, Enrollment and Grower" --body-file pr.md
```

In the PR description:

- Summarise what changed and why.
- Include `Closes #191` so the issue closes automatically when the PR merges.
- Describe how you tested it, and add screenshots for UI changes.
- Call out anything reviewers should look at closely, or anything left for a follow-up issue.

Open the PR as a draft (`gh pr create --draft`) if you want early feedback before it's finished.

### Review and Merge

- No approval is required to merge, but ask for a review on anything non-trivial: `gh pr edit --add-reviewer <username>`.
- Respond to review comments with a follow-up commit or a reply on the comment.
- Merge once you're confident it's ready:

  ```bash
  gh pr merge --merge --delete-branch
  ```

- Check that the linked issue closed. If the PR only partly addressed it, reopen the issue or open a follow-up.

### Useful Commands

```bash
gh pr status               # your PRs and their checks
gh pr checkout 183         # check out someone else's PR locally
gh pr view --web           # open the current branch's PR in the browser
```
