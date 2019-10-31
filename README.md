![alt text](https://raw.githubusercontent.com/Timmoth/ChartATask/master/Chartatask_512.png)

# ChartATask
A lightweight time analysis application.

- Automate the logging of the time you spend on different tasks.

- Generate charts to show usage patterns.

- Use machine learning to give you a real time prediction of the likely hood you will achieve set usage goals.

## Why

- I to automate the logging and analysis of the time I spend working on different projects In order to give accurate time estimations.

- Procrastination is a subconscious process, by the time I realise my attention is slipping away from the tasks I want to focus on I've totally lost my momentum and reside myself to the YouTube / Reddit Rabbit hole. It's possible for me to get back on track if I notice myself early on, as such I want a piece of software to show a real time 'Procrastination Meter' to notify me that I'm showing 'dangerous' patterns.

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/120f83a3612544769ad4ec291d98a105)](https://www.codacy.com/manual/Timmoth/Chartatask?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Timmoth/Chartatask&amp;utm_campaign=Badge_Grade)

## Project structure

### Core (dotnet standard library)

A platform agnostic library containing the core business logic and interfaces.

### WindowsInteractor (dotnet Framework library)

A windows implementation of the EventWatchers and RequestEvaluators which tell the Core Engine when to trigger a session Start / Stop

### WindowsConsole (dotnet Framework library)

ChartATask Engine embedded in a Windows Console Application used to listen to system events and collect data

### WindowsWPF (dotnet Framework WPF app)

A Windows application to chart the data collected.
