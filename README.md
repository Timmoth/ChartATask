![alt text](https://raw.githubusercontent.com/Timmoth/ChartATask/master/Chartatask_512.png)

# ChartATask
A lightweight time analysis application.

- Automate the logging of time spent on different tasks.

- Generate charts which show meaningful usage trends.

- Set daily usage goals and use machine learning to give you a real time prediction of the likely hood you will achieve them.

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/120f83a3612544769ad4ec291d98a105)](https://www.codacy.com/manual/Timmoth/Chartatask?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Timmoth/Chartatask&amp;utm_campaign=Badge_Grade)

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
