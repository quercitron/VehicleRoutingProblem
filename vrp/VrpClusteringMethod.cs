using System;
using System.Linq;

using Temp;

namespace vrp
{
    public class VrpClusteringMethod : VrpSolverBase
    {
        public override VrpResult Solve(VrpData data)
        {
            var clusters = GetKmeanClusters(data, data.V);

            // burn
            var w = new int[data.V];
            for (int i = 0; i < data.N; i++)
            {
                w[clusters.Color[i]] += data.Customers[clusters.Color[i]].Demand;
            }

            throw new NotImplementedException();

            while (w.Any(x => x > data.C))
            {
                
            }
        }

        public ClustersModel GetKmeanClusters(VrpData data, int clustersCount)
        {
            var centers = new Point2DReal[clustersCount];
            for (int i = 0; i < clustersCount; i++)
            {
                centers[i] = data.Customers[(i + 1) % data.N].Point;
            }

            var colors = new int[data.N];
            var count = new int[clustersCount];

            var changed = true;
            while (changed)
            {
                changed = false;
                count = new int[clustersCount];
                for (int i = 1; i < data.N; i++)
                {
                    var bestCenter = -1;
                    var bestDist = 0D;
                    for (int j = 0; j < clustersCount; j++)
                    {
                        if (bestCenter == -1 || centers[j].Dist(data.Customers[i].Point) < bestDist)
                        {
                            bestCenter = j;
                            bestDist = centers[j].Dist(data.Customers[i].Point);
                        }
                    }
                    if (colors[i] != bestCenter)
                    {
                        colors[i] = bestCenter;
                        changed = true;
                    }
                }
                for (int j = 0; j < clustersCount; j++)
                {
                    var center = new Point2DReal(0, 0);
                    for (int i = 1; i < data.N; i++)
                    {
                        if (colors[i] == j)
                        {
                            center = center + data.Customers[i].Point;
                            count[j]++;
                        }
                    }
                    centers[j] = center * (1D / count[j]);
                }
            }

            var result = new ClustersModel();
            result.Color = colors;
            result.Count = count;
            result.Centers = centers;
            return result;
        }
    }
}
