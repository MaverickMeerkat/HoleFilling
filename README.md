# HoleFilling

Hole filling solution, written in C#. Divided into 3 separate project:

* Class library - contains the core logic
* Console app - used to manual test it
* Test library - MStests for the Trace algorithm

The class library contains 3 main object:

* ImageHandler - loads an image using Emgu (OpenCv for .NET), grayscales it, and converts it into a Pixel Matrix, each containing a float value between [0,1]. Can also be used to create missing values (holes) in it (-1), and to save changes to the file or to a new file.
* HoleFinder - Takes a Matrix and Finds the hole (boundary of the hole, minimal covering rectangle, and the hole-pixels themselves) in it. Uses Moore-Tracing algorithm as default, but can be supplied other algorithms as well.
* HoleFixer - Takes a Matrix and a Hole, and fixes the hole. Default fix is using a weight function. Default weight function is implemented.

  * Weight Function - hole pixel new value is weighted averaged between of distance between it and all boundary pixels
  * Average - hole pixel new value is the average of all boundary pixels
  * Gradient - hole pixel new value is calculated by a gradient from 4 edges of the covering rectangle
  * Spiral 8-connected-pixels - hole pixel new value is average of all non-missing 8-connected pixels. This is done in spiral manner for best results.
  
# Performance

* Hole:
* Best: Weight Function.
* 2nd: Spiral 8-Connected
* Poor: Gradient
* Very poor: Average
