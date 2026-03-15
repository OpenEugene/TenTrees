# Cohort Management — Implementation Plan

Feature spec: `Specs/Features/CohortManagement.feature`
Backlog task: `kanban/backlog/implement-cohort-management-module.md`

---

## Overview

Cohorts are named groups of growers (and mentors) within a village, with a
`Planned → Active → Completed` lifecycle. Both growers and mentors can belong
to multiple cohorts (many-to-many join tables). Assessment frequency is derived
from the cohort's activation year, not the grower's oldest membership.

The module follows the same layered pattern as Village: SQL → Model → Context →
Repository → ServerService → Controller → ClientService → ClientStartup → UI.

---

## Step 1 — SQL Tables

Create three new table files in `Sql/dbo/Tables/`.

### `Cohort.sql`
```sql
CREATE TABLE [dbo].[Cohort] (
    [CohortId]   INT            IDENTITY (1, 1) NOT NULL,
    [VillageId]  INT            NOT NULL,
    [Name]       NVARCHAR (200) NOT NULL,
    [Status]     INT            NOT NULL DEFAULT 0,  -- 0=Planned, 1=Active, 2=Completed
    [ActivatedOn] DATETIME2 (7) NULL,
    [CreatedBy]  NVARCHAR (256) NOT NULL,
    [CreatedOn]  DATETIME2 (7)  NOT NULL,
    [ModifiedBy] NVARCHAR (256) NOT NULL,
    [ModifiedOn] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Cohort] PRIMARY KEY CLUSTERED ([CohortId] ASC),
    CONSTRAINT [FK_Cohort_Village] FOREIGN KEY ([VillageId]) REFERENCES [dbo].[Village] ([VillageId])
);
```

### `GrowerCohort.sql`
```sql
CREATE TABLE [dbo].[GrowerCohort] (
    [GrowerCohortId] INT           IDENTITY (1, 1) NOT NULL,
    [GrowerId]       INT           NOT NULL,
    [CohortId]       INT           NOT NULL,
    [JoinedOn]       DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_GrowerCohort] PRIMARY KEY CLUSTERED ([GrowerCohortId] ASC),
    CONSTRAINT [FK_GrowerCohort_Grower] FOREIGN KEY ([GrowerId]) REFERENCES [dbo].[Grower] ([GrowerId]),
    CONSTRAINT [FK_GrowerCohort_Cohort] FOREIGN KEY ([CohortId]) REFERENCES [dbo].[Cohort] ([CohortId]),
    CONSTRAINT [UQ_GrowerCohort] UNIQUE ([GrowerId], [CohortId])
);
```

### `MentorCohort.sql`
```sql
CREATE TABLE [dbo].[MentorCohort] (
    [MentorCohortId] INT            IDENTITY (1, 1) NOT NULL,
    [MentorId]       NVARCHAR (256) NOT NULL,   -- Oqtane UserId (string)
    [CohortId]       INT            NOT NULL,
    [AssignedOn]     DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_MentorCohort] PRIMARY KEY CLUSTERED ([MentorCohortId] ASC),
    CONSTRAINT [FK_MentorCohort_Cohort] FOREIGN KEY ([CohortId]) REFERENCES [dbo].[Cohort] ([CohortId]),
    CONSTRAINT [UQ_MentorCohort] UNIQUE ([MentorId], [CohortId])
);
```

---

## Step 2 — Shared Models

Add to `Shared/Models/`:

### `Cohort.cs`
- `CohortId`, `VillageId`, `Name`, `Status` (enum), `ActivatedOn`
- `CohortStatus` enum: `Planned = 0`, `Active = 1`, `Completed = 2`
- Inherits `ModelBase` (for audit fields)

### `GrowerCohort.cs`
- `GrowerCohortId`, `GrowerId`, `CohortId`, `JoinedOn`
- No `ModelBase` — no audit fields needed on the join

### `MentorCohort.cs`
- `MentorCohortId`, `MentorId` (string), `CohortId`, `AssignedOn`

---

## Step 3 — EF Context

In `Server/Repository/TenTreesContext.cs`, add:

```csharp
public virtual DbSet<Models.Cohort> Cohort { get; set; }
public virtual DbSet<Models.GrowerCohort> GrowerCohort { get; set; }
public virtual DbSet<Models.MentorCohort> MentorCohort { get; set; }
```

And in `OnModelCreating`:
```csharp
modelBuilder.Entity<Models.Cohort>().ToTable(ActiveDatabase.RewriteName("Cohort"));
modelBuilder.Entity<Models.GrowerCohort>().ToTable(ActiveDatabase.RewriteName("GrowerCohort"));
modelBuilder.Entity<Models.MentorCohort>().ToTable(ActiveDatabase.RewriteName("MentorCohort"));
```

---

## Step 4 — Server Repository

Create `Server/Repository/CohortRepository.cs` with interface and implementation.

Interface methods:
```
GetCohorts()                          → IEnumerable<Cohort>
GetCohortsByVillage(villageId)        → IEnumerable<Cohort>
GetCohort(cohortId)                   → Cohort
AddCohort(cohort)                     → Cohort
UpdateCohort(cohort)                  → Cohort
DeleteCohort(cohortId)                → void

GetGrowerCohorts(cohortId)            → IEnumerable<GrowerCohort>
GetCohortsByGrower(growerId)          → IEnumerable<Cohort>
AddGrowerCohort(growerId, cohortId)   → GrowerCohort
DeleteGrowerCohort(growerId, cohortId)→ void

GetCohortsByMentor(mentorId)          → IEnumerable<Cohort>
AddMentorCohort(mentorId, cohortId)   → MentorCohort
DeleteMentorCohort(mentorId, cohortId)→ void
```

Auto-suggest logic (name generation) lives in the service layer, not the repository.

---

## Step 5 — Server Service

Create `Server/Services/CohortService.cs`.

Key logic beyond simple delegation:

- **`SuggestCohortName(villageId, year)`** — counts existing cohorts for that
  village+year; returns `"{VillageName} {year}"` if zero, `"{VillageName} 2 {year}"`
  if one already exists, etc.
- **`SetStatus(cohortId, newStatus)`** — sets `ActivatedOn` when transitioning to
  Active; validates the transition (no going backward from Completed).

---

## Step 6 — Server Startup

Create `Server/Startup/CohortServerStartup.cs`:

```csharp
services.AddTransient<ICohortService, ServerCohortService>();
services.AddTransient<ICohortRepository, CohortRepository>();
```

---

## Step 7 — Server Controller

Create `Server/Controllers/CohortController.cs`, inheriting `ModuleControllerBase`.

Endpoints:

| Method | Route | Auth | Purpose |
|---|---|---|---|
| GET | `/api/cohort` | Edit | All cohorts |
| GET | `/api/cohort/{id}` | View | Single cohort |
| GET | `/api/cohort/village/{villageId}` | View | Cohorts by village |
| GET | `/api/cohort/suggest?villageId=&year=` | Edit | Name suggestion |
| POST | `/api/cohort` | Edit | Create |
| PUT | `/api/cohort/{id}` | Edit | Update / status change |
| DELETE | `/api/cohort/{id}` | Edit | Delete |
| GET | `/api/cohort/{id}/growers` | View | Grower memberships |
| GET | `/api/cohort/grower/{growerId}` | View | Cohorts for a grower |
| POST | `/api/cohort/{id}/growers/{growerId}` | Edit | Add grower to cohort |
| DELETE | `/api/cohort/{id}/growers/{growerId}` | Edit | Remove grower |
| GET | `/api/cohort/mentor/{mentorId}` | View | Cohorts for a mentor |
| POST | `/api/cohort/{id}/mentors/{mentorId}` | Edit | Assign mentor |
| DELETE | `/api/cohort/{id}/mentors/{mentorId}` | Edit | Remove mentor |

---

## Step 8 — Client Service

Create `Client/Services/CohortService.cs` with `ICohortService` and `CohortService : ServiceBase`.

Mirror the controller endpoints. Key additions over basic CRUD:
- `GetSuggestedNameAsync(villageId, year)` → string
- `GetCohortsByGrowerAsync(growerId)` → List<Cohort>
- `GetCohortsByMentorAsync(mentorId)` → List<Cohort>
- `AddGrowerToCohortAsync(cohortId, growerId)` / `RemoveGrowerFromCohortAsync`
- `AssignMentorToCohortAsync(cohortId, mentorId)` / `RemoveMentorFromCohortAsync`

---

## Step 9 — Client Startup

Create `Client/Startup/CohortClientStartup.cs`:

```csharp
if (!services.Any(s => s.ServiceType == typeof(ICohortService)))
    services.AddScoped<ICohortService, CohortService>();
```

---

## Step 10 — Admin UI

Create `Client/Modules/Cohort/`.

### `ModuleInfo.cs`
Standard module registration.

### `Index.razor` — Cohort List
- Status filter tabs: All | Planned | Active | Completed (default hides Completed)
- Table: Name, Village, Status, Member Count, Activated On, actions
- Add Cohort button (admin only)
- Status badge with colour (Planned=grey, Active=green, Completed=muted)

### `Edit.razor` — Create / Edit Cohort
- Village dropdown (populated from VillageService)
- Name field, pre-filled via `SuggestCohortName` on village/year selection,
  overridable
- Status selector with transition guard (can't go from Completed → Active)
- **Grower Membership** panel:
  - List of current member growers with Remove button
  - Grower search/picker to add new members
- **Mentor Assignment** panel:
  - List of assigned mentors with Remove button
  - Mentor picker to assign

---

## Step 11 — Integration Points

These touch existing modules and should be done after the core module is stable.

### 11a. Enrollment form — cohort picker
In `Client/Modules/Enrollment/`, when a grower is enrolled into a village,
add a multi-select cohort picker showing Active cohorts for that village.
On save, call `AddGrowerToCohortAsync` for each selected cohort.

### 11b. Grower list — cohort filter
In `Client/Modules/Grower/`, add a cohort filter dropdown.
- Admin: all cohorts for the selected village
- Mentor: union of cohorts they are assigned to (fetched via
  `GetCohortsByMentorAsync(currentUserId)`)

### 11c. Assessment frequency
In the assessment scheduling logic, look up the grower's cohort memberships
and derive frequency per cohort:
- Cohort activated in current year → twice monthly
- Cohort activated in prior year(s) → monthly

If a grower is in multiple cohorts, use the most recent Active cohort's
frequency for scheduling purposes (or display per-cohort if needed).

### 11d. Reporting
In `Client/Modules/` reporting views, add a cohort scope dropdown alongside
the existing village filter. Pass `cohortId` as an optional query parameter
to report endpoints.

---

## Recommended Build Order

1. SQL tables (Step 1)
2. Models + Context (Steps 2–3)
3. Repository (Step 4)
4. Server service + startup (Steps 5–6)
5. Controller (Step 7)
6. Client service + startup (Steps 8–9)
7. Admin UI — Index and Edit (Step 10)
8. Integration — enrollment picker (Step 11a)
9. Integration — grower list filter (Step 11b)
10. Integration — assessment frequency (Step 11c)
11. Integration — reporting scope (Step 11d)

Each step can be committed separately. Steps 1–7 (backend) can be tested via
the Swagger/API before any UI exists.
