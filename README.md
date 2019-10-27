![alt text](https://raw.githubusercontent.com/Timmoth/ChartATask/master/Chartatask_512.png)

# ChartATask
A lightweight time analysis application.

- Automate the logging of time spent on different tasks.

- Generate charts which show meaningful usage trends.

- Set daily usage goals and use machine learning to give you a real time prediction of the likely hood you will achieve them.

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/120f83a3612544769ad4ec291d98a105)](https://www.codacy.com/manual/Timmoth/Chartatask?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Timmoth/Chartatask&amp;utm_campaign=Badge_Grade)

## Why

-  using an off the shelf solution is no way near as fun as paving your own way!

-  In order to give accurate time estimations I need a way to automate the logging and analysis of the time I spend working on different projects.

-  Procrastination is a subconscious process, sometimes I'm aware my attention is slipping away from the tasks I want to focus on. More often than not however by the time I realise what im doing I've totally lost my momentum and reside myself to the YouTube Rabbit hole. It's possible for me to get back on track if I notice myself early on. As such I want a piece of software to show a real time 'Procrastination Meter' to notify me that I'm showing 'dangerous' patterns.

## Project structure

### Core (dotnet standard library)

A platform agnostic library containing the core business logic and interfaces.

### WindowsInteractor (dotnet Framework library)

A windows implementation of the SystemInteractor components

### WindowsConsole (dotnet Framework library)

A Windows Console version of ChartATask
