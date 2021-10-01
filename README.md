# IntelliOverflow
IntelliOverflow is a course project by Ryan Green for CS6030 Advanced Software Engineering

This project is an IDE extension (initially targetting Visual Studio) that will recommend Stack Overflow posts for various warnings/errors a developer may face while writing code. The extension will also be able to recommend posts on suggested implementation details (algorithms and data structures) for functions after the developer writes a header with a descriptive comment.

The code is split into two portions. A standalone API that is compiled into a DLL, and IDE extension(s) that will use the DLL.