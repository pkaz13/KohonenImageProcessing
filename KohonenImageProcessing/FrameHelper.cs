using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KohonenImageProcessing
{
    public static class FrameHelper
    {
        public static int[] SelectRandomFrameBeginning(int imgArraySize, int frameSize, Random random)
        {
            return new int[2] { random.Next(imgArraySize - frameSize), random.Next(imgArraySize - frameSize) };
        }

        //flatten frame
        public static int[] GetFrame(int[] frameBeginning, int[,] imgArray, int frameSize)
        {
            int[] frame = new int[frameSize * frameSize];
            int counter = 0;
            for (int i = 0; i < frameSize; i++)
            {
                for (int j = 0; j < frameSize; j++)
                {
                    frame[counter] = imgArray[frameBeginning[0] + i, frameBeginning[1] + j];
                    counter++;
                }
            }
            return frame;
        }

        //normalize frame (vector size to 1)
        public static double[] Normalize(int[] frame, double squareOfVectorSum)
        {
            double[] normaliazedFrame = new double[frame.Length];
            for (int i = 0; i < frame.Length; i++)
            {
                if (squareOfVectorSum == 0)
                    normaliazedFrame[i] = 0;
                else
                    normaliazedFrame[i] = frame[i] / squareOfVectorSum;
            }
            return normaliazedFrame;
        }

        public static double CountSquareOfVectorSum(double[] frame)
        {
            double sum = 0;
            for (int i = 0; i < frame.Length; i++)
            {
                sum += Math.Pow(frame[i], 2);
            }
            return Math.Sqrt(sum);
        }

        public static double CountSquareOfVectorSum(int[] frame)
        {
            double sum = 0;
            for (int i = 0; i < frame.Length; i++)
            {
                sum += Math.Pow(frame[i], 2);
            }
            return Math.Sqrt(sum);
        }
    }
}
