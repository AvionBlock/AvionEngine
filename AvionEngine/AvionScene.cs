using AvionEngine.Interfaces;
using System.Collections.Generic;

namespace AvionEngine
{
    public class AvionScene : IScene
    {
        public List<EngineObject> EngineObjects { get; set; } = new List<EngineObject>();
    }
}