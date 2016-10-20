# ExtCore 1.0.0-rc1

[![Join the chat at https://gitter.im/ExtCore/ExtCore](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/ExtCore/ExtCore?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

![ExtCore logotype](http://extcore.net/extcore_github_icon.png)

## Introduction

ExtCore is free, open source and cross-platform framework for creating modular and extendable web applications
based on ASP.NET Core. It is built using the best and the most modern tools and languages (Visual Studio 2015, C#
etc). Join our team!

ExtCore allows you to build your web applications from the different independent reusable modules or extensions.
Each of these modules or extensions may consist of one or more ASP.NET Core projects and each of these projects
may include everything you want as any other ASP.NET Core project. Controllers, view components, views (added
as resources and/or precompiled), static content (added as resources) are resolved automatically. These projects
may be then added to the web application in two ways: as direct dependencies in project.json (as source code or
NuGet packages) or by copying compiled DLLs to the Extensions folder. ExtCore supports both of these options out
of the box and at the same time.

Furthermore, any project of the ExtCore-based web application is able to get the implementations or instances of
some given type from all the projects (optionally using the predicates for assemblies filtering).

ExtCore consists of two general packages and two optional basic extensions.

### General Packages

ExtCore general packages are:

* ExtCore.Infrastructure;
* ExtCore.WebApplication.

#### ExtCore.Infrastructure

This package describes such basic shared things as IExtension interface and its abstract implementation –
ExtensionBase class. Also it contains ExtensionManager class – the central element in the ExtCore types
discovering mechanism. Most of the modules or extensions need this package as dependency.

#### ExtCore.WebApplication

This package describes basic web application behavior with Startup abstract class. This behavior includes
modules and extensions assemblies discovering, ExtensionManager initialization etc. Any ExtCore web
application must inherit its Startup class from ExtCore.WebApplication.Startup class in order to work
properly. Also this package contains IAssemblyProvider interface and its implementation –
AssemblyProvider class which is used to discover assemblies and might be replaced with the custom one.

### Basic Extensions

ExtCore basic extensions are:

* ExtCore.Data;
* ExtCore.Mvc.

#### ExtCore.Data

By default, ExtCore doesn’t know anything about data and storage, but you can use ExtCore.Data extension to have
unified approach to working with data and single storage context among all the extensions. Currently it supports
Microsoft SQL Server, PostgreSql and SQLite, but it is very easy to add another storage support.

#### ExtCore.Mvc

By default, ExtCore web applications are not MVC ones. MVC support is provided for them by ExtCore.Mvc extension.
This extension initializes MVC, makes it possible to use controllers, view components, views (added as resources
and/or precompiled), static content (added as resources) from other extensions etc.

You can find more information using the links at the bottom of this page.

## Getting Started

All you need to do to have modular and extendable web application is:

* add ExtCore.WebApplication as dependency to your main web application project;
* inherit your main application’s Startup class from ExtCore.WebApplication.Startup;
* implement ExtCore.Infrastructure.IExtension interface in each of your extensions (optional);
* tell main web application about the extensions.

### Samples

Please take a look at our [sample](https://github.com/ExtCore/ExtCore-Sample) on GitHub.

You can also download our [ready to use sample](http://extcore.net/files/ExtCore-Sample-1.0.0-rc1.zip).
It contains everything you need to run ExtCore-based web application from Visual Studio 2015, including SQLite
database with the test data.

### Tutorials

We have written [several tutorials](http://docs.extcore.net/en/latest/getting_started/index.html)
to help you start developing your ExtCore-based web applications.

### Projects

Please take a look at [Platformus](https://github.com/Platformus/Platformus) on GitHub. It is CMS
built on ExtCore framework with 8 extensions and 58 projects.

## Links

Website: http://extcore.net/ (under construction)

Docs: http://docs.extcore.net/ (under construction)