# IntelliOverflow
IntelliOverflow is a course project by Ryan Green for CS6030 Advanced Software Engineering.

This project is an IDE extension (initially targetting Visual Studio) that will recommend Stack Overflow posts for various warnings/errors a developer may face while writing code. In the future, the extension may also be able to recommend posts on suggested implementation details (algorithms and data structures) for functions after the developer writes a header with a descriptive comment.

The code is split into two portions. A standalone API that is compiled into a DLL, and IDE extension(s) that will use the DLL.

More details about this project can be found in the report, ProjectReport.md or through the PowerPoint slides uploaded.

## Setup
Below are instructions to get the plugin running on your own machine.
### Requirements
- As this is a plugin for Visual Studio, you must be using a Windows machine
- Visual Studio 2019 or newer is required
	- A basic understanding of how Visual Studio is used and works will help.
- .NET Framework 4.7.2 or later must be installed on the machine (should install with Visual Studio)
- Visual Studio SDK must be installed - https://docs.microsoft.com/en-us/visualstudio/extensibility/visual-studio-sdk?view=vs-2022

### Downloading
When the above requirements are met, the project can be downloaded by cloning/downloading this repository.

### Building and Running
Visual Studio Solution files are included and configured, to avoid dealing with makefiles or other generation tools.
In the Code directory, there are three folders
- IntelliOverflowAPI - Standalone DLL API for the project
- IOVSPlugin - Visual Studio Plugin implementation of IntelliOverflow
- TestSlns - Test Visual Studio Solutions to use the plugin on
These folders should not be renamed or moved outside of the Code folder, due to DLL linking being pre-configured in the solutions

The Visual Studio plugin is dependent on the API (DLL), so that must be built first.
1. Open IntelliOverflowAPI folder and locate the solution file IntelliOverflowAPI.sln
2. Open this solution using your version of Visual Studio
3. Upon first loading, the solution should download required NuGet packages
4. In the Solution Explorer there are two projects, IntelliOverflowAPI and APITest
5. Build IntelliOverflowAPI using any method (Right click -> Build or using toolbar). Solution is configured so that building APITest will build IntelliOverflowAPI first
6. A DLL should now be present in the IntelliOverflowAPI/bin/Debug folder. If Release build is used, DLL will need to manually be referenced by plugin.

With the DLL built, the plugin itself can now be built and ran
1. Open IOVSPlugin folder and locate the solution file IOVSPlugin.sln
2. Open this solution using your version of Visual Studio
3. Upon first loading, the solution should download required NuGet packages
4. Build the IOVSPlugin project using any method (Right click -> Build or using toolbar)
5. The plugin can now be launched using the Run (Current Instance) button or Debug toolbar
6. This will launch an experimental version of Visual Studio with the plugin installed

## Using the Plugin
The Intelli Overflow Tool Window can be launched manually by going to the View toolbar > Other Windows > Intelli Overflow. This will bring up the main plugin tool window.

The window can be moved around, resized, and docked like any other Tool Window in Visual Studio (Solution Explorer, Error List). In the search bar you can manually type a query and search, or search automatically from an Error/Warning/Messsage in the Error List. To do that, right click the Error/Warning/Message to debug and click "Search on Stack Overflow". This will bring up the Intelli Overflow window if it is not already and perform the search on the query it constructs. You may sort the resulting posts using the sorting buttons at the top.

To open a post, double clicking it in the post list will launch the post in the Intelli Overflow Browser. This is a built in simple browser (Tool Window, dock and resize as you wish) meant for browsing Stack Overflow posts. There are simple navigation buttons and a URL bar. This browser Tool Window can be launched manually from View > Other Windows > Intelli Overflow Browser. In the post list, you can also use the chain button on the right to open the Stack Overflow post in your default browser.