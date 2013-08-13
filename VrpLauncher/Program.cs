using System;
using System.Globalization;
using System.IO;
using System.Threading;

using Temp;

using vrp;

namespace VrpLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var path = args[0];

            var data = VrpData.ReadData(path);

            var solver = new VrpClusteringMethod();

            var result = solver.Solve(data);

            Console.WriteLine("{0} 0", result.Dist);
            for (int i = 0; i < data.V; i++)
            {
                Console.WriteLine(string.Join(" ", result.Routes[i]));
            }
        }
    }
}
