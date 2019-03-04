using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KohonenImageProcessing
{
    public static class ImageHelper
    {

        public static int[,] LoadImage(string path)
        {
            Bitmap bitmap = new Bitmap(path);
            int width = bitmap.Width;
            int height = bitmap.Height;
            int[,] imgArray = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    imgArray[i, j] = bitmap.GetPixel(i, j).R;
                }
            }
            return imgArray;
        }

        //public static int[,] LoadImage1(string path)
        //{
        //    Bitmap bitmap = new Bitmap(path);
        //    Bitmap newBitmap = new Bitmap(bitmap);
        //    Bitmap target = newBitmap.Clone(new Rectangle(0, 0, newBitmap.Width, newBitmap.Height), PixelFormat.Format8bppIndexed);
        //    int width = target.Width;
        //    int height = target.Height;
        //    int[,] imgArray = new int[width, height];
        //    for (int i = 0; i < width; i++)
        //    {
        //        for (int j = 0; j < height; j++)
        //        {
        //            imgArray[i, j] = target.GetPixel(i, j).R;
        //        }
        //    }
        //    return imgArray;
        //}

        public static void SaveImage(int[,] imgArray, string path)
        {
            Bitmap bitmap = new Bitmap(imgArray.GetLength(0), imgArray.GetLength(1));
            for (int i = 0; i < imgArray.GetLength(0); i++)
            {
                for (int j = 0; j < imgArray.GetLength(1); j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    if (imgArray[i,j] > 255)
                    {
                        imgArray[i, j] = 255;
                    }
                    Color newColor = Color.FromArgb(imgArray[i, j], imgArray[i, j], imgArray[i, j]);
                    bitmap.SetPixel(i, j, newColor);
                }
            }
            bitmap.Save(path);
        }
    }
}
