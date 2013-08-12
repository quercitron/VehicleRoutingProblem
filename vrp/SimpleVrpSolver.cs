using System.Collections.Generic;
using System.Linq;

using TravellingSalesmanProblem;

namespace vrp
{
    public class SimpleVrpSolver : VrpSolverBase 
    {
        public override VrpResult Solve(VrpData data)
        {
            var sortedCustomers = data.Customers.OrderByDescending(c => c.Demand).ToArray();

            var result = new VrpResult();
            result.Routes = new List<int>[data.V];
            var n = data.N;
            var v = data.V;
            var used = new bool[n];
            used[0] = true;

            var totalDist = 0D;


            var tspSolver = new Opt2();

            for (int i = 0; i < v; i++)
            {
                var route = new List<int> { 0 };

                var cap = data.C;
                foreach (var customer in sortedCustomers.Where(c => !used[c.Id]).ToList())
                {
                    if (customer.Demand <= cap)
                    {
                        cap -= customer.Demand;
                        route.Add(customer.Id);
                        used[customer.Id] = true;
                    }
                }

                if (route.Count > 3)
                {
                    var measure = new MatrixMeasureFactory().CreateMatrixMeasure(route.Select(x => data.Customers[x].Point).ToArray());
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
                }
                else
                {
                    result.Routes[i] = route;
                }

                result.Routes[i].Add(0);
            }

            CalcTotalDist(data, result, totalDist);

            return result;
        }
    }
}
