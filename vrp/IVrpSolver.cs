namespace vrp
{
    public interface IVrpSolver
    {
        VrpResult Solve(VrpData data);
    }
}
