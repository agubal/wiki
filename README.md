# Core patterns
- CQRS with Event sourcing
- Mediator
- Dependency Injection

# Libraries
- <b>ASP.NET Web.API 2</b> for REST service
- <b>StructureMap</b> as IoC Container
- <b>MediatR</b> as mediator pattern implementation
- <b>Dapper</b> for data access

# Steps to install
- Build project to restore Nuget packages
- Run script to generate database: `\Wiki\Wiki.Data\Script\InitialScript.sql`
- Change connection string in `\Wiki\Wiki.Api\Web.config`
- Run project

# Engpoints
- <i>GET /api/pages</i> - Get all pages
- <i>GET /api/pages/{id}</i> - Get specific page
- <i>GET /api/pages/{id}?version={version}</i> - Get specific version of specific page
- <i>GET /api/pages/{id}/versions</i> - Get all page versions
- <i>POST /api/pages/{id}/versions/{version}</i> - Change default page vesion
- <i>POST /api/pages Body: {"Title" : "title", "Text" : "text"}</i> - Create new page
- <i>PUT /api/pages Body: {"Title" : "title", "Text" : "text", "Id" : 1}</i> - Update page

