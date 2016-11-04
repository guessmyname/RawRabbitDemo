namespace SET.IR.Worker.Core
{
    public interface IDetailedContext
    {
        string Exchange { get; set; }
        string RoutingKey { get; set; }
    }
}