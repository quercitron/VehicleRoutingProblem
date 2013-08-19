using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Temp;
using Warehouse;
using Warehouse = Warehouse.Warehouse;

namespace vrp
{
    public class VrpClusteringMethod : VrpSolverBase
    {
        //private const int BaseTimelimit = 3 * 1000;
        private const int BaseTimelimit = 290 * 60 * 1000;
        private const double K = 0.4;

        public override VrpResult Solve(VrpData data)
        {
            VrpResult bestResult = null;
            var timelimit = BaseTimelimit;
            var stopwatch = Stopwatch.StartNew();

            /*var rnd = new Random();

            var pr = new double[data.V];
            for (int i = 0; i < data.V; i++)
            {
                pr[i] = 1D / (data.V - 1);
            }
            pr[0] = 0;*/

            while (true)
            {
                stopwatch.Stop();
                if (bestResult != null && (bestResult.Dist < 1400 || stopwatch.ElapsedMilliseconds > timelimit))
                {
                    break;
                }
                stopwatch.Start();

                /*var r = rnd.NextDouble() * pr.Sum();
                var sum = 0D;
                var v = 2;
                for (int i = 0; i < data.V; i++)
                {
                    sum += pr[i];
                    if (sum > r)
                    {
                        v = i + 1;
                        break;
                    }
                }*/
                var v = data.V;

                var baseClusters = GetKmeanClusters(data, v);

                var found = UpdateClustersBurn(data, baseClusters, v);

                if (!found)
                {
                    //pr[v - 1] /= 2;
                    continue;
                }

                var result = new VrpResult();
                result.Routes = new List<int>[data.V];
                for (int i = 0; i < data.V; i++)
                {
                    result.Routes[i] = new List<int> { 0 };
                }
                for (int i = 1; i < data.N; i++)
                {
                    result.Routes[baseClusters.Color[i]].Add(i);
                }
                for (int i = 0; i < data.V; i++)
                {
                    result.Routes[i].Add(0);
                }

                ApplyTsp(data, result);
                CalcTotalDist(data, result);

                if (bestResult == null || bestResult.Dist > result.Dist)
                {
                    //pr[v - 1] *= (1 << (data.V - v));
                    bestResult = result;
                }
            }

            return bestResult;
        }

        private bool UpdateClustersMIP(VrpData data, ClustersModel baseClusters, int v)
        {
            throw new NotImplementedException();
        }

        private bool UpdateClustersWarehouse(VrpData data, ClustersModel clusters, int v)
        {
            var solver = new GreedyHeuristic();

            var warehouseData = new WarehouseInputData();
            warehouseData.N = v;
            warehouseData.M = data.N;
            warehouseData.T = new double[data.N,v];
            for (int i = 0; i < data.N; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    warehouseData.T[i, j] = clusters.Centers[j].Dist(data.Customers[i].Point);
                }
            }
            warehouseData.Consumers = data.Customers.Select(c => new Consumer {Demand = c.Demand, Id = c.Id}).ToArray();
            warehouseData.Warehouses = new global::Warehouse.Warehouse[v];
            for (int i = 0; i < v; i++)
            {
                warehouseData.Warehouses[i] = new global::Warehouse.Warehouse {Cap = data.C, Id = i, S = 0};
            }
            var result = solver.Solve(warehouseData);
            if (result.SolutionFound)
            {
                clusters.Color = result.Solution;
                return true;
            }
            return false;
        }

        // burn
        private static bool UpdateClustersBurn(VrpData data, ClustersModel baseClusters, int v)
        {
            var rnd = new Random();

            var found = false;

            for (int count = 0; count < 10; count++)
            {
                var clusters = new ClustersModel();
                clusters.Color = new int[data.N];
                Array.Copy(baseClusters.Color, clusters.Color, data.N);
                clusters.Count = new int[v];
                Array.Copy(baseClusters.Count, clusters.Count, v);
                clusters.Centers = new Point2DReal[v];
                for (int i = 0; i < v; i++)
                {
                    clusters.Centers[i] = new Point2DReal(baseClusters.Centers[i]);
                }

                var w = new int[v];
                for (int i = 0; i < data.N; i++)
                {
                    w[clusters.Color[i]] += data.Customers[i].Demand;
                }

                var baseOverweight = w.Sum(x => x > data.C ? x - data.C : 0);
                var overweight = baseOverweight;

                var radiuses = new double[v];
                for (int i = 0; i < data.N; i++)
                {
                    var color = clusters.Color[i];
                    var dist = data.Customers[i].Point.Dist(clusters.Centers[color]);
                    if (dist > radiuses[color])
                    {
                        radiuses[color] = dist;
                    }
                }
                var baseRadius = radiuses.Sum();
                var radius = baseRadius;

                var E = 1D;
                var minE = double.MaxValue;

                var bestColors = new int[data.N];
                var bestW = new int[v];
                for (int k = 0; k < 1000; k++)
                {
                    if (E < minE)
                    {
                        minE = E;
                        Array.Copy(clusters.Color, bestColors, data.N);
                        Array.Copy(w, bestW, v);
                    }

                    var id = rnd.Next(data.N);
                    var weight = data.Customers[id].Demand;
                    var newColor = rnd.Next(v - 1);
                    var oldColor = clusters.Color[id];
                    if (oldColor <= newColor)
                    {
                        newColor++;
                    }
                    var weightDelta = -(w[newColor] + weight > data.C ? w[newColor] + weight - data.C : 0)
                                      + (w[newColor] > data.C ? w[newColor] - data.C : 0)
                                      - (w[oldColor] - weight > data.C ? w[oldColor] - weight - data.C : 0)
                                      + (w[oldColor] > data.C ? w[oldColor] - data.C : 0);
                    var newOverweight = overweight - weightDelta;

                    var point = data.Customers[id].Point;
                    var newCenter = (clusters.Centers[newColor] * clusters.Count[newColor] + point)
                                    * (1D / (clusters.Count[newColor] + 1));
                    var newDist = point.Dist(newCenter);
                    var newRadius = radius;
                    if (newDist > radiuses[newColor])
                    {
                        newRadius += newDist - radiuses[newColor];
                    }

                    var newE = (double) newOverweight / baseOverweight + K * (newRadius - baseRadius) / baseRadius;

                    var alpha = rnd.NextDouble();

                    var T = 10 / (k + 1);
                    var h = Math.Exp(-(newE - E) / T);
                    if (alpha < h)
                    {
                        E = newE;
                        overweight = newOverweight;
                        radius = newRadius;
                        w[newColor] += weight;
                        w[oldColor] -= weight;
                        clusters.Color[id] = newColor;
                        clusters.Centers[newColor] = newCenter;
                        clusters.Centers[oldColor] = (clusters.Centers[oldColor] * clusters.Count[oldColor] - point)
                                                     * (1D / (clusters.Count[oldColor] - 1));
                        clusters.Count[newColor]++;
                        clusters.Count[oldColor]--;
                        if (newDist > radiuses[newColor])
                        {
                            radiuses[newColor] = newDist;
                        }
                    }
                }


                if (bestW.All(x => x <= data.C))
                {
                    found = true;
                    baseClusters.Color = bestColors;
                    break;
                }
            }
            return found;
        }

        public ClustersModel GetKmeanClusters(VrpData data, int clustersCount)
        {
            var centers = new Point2DReal[clustersCount];
            var perm = Permutations.GetRandomPermutation(data.N - 1);
            for (int i = 0; i < clustersCount; i++)
            {
                centers[i] = data.Customers[perm[i] + 1].Point;
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
