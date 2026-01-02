# LaserGRBL Copilot Instructions

## Project Overview
LaserGRBL is a Windows desktop application for controlling laser engravers running GRBL firmware. It's written in C# using Windows Forms.

## Build and Test Commands

The project includes PowerShell scripts for common development tasks:

- **`.\build.ps1`** - Build the solution
- **`.\test.ps1`** - Run tests  
- **`.\run.ps1`** - Build and run the application
- **`.\build-and-run.ps1`** - Build and run in one step

**Always use these scripts instead of running msbuild or dotnet commands directly.**

## Project Structure

- **LaserGRBL/** - Main application source code
- **LaserGRBL.Tests/** - Test projects
- **LaserGRBL.sln** - Visual Studio solution file

## Key Technologies

- Language: C#
- Framework: .NET Framework (Windows Forms)
- Build System: MSBuild
- Version Control: Git

## Development Guidelines

1. Use the PowerShell build scripts (build.ps1, test.ps1) for all build operations
2. Test changes using `.\test.ps1` before committing
3. The application uses GRBL protocol for CNC/laser control
4. Multi-language support via .resx files
5. Main control logic is in `LaserGRBL/Core/GrblCore.cs`
