# MainProject
## Table of contents
- [MainProject](#mainproject)
  - [Table of contents](#table-of-contents)
  - [What is BasicDotnetTemplate?](#what-is-basicdotnettemplate)
  - [Technologies](#technologies)
    - [Dotnet](#dotnet)
    - [Sonarcloud](#sonarcloud)
      - [Quality gate](#quality-gate)
        - [Code smells](#code-smells)

## What is BasicDotnetTemplate?
BasicDotnetTemplate is a basic project written in .NET 8. It contains MainProject, a WebApi project, and MainProject.Tests written in .NET 8 that contains tests for MainProject.

## Technologies
### Dotnet 
[![dotnet](https://img.shields.io/badge/.NET%20-8.0.201-blue)](https://dotnet.microsoft.com/en-us)

Every component is developed using **dotnet-core 8.0.201** and was generated with [dotnet](https://dotnet.microsoft.com/).

> .NET is the free, open-source, cross-platform framework for building modern apps and powerful cloud services. Supported on Windows, Linux, and macOS.

### Sonarcloud
[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=csimonapastore_BasicDotnetTemplate)

This project is scanned on [SonarCloud](https://www.sonarsource.com/lp/products/sonarcloud/), a powerful cloud-based code analysis service designed to detect coding issues. It works with GitHub, Bitbucket Cloud, Azure DevOps and GitLab.
You can find the integration in [build.yml](.github/workflows/build.yml).

#### Quality gate
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=csimonapastore_BasicDotnetTemplate&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=csimonapastore_BasicDotnetTemplate)  

A Quality Gate is a set of measure-based, Boolean conditions. It helps you know immediately whether your projects are production-ready.
This project uses **Sonar way** quality gate:

| Condition      | Metric and rating |  Real rating |
| ----------- | ----------- | ----------- |
| No new bugs are introduced      | Reliability rating is A       | [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=csimonapastore_BasicDotnetTemplate&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=csimonapastore_BasicDotnetTemplate) [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=csimonapastore_BasicDotnetTemplate&metric=bugs)](https://sonarcloud.io/summary/new_code?id=csimonapastore_BasicDotnetTemplate)  |
| No new vulnerabilities are introduced   | Security rating is A        | [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=csimonapastore_BasicDotnetTemplate&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=csimonapastore_BasicDotnetTemplate) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=csimonapastore_BasicDotnetTemplate&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=csimonapastore_BasicDotnetTemplate)  |
| New code has limited technical debt | Maintainability rating is A |  [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=csimonapastore_BasicDotnetTemplate&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=csimonapastore_BasicDotnetTemplate) [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=csimonapastore_BasicDotnetTemplate&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=csimonapastore_BasicDotnetTemplate) |
| All new security hotspots are reviewed |  |  |
| New code is sufficiently covered by test | Coverage is greater than or equal to 80.0% | [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=csimonapastore_BasicDotnetTemplate&metric=coverage)](https://sonarcloud.io/summary/new_code?id=csimonapastore_BasicDotnetTemplate)  |
| New code has limited duplication | Duplicated Lines (%) is less than or equal to 3.0% |  [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=csimonapastore_BasicDotnetTemplate&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=csimonapastore_BasicDotnetTemplate) |

##### Code smells
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=csimonapastore_BasicDotnetTemplate&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=csimonapastore_BasicDotnetTemplate) 
 
> One way to look at smells is with respect to principles and quality: "Smells are certain structures in the code that indicate violation of fundamental design principles and negatively impact design quality".[Suryanarayana, Girish (November 2014). Refactoring for Software Design Smells. Morgan Kaufmann.] 
> Code smells are usually not bugs; they are not technically incorrect and do not prevent the program from functioning. Instead, they indicate weaknesses in design that may slow down development or increase the risk of bugs or failures in the future. Bad code smells can be an indicator of factors that contribute to technical debt.[Tufano, Michele; Palomba, Fabio; Bavota, Gabriele; Oliveto, Rocco; Di Penta, Massimiliano; De Lucia, Andrea; Poshyvanyk, Denys (2015).] Robert C. Martin calls a list of code smells a "value system" for software craftsmanship.[Martin, Robert C. (2009). "17: Smells and Heuristics". Clean Code: A Handbook of Agile Software Craftsmanship. Prentice Hall.]
>


