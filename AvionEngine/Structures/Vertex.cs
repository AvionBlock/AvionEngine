using System.Numerics;
namespace AvionEngine.Structures
{
    public struct Vertex
    {
        [VertexFieldType(typeof(float))]
        public Vector3 Position;
        /*
        [VertexFieldType(typeof(float))]
        public Vector3 Normal;
        [VertexFieldType(typeof(float))]
        public Vector2 TexLoc;
        */

        public Vertex(float x, float y, float z)
        {
            Position = new Vector3(x, y, z);
            /*
            Normal = new Vector3();
            TexLoc = new Vector2();
            */
        }
    }
}
