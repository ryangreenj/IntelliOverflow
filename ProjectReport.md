# IntelliOverflow
Ryan Green

Electrical Engineering and Computer Science, University of Cincinnati

CS 6030 Advanced Software Engineering

Dr. Tingting Yu

December 10, 2021

## Introduction
IntelliOverflow is an extension for Integrated Development Environments that will be able to recommend Stack Overflow posts to a developer based on various warnings/errors they may encounter while writing their code.

## Motivation
Domain experts (mathematicians, scientists, researchers) may occasionally need to run some code for their work. For example, they may need to analyze some data or run certain algorithms for their research. These experts often do not have the specialized expertise and theory of a Software Engineer. Their solutions are to either hire someone to write the code for them, which costs money, or they can attempt to write the code themselves but may be unsuccesful and spend a lot more time trying to get it working than necessary. The aim for this project is to help this target group in writing their own code and solving issues or errors that may arise in their attempt. Techniques for construction search queries from error messages received aim to help bridge the gap in debugging experience that these experts have with Software Engineers.

Another target group for this project is experienced software developers that can use the plugin as a companion while coding for quick and easy in-app solutions to problems they run into. The goal for this is to speed up these developers in their workflows of fixing bugs, and adding some convenience by giving them quick search and sort tools. All of the user interface components will be incorporated into the IDE (including post browsing), so it aims to help with scenarios like developing on one monitor without having to switch back and forth between the IDE and a web browser.

## Implementation
Implementation is split into two main components. A standalone DLL API that handles the backend searching and interfacing with the Stack Exchange web API, and the actual implementation of the Visual Studio plugin, Intelli Overflow, utilizing this DLL.

### API
The backend API is split from the project and packaged as a standalone DLL. The purpose of this is to allow for plugins to be created for multiple IDEs (outside of Visual Studio) that can utilize the same framework. If improvements are made to the framework or algorithms, these changes would not need to be replicated on all existing platforms/plugins that implement this API.

This API is what interfaces with the public StackExchange web API, and sends the data (after processing) back to the implementing client.

The API is written in C# using the .NET Framework 4.7.2. The code is asynchronous, which means it can be called without blocking execution on a thread. This is an important property for code that sends web requests and waits for responses, as it would otherwise freeze the implementing program (Visual Studio in this case) in the case of latency. The API uses a free library called Newtonsoft Json.NET to parse json data. This library makes it easy to convert json data (received from a web API) to mutable C# object and structures. API calls (search query, obtain post data) are sent over HTTP to the public StackExchange API. StackExchange returns GZIP-compressed json data which is then decompressed and parsed into C# objects. Filtering and ranking or sorting is performed on the returned post data, and these are send back to the implementing program as C# objects for them to be displayed to the user.

Filtering and ranking is implemented to try to bring more 'relevant' posts to the top of the listing and make them more visible to the user. The meaning of a post being 'relevant' here is subjective to the constraints that I came up with for the ranking process. Posts with answers are usually always shown higher in the list than those without. After this, the number of answers is considered and weighted; a higher number of answers will bring the post closer to the top. This is based on the assumption that posts with answers are more desired and likely to contain useful information for the user. Post score or votes is then considered, with a higher number of votes meaning higher ranking (weighted less than number of answers). It is assumed posts with upvotes are likely to be more useful than posts without. This has a downside where a post may be downvoted because other users consider the question or asker as 'stupid' in the case of the question being simple or having an obvious answer in their minds. This is why votes are less impactful than answers. The last ranking criteria is the number of tags the post has. Since tags are displayed, this will likely help the user mentally filter the post themselves by seeing if the tags align with their context.

Outside of the ranking described above, simple sorting options are also provided: Sort by number of votes, Sort by number of answers, Sort by number of tags, Sort by creation date. The intent here is to add sorting buttons to the plugins for users to control.

### Visual Studio Plugin
The frontend plugin is where the API above is implemented and wrapped into a user-friendly interface. The plugin consists of a Tool Window "Intelli Overflow" where the user can search their own query and browser through a list of posts returned for the current query. This is a Tool Window just like any other in Visual Studio, so it can be moved, resized, and docked wherever the user pleases. The plugin also includes an in-app web browser Tool Window meant for browsing Stack Overflow posts within the IDE. This browser makes use of a WebView component library which handles the netowrking and displaying of the webpage. The last component is a right-click command added to the Error List to construct and send queries to the main plugin window from any Errors/Warning/Messages.

The plugin is written in C# using the Visual Studio SDK (VSSDK). The user interface components (Tool Windows) use the Windows Presentation Foundation (WPF) framework. This is a framework that uses XAML to layout interfaces using different component or elements. WPF is similar to how layouts are created for Android applications. The plugin interface components use grid-based element positioning and scaling to ensure they look good as the user resizes or moves the window.

Error list searching is implemented through a command added to the context menu of the Error List Tool Window. Users can right-click any Error/Warning/Message and click "Search on Stack Overflow", and a query will be automatically constructed and searched from that error. This is done by first extracting useful information from the error, such as the error code and the message text associated with it. The code context is then stripped from this error message, by looking for structures that may represent user defined variables, classes, file names, and anything else that is not generic. This has the issue of some error texts being inconsistent with others and so this context-stripping removes valuable, relevant information. To combat this, multiple queries can be performed at the same time and the results combined, filtered, and ranked all together. Some examples of these queries are just the error code, the error text with/without context, the error code prefixing the error text and so on.

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

## Milestone 3
Milestone 3 was focused on adding an in-app post browser and improving the quality of posts returned by changing how queries are constructed and also combining multiple queries into one result.

### API
The API was enhanced with the following featues

- Modify the weighting to give post score more importance
- Support ranking multiple requests at the same time

### Visual Studio Plugin
The main enhancement to the Visual Studio Plugin for this milestone is the creation of an in-app post browser.

- Support the API changes and combine multiple queries for better results
    - In the future I want to move more of this to API side
- Create an in-app post browser (new Tool Window)
    - Functional web browser, though meant mainly for viewing Stack Overflow posts
    - Forward, back, home, refresh buttons along with URL bar
    - Supports modern web capabilities (HTML5, JS, CSS, PHP, etc)
- Intelli Overflow Tool Window can launch in-app browser with post
- Add a button in post listing to open post in external (default) web browser

## Post Milestone 3
The remaining time between Milestone 3 and project presentation and delivery was spent on improving the user experience for the plugin. The design was drastically improved to display more information on the posts, make the post listing more compact and concise, and clean up some inconsistencies and spacing issues. Sorting options were also added instead of using the ranked sorting produced by the API as an uncontrollable default. Apart from improving the user interface, some bugs were resolved after testing and backend changes were made to improve the quality and relevance rankings of posts returned by the API.

## Results
For this project, a Visual Studio plugin was successfully created that can recommend helpful posts to a developer based on any Errors/Warnings/Messages they may receive in the Error List. The plugin and query construction was able to return helpful posts for the scenarios that I tested it with, and I found it convenient to use. While I was testing this program by working on another course project, I found that the relevance of the posts it would return for some errors would actually surprise me, as I was expecting generic posts for these particular errors that would not be of much use. I found it convenient to use while developing on my single screen laptop, as I did not need to switch back and forth between two or more applications. I could dock my tool windows in positions where I wanted and work with them from there. The quick right-click > search on errors in the Error List and being presented with a list of posts for that error saved me some time as I would have had to manually construct queries and search them myself in a similar manner that I created for the plugin to do automatically.

In order for a project such as this to be properly evaluated, a study with some human participants should be conducted. This could consist of giving the tool to participants from the two audiences identified in the project motivation (domain experts and experienced software developers) and recording some metrics as they use it. How often did they use the plugin? Were the results they received relevant to their error? Did the results help them in resolving the error? What are their thoughts on the usefulness and other factors of the plugin after gaining some experience with it? Performing a study like this would give the plugin some real use outside of the comparatively few errors I found or fabricated to test it with.

**Instructions for downloading, building, and using the plugin are available in README.md**

## Future Considerations
While completing this project, there were some ideas that came up that could not be included due to time and work constraints. If I continue to work on this project, these are the next tasks or features I will consider implementing.
- Using Natural Language Processing techniques when ranking post relevancy. This can be applied to things such as post tags, programming language used, or even text within the post itself.
- Develop plugins targeting other IDEs such as Eclipse and JetBrains.
- Allow users to login/authenticate to the plugin using their Stack Overflow account, where they can do things such as leave comments on posts, upvote/downvote, and even submit and view their own posts from the plugin directly.
- Perform some offline mining of a Stack Overflow archive to improve the relevancy of posts returned by having more control on how searching and filtering is performed instead of relying on the live public Stack Exchange web API search call.
- Expand automatic query construction for different use cases such as recommending data structures, algorithms, and function implementations based on headers or descriptive comments that the developer writes.