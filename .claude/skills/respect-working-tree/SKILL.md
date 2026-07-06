---
name: respect-working-tree
description: |
  Prevents Copilot from reverting or re-applying changes the user has already made manually
  to their working tree. Use when Copilot begins a response by verifying or "re-applying"
  edits that the user intentionally made themselves — the agent should trust the working tree
  as the source of truth, not its own session history. Also covers the pattern of always
  reading a file before editing to avoid fighting over state.
author: Skiller
version: 1.0.0
date: 2026-02-28
---

# Respect the Working Tree

## Problem

Copilot keeps a mental model of what files look like from its own edits during a session.
When the user manually edits a file outside of Copilot's view (e.g., in the editor, via
git operations, or by reverting a change), Copilot will try to "re-apply" its version of
the change — effectively overwriting what the user intentionally did.

This creates a tug-of-war over the working tree and is frustrating for developers who want
to collaborate on the code, not just accept AI output wholesale.

## Context / Trigger Conditions

- User says "STOP", "don't change that", "I made that change myself", or "leave it"
- User manually edits a file that Copilot previously modified
- User asks "what's next?" and Copilot starts re-verifying previous changes instead of moving forward
- File content in the IDE context doesn't match what Copilot wrote in the last session
  (this is normal — sessions are stateless; the file on disk is always correct)
- Copilot notices a file looks "old" compared to its session memory

## Solution

1. **The working tree is always the source of truth.** If a file doesn't match what
   Copilot wrote last session, assume the user changed it intentionally.

2. **Never re-apply a change without being asked.** If Copilot thinks a change is
   missing, it should ask: *"I notice X doesn't have Y — did you intentionally revert
   that?"* — not silently re-apply it.

3. **Always read before editing.** Use `get_file` to read the current file state before
   making any change. Never assume the file matches a prior session's output.

4. **Move forward, not backward.** When a user asks "what's next?", answer that question.
   Don't use it as a trigger to verify or re-do prior work.

5. **Session memory ≠ file state.** Copilot's memory of "what I wrote" is irrelevant once
   the session ends. The file on disk is what matters.

## Verification

Ask yourself before any file edit:
- Did I just read this file with `get_file`?
- Is the user asking me to change this, or am I doing it because I *think* it should match my prior work?
- If the user's version differs from mine, did I ask them about it?

## Example

**Bad pattern:**
> User: "What's next?"
> Copilot: "Let me verify the #2566 changes are actually in the file before moving on..."
> *[Re-reads file, sees user's version, re-applies its own version anyway]*

**Good pattern:**
> User: "What's next?"
> Copilot: "Next up is #2564 — Surveyor Name autocomplete. Here's what it needs..."

**Bad pattern:**
> User: "STOP"
> Copilot: *[Continues applying changes anyway on the next turn]*

**Good pattern:**
> User: "STOP"
> Copilot: "Stopped. Waiting for your instructions."

## Notes

- This is especially important in long sessions where multiple back-and-forth edits have
  occurred. The user's last manual edit is intentional, not an error to correct.
- The user may revert Copilot changes deliberately (to test, to simplify, or because
  they preferred a different approach) — that is their right.
- Git commits are also the user's domain. Never stage or commit without explicit instruction.
  If changes are uncommitted, that is also intentional — do not push or commit them.

## References

- Copilot instructions: `.github/copilot-instructions.md`

## Activation History
- 2026-02-28: Session where Copilot re-applied PwDatePicker changes the user had already made manually, and re-verified #2567 changes the user had committed themselves.
