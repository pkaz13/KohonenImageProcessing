using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KohonenImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "..\\..\\img\\lena.png";
            int[,] imgArray = ImageHelper.LoadImage(path);
            Console.Write("Rozmiar ramki: ");
            int frameSize = int.Parse(Console.ReadLine());
            //int frameSize = 2;
            Console.Write("Liczba neuronów: ");
            int numOfNeurons = int.Parse(Console.ReadLine());
            Console.Write("Liczba epok: ");
            int epochs = int.Parse(Console.ReadLine());
            Random random = new Random();
            Kohonen kohonen = new Kohonen(frameSize, numOfNeurons, 0.01, random);
            kohonen.Train(epochs, imgArray);
            Result result = kohonen.Compress(imgArray);
            int[,] decompressedImage = Decompress(result);
            ImageHelper.SaveImage(decompressedImage, "..\\..\\img\\test.png");
            double PSNR = Coefficient.PSNR(imgArray, decompressedImage);
            Console.WriteLine($"Współczynnik PSNR: {PSNR} dB");
            double compressionCoefficient = CountCompressionCoefficient(imgArray, frameSize, numOfNeurons, 8);
            Console.WriteLine($"Współczynnik kompresji: {compressionCoefficient}");
            Console.ReadLine();
        }

        static int[,] Decompress(Result result)
        {
            int frameSize = result.FrameSize;
            int imgSize = (int)Math.Sqrt(result.NeuronIndexes.Length * (frameSize * frameSize));
            int[,] decompressionResult = new int[imgSize, imgSize];
            int numOfFramesInRow = decompressionResult.GetLength(0) / frameSize;
            int counter = 0;
            for (int i = 0; i < numOfFramesInRow; i++)
            {
                for (int j = 0; j < numOfFramesInRow; j++)
                {
                    double[] weights = result.IndexToWeights[result.NeuronIndexes[counter]];
                    int[] decompressedFrame = DecompressFrame(weights, result.Lenghts[counter]);
                    PutFrame(decompressionResult, decompressedFrame, i * frameSize, j * frameSize, frameSize);
                    counter++;
                }
            }
            return decompressionResult;
        }

        static void PutFrame(int[,] decompressionResult, int[] decompressedFrame, int i, int j, int frameSize)
        {
            int counter = 0;
            for (int x = 0; x < frameSize; x++)
            {
                for (int y = 0; y < frameSize; y++)
                {
                    decompressionResult[i + x, j + y] = decompressedFrame[counter];
                    counter++;
                }
            }
        }

        static int[] DecompressFrame(double[] weights, double length)
        {
            double[] decompressedFrame = new double[weights.Length];
            for (int i = 0; i < weights.Length; i++)
            {
                decompressedFrame[i] = weights[i] * length;
            }
            return ConvertFrameToInt(decompressedFrame);
        }

        static int[] ConvertFrameToInt(double[] decompressedFrame)
        {
            int[] frame = new int[decompressedFrame.Length];
            for (int i = 0; i < decompressedFrame.Length; i++)
            {
                frame[i] = (int)decompressedFrame[i];
            }
            return frame;
        }

        static double CountCompressionCoefficient(int[,] imgArray, int frameSize, int numOfNeurons, int bitsPerValue)
        {
            int numOfPixels = imgArray.GetLength(0) * imgArray.GetLength(1);
            double b1 = numOfPixels * 8;
            int pixelsPerFrame = frameSize * frameSize;
            int numOfFrames = numOfPixels / pixelsPerFrame;
            double b2 = numOfFrames * (Math.Ceiling(Log2(numOfNeurons)) + bitsPerValue) + (numOfNeurons * pixelsPerFrame * bitsPerValue);
            double b = b2 / b1;
            return (1 - b) * 100;
        }

        static double Log2(int n)
        {
            return (Math.Log(n) / Math.Log(2));
        }
    }
}
