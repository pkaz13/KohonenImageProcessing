using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KohonenImageProcessing
{
    public static class Coefficient
    {
        public static double PSNR(int[,] oldImg, int[,] newImg)
        {
            double MSE = Coefficient.MSE(oldImg, newImg);
            return 10 * Math.Log10(Math.Pow(255, 2) / MSE);
        }

        public static double MSE(int[,] oldImg, int[,] newImg)
        {
            int imgSize = oldImg.GetLength(0);
            double pixels = imgSize * imgSize;
            double sum = 0;
            for (int i = 0; i < imgSize; i++)
            {
                for (int j = 0; j < imgSize; j++)
                {
                    sum += Math.Pow(oldImg[i, j] - newImg[i, j], 2);
                }
            }
            return 1 / pixels * sum;
        }
    }
}
