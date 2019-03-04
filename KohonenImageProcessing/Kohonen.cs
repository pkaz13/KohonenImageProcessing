using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KohonenImageProcessing
{
    public class Kohonen
    {
        public Neuron[] Neurons { get; set; }
        public double Step { get; set; }
        public int FrameSize { get; set; }

        private Random random;

        public Kohonen(int frameSize, int numberOfNeurons, double step, Random random)
        {
            this.FrameSize = frameSize;
            this.Step = step;
            this.random = random;
            InitNeurons(numberOfNeurons, random);
        }

        private void InitNeurons(int numberOfNeurons, Random random)
        {
            Neurons = new Neuron[numberOfNeurons];
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new Neuron(FrameSize * FrameSize, random);
            }
        }

        public void Train(int numberOfEpochs, int[,] imgArray)
        {
            for (int i = 0; i < numberOfEpochs; i++)
            {
                int[] frameBeginning = FrameHelper.SelectRandomFrameBeginning(imgArray.GetLength(0), FrameSize, random);
                int[] frame = FrameHelper.GetFrame(frameBeginning, imgArray, FrameSize);
                double[] normalizedFrame = FrameHelper.Normalize(frame, FrameHelper.CountSquareOfVectorSum(frame));
                Train(normalizedFrame);
            }
        }

        private void Train(double[] frame)
        {
            int bestNeuronIndex = FindBestNeuron(frame);
            UpdateWeights(frame, bestNeuronIndex);
        }

        private int FindBestNeuron(double[] frame)
        {
            int bestNeuronIndex = -1;
            double theSmallestDist = Double.PositiveInfinity;
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].Inputs = frame;
                double distance = Neurons[i].CountDistance();
                if (distance < theSmallestDist)
                {
                    theSmallestDist = distance;
                    bestNeuronIndex = i;
                }
            }
            return bestNeuronIndex;
        }

        private void UpdateWeights(double[] frame, int bestNeuronIndex)
        {
            double[] weights = Neurons[bestNeuronIndex].Weights;
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = weights[i] + Step * (frame[i] - weights[i]);
            }
            double[] normWeights = NormalizeWeights(weights);
            Neurons[bestNeuronIndex].Weights = normWeights;
        }

        private double[] NormalizeWeights(double[] weights)
        {
            double[] normalizedWeights = new double[weights.Length];
            double squareOfVectorSum = FrameHelper.CountSquareOfVectorSum(weights);
            for (int i = 0; i < weights.Length; i++)
            {
                if (squareOfVectorSum == 0)
                    normalizedWeights[i] = 0;
                else
                    normalizedWeights[i] = weights[i] / squareOfVectorSum;
            }
            return normalizedWeights;
        }

        public Result Compress(int[,] imgArray)
        {
            int totalFrames = (imgArray.GetLength(0) * imgArray.GetLength(1)) / (FrameSize * FrameSize);
            int numOfFramesInrow = imgArray.GetLength(0) / FrameSize;
            int counter = 0;
            Result result = new Result(totalFrames);
            result.FrameSize = FrameSize;
            for (int i = 0; i < numOfFramesInrow; i++)
            {
                for (int j = 0; j < numOfFramesInrow; j++)
                {
                    int[] frameBeginning = { FrameSize * i, FrameSize * j };
                    int[] frame = FrameHelper.GetFrame(frameBeginning, imgArray, FrameSize);
                    double squareOfVectorSum = FrameHelper.CountSquareOfVectorSum(frame);
                    double[] normalizedFrame = FrameHelper.Normalize(frame, squareOfVectorSum);
                    int bestNeuronIndex = FindBestNeuron(normalizedFrame);
                    result.NeuronIndexes[counter] = bestNeuronIndex;
                    result.Lenghts[counter] = squareOfVectorSum;
                    counter++;
                }
            }
            if (counter != totalFrames)
                throw new Exception("Counter must be equal to total frames");
            for (int i = 0; i < Neurons.Length; i++)
            {
                result.IndexToWeights.Add(i, Neurons[i].Weights);
            }
            return result;
        }
    }
}
