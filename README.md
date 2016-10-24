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
- 'GET /api/pages' - Get all pages
- 'GET /api/pages/{id}' - Get specific page
- 'GET /api/pages/{id}?version={version}' - Get specific version of specific page
- 'GET /api/pages/{id}/versions' - Get all page versions
- 'POST /api/pages/{id}/versions/{version}' - Change default page vesion
- 'POST /api/pages Body: {"Title" : "title", "Text" : "text"}' - Create new page
- 'PUT /api/pages Body: {"Title" : "title", "Text" : "text", "Id" : 1}' - Update page

