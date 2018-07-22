using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoleFilling
{
    /// <summary>
    /// Uses OpenCV nuget package to load image, transform it to an ImageMatrix, and save any changes made to that matrix.
    /// </summary>
    public class ImageHandler
    {
        private Emgu.CV.Image<Gray, Byte> _img;
        private int _rows, _cols;
        private ImageMatrix _arrayImage;

        public string Path { get; set; }        

        // CTOR
        public ImageHandler(string path)
        {
            Mat mat = CvInvoke.Imread(path, Emgu.CV.CvEnum.ImreadModes.Grayscale);
            _img = mat.ToImage<Gray, Byte>();
            _rows = _img.Rows;
            _cols = _img.Cols;
            Path = path;
        }

        /// <summary>
        /// Converts the loaded image into a Matrix with float value between [0,1]
        /// </summary>
        /// <returns></returns>
        public ImageMatrix GetImageMatrix()
        {
            if (_arrayImage == null)
            {
                float[,] newArray = new float[_rows, _cols];
                for (int i = 0; i < _rows; i++)
                    for (int j = 0; j < _cols; j++)
                        newArray[i, j] = Convert.ToSingle(_img.Data[i, j, 0]) / 255;

                _arrayImage = new ImageMatrix(newArray);
            }
            return _arrayImage;
        }

        /// <summary>
        /// Saves an image to file.
        /// </summary>
        /// <param name="suppliedPath">If no path is specified, will overwrite original file</param>
        public void SaveChanges(string suppliedPath = null)
        {
            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _cols; j++)
                {
                    var val = _arrayImage.GetArrayElement(i, j).Value;

                    // we cannot put -1 in the real image
                    if (val == -1)
                        val = 1;

                    _img.Data[i, j, 0] = Convert.ToByte(val * 255);
                }

            var t = _img.ToBitmap();

            if (suppliedPath == null)
                t.Save(Path);
            else
                t.Save(suppliedPath);
        }           

    }
}
