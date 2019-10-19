# ChartATask
Log your time spent on tasks based on user defined triggers.

## Why

I need a way to automate the logging and analysis of time spent working on different projects, and using off the shelf solutions are no way near as fun as paving your own way!


## Project structure

### Core (dotnet standard library)

A library containing the core business logic which ties together all of the modules.

### Interactor (dotnet standard library)

A defines the a platform agnostic interface 'Core' uses to interact with platform specific interactors.

### Persistence (dotnet standard library)

A defines the interface 'Core' uses to interact with platform specific persistence technologies.

### ChartATaskConsole (dotnet Core Console App)

A cross platform console version of ChartATask

### WindowsInteractor (dotnet Framework library)

A windows implementation of the interactor interface.
