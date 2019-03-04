using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KohonenImageProcessing
{
    public class Result
    {
        public int[] NeuronIndexes { get; set; }
        public double[] Lenghts { get; set; }
        public Dictionary<int, double[]> IndexToWeights { get; set; }
        public int FrameSize { get; set; }

        public Result(int totalFrames)
        {
            IndexToWeights = new Dictionary<int, double[]>();
            NeuronIndexes = new int[totalFrames];
            Lenghts = new double[totalFrames];
        }
    }
}
