# PI System Silent Installer

This projects was created to facilitate the creation of silent installations for the PI System.  The approch taken to achieve this was to look at what people were doing to accomplish this task and what came out of it was that many people would be using powershell to do so.  So starting from there, it was necessary to imagine a way to re-use the work done in this area and find a way to save the knownledge of all the steps that are necessary to create a silent installation for many different components of the PI System.

## Principle of operation

To make the silent installation possible, there is a powershell script called setup.ps1.  When run, this script will look at the file InstallConfig.psd1 and run all the setup kits and scripts that are defined in this file.  Setup.ps1 references several powershell modules that are coming togheter with the script, those modules contain functions that were created to make installations for the different possible types of installation kits. 


This repository includes two main components:

- A C# User Interface Utility that helps creating definitions of silent installations.
- A Powershell Script and associated modules that can run the installation definitions.  

Once the definition of a silent installation has been created with the user interface, a complete installation package can be generated. And this package contains several files and folders:


This solution is composed of:

* **silent-installation-scripts** a set of poweshell files : These powershell files are the "engine" to execute and orchestrate the installation. They are deployed in with the installation package 

* **silent-installation-scripts-tests** a few powershell test, there are very few at the moment.

* **SilentPackagesBuilderGUI** a grapical user interface to generate the configuration file that can be consumed by the silent-installation-scripts.  This GUI also put all the files toghether to generate a silent installation package that is ready to use.

 
# Development Environment Used

Microsoft Visual Studio 2019

# How to Build and use

**SilentPackagesBuilderGUI** has a post build event that will create a Build directory in the same folder as the solution.  this build folder contains all the files and the executable (SilentPackagesBuilder.exe) you need to use the Silent Packages Builder tool.

Once your package is generated, and if it will be installing the PI Data Archive, **you will need to place a valid piserver.dat file** at the root of your package folder.  There should be an existing pilicense.dat but its an empty file 0kb.   


# Unit Tests for powershell scripts
If you want to run unit tests you need to install Pester, and to install Pester you need chocolatey, so here is how to install both, in an administrator command line prompt:

	iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1'))
    choco install pester

More info on Pester: https://writeabout.net/2016/01/01/visual-studio-github-powershell-pester/



# Code Implementation strategies

* UI code is spread in several different users controls, this is an attempt to make the code much more readable that in a single winform.
* UI code make use of data binding.  For example, when a textbox has a binding to a property of a class, if the text changes in the textbox the class property will be updated also.  And vice versa.  this makes the colde lighter.  With this approach there is no need for a "save" button at runtime to transfer data into the model, everything in the model is updated as data is typed in the GUI.





