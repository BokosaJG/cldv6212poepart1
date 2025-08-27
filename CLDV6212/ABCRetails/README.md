# ABCRetails

## Overview
ABCRetails is a retail management application developed in **C# using .NET 8.0**. The system is designed to handle retail operations efficiently, making use of modern .NET frameworks and configurations through `appsettings.json`.

## Features
- Built on **.NET 8.0** for performance and scalability
- Centralized configuration via `appsettings.json`
- Modular project structure (`.csproj` and `.sln` files)
- Debug and runtime support for multiple environments
- Extensible for future retail system enhancements

## Installation
1. Install the [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download).
2. Clone or download this repository.
3. Open the solution file `ABCRetails.sln` in **Visual Studio 2022** or later.
4. Restore dependencies using NuGet package manager.
5. Build the solution.

## Usage
- Run the project by pressing **F5** in Visual Studio or using the terminal:
  ```bash
  dotnet run --project ABCRetails
  ```
- Configuration values can be modified in `appsettings.json`.

## Project Structure
```
ABCRetails/
│── ABCRetails.csproj         # Project file
│── Program.cs                # Main application entry
│── appsettings.json          # Application configuration
│── appsettings.Development.json # Development configuration
│── bin/                      # Build outputs
│── obj/                      # Temporary build files
│── ABCRetails.sln            # Solution file
```

## Requirements
- **.NET SDK 8.0**
- **Visual Studio 2022** (or JetBrains Rider / VS Code with C# extension)
- Windows, Linux, or macOS

## References (Harvard Style)
- Microsoft (2023) *What is .NET?*. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/core/introduction (Accessed: 27 August 2025).
- Microsoft (2023) *.NET SDK overview*. Microsoft Learn. Available at: https://learn.microsoft.com/en-us/dotnet/core/sdk (Accessed: 27 August 2025).
- Microsoft (2023) *Visual Studio IDE*. Microsoft Learn. Available at: https://visualstudio.microsoft.com/ (Accessed: 27 August 2025).
- Albahari, J. and Albahari, B. (2022) *C# 10 in a Nutshell: The Definitive Reference*. 1st edn. O’Reilly Media.(Accessed: 27 August 2025).

---
© 2025 ABCRetails Project
