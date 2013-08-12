namespace vrp
{
    public abstract class VrpSolverBase : IVrpSolver
    {
        protected void CalcTotalDist(VrpData data, VrpResult result, double totalDist)
        {
            foreach (var route in result.Routes)
            {
                for (int i = 0; i < route.Count - 1; i++)
                {
                    totalDist += data.Customers[route[i]].Point.Dist(data.Customers[route[i + 1]].Point);
                }
            }

            result.Dist = totalDist;
        }

        public abstract VrpResult Solve(VrpData data);
    }
}