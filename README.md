# GIBS.Module.FAQ

FAQ module for [Oqtane](https://www.oqtane.org/) built with .NET 10 and Blazor WebAssembly.

## Overview

`GIBS.Module.FAQ` provides a categorized Frequently Asked Questions experience for Oqtane sites.

- Public FAQ listing with accordion-style question/answer display
- Category-based filtering using query string (`?Category={slug}`)
- Direct question linking (`?faq={id}` or `?question={id}`)
- View count tracking per FAQ item
- Admin UI for managing FAQs and categories
- Support for module export/import, search indexing, and sitemap URL generation

## Solution Structure

- `Client/` - Blazor UI, module views, and client services
- `Server/` - API controllers, repositories, EF Core context, migrations, and module manager
- `Shared/` - shared models and service interfaces
- `Package/` - NuGet/Oqtane packaging assets (`.nuspec`, build scripts, icon)

## Data Model

### FAQ
- `FAQId`
- `ModuleId`
- `Question`
- `Answer`
- `CategoryId`
- `SortOrder`
- `Status` (`draft`, `published`, `archived`)
- `ViewCount`

### Category
- `CategoryId`
- `ModuleId`
- `Name`
- `Slug`
- `ParentId`
- `SortOrder`
- `IsActive`

## Oqtane Integration

The module definition is registered as:

- **Name:** FAQ
- **Version:** 1.0.0
- **Package Name:** `GIBS.Module.FAQ`
- **Dependencies:** `GIBS.Module.FAQ.Shared.Oqtane`

Server manager features include:

- Install/uninstall migrations
- Export/import of FAQ content
- Search content indexing
- Sitemap URL generation for FAQ links

## Development

### Prerequisites

- .NET SDK 10
- Oqtane framework environment

### Local Debug Workflow

Build the package project in **Debug** configuration. Post-build scripts copy module assemblies and static assets into the local Oqtane Server output.

### Release Packaging

Build the package project in **Release** configuration to generate a `.nupkg` from `Package/GIBS.Module.FAQ.nuspec` and copy it to the Oqtane `Packages` folder.

## Compatibility

- Target Framework: `net10.0`
- Oqtane package type: `Oqtane.Framework` `10.1.2`

## License

MIT
