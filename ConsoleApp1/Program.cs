using HoleFilling;
using HoleFilling.DataObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace HoleFillingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ImageHandler imgHandler = null;
            HoleFinder holeFinder = null;
            HoleFixer holeFixer = null;

            string key = "";
            while (key != "q")
            {
                Console.WriteLine("Please choose what to do:");
                Console.WriteLine(" (L)oad an image ");
                if (imgHandler != null) Console.WriteLine(" (E)nter a hole location ");
                if (imgHandler?.Matrix.IsHoled == true) Console.WriteLine(" (F)ind hole ");
                if (holeFinder?.Hole != null) Console.WriteLine(" (M)end hole ");
                if (imgHandler != null) Console.WriteLine(" (S)ave the image ");
                Console.WriteLine(" (Q)uit ");

                key = Console.ReadLine();

                switch (key.ToUpper())
                {
                    case "L":
                        Load(ref imgHandler, ref holeFinder, ref holeFixer);
                        break;
                    case "E":
                        if (imgHandler != null)
                            EnterHole(imgHandler);
                        break;
                    case "F":
                        if (imgHandler?.Matrix.IsHoled == true)
                            FindHole(holeFinder);
                        break;
                    case "M":
                        if (imgHandler?.Matrix.IsHoled == true && holeFinder?.Hole != null)
                        {
                            MendHole(holeFinder.Hole, holeFixer);
                            holeFinder.FindHole(); // will set Hole to null if no hole found
                        }
                        break;
                    case "S":
                        if (imgHandler != null)
                            SaveImage(imgHandler);
                        break;
                    case "Q":
                        return;
                    default:
                        Console.WriteLine("No such function; Please choose again");
                        break;
                }
                Console.WriteLine();
                Console.WriteLine("***************");
            }
        }

        private static void SaveImage(ImageHandler imgHandler)
        {
            Console.WriteLine("Please enter a valid path to save the image (including file name)");
            var path = Console.ReadLine();
            if (Directory.Exists(Path.GetDirectoryName(path)))
            {
                imgHandler.SaveChanges(path);
                Console.WriteLine("Image was successfully saved.");
            }
            else
                Console.WriteLine("Path directory wasn't found, please try again.");
        }

        private static void MendHole(Hole hole, HoleFixer holeFixer)
        {
            Console.WriteLine("Choose:");
            Console.WriteLine("(1) Mend hole by default weight function (excellent) ");
            Console.WriteLine("(2) Mend hole by average approximation (very poor) ");
            Console.WriteLine("(3) Mend hole by gradient approximation (poor) ");
            Console.WriteLine("(4) Mend hole by spiral 8-connected approximation (good) ");

            var choice = Console.ReadLine();
            try
            {
                if (choice == "1")
                {
                    Console.WriteLine("Choose:");
                    Console.WriteLine("(U)se default parameters (z = 5, eps = 0.0001)");
                    Console.WriteLine("(C)hange parameters");
                    var wChoice = Console.ReadLine().ToUpper();
                    if (wChoice == "U")
                    {
                        holeFixer.FillHoleWithWeightFunction(hole);
                    }
                    else if (wChoice == "C")
                    {
                        var weightFunction = TweakWeightFunction();
                        holeFixer.FillHoleWithWeightFunction(hole, weightFunction);
                    }
                    else
                    {
                        Console.WriteLine("Wrong input");
                        return;
                    }
                }
                else if (choice == "2")
                    holeFixer.FillHoleApproximateAverage(hole);
                else if (choice == "3")
                    holeFixer.FillHoleApproximateGradient(hole);
                else if (choice == "4")
                    holeFixer.FillHoleApproximateConnected(hole);
                else
                {
                    Console.WriteLine("Wrong input");
                    return;
                }

                Console.WriteLine("Hole mended successfully");                
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
                Console.WriteLine("Something went wrong... Please try again");
            }
        }

        private static DefaultWeightFunction TweakWeightFunction()
        {
            Console.WriteLine("Enter the Z value:");
            var zString = Console.ReadLine();
            var zFloat = Convert.ToSingle(zString);

            Console.WriteLine("Enter the Eps value:");
            var epsString = Console.ReadLine();
            var epsFloat = Convert.ToSingle(epsString);

            return new DefaultWeightFunction(new Dictionary<string, object>
            {
                ["z"] = zFloat,
                ["e"] = epsFloat
            });            
        }

        private static Hole FindHole(HoleFinder holeFinder)
        {
            var hole = holeFinder.FindHole();
            
            if (hole != null)
            {
                Console.WriteLine($"Total holes pixels: {hole.HolePixels.Count}");
                Console.WriteLine($"Total boundary pixels: {hole.Boundary.Count}");
                foreach (var pt in hole.Boundary)
                    Console.Write($"({pt.Xi}, {pt.Yi}), ");
                Console.WriteLine();
            }

            return hole;
        }

        private static void EnterHole(ImageHandler imgHandler)
        {
            var img = imgHandler.Matrix;
            Console.WriteLine($"Image size is {img.LenX} x {img.LenY} pixels.");
            Console.WriteLine("Enter x-axis start and end location, and y-axis start and end location, separated by a , :");
            var input = Console.ReadLine();
            var inputStringArray = input.Split(',');
            if (inputStringArray.Length > 4)
                Console.WriteLine("Too many arguments, please try again");
            else
            {
                try
                {
                    var xStart = Convert.ToInt32(inputStringArray[0]);
                    var xEnd = Convert.ToInt32(inputStringArray[1]);
                    var yStart = Convert.ToInt32(inputStringArray[2]);
                    var yEnd = Convert.ToInt32(inputStringArray[3]);
                    imgHandler.CreateHole(xStart, xEnd, yStart, yEnd);
                    Console.WriteLine("Hole was created successfully");
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong input. Please try again");
                }
            }
        }

        private static void Load(ref ImageHandler imgHandler, ref HoleFinder holeFinder, ref HoleFixer holeFixer)
        {
            Console.WriteLine("Please enter a valid path to an image");
            var path = Console.ReadLine();
            if (File.Exists(path))
            {
                imgHandler = new ImageHandler(path);
                holeFinder = new HoleFinder(imgHandler.Matrix);
                holeFixer = new HoleFixer(imgHandler.Matrix);
                Console.WriteLine("Image was successfully loaded.");
            }
            else
                Console.WriteLine("Image wasn't found, please try again.");
        }
    }



}
