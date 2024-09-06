# Loudenvier.CommandLineFramework
A framework to help implementing Command Line Interface (CLI) programs based on textual commands. It has rich editing capabilities and command history support. Adding new commands is as simple as writing a static method!

> **NOTE**: _This project is in very early alpha stage. It's already being used in my own production code but be aware that ***BREAKING CHANGES will*** occur._

This framework differs somewhat from other .NET console libraries as its main focus is on registering and running commands typed by the user: it implements easy registration of new commands, automatic discovery of commands based on (customizable) conventions, map user input to specific commands while automatically parsing arguments, autocomplete user input, etc.

In many ways it is more akin to Python's [cmd package](https://docs.python.org/3/library/cmd.html) (which I didn't know until one day before the initial commit of this library :-) than .NET libraries like Spectre.Console and PrettyPrompt. In fact, I've found it through the [cmd2 package](https://cmd2.readthedocs.io/en/stable/), which inspired me to change from using object hierarquies to plain static methods as a way to define commands.
 
The current focus of the project is to get the API for registering and running commands frozen. The roadmap includes adding other nice console interactive features like menus, choices, RGB color support, etc. It also includes integration with other console "interactive" frameworks like Spectre.Console, PrettyPrompt, etc.
