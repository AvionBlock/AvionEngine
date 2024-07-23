using System.Collections.Generic;

namespace AvionEngine.Interfaces
{
    public interface IScene
    {
        List<EngineObject> EngineObjects { get; set; }
    }
}
