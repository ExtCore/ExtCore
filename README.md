# ExtCore 1.0.0-alpha9

[![Join the chat at https://gitter.im/ExtCore/ExtCore](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/ExtCore/ExtCore?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Introduction

ExtCore is free, open source and cross-platform framework for creating modular and extendable web applications
based on ASP.NET Core. It is built using the best and the most modern tools and languages (Visual Studio 2015, C#
etc). Join our team!

ExtCore allows you to decouple your application into the modules (or extensions) and reuse that modules in other
applications in various combinations. Each ExtCore extension may consist of one or more projects and each project
may include everything you want (as any other ASP.NET Core project). Controllers, view components, views (added as
resources and/or precompiled), static content (added as resources) will be resolved automatically. These projects
(extension pieces) may be added to the application directly as dependencies in project.json of your main
application project (as source code or NuGet packages), or by copying compiled DLL-files to the Extensions
folder. ExtCore supports both of these approaches out of the box and at the same time.

By default, ExtCore doesn’t know anything about data and storage, but you can use ExtCore.Data extension to have
unified approach to working with data and single storage context. It supports Microsoft SQL Server, PostgreSql
and SQLite, but it is very easy to add another storage support.

You can find more information using the links at the bottom of this page.

## Basic Concepts

### Unified Extension Structure

Here X is your application name and Y is your extension name:

*	X.Y;
*	X.Y.Data.Models;
*	X.Y.Data.Abstractions;
*	X.Y.Data.SpecificStorageA;
*	X.Y.Data.SpecificStorageB;
*	X.Y.Data.SpecificStorageC;
*	X.Y.Frontend;
*	X.Y.Backend;
*	etc.

For example, we can take a look at ExtCore.Data extension structure:

* ExtCore.Data;
* ExtCore.Data.Models.Abstractions;
* ExtCore.Data.Abstractions;
* ExtCore.Data.EntityFramework.Sqlite;
* ExtCore.Data.EntityFramework.SqlServer.

## Quick Start

All you need to do to have modular and extendable web application is:

* add ExtCore.WebApplication as dependency to your main application project;
* inherit your main application’s Startup class from ExtCore.WebApplication.Startup;
* add ExtCore.Infrastructure as dependency to each of your extension projects;
* implement ExtCore.Infrastructure.IExtension interface for each of your extensions (optional);
* tell main application about the extension.

### Samples

Please take a look at our [sample](https://github.com/ExtCore/ExtCore-Sample) on GitHub.

You can also download our [ready to use sample](http://extcore.net/files/ExtCore-Sample-1.0.0-alpha9.zip).
It contains everything you need to run ExtCore-based web application from Visual Studio 2015, including SQLite
database with the test data.

### Projects

Please take a look at [Platformus](https://github.com/Platformus/Platformus) on GitHub. It is CMS
built on ExtCore framework with 8 extensions and 58 projects.

## Links

Website: http://extcore.net/ (under construction)

Docs: http://docs.extcore.net/ (under construction)