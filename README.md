# ASP.NET Core-8-Dapper

ASP .NET Core Project for learning. Every one should be able to use this templae to build a ASP .NET Core web API with Dapper and Dapper Plus.

### Key Functions

1. User's API
2. Swagger for API's Endpoint
3. Dapper and Dapper Plus
4. AutoMapper
5. FluentValidation
6. EPPlus

## Getting Started

These instructions will get you to setup the project, install sdk and add package (CLI or Package manager console).

### Prerequisites

- Visual Studio 2022 or higher
- .NET 8.x SDK

### Installing

1.  Install .net SDK 8<br>
    [Download .NET SDK here.](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks)
2.  Create new Web API's project (Please create main project with empty and add below to the main project)<br>
    `dotnet new webapi –-name DapperWebAPIProject`<br>
3.  Add package to DapperWebAPIProject's project
    - Dapper<br>
      `dotnet add package Dapper`
    - Dapper Plus<br>
      `dotnet add package Z.Dapper.Plus`
    - Npgsql <br>
      `dotnet add package Npgsql`
    - EPPlus <br>
      `dotnet add package EPPlus`
    - FluentValidation <br>
      `dotnet add package FluentValidation`
    - Newtonsoft Json <br>
      `dotnet add package Newtonsoft.Json`
    - AutoMapper
      `dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection`

## Languages and Tools

<div>
  <img src="https://github.com/devicons/devicon/blob/master/icons/dotnetcore/dotnetcore-original.svg" title="dotnet core" alt="dotnet core" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/csharp/csharp-original.svg" title="csharp" alt="csharp" width="40" height="40"/>&nbsp;
  <img src="https://github.com/devicons/devicon/blob/master/icons/postgresql/postgresql-original.svg" title="postgresql" alt="postgresql" width="40" height="40"/>&nbsp;
  <img src="https://dapper-plus.net/images/logo256X256.png" title="dapper" alt="dapper" width="40" height="40"/>&nbsp;
  <img src="https://miro.medium.com/v2/resize:fit:828/format:webp/1*1d5MAXPALi0itRne9lPfhg.png" title="automapper" alt="automapper" width="40" height="40"/>&nbsp;
  <img src="https://raw.githubusercontent.com/FluentValidation/FluentValidation/gh-pages/assets/images/logo/fluent-validation-logo.png" title="fluent-validation" alt="fluent-validation" width="40" height="40"/>&nbsp;
</div>
