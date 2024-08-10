using AvionEngine.Enums;
using AvionEngine.Structures;
using System.Numerics;
namespace Tester.Structures
{
    public struct Vertex
    {
        [VertexField(FieldType.Single)]
        public Vector3 Position;
        [VertexField(FieldType.Single)]
        public Vector3 Normal;
        [VertexField(FieldType.Single)]
        public Vector2 TexLoc;

        public Vertex(float x, float y, float z)
        {
            Position = new Vector3(x, y, z);
            Normal = new Vector3();
            TexLoc = new Vector2();
        }
    }
}
