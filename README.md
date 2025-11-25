# print-workspace
Console app to print folders and files of the workspace to the console.

This will print a folder and file structure of the workspace you are in.
Printed to the console, with levels of indentation to reflect folder structure.
Copy paste from there, and use as needed.

You can pass the names of directories you want to ignore, like 'node_module' for example, as arguments.
The directory name will still be printed, but with an indented message '~contents ignored~'.
For example:

    myProject\
        .vscode\
            launch.json
            settings.json
        node_modules\
            ~contents ignored~
        src\
            -etc.-
        -etc.-
    -etc.-

Run this in the console, from the workspace folder you want to print, with: 'dotnet run --project "[your path to this repo]\print-workspace\print-workspace\print-workspace.csproj" -- [name of directory you want to ignore] [another ignore dir] [etc.]'