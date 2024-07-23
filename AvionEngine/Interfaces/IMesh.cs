namespace AvionEngine.Interfaces
{
    public interface IMesh : IRenderable
    {
        uint[] Indices { get; }
    }
}