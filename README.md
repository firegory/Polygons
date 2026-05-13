# Polygons

Polygons is a Windows Forms desktop application written in C# for drawing and editing simple 2D geometric shapes. The project was built as an object-oriented programming exercise: every figure is represented as an object with its own geometry, hit-testing, drawing behavior, and undo/redo history integration.

Russian version: [README.ru.md](README.ru.md)

## Features

- Draw triangles, squares, and circles on a canvas.
- Move existing figures with the mouse.
- Delete figures with the right mouse button.
- Change the global figure radius and color.
- Undo and redo create, delete, move, radius, and color changes.
- Save and load drawings as binary `.bin` files.
- Build and display a polygon/convex hull from placed figures.
- Compare polygon-building algorithms with performance charts.

## Tech stack

- C#
- .NET Framework 4.8
- Windows Forms
- Visual Studio solution/project format

## Project structure

```text
.
├── figuresProject.sln              # Visual Studio solution
└── figuresProject/
    ├── figuresProject.csproj       # Windows Forms project
    ├── Program.cs                  # Application entry point
    ├── Form1.*                     # Main drawing window and menu logic
    ├── Form2.*                     # Performance/chart window
    ├── Form3.*                     # Auxiliary dialog/form
    ├── Shape.cs                    # Shape hierarchy: triangle, square, circle
    ├── UndoRedo.cs                 # Undo/redo action model
    └── Properties/                 # Assembly metadata, resources, settings
```

## How to run

1. Install Visual Studio with the **.NET desktop development** workload.
2. Open `figuresProject.sln`.
3. Make sure the target framework **.NET Framework 4.8** is installed.
4. Build and run the `figuresProject` project.

You can also build from a Developer Command Prompt for Visual Studio:

```powershell
msbuild figuresProject.sln /p:Configuration=Release
```

## Basic usage

- Select the figure type from the menu: triangle, square, or circle.
- Left-click an empty area to create a figure.
- Drag a figure to move it.
- Right-click a figure to delete it.
- Use the file menu to create a new drawing, open an existing `.bin` file, or save the current drawing.
- Use undo/redo menu actions to navigate editing history.

## Screenshots

No screenshots are included in the repository yet.

## Notes

This is an educational project and keeps the original Windows Forms implementation style. The repository intentionally excludes local Visual Studio state and build outputs via `.gitignore`.
