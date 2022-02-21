# Alias
This command line tool allows you to create alias(es) for specific programs in PATH.

# Usage
An example to create an alias for a program:
```
alias add photos "./My Favorite Photo Viewer.exe"
```
From now on this executable is accessable in the same way as any executable added in PATH
You can even use it as a shortcut in `Run` window  (`win+R`)

![Run dialoge box](https://i.imgur.com/KSLJMMc.png)

Remove an existing alias:
```
alias remove photos
```
Get a list of all existing aliases:
```
alias list
```

# Notes
- This program modifies Registry keys at
  `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\`
- [Previous version](https://github.com/Remmoze/Alias-for-PATH-executables) of this program. Current version uses [Cocona](https://github.com/mayuki/Cocona) for better user experience.
