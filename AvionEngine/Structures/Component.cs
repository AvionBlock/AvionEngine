namespace AvionEngine.Structures
{
    public abstract class Component
    {
        public EngineObject? Object { get; set; }

        public virtual void FixedUpdate()
        { }

        public virtual void Update(double delta)
        { }

        public virtual void Render(double delta)
        { }
    }
}