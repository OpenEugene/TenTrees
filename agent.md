# GitHub Copilot Agent Instructions — 10 Trees Oqtane Project

## Project Overview
This is the 10 Trees Digital Platform - a Blazor WebAssembly application built on the Oqtane framework (.NET 10) for tracking grower enrollments, garden mapping, tree monitoring, and program reporting in rural South Africa.

## Task & Issue Tracking

All tasks and issues are tracked on the **local Kanban board** at `./kanban/`. When the user refers to "tasks", "issues", or "what to work on next", always look there — never assume GitHub Issues or any external tracker.

## Communication Guidelines
- **DO NOT create summary documents** - Just do the work and report completion
- Provide concise status updates during work
- Report errors immediately with relevant context
- Keep responses focused on actions taken, not explanations

### Two-Phase Workflow: Planning → Implementation

**PHASE 1: PLANNING MODE (Default)**
- When asked to work on a feature/issue, START with planning
- Gather context, analyze existing code, identify files to modify
- Present a detailed plan including:
  - Architecture decisions and trade-offs
  - Files that will be modified
  - Implementation approach
  - Risks and alternatives
- **⏸️ STOP and WAIT for explicit approval** before implementing

**PHASE 2: IMPLEMENTATION MODE (Only after approval)**
- User signals: "go ahead", "implement it", "let's do it", "proceed", "make those changes"
- Make the actual code changes
- Run builds and tests
- Report completion status

**Key Phrases:**
- Planning triggers: "create a plan", "review approach", "what would we need"
- Implementation triggers: "go ahead", "implement", "do it", "proceed"

**NEVER** jump directly into implementation unless the user explicitly requests it. When the user says "get started with the first phase" or references a specific phase from a plan, implement ONLY that phase and stop. Wait for explicit approval before proceeding to the next phase. Do not implement multiple phases at once unless explicitly told to.

## Oqtane Framework Guidelines
- You are an Oqtane expert and will always follow Oqtane best practices
- Oqtane repo: https://github.com/oqtane/oqtane.framework

## BDD & Specifications
- Specs project: `/Specs`
- All features are defined in Gherkin syntax in `.feature` files

### Module Structure
- Oqtane uses a modular architecture with Client, Server, and Shared projects
- Each module follows the pattern: `Client/Modules/[ModuleName]`, `Server/Controllers`, `Server/Manager`, `Server/Repository`, `Shared/Models`
- Always use Oqtane's `ModuleBase` as the base class for Razor components
- Use `IModule` interface for module definition in `ModuleInfo.cs`

### Navigation Patterns
- **`NavigateUrl()`** — Navigate to the module's default view (Index) or pages within the site
- **`EditUrl(action)`** — Navigate between different actions within the same module (use this for multi-step workflows)
- **`NavigateUrl(action, parameters)`** — Navigate to module action with query parameters
- Never construct URLs manually; always use Oqtane's navigation helpers

### Action-Based Components
- Multi-step workflows use separate actions (e.g., `BasicInfo`, `Criteria`, `Commitments`, `Signature`)
- Each action is a separate Razor component
- Declare all actions in the component: `public override string Actions => "Action1,Action2,Action3";`
- Use state services to pass data between actions

### Security & Authorization
- Always check: `public override SecurityAccessLevel SecurityAccessLevel => SecurityAccessLevel.Edit;`
- Use `[Authorize(Policy = PolicyNames.ViewModule)]` on API endpoints
- Use `IsAuthorizedEntityId(EntityNames.Module, ModuleId)` to verify authorization

### Data Access Pattern
```
Repository → Service → Controller → Client Service → Razor Component
```
- Use `IDbContextFactory` for database access
- Always implement both `IService` interface and implementation
- Server services use `IUserPermissions` for authorization checks

### Localization
- All user-facing text must use ResourceKeys
- Razor: `@Localizer["Key"]` or `<Label ResourceKey="Key">`
- Create `.resx` files in `Client/Resources/[Namespace]/[ComponentName].resx`
- Support bilingual requirements: `en-ZA` (English) and `ts-ZA` (Xitsonga)
- Use single-language display (not side-by-side bilingual)

### Models & Entities
- Shared models inherit from `ModelBase` (provides audit fields)
- Use `[Table("ModelName")]` attribute — simple table names matching model names
- No prefixes or schema qualifiers needed
- **Database schema managed via SQL project only** (see Database Development section)

### Module Versioning
- **ALWAYS version modules in `ModuleInfo.cs`** (e.g., `Client/Modules/[ModuleName]/ModuleInfo.cs`)
- Update `Version` property: Use semantic versioning (e.g., `"1.3.0"`)
- Update `ReleaseVersions` property: Append new version to comma-separated list
- **DO NOT modify project `.csproj` Version properties** — those stay at `1.0.0`
- Version bump guidelines:
  - Patch (`1.2.1 → 1.2.2`): Bug fixes only
  - Minor (`1.2.1 → 1.3.0`): New features, backward compatible
  - Major (`1.2.1 → 2.0.0`): Breaking changes

### Client-Side Services
- Register all services in consolidated `TenTreesClientStartup.cs`: `services.AddScoped<IService, ServiceImpl>()`
- Use scoped lifetime for state services
- Do NOT create separate startup files per module

### Server-Side Services
- Register all services in consolidated `TenTreesServerStartup.cs`: `services.AddTransient<IService, ServerServiceImpl>()`
- Register shared `DbContextFactory` once for all modules
- Do NOT create separate startup files per module

## Database Development

### SQL Project as Source of Truth
- **Primary and ONLY database development**: Use the Visual Studio SQL Project at `Sql/Sql.sqlproj`
- All table definitions live in `Sql/dbo/Tables/*.sql`
- Use Visual Studio SQL tooling for schema design, refactoring, and comparison
- SQL project provides IntelliSense, validation, and database publishing
- **DO NOT create Entity Framework migrations** — we use SQL project exclusively

### Database Deployment
- Use SQL Server Data Tools (SSDT) to publish schema changes
- Use Visual Studio's "Publish" feature from the SQL project
- Schema comparison tools for comparing database to project
- Generate deployment scripts using SSDT
- For production deployments, generate and review scripts before applying

### Creating New Tables
1. Create table in SQL Project: `Sql/dbo/Tables/[ModelName].sql`
2. Use standard SQL Server `CREATE TABLE` syntax with simple table names (e.g., `[dbo].[Village]`)
3. Include audit columns: `CreatedBy`, `CreatedOn`, `ModifiedBy`, `ModifiedOn`
4. Use Visual Studio SQL project tools to validate syntax
5. Publish to development database using SQL project
6. EF Core will automatically discover tables via `DbContext`

### Example Table Definition
```sql
CREATE TABLE [dbo].[ModelName] (
    [ModelNameId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [CreatedBy]   NVARCHAR (256) NOT NULL,
    [CreatedOn]   DATETIME2 (7)  NOT NULL,
    [ModifiedBy]  NVARCHAR (256) NOT NULL,
    [ModifiedOn]  DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_ModelName] PRIMARY KEY CLUSTERED ([ModelNameId] ASC)
);
```

### Database Schema Changes
1. Modify SQL file in SQL project
2. Use Schema Compare to see differences
3. Generate script or publish directly to development database
4. For production, generate deployment script and review before applying
5. EF Core `DbContext` automatically maps to updated schema

## 10 Trees Specific Patterns

### User Roles & Permissions
- **Tree Mentor** — Submit enrollment/assessment forms; sees only their 10 assigned households
- **Educator** — View all villages, add home visit notes; cannot edit enrollment or assessment data. Role may be renamed — Rebecca to confirm with Tri (Director of Permaculture Education and Community Development)
- **Project Manager** — View all villages, add notes, export data; no form submission
- **10 Trees Admin** — Full access: data entry, user management, village/cohort assignment, export. Previously called "center admin" — use "10 Trees Admin" exclusively
- **No Executive Director role** — The organisation uses a distributed leadership model

### Cohort Management
- Cohorts group households by village and intake year (e.g., `"Roebuck 1 2026"`)
- System auto-generates a cohort name from village + year; admin can overwrite
- A village can have multiple numbered cohorts in the same year
- All data queries, reports, and filters support cohort scoping

### Home Visit Tracking
- Home visits are recorded separately from garden assessments
- Multiple staff can log visits and notes against the same grower
- Visit records must include: staff name, date, free-text notes
- Monthly visit counts per staff member are required for funder reporting

### Class Attendance
- Growers must complete 5 permaculture classes to be eligible for trees
- Standard sequence: Tree Selection → Tree Planting → Water Management → Soil Fertility → General Permaculture
- Class sequence can vary by cohort (e.g., Roebuck 2026 front-loads Tree Selection and Planting before winter)
- Track attendance by named class, not just a count

### Mobile-First Design
- Target small smartphones (Nokia, Samsung, off-brand)
- Minimize text entry; maximize checkboxes and Yes/No questions
- Use Bootstrap cards for section organization
- One section per page in multi-step workflows
- For larger screens, show sections side-by-side when possible
- Include progress bars: `<div class="progress-bar" style="width: 25%">25%</div>`
- **NEVER use admin containers** — render components directly for mobile users
- Avoid desktop-only UI patterns (hover effects, right-click menus, etc.)
- Touch-friendly buttons and form controls (minimum 44×44px touch targets)
- Large, easy-to-read fonts (minimum 16px for body text)

### BDD-Driven Development
- All features defined in `Specs/Features/*.feature` files
- Use Reqnroll (SpecFlow successor) with xUnit
- Feature files use Gherkin syntax with tags: `@workflow-*`, `@priority-*`, `@mobile`
- Implement features to match BDD scenarios exactly

### Offline Support
- Forms must work offline (poor rural connectivity)
- Cache language resources for offline use
- Store drafts locally before sync
- Use state services to preserve data across page refreshes

### Data Collection Workflow
1. **Enrollment** — BasicInfo → Criteria → Commitments → Signature
2. **Mapping** — GPS location, water resources, existing trees
3. **Assessment** — Tree survival, health, permaculture practices (recurring)
4. **Attendance** — Permaculture training class tracking
5. **Reporting** — Export data, generate reports

### Village Multi-Tenancy
- All data scoped by `VillageId`
- Tree Mentors see only their assigned households (up to 10)
- Educators and above can view/filter all villages
- Use village filter in all queries

## Code Style

### Naming Conventions
- Private fields: `_camelCase`
- Properties: `PascalCase`
- Methods: `PascalCase`
- Resource keys: `Dot.Separated.PascalCase` (e.g., `"Section.BasicInfo"`, `"Message.SaveError"`)

### Component Structure Order
1. `@using` directives
2. `@namespace`, `@inherits`, `@inject`
3. HTML markup
4. `@code` block with:
   - Override properties (`SecurityAccessLevel`, `Actions`, `Title`)
   - Private fields
   - Lifecycle methods (`OnInitialized`, `OnParametersSet`)
   - Event handlers
   - Helper methods

### Error Handling
- Always wrap async operations in try-catch
- Log errors: `await logger.LogError(ex, "Message {Param}", param)`
- Show user-friendly messages: `AddModuleMessage(Localizer["Message.Error"], MessageType.Error)`
- Log information on success: `await logger.LogInformation("Success {Entity}", entity)`

### Validation
- Use Bootstrap validation: `class="@(validated ? "was-validated" : "needs-validation")"`
- Mark required fields with asterisk: `Grower Name: *`
- Use `required` attribute on inputs
- Validate before saving: `await interop.FormValid(form)`

## Common Patterns

### State Management
```csharp
public interface IStateService
{
    DraftModel CurrentDraft { get; set; }
    void InitializeDraft(int moduleId);
    void ClearDraft();
}
```

### Multi-Step Navigation
```csharp
private async Task NextStep()
{
    validated = true;
    var interop = new Oqtane.UI.Interop(JSRuntime);
    if (await interop.FormValid(form))
    {
        StateService.CurrentDraft.Field = _field;
        NavigationManager.NavigateTo(EditUrl("NextAction"));
    }
}
```

### Audit Fields
```csharp
// Always available from ModelBase
string CreatedBy { get; set; }
DateTime CreatedOn { get; set; }
string ModifiedBy { get; set; }
DateTime ModifiedOn { get; set; }
```

## Testing
- BDD scenarios drive development
- Step definitions not yet implemented (Reqnroll framework configured)
- Manual testing by bilingual testers: Trygive and Quentan

## Build & Deploy
- .NET 10 target framework
- C# 14 language version
- Blazor WebAssembly (client-side)
- SQL Server database (via Entity Framework Core)
- Default ports: `http://localhost:5000`, `https://localhost:5001`

## Important Notes
- Never use dynamic port binding (`:0`) — causes Blazor WebAssembly errors
- Always use `EditUrl()` for action-based navigation within modules
- Localization is required for all user-facing text
- Mobile-first design is critical (small screens, touch input)
- Offline support is essential (poor connectivity)
- Forms should be as short as possible (minimize scrolling on small screens)
- Use checkboxes instead of text input whenever possible

## Git & Version Control

### Commit Messages
Keep them simple.

### PowerShell Git Commands

**✅ RECOMMENDED — Multiple `-m` flags:**
```powershell
git commit -m "Title of commit" `
          -m "Body paragraph explaining what changed" `
          -m "Closes #123"
```

**✅ ALTERNATIVE — PowerShell here-string:**
```powershell
$message = @"
Title of commit

Body paragraph explaining what changed.

Closes #123
"@
git commit -m $message
```

**❌ AVOID — Does not work in PowerShell:**
```powershell
git commit -m "Line 1
Line 2"
```

### Copilot Git Workflow
1. ✅ Prepare commit messages — draft proper conventional commit messages
2. ✅ Show what will be committed — display `git status` and proposed changes
3. ❌ DO NOT commit automatically — always wait for user's explicit "commit" command
4. ❌ DO NOT push automatically — user must explicitly request "push"

### Branch Naming
- Feature branches: `feature/5-village-data-management`
- Bug fixes: `fix/123-bug-description`
- Documentation: `docs/update-readme`
- Format: `<type>/<issue-number>-<short-description>`
