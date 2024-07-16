namespace AvionEngine.Interfaces
{
    public interface IRenderer
    {
        IShader CreateShader(string vertexCode, string fragmentCode);
        void AddShader(IShader shader);
    }
}
