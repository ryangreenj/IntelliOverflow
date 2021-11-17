# IntelliOverflow
Ryan Green

CS 6030 Advanced Software Engineering

## Introduction
IntelliOverflow is an extension for Integrated Development Environments that will be able to recommend Stack Overflow posts to a developer based on various warnings/errors they may encounter while writing their code.

## Motivation
Domain experts (mathematicians, scientists, researchers) may occasionally need to run some code for their work. For example, they may need to analyze some data or run certain algorithms for their research. These experts often do not have the specialized expertise and theory of a Software Engineer. Their solutions are to either hire someone to write the code for them, which costs money, or they can attempt to write the code themselves but may be unsuccesful. The aim for this project is to help this target group in writing their own code and solving issues or errors that may arise in their attempt.

Another target group for this project is experienced software developers that can use the plugin as a companion while coding for quick and easy in-app solutions to problems they run into.

## Implementation

## Milestone 1
Milestone 1 has consisted of research and learning in two areas. The first being how to use the StackExchange API to search for Stack Overflow posts and the other area being learning the Visual Studio SDK for Visual Studio extension development. Some of the resources I have used for this milestone are listed below

- Stack Exchange API Docs - https://api.stackexchange.com/docs
- VSSDK Docs - https://docs.microsoft.com/en-us/visualstudio/extensibility/visual-studio-sdk?view=vs-2019
- Sample VS Extensions - https://github.com/microsoft/VSSDK-Extensibility-Samples
- Windows Presentation Foundation (WPF) Docs - https://docs.microsoft.com/en-us/visualstudio/designers/getting-started-with-wpf?view=vs-2019
- VS Extension Development Video Series - https://www.youtube.com/watch?v=u0pRDM8qW04

## Milestone 2
Milestone 2 included implementation of basic functionality of the project and showed some of the first results of being able to use the plugin in Visual Studio.

### API
The API was enhanced with the following features

- Send search queries and obtain results from StackExchange Web API
- Properly parse raw Json returned from API calls using Newtonsoft Json library
- Restructure as .NET Framework project to support Visual Studio plugin
- Filter and rank results using post metrics (score, answers, tags)

### Visual Studio Plugin
The Visual Studio plugin was created to implement the API functionality in a user-friendly package.

Throughout the course of the additions below, lack of proper, in-depth documentation for the Visual Studio SDK became increasingly apparent. Outside of the basic tutorials for some components on their website, there really is not much official reference documentation to assist in using the SDK. Many problems I ran into I tried to solve using 3rd party online developer forums (like Stack Overflow), however I found that a lot of solutions were for very old versions of the SDK and were since removed/changed from the most recent version. Despite this setback, I was able to get the following functionality working.

- Add an IntelliOverflow Tool Window that could be interacted with/docked like other Tool Windows (Solution Explorer, Error List..)
- To the Tool Window, add a query search box and button where users can submit their own queries
- Add a post listing that dynamically updates from a list of posts to currently display
- Integrate the Tool Window with the API (.dll) and get searching, post display working
- Double-clicking a post in the listing will open the page in your default browser
- Add a Command to the Error List, "Search on Stack Overflow"
	- A query will be constructed using the components of the error/warning/message
	- Strip context of query (variable names, line numbers) to allow better results
	- Experiment with different ways of constructing query from error

## Results

## Conclusion