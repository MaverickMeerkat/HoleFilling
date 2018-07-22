using HoleFilling;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string key = "";
            ImageHandler img_handler = null;
            HoleHandler hole_handler = null;
            while (key != "q")
            {
                Console.WriteLine("Please choose what to do:");
                Console.WriteLine(" (L)oad an image ");
                if (img_handler != null) Console.WriteLine(" (E)nter a hole location ");
                if (img_handler?.GetImageMatrix().IsHoled == true) Console.WriteLine(" (F)ind boundary of hole ");
                if (img_handler?.GetImageMatrix().IsHoled == true) Console.WriteLine(" (T)weak weight function parameters");
                if (hole_handler?.Boundary != null) Console.WriteLine(" (M)end hole ");
                if (img_handler != null) Console.WriteLine(" (S)ave the image ");
                Console.WriteLine(" (Q)uit ");

                key = Console.ReadLine();

                switch (key.ToUpper())
                {
                    case "L":
                        Load(ref img_handler, ref hole_handler);
                        break;
                    case "E":
                        EnterHole(img_handler, hole_handler);
                        break;
                    case "F":
                        FindBoundary(hole_handler);
                        break;
                    case "T":
                        TweakWeightFunction(hole_handler);
                        break;
                    case "M":
                        MendHole(hole_handler);
                        break;
                    case "S":
                        SaveImage(img_handler);
                        break;
                    case "Q":
                        return;
                }
                Console.WriteLine();
                Console.WriteLine("***************");
            }
        }

        private static void SaveImage(ImageHandler img_handler)
        {
            Console.WriteLine("Please enter a valid path to save the image (including file name)");
            var path = Console.ReadLine();
            if (Directory.Exists(Path.GetDirectoryName(path)))
            {
                img_handler.SaveChanges(path);
                Console.WriteLine("Image was successfully saved.");
            }
            else
                Console.WriteLine("Path directory wasn't found, please try again.");
        }

        private static void MendHole(HoleHandler hole_handler)
        {
            Console.WriteLine("Choose:");
            Console.WriteLine("(1) Mend hole by entering location");
            Console.WriteLine("(2) Mend hole automatically");
            Console.WriteLine("(3) Mend hole automatically by approximation");
            Console.WriteLine("(4) Mend hole automatically by better approximation");

            var choice = Console.ReadLine();
            try
            {
                if (choice == "1")
                {
                    Console.WriteLine("Enter x-axis start and end location, and y-axis start and end location, separated by a , :");
                    var input = Console.ReadLine();
                    var inputStringArray = input.Split(',');
                    if (inputStringArray.Length > 4)
                        Console.WriteLine("Too many arguments, please try again");
                    else
                    {

                        var x_start = Convert.ToInt32(inputStringArray[0]);
                        var x_end = Convert.ToInt32(inputStringArray[1]);
                        var y_start = Convert.ToInt32(inputStringArray[2]);
                        var y_end = Convert.ToInt32(inputStringArray[3]);
                        hole_handler.FillHole(x_start, x_end, y_start, y_end);
                    }
                }
                else if (choice == "2")
                    hole_handler.FillHole();
                else if (choice == "3")
                    hole_handler.FillHoleApproximate();
                else if (choice == "4")
                    hole_handler.FillHoleBetterApproximate();
                else
                    return;

                Console.WriteLine("Hole mended successfully");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Something went wrong... Please try again");
            }
        }

        private static void TweakWeightFunction(HoleHandler hole_handler)
        {
            Console.WriteLine("Enter the Z value:");
            var zString = Console.ReadLine();
            var zFloat = Convert.ToSingle(zString);

            Console.WriteLine("Enter the Eps value:");
            var epsString = Console.ReadLine();
            var epsFloat = Convert.ToSingle(epsString);

            hole_handler.WeightFunction.SetAdditionalParameters(new Dictionary<string, object>
            {
                ["z"] = zFloat,
                ["e"] = epsFloat
            });

            Console.WriteLine("Parameters changed successfully");
        }

        private static void FindBoundary(HoleHandler hole_handler)
        {
            hole_handler.FindBoundary(new MooreTrace());
            if (hole_handler?.Boundary != null)
            {
                Console.WriteLine($"Total elements: {hole_handler?.Boundary?.Count}");
                foreach (var pt in hole_handler.Boundary)
                    Console.Write($"({pt.Xi}, {pt.Yi}), ");
            }
        }

        private static void EnterHole(ImageHandler img_handler, HoleHandler hole_handler)
        {
            var img = img_handler.GetImageMatrix();
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
                    var x_start = Convert.ToInt32(inputStringArray[0]);
                    var x_end = Convert.ToInt32(inputStringArray[1]);
                    var y_start = Convert.ToInt32(inputStringArray[2]);
                    var y_end = Convert.ToInt32(inputStringArray[3]);
                    hole_handler.CreateHole(x_start, x_end, y_start, y_end);
                    Console.WriteLine("Hole was created successfully");
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong input. Please try again");
                }
            }
        }

        private static void Load(ref ImageHandler img_handler, ref HoleHandler hole_handler)
        {
            Console.WriteLine("Please enter a valid path to an image");
            var path = Console.ReadLine();
            if (File.Exists(path))
            {
                img_handler = new ImageHandler(path);
                var img = img_handler.GetImageMatrix();
                hole_handler = new HoleHandler(img);
                Console.WriteLine("Image was successfully loaded.");
            }
            else
                Console.WriteLine("Image wasn't found, please try again.");
        }
    }



}
