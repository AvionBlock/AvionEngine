namespace AvionEngine.Structures
{
    public abstract class Component
    {
        public EngineObject? Object { get; set; }

        public virtual void FixedUpdate()
        { }

        public virtual void Update()
        { }

        public virtual void Render()
        { }
    }
}