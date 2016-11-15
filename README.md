# StarfleetMineClearingExerciseEvaluator

This project is a solution to a programming exercise described in ProblemDescription.pdf

## Getting Started

These instructions will walk you through building and running the project for development and testing purposes.

### Prerequisites

To build and run this project, make sure you have the .NET 4.5.2 framework installed on your machine. It will be beneficial to have Visual Studio 2015 installed as well. You can download Visual Studio Community for free from this page: https://www.visualstudio.com/downloads/

The unit tests for this project are written on the MSpec framework and require an MSpec test runner. If you have Resharper, just install the Machine.Specifications.Runner.Resharper extension to run these tests in Visual Studio. You can also reference this page for additional test runner options: https://github.com/machine/machine.specifications/wiki

The integration tests for this project are written on the MSTest framework, which is the default testing framework for Visual Studio. These tests will run without any additional requirements. 

### Build and Run

Download or clone the project on your machine. In the project directory, you will see ProblemDescription.pdf. Read this first to understand the goal of this project. In the project directory you will also see the ExampleFiles folder and the MineClearingEvaluator folder. The ExampleFiles folder contains the input and output files for the examples given in the problem description. THe MineClearingEvaluator folder contains the .NET project to be built and run as a solution to the problem.

If you are using Visual Studio, open the solution:
```
.\StarfleetMineClearingExerciseEvaluator\MineClearingEvaluator\MineClearingEvaluator.sln
```
and navigate to the Build menu to find Build Solution. Alternatively, press Ctrl+Shift+B. You are now built. If you are building from the command line, you can follow these instructions: https://msdn.microsoft.com/en-us/library/78f4aasd.aspx. You may choose to build either in debug or in release mode. The program will produce identical results.


To run the program, navigate to where the executable is located. For a release build, it will be located at:
```
.\StarfleetMineClearingExerciseEvaluator\MineClearingEvaluator\MineClearingEvaluator\bin\Release\MineClearingEvaluator.exe
```
Open up a command prompt at this location. The program takes two arguments as input, the path to the field file and the path to the script file. These files can be found in the ExampleFiles folder mentioned above. To execute, run the following command:
```
> ./MineClearingEvaluator.exe [field file path] [script file path]
```
The program will output the results of the mine clearing simulation to the console. These results may be compared to the examples given in the problem definition.

## Running the tests

The unit tests for this project can be run by a MSpec test runner described above. Using the Resharper extension in Visual Studio, you can right click the MineClearingEvaluatorUnitTests project in the solution explorer and click Run Unit Tests. If you are using a different test runner, please follow the instructions for your specific test runner.

The integration tests for this project can be run by the MSTest framework that comes default with Visual Studio. To run these tests, simply open the Test Explorer and run the tests shown.

## Design decisions

Comments can be found throughout the codebase describing why certain design decisions were made. 

## Authors

* **Doug Wile** 
