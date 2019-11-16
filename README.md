# ExtCore 5.0.0

[![Join the chat at https://gitter.im/ExtCore/ExtCore](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/ExtCore/ExtCore?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

![ExtCore logotype](http://extcore.net/extcore_github_icon.png)

## Introduction

ExtCore is free, open source and cross-platform framework for creating modular and extendable web applications
based on ASP.NET Core. It is built using the best and the most modern tools and languages (Visual Studio 2019, C#
etc). Join our team!

ExtCore allows you to build your web applications from the different independent reusable modules or extensions.
Each of these modules or extensions may consist of one or more ASP.NET Core projects and each of these projects
may include everything you want as any other ASP.NET Core project. You don’t need to perform any additional
actions to make it all work: any ASP.NET Core project can be used as an ExtCore-based web application extension
by default. Controllers, view components, views (~~added as resources and/or~~ precompiled), static content (added as
resources) are resolved automatically. These projects may be then added to the web application in two ways: as
direct dependencies (as source code or NuGet packages) or by copying compiled DLLs to the Extensions folder.
ExtCore supports both of these options out of the box and at the same time.

Furthermore, any project of the ExtCore-based web application is able to discover the types that are defined
inside all the projects (optionally using the predicates for assemblies filtering) and to get the implementations
or instances of that types.

Any module or extension can execute its own code during the web application initialization and startup. You can
use priorities to specify the correct order of the calls. This feature might be used for configuration,
to register services etc.

ExtCore consists of two general packages and four optional basic extensions.

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
* ExtCore.Data.EntityFramework;
* ExtCore.Mvc;
* ExtCore.Events.

#### ExtCore.Data

By default, ExtCore doesn’t know anything about data and storage, but you can use ExtCore.Data extension to have
unified approach to working with data and single storage context among all the extensions. Storage might be represented
by a database, a web API, a file structure or anything else.

#### ExtCore.Data.EntityFramework

Currently it supports MySQL, PostgreSql, SQLite, and SQL Server, but it is very easy to add another storage support.

#### ExtCore.Mvc

By default, ExtCore web applications are not MVC ones. MVC support is provided for them by ExtCore.Mvc extension.
This extension initializes MVC, makes it possible to use controllers, view components, views (~~added as resources
and/or~~ precompiled), static content (added as resources) from other extensions etc.

#### ExtCore.Events

It can be used by the extension to notify the code in this or any other extension about some events.

You can find more information using the links at the bottom of this page.

## Getting Started

All you need to do to have modular and extendable web application is:

* add ExtCore.WebApplication as dependency to your main web application project;
* call AddExtCore and UseExtCore inside your web application's Startup class;
* implement the ExtCore.Infrastructure.IConfigureServicesAction and IConfigureAction interfaces in your extensions
in order to execute some code inside the ConfigureServices and Configure methods of the web application's Startup class (optional);
* tell main web application about the extensions (with implicit dependencies or by copying them into the extensions folder).

### Samples

Please take a look at our samples on GitHub:

* [Full-featured ExtCore 5.0.0 framework sample web application](https://github.com/ExtCore/ExtCore-Sample);
* [ExtCore framework 5.0.0 sample simplest web application](https://github.com/ExtCore/ExtCore-Sample-Simplest);
* [ExtCore framework 5.0.0 sample MVC web application](https://github.com/ExtCore/ExtCore-Sample-Mvc);
* [ExtCore framework 5.0.0 sample web application that uses file storage](https://github.com/ExtCore/ExtCore-Sample-FileStorage);
* [ExtCore framework 5.0.0 sample web application that uses a database](https://github.com/ExtCore/ExtCore-Sample-Data);
* [ExtCore framework 5.0.0 sample web application that uses Identity](https://github.com/ExtCore/ExtCore-Sample-Identity);
* [ExtCore framework 5.0.0 sample web application with modular UI](https://github.com/ExtCore/ExtCore-Sample-Modular-Ui);
* [ExtCore framework 5.0.0 advanced sample web application with modular UI](https://github.com/ExtCore/ExtCore-Sample-Modular-Ui-Adv);
* [ExtCore framework 5.0.0 advanced sample accounting web application](https://github.com/ExtCore/ExtCore-Sample-Accounting);
* [ExtCore framework 5.0.0 sample web application that registers a service inside the extension](https://github.com/ExtCore/ExtCore-Sample-Service);
* [ExtCore framework 5.0.0 sample web application that uses the events](https://github.com/ExtCore/ExtCore-Sample-Events);
* [ExtCore framework 5.0.0 sample API web application](https://github.com/ExtCore/ExtCore-Sample-Api).

You can also download our [ready to use full-featured sample](http://extcore.net/files/ExtCore-Sample-5.0.0.zip).
It contains everything you need to run ExtCore-based web application from Visual Studio 2019, including SQLite
database with the test data.

### Tutorials

We have written [several tutorials](http://docs.extcore.net/en/latest/getting_started/index.html)
to help you start developing your ExtCore-based web applications.

### Real Projects

Please take a look at [Platformus](https://github.com/Platformus/Platformus) on GitHub. It is CMS
built on ExtCore framework with more than 10 extensions and 80 projects.

## Development and Debug

To be able to debug an ExtCore extension you need to add the explicit project references to all of its projects
to the main web application. When development process is complete, you may remove that references and use the
extension as the DLL files.

## Links

Website: http://extcore.net/

Docs: http://docs.extcore.net/

Author: http://sikorsky.pro/