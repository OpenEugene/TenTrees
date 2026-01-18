# GitHub Copilot Instructions for 10 Trees Oqtane Project

## Project Overview
This is the 10 Trees Digital Platform - a Blazor WebAssembly application built on the Oqtane framework (.NET 10) for tracking beneficiary enrollments, garden mapping, tree monitoring, and program reporting in rural South Africa.

## Oqtane Framework Guidelines

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
- Use `[Table("Schema.TableName")]` attribute
- Database tables prefixed with "OpenEug.TenTrees"
- Use EntityBuilders for migrations (inherit from `AuditableBaseEntityBuilder`)

### Client-Side Services
- Register services in ClientStartup.cs: `services.AddScoped<IService, ServiceImpl>()`
- Use scoped lifetime for state services
- Check if service already registered: `if (!services.Any(s => s.ServiceType == typeof(IService)))`

## 10 Trees Specific Patterns

### Mobile-First Design
- Target small smartphones (Nokia, Samsung, off-brand)
- Minimize text entry; maximize checkboxes and Yes/No questions
- Use Bootstrap cards for section organization
- One section per page in multi-step workflows
- Include progress bars: `<div class="progress-bar" style="width: 25%">25%</div>`

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
- **Executive Director** - Full access

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
