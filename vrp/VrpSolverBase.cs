using System.Collections.Generic;
using System.Linq;

using TravellingSalesmanProblem;

namespace vrp
{
    public abstract class VrpSolverBase : IVrpSolver
    {
        protected void CalcTotalDist(VrpData data, VrpResult result)
        {
            double totalDist = 0;
            foreach (var route in result.Routes)
            {
                for (int i = 0; i < route.Count - 1; i++)
                {
                    totalDist += data.Customers[route[i]].Point.Dist(data.Customers[route[i + 1]].Point);
                }
            }

            result.Dist = totalDist;
        }

        protected static void ApplyTsp(VrpData data, VrpResult result)
        {
            var tspSolver = new Opt2();
            for (int i = 0; i < data.V; i++)
            {
                var route = result.Routes[i];
                if (route.Count > 4)
                {
                    route.RemoveAt(route.Count - 1);
                    var measure =
                        new MatrixMeasureFactory().CreateMatrixMeasure(route.Select(x => data.Customers[x].Point).ToArray());
                    var tspRoute = tspSolver.GetPath(route.Count, measure);

                    int startIndex = 0;
                    for (int j = 0; j < tspRoute.Length; j++)
                    {
                        if (tspRoute[j] == 0)
                        {
                            startIndex = j;
                            break;
                        }
                    }
                    result.Routes[i] = new List<int>();
                    for (int j = startIndex; j < tspRoute.Length; j++)
                    {
                        result.Routes[i].Add(route[tspRoute[j]]);
                    }
                    for (int j = 0; j < startIndex; j++)
                    {
                        result.Routes[i].Add(route[tspRoute[j]]);
                    }
                    result.Routes[i].Add(0);
                }
                else
                {
                    result.Routes[i] = route;
                }
            }
        }

        public abstract VrpResult Solve(VrpData data);
    }
}