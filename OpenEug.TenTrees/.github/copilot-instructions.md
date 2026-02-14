# GitHub Copilot Instructions for 10 Trees Oqtane Project

## Project Overview
This is the 10 Trees Digital Platform - a Blazor WebAssembly application built on the Oqtane framework (.NET 10) for tracking beneficiary enrollments, garden mapping, tree monitoring, and program reporting in rural South Africa.

## Oqtane Framework Guidelines
- you are an oqten expert and will always follow oqtane best practices
- the oqtane repo is here: https://github.com/oqtane/oqtane.framework

## BDD & Specifications
- the specs project is here: /Specs
- all features are defined in Gherkin syntax in .feature files

### Module Structure
- Oqtane uses a modular architecture with Client, Server, and Shared projects
- Each module follows the pattern: Client/Modules/[ModuleName], Server/Controllers, Server/Manager, Server/Repository, Shared/Models
- Always use Oqtane's `ModuleBase` as the base class for Razor components
- Use `IModule` interface for module definition in ModuleInfo.cs

### Navigation Patterns
- **NavigateUrl()** - Navigate to the module's default view (Index) or pages within the site
- **EditUrl(action)** - Navigate between different actions within the same module (use this for multi-step workflows)
- **NavigateUrl(action, parameters)** - Navigate to module action with query parameters
- Never construct URLs manually; always use Oqtane's navigation helpers

### Action-Based Components
- Multi-step workflows should use separate actions (e.g., BasicInfo, Criteria, Commitments, Signature)
- Each action is a separate Razor component
- Declare all actions in the component using: `public override string Actions => "Action1,Action2,Action3";`
- Use state services to pass data between actions

### Security & Authorization
- Always check: `public override SecurityAccessLevel SecurityAccessLevel => SecurityAccessLevel.Edit;`
- Use `[Authorize(Policy = PolicyNames.ViewModule)]` on API endpoints
- Use `IsAuthorizedEntityId(EntityNames.Module, ModuleId)` to verify authorization

### Data Access Pattern
- Repository ? Service ? Controller ? Client Service ? Razor Component
- Use IDbContextFactory for database access
- Always implement both IService interface and implementation
- Server services use `IUserPermissions` for authorization checks

### Localization
- All user-facing text must use ResourceKeys
- Razor: `@Localizer["Key"]` or `<Label ResourceKey="Key">`
- Create .resx files in Client/Resources/[Namespace]/[ComponentName].resx
- Support bilingual requirements: en-ZA (English) and ts-ZA (Xitsonga)
- Use single-language display (not side-by-side bilingual)

### Models & Entities
- Shared models inherit from `ModelBase` (provides audit fields)
- Use `[Table("ModelName")]` attribute - simple table names matching model names
- No prefixes or schema qualifiers needed
- **Database schema managed via SQL project only** (see Database Development section below)

### Client-Side Services
- Register all services in consolidated `TenTreesClientStartup.cs`: `services.AddScoped<IService, ServiceImpl>()`
- Use scoped lifetime for state services
- Do NOT create separate startup files per module

### Server-Side Services
- Register all services in consolidated `TenTreesServerStartup.cs`: `services.AddTransient<IService, ServerServiceImpl>()`
- Register shared DbContextFactory once for all modules
- Do NOT create separate startup files per module

## Database Development

### SQL Project as Source of Truth
- **Primary and ONLY database development**: Use the Visual Studio SQL Project at `Sql/Sql.sqlproj`
- All table definitions live in `Sql/dbo/Tables/*.sql`
- Use Visual Studio SQL tooling for schema design, refactoring, and comparison
- SQL project provides IntelliSense, validation, and database publishing
- **DO NOT create Entity Framework migrations** - we use SQL project exclusively

### Database Deployment
- Use SQL Server Data Tools (SSDT) to publish schema changes
- Use Visual Studio's "Publish" feature from the SQL project
- Schema comparison tools for comparing database to project
- Generate deployment scripts using SSDT
- For production deployments, generate and review scripts before applying

### Creating New Tables
1. Create table in SQL Project: `Sql/dbo/Tables/[ModelName].sql`
2. Use standard SQL Server CREATE TABLE syntax with simple table names (e.g., `[dbo].[Village]`)
3. Include audit columns: `CreatedBy NVARCHAR(256), CreatedOn DATETIME2(7), ModifiedBy NVARCHAR(256), ModifiedOn DATETIME2(7)`
4. Use Visual Studio SQL project tools to validate syntax
5. Publish to development database using SQL project
6. EF Core will automatically discover tables via DbContext

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
5. EF Core DbContext automatically maps to updated schema

### Initial Database Setup
1. Create empty database in SQL Server
2. Right-click SQL project ? Publish
3. Select target database
4. Review changes and publish
5. Run application - EF Core will connect to existing schema

## 10 Trees Specific Patterns

### Mobile-First Design
- Target small smartphones (Nokia, Samsung, off-brand)
- Minimize text entry; maximize checkboxes and Yes/No questions
- Use Bootstrap cards for section organization
- One section per page in multi-step workflows
- for larger screens, show sections side-by-side when possible
- Include progress bars: `<div class="progress-bar" style="width: 25%">25%</div>`
- **NEVER use admin containers** - render components directly for mobile users
- Avoid desktop-only UI patterns (hover effects, right-click menus, etc.)
- Touch-friendly buttons and form controls (minimum 44x44px touch targets)
- Large, easy-to-read fonts (minimum 16px for body text)

### BDD-Driven Development
- All features defined in Specs/Features/*.feature files
- Use Reqnroll (SpecFlow successor) with xUnit
- Feature files use Gherkin syntax with tags: @workflow-*, @priority-*, @mobile
- Implement features to match BDD scenarios exactly

### Offline Support
- Forms must work offline (poor rural connectivity)
- Cache language resources for offline use
- Store drafts locally before sync
- Use state services to preserve data across page refreshes

### Data Collection Workflow
1. **Enrollment** - BasicInfo ? Criteria ? Commitments ? Signature
2. **Mapping** - GPS location, water resources, existing trees
3. **Assessment** - Tree survival, health, permaculture practices (recurring)
4. **Attendance** - Permaculture training class tracking
5. **Reporting** - Export data, generate reports

### Village Multi-Tenancy
- All data scoped by VillageId
- Mentors see only their village data
- Admins can view/filter all villages
- Use village filter in all queries

### User Roles & Permissions
- **Mentor** - Submit forms, view assigned village only
- **Educator** - Submit forms, view all villages, export data
- **Project Manager** - Same as Educator
- **Admin** - Full access including user management

## Code Style

### Naming Conventions
- Private fields: `_camelCase`
- Properties: `PascalCase`
- Methods: `PascalCase`
- Resource keys: `Dot.Separated.PascalCase` (e.g., "Section.BasicInfo", "Message.SaveError")

### Component Structure Order
1. @using directives
2. @namespace, @inherits, @inject
3. HTML markup
4. @code block with:
   - Override properties (SecurityAccessLevel, Actions, Title)
   - Private fields
   - Lifecycle methods (OnInitialized, OnParametersSet)
   - Event handlers
   - Helper methods

### Error Handling
- Always wrap async operations in try-catch
- Log errors: `await logger.LogError(ex, "Message {Param}", param)`
- Show user-friendly messages: `AddModuleMessage(Localizer["Message.Error"], MessageType.Error)`
- Log information on success: `await logger.LogInformation("Success {Entity}", entity)`

### Validation
- Use Bootstrap validation: `class="@(validated ? " was-validated" : "needs-validation")"`
- Mark required fields with asterisk: `Beneficiary Name: *`
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
        // Save to draft
        StateService.CurrentDraft.Field = _field;
        // Navigate using EditUrl
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
- C# 14.0 language version
- Blazor WebAssembly (client-side)
- SQL Server database (via Entity Framework Core)
- Default ports: http://localhost:5000, https://localhost:5001

## Important Notes
- Never use dynamic port binding (`:0`) - causes Blazor WebAssembly errors
- Always use `EditUrl()` for action-based navigation within modules
- Localization is required for all user-facing text
- Mobile-first design is critical (small screens, touch input)
- Offline support is essential (poor connectivity)
- Forms should be as short as possible (minimize scrolling on small screens)
- Use checkboxes instead of text input whenever possible

## Git & Version Control

### Commit Messages
keep them simple

### PowerShell-Specific Git Commands

PowerShell has issues with multi-line strings in git commit messages. Use these patterns:

**? RECOMMENDED - Multiple -m flags:**
```powershell
git commit -m "Title of commit" `
          -m "Body paragraph explaining what changed" `
          -m "Additional details if needed" `
          -m "Closes #123"
```

**? ALTERNATIVE - PowerShell here-string:**
```powershell
$message = @"
Title of commit

Body paragraph explaining what changed.

Closes #123
"@
git commit -m $message
```

**? AVOID - This doesn't work in PowerShell:**
```powershell
git commit -m "Line 1
Line 2
Line 3"  # PowerShell won't parse this correctly
```

### Commit Best Practices

1. **Atomic Commits** - One logical change per commit
2. **Meaningful Messages** - Explain WHY, not just WHAT
3. **Reference Issues** - Use `Closes #5` or `Relates to #5`
4. **Test Before Commit** - Ensure code compiles and runs
5. **Review Changes** - Use `git status` and `git diff` before committing
6. **?? WAIT FOR APPROVAL** - Never commit and push without explicit user approval

### Copilot Assistant Git Workflow

**IMPORTANT**: When assisting with Git operations:
1. ? **Prepare commit messages** - Draft proper conventional commit messages
2. ? **Show what will be committed** - Display git status and proposed changes
3. ? **DO NOT commit automatically** - Always wait for user's explicit "commit" or "push" command
4. ? **DO NOT push automatically** - User must explicitly request "push" operation
5. ? **Ask for confirmation** - "Ready to commit? Type 'commit' to proceed."

### Common Git Workflow
```powershell
# Check current status
git status

# Stage all changes
git add -A

# Commit with proper message
git commit -m "feat(module): add new feature" `
          -m "Detailed description of changes" `
          -m "Closes #5"

# Push to remote
git push origin feature/branch-name

# Create PR via GitHub CLI or web interface
```

### Branch Naming
- Feature branches: `feature/5-village-data-management`
- Bug fixes: `fix/123-bug-description`
- Documentation: `docs/update-readme`
- Format: `<type>/<issue-number>-<short-description>`

### Pull Request Guidelines
- Reference the issue number in PR title and description
- Include detailed summary of changes
- List files added, modified, deleted
- Provide testing checklist
- Tag reviewers with `cc @username`

## Blazor Code Style and Structure

- Write idiomatic and efficient Blazor and C# code.
- Follow .NET and Blazor conventions.
- Use Razor Components appropriately for component-based UI development.
- Use Blazor Components appropriately for component-based UI development.
- Prefer inline functions for smaller components but separate complex logic into code-behind or service classes.
- Async/await should be used where applicable to ensure non-blocking UI operations.


## Naming Conventions

- Follow PascalCase for component names, method names, and public members.
- Use camelCase for private fields and local variables.
- Prefix interface names with "I" (e.g., IUserService).

## Blazor and .NET Specific Guidelines

- Utilize Blazor's built-in features for component lifecycle (e.g., OnInitializedAsync, OnParametersSetAsync).
- Use data binding effectively with @bind.
- Leverage Dependency Injection for services in Blazor.
- Structure Blazor components and services following Separation of Concerns.
- Always use the latest version C#, currently C# 13 features like record types, pattern matching, and global usings.

## Oqtane specific Guidelines
- See base classes and patterns in the [Main Oqtane repo](https://github.com/oqtane/oqtane.framework)
- Follow client server patterns for module development.
- The Client project has various modules in the modules folder.
- Each action in the client module is a seperate razor file that inherits from ModuleBase with index.razor being the default action.
- For complex client processing like getting data, create a service class that inherits from ServiceBase and lives in the services folder. One service class for each module. 
- Client service should call server endpoint using ServiceBase methods
- Server project contains MVC Controllers, one for each module that match the client service calls.  Each controller will call server-side services or repositories managed by DI
- Server projects use repository patterns for modules, one repository class per module to match the controllers. 

## Error Handling and Validation

- Implement proper error handling for Blazor pages and API calls.
- Use built-in Oqtane logging methods from base classes.
- Use logging for error tracking in the backend and consider capturing UI-level errors in Blazor with tools like ErrorBoundary.
- Implement validation using FluentValidation or DataAnnotations in forms.

## Blazor API and Performance Optimization

- Utilize Blazor server-side or WebAssembly optimally based on the project requirements.
- Use asynchronous methods (async/await) for API calls or UI actions that could block the main thread.
- Optimize Razor components by reducing unnecessary renders and using StateHasChanged() efficiently.
- Minimize the component render tree by avoiding re-renders unless necessary, using ShouldRender() where appropriate.
- Use EventCallbacks for handling user interactions efficiently, passing only minimal data when triggering events.

## Caching Strategies

- Implement in-memory caching for frequently used data, especially for Blazor Server apps. Use IMemoryCache for lightweight caching solutions.
- For Blazor WebAssembly, utilize localStorage or sessionStorage to cache application state between user sessions.
- Consider Distributed Cache strategies (like Redis or SQL Server Cache) for larger applications that need shared state across multiple users or clients.
- Cache API calls by storing responses to avoid redundant calls when data is unlikely to change, thus improving the user experience.

## State Management Libraries

- Use Blazor's built-in Cascading Parameters and EventCallbacks for basic state sharing across components.
- use built-in Oqtane state management in the base classes like PageState and SiteState when appropriate.
- Avoid adding extra depenencies like Fluxor or BlazorState when the application grows in complexity.
- For client-side state persistence in Blazor WebAssembly, consider using Blazored.LocalStorage or Blazored.SessionStorage to maintain state between page reloads.
- For server-side Blazor, use Scoped Services and the StateContainer pattern to manage state within user sessions while minimizing re-renders.

## API Design and Integration

- Use service base methods to communicate with external APIs or server project backend.
- Implement error handling for API calls using try-catch and provide proper user feedback in the UI.

## Testing and Debugging in Visual Studio

- All unit testing and integration testing should be done in Visual Studio Enterprise.
- Test Blazor components and services using xUnit, NUnit, or MSTest.
- Use Moq or NSubstitute for mocking dependencies during tests.
- Debug Blazor UI issues using browser developer tools and Visual Studio's debugging tools for backend and server-side issues.
- For performance profiling and optimization, rely on Visual Studio's diagnostics tools.

## Security and Authentication

- Implement Authentication and Authorization using built-in Oqtane base class members like User.Roles.
- Use HTTPS for all web communication and ensure proper CORS policies are implemented.



