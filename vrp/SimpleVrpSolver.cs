using System.Collections.Generic;
using System.Linq;

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

                route.Add(0);
                result.Routes[i] = route;
            }

            ApplyTsp(data, result);

            CalcTotalDist(data, result);

            return result;
        }
    }
}
