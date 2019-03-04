using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KohonenImageProcessing
{
    public class Neuron
    {
        public double[] Inputs { get; set; }
        public double[] Weights { get; set; }

        private Random random;

        public Neuron(int numOfWeights, Random random)
        {
            this.random = random;
            InitWeights(numOfWeights);
        }

        private void InitWeights(int numberOfWeights)
        {
            Weights = new double[numberOfWeights];
            for (int i = 0; i < numberOfWeights; i++)
            {
                Weights[i] = random.NextDouble();
            }
        }

        public double CountDistance()
        {
            double sum = 0;
            for (int i = 0; i < Inputs.Length; i++)
            {
                sum += Math.Pow(Inputs[i] - Weights[i], 2);
            }
            return Math.Sqrt(sum);
        }

        public string PrintWeights()
        {
            string weights = "";
            for (int i = 0; i < Weights.Length; i++)
            {
                weights += Weights[i].ToString() + " ";
            }
            return weights;
        }
    }
}
