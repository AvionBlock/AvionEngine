using Silk.NET.Maths;

namespace Tester.Components
{
    public struct TransformComponent<T> where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
    {
        public Vector3D<T> Position;
        public Quaternion<T> Rotation;
        public Vector3D<T> Scale;

        public TransformComponent()
        {
            Position = Vector3D<T>.Zero;
            Rotation = Quaternion<T>.Identity;
            Scale = Vector3D<T>.One;
        }

        public Matrix4X4<T> Model => Matrix4X4.CreateScale(Scale) * Matrix4X4.CreateFromQuaternion(Rotation) * Matrix4X4.CreateTranslation(Position);
    }
}
