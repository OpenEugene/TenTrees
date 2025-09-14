# OE.TenTrees Oqtane Module

A comprehensive Oqtane module for managing tree planting and conservation efforts as part of the Ten Trees project.

## Overview

The OE.TenTrees module provides a complete solution for:

- **Tree Management**: Track individual trees with species, location, health status, and measurements
- **Planting Events**: Organize and record tree planting events with location and participant details
- **Tree Monitoring**: Monitor tree health and growth over time with regular check-ins
- **Dashboard**: View comprehensive statistics and recent tree activity
- **Settings**: Configure default locations and module behavior

## Architecture

This module follows the standard Oqtane architecture pattern with three main projects:

### Shared (`OE.TenTrees.Shared`)
Contains the data models shared between client and server:

- **Tree**: Individual tree records with species, location, health status
- **PlantingEvent**: Tree planting event organization and tracking
- **TreeMonitoring**: Health monitoring and growth tracking records

### Client (`OE.TenTrees.Client`)
Blazor WebAssembly client components:

- **Index.razor**: Dashboard view with statistics and recent trees
- **Edit.razor**: Form for adding/editing tree records
- **Settings.razor**: Module configuration
- **Services**: Client-side services for API communication

### Server (`OE.TenTrees.Server`)
ASP.NET Core server components:

- **Controllers**: API endpoints for trees and planting events
- **Repositories**: Data access layer with Entity Framework
- **Migrations**: Database schema management
- **Manager**: Module installation and management

## Features

### Core Models

#### Tree
- Species (scientific name)
- Common name
- GPS coordinates (latitude/longitude)
- Location description
- Planting date and person who planted
- Current health status
- Physical measurements (height, diameter)
- Notes and observations

#### Planting Event
- Event name and description
- Date and location
- Number of trees planted
- Organizer and sponsor information
- Participant count
- Event notes

#### Tree Monitoring
- Reference to specific tree
- Monitoring date
- Health status assessment
- Growth measurements
- Observations and photos
- Action recommendations

### User Interface

#### Dashboard (Index)
- Statistics cards showing total trees, events, and healthy trees
- Recent trees table with key information
- Quick navigation to management functions
- Responsive design for mobile and desktop

#### Tree Management (Edit)
- Comprehensive form for tree data entry
- GPS coordinate input
- Health status selection
- File upload support for photos
- Validation and error handling

#### Module Settings
- Default location and coordinates
- Notification preferences
- Organization information
- Customizable module behavior

## Installation

This module is designed to be installed in an existing Oqtane installation:

1. Copy the module files to your Oqtane installation
2. The module will be automatically detected and available for installation
3. Install the module through the Oqtane admin interface
4. Add the module to any page where you want tree management functionality

## Database Schema

The module creates the following database tables:

- `OETenTreesTree`: Individual tree records
- `OETenTreesPlantingEvent`: Tree planting events
- `OETenTreesTreeMonitoring`: Tree monitoring records

All tables include audit fields (CreatedBy, CreatedOn, ModifiedBy, ModifiedOn) for tracking changes.

## API Endpoints

### Trees
- `GET /api/Tree?moduleid={id}` - Get all trees for a module
- `GET /api/Tree/{id}?moduleid={id}` - Get specific tree
- `POST /api/Tree` - Create new tree
- `PUT /api/Tree/{id}` - Update tree
- `DELETE /api/Tree/{id}?moduleid={id}` - Delete tree

### Planting Events
- `GET /api/PlantingEvent?moduleid={id}` - Get all events for a module
- `GET /api/PlantingEvent/{id}?moduleid={id}` - Get specific event
- `POST /api/PlantingEvent` - Create new event
- `PUT /api/PlantingEvent/{id}` - Update event
- `DELETE /api/PlantingEvent/{id}?moduleid={id}` - Delete event

## Security

The module implements Oqtane's standard security model:

- **View**: Users can view tree information
- **Edit**: Users can create, update, and delete trees and events
- **Admin**: Full access to all module functionality

## Localization

The module is designed to support multiple languages through Oqtane's localization system. Resource keys are used throughout the UI components for easy translation.

## Development Notes

### Framework Versions
- Built for .NET 8.0
- Uses Oqtane.Client, Oqtane.Server, and Oqtane.Shared packages version 5.2.4
- Compatible with Entity Framework Core 8.0

### Dependencies
- Oqtane Framework 5.2.4+
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.AspNetCore.Components

### Building
This module is designed to be built as part of an Oqtane installation rather than as a standalone project. The build process requires the full Oqtane framework context.

## Ten Trees Project Integration

This module is specifically designed for the Ten Trees environmental conservation project mentioned in the requirements. It provides the necessary tools for:

- Tracking community tree planting efforts
- Monitoring tree health and survival rates
- Organizing community planting events
- Maintaining records for environmental impact reporting
- Supporting conservation education and outreach

## Contributing

To contribute to this module:

1. Ensure you have a working Oqtane development environment
2. Follow Oqtane module development best practices
3. Test thoroughly with various tree data scenarios
4. Submit pull requests with clear descriptions of changes

## License

This module is released under the same license as the parent repository.

## Support

For support with this module, please refer to the Oqtane documentation and community forums for general framework questions, and create issues in this repository for module-specific problems.
