# 10 Trees Digital Tracking Platform

A Blazor WebAssembly application built on the Oqtane framework for tracking beneficiary enrollments, garden mapping, tree monitoring, and program reporting for the 10 Trees permaculture program in rural South Africa.

## Project Overview

The 10 Trees Digital Platform replaces paper-based processes for tracking permaculture training and tree distribution programs in rural villages near Hoedspruit, South Africa. The platform is designed for use by tree mentors working in the field and staff at the Centre for data analysis and reporting.

### Core Objectives

- **Mobile-First Design**: Optimized for small smartphones (Nokia, Samsung, off-brand) with touch-friendly interfaces
- **Bilingual Support**: English (en-ZA) and Xitsonga (ts-ZA) with single-language display
- **Multi-Village Management**: Secure data isolation with role-based access
- **Comprehensive Tracking**: End-to-end tracking from enrollment through tree survival monitoring

## Key Features

### 1. Beneficiary Enrollment
Multi-step workflow for enrolling new participants:
- **Basic Information**: Name, village, household details
- **Eligibility Criteria**: Program requirements and preferred criteria
- **Commitments**: Acknowledgment of program commitments
- **Digital Signature**: E-signature capture for consent

### 2. Garden Location Mapping
Document garden sites and existing resources:
- GPS coordinate capture (field or manual entry)
- Water availability and catchment systems
- Existing tree inventory (indigenous, fruit, nut trees)
- Space assessment for new trees
- Fence and resource documentation

### 3. Tree Monitoring & Assessment
Recurring garden health assessments:
- Tree survival tracking (planted vs. alive)
- Automatic survival rate calculation
- Problem identification (disease, pests, environmental)
- Permaculture practice compliance:
  - Composting, mulching
  - Water conservation and greywater use
  - Chemical-free practices
- Help request flagging

### 4. Class Attendance Tracking
Monitor permaculture training completion:
- Mark participant attendance
- Track progress toward tree eligibility
- View completion status

### 5. Program Reporting & Data Export
Generate reports and export data:
- Tree survival rates by village
- Permaculture practice compliance
- Monthly village reports
- Export to Excel (.xlsx) and CSV
- Date range and village filtering

### 6. Village Data Management
Multi-tenant architecture with role-based access:
- **Mentors**: View only their assigned village
- **Educators/Project Managers**: View all villages, export data
- **Administrators/Executive Director**: Full access including user management

### 7. Localization & Language Support
Comprehensive bilingual implementation:
- Auto-detect device language with English fallback
- Manual language picker
- Single-language display (not side-by-side)
- All forms, labels, validation messages, and buttons localized

## Technology Stack

- **Framework**: .NET 10
- **Platform**: Oqtane Framework (Blazor WebAssembly)
- **Language**: C# 14.0
- **Database**: SQL Server with Entity Framework Core
- **Testing**: Reqnroll (BDD) with xUnit
- **Localization**: .NET resource files (.resx)

## Architecture

### Module Structure
```
Client/
  ?? Modules/
  ?   ?? Enrollment/         # Beneficiary enrollment workflows
  ?? Services/               # Client-side services and state management
  ?? Resources/              # Localization .resx files (en-ZA, ts-ZA)
  ?? Themes/                 # Mobile-optimized theming
Server/
  ?? Controllers/            # API endpoints
  ?? Services/               # Business logic
  ?? Repository/             # Data access layer
  ?? Manager/                # Entity management
  ?? Migrations/             # Database migrations
Shared/
  ?? Models/                 # Data models shared between client and server
Specs/
  ?? Features/               # BDD scenarios (Gherkin)
  ?? Docs/                   # Feature documentation
```

### Security Model
- Oqtane authentication and authorization
- Role-based access control
- Module-level permissions
- Village data isolation for mentors

## User Roles

| Role | Permissions |
|------|-------------|
| **Mentor** | Submit forms, view assigned village only |
| **Educator** | Submit forms, view all villages, export data |
| **Project Manager** | Same as Educator |
| **Administrator** | Full access including user management |
| **Executive Director** | Full access |

## Design Principles

### Mobile-First
- Touch-friendly UI elements (minimum 44x44px touch targets)
- Large, readable fonts (minimum 16px body text)
- Minimal text entry; maximize checkboxes and Yes/No questions
- One section per page in multi-step workflows
- Progress indicators for multi-step forms

### User Experience
- Bootstrap card-based section organization
- No admin containers (direct component rendering)
- Form validation with clear error messages
- Auto-populated fields where possible

## Development Approach

The project follows **Behavior-Driven Development (BDD)** practices:
- All features defined in Gherkin syntax (`.feature` files)
- Scenarios drive implementation
- Test framework: Reqnroll with xUnit
- Tags for organization: `@workflow-*`, `@priority-*`, `@mobile`

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server
- Compatible IDE (Visual Studio 2022+ recommended)

### Running the Application
```bash
# Default ports
http://localhost:5000
https://localhost:5001
```

?? **Important**: Never use dynamic port binding (`:0`) as it causes Blazor WebAssembly errors.

## Contributing

This project serves rural South African communities with limited resources and connectivity. When contributing:
- Prioritize mobile optimization
- Include bilingual localization (en-ZA and ts-ZA)
- Follow Oqtane framework patterns
- Write BDD scenarios for new features
- Test on small/low-end smartphones

## Project Context

The 10 Trees program trains rural community members in permaculture practices and provides fruit, nut, and indigenous trees to establish sustainable food gardens. The digital platform tracks participants through the complete journey:
1. Training enrollment
2. Permaculture class attendance
3. Garden site mapping
4. Tree distribution
5. Ongoing garden assessment and support

Target communities have limited smartphone access and primarily speak Xitsonga. The platform is designed to work within these constraints while providing program staff with data for impact reporting and continuous improvement.

## License

MIT License

Copyright (c) 2025 Open Eugene

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

## Contact

[Contact information to be added]

