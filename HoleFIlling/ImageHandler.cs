using Emgu.CV;
using Emgu.CV.Structure;
using HoleFilling.DataObjects;
using System;

namespace HoleFilling
{
    /// <summary>
    /// Uses Emgu (OpenCV nuget package) to load image as grayscale and transform it to an ImageMatrix.
    /// You can also create holes in the image and save any changes made to that matrix.
    /// </summary>
    public class ImageHandler
    {
        private readonly Emgu.CV.Image<Gray, Byte> _img;
        private readonly int _rows, _cols;
        private readonly string _path;

        public ImageMatrix Matrix { get; }

        // CTOR
        public ImageHandler(string path)
        {
            Mat mat = CvInvoke.Imread(path, Emgu.CV.CvEnum.ImreadModes.Grayscale);
            _img = mat.ToImage<Gray, Byte>();
            _rows = _img.Rows;
            _cols = _img.Cols;
            _path = path;
            Matrix = LoadImageMatrix();
        }

        //Converts the loaded image into a Matrix with float value between [0,1]
        private ImageMatrix LoadImageMatrix()
        {
            float[,] newArray = new float[_rows, _cols];
            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _cols; j++)
                    newArray[i, j] = Convert.ToSingle(_img.Data[i, j, 0]) / 255;

            return new ImageMatrix(newArray);
        }

        /// <summary>
        /// Saves an image to file.
        /// If image is holed, converts hole to white space.
        /// </summary>
        /// <param name="suppliedPath">If no path is specified, will overwrite original file</param>
        public void SaveChanges(string suppliedPath = null)
        {
            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _cols; j++)
                {
                    var val = Matrix.GetArrayElement(i, j).Value;

                    // we cannot put -1 in the real image
                    if (val == -1)
                        val = 1;

                    _img.Data[i, j, 0] = Convert.ToByte(val * 255);
                }

            var t = _img.ToBitmap();

            if (suppliedPath == null)
                t.Save(_path);
            else
                t.Save(suppliedPath);
        }

        /// <summary>
        /// Creates a hole in an image matrix
        /// </summary>
        /// <param name="xStart"></param>
        /// <param name="xStop"></param>
        /// <param name="yStart"></param>
        /// <param name="yStop"></param>
        public void CreateHole(int xStart, int xStop, int yStart, int yStop)
        {
            for (int i = xStart; i < xStop; i++)
                for (int j = yStart; j < yStop; j++)
                {
                    var pix = Matrix.GetArrayElement(i, j);
                    pix.Value = -1;
                }

            Matrix.IsHoled = true;
        }
    }
}
