using Silk.NET.Maths;

namespace AvionEngine.Components
{
    public struct TransformComponent<TPosition, TRotation, TScale>
        where TPosition : unmanaged, System.IFormattable, System.IEquatable<TPosition>, System.IComparable<TPosition>
        where TRotation : unmanaged, System.IFormattable, System.IEquatable<TRotation>, System.IComparable<TRotation>
        where TScale : unmanaged, System.IFormattable, System.IEquatable<TScale>, System.IComparable<TScale>
    {
        public Vector3D<TPosition> Position;
        public Vector3D<TRotation> Rotation;
        public Vector3D<TScale> Scale;
    }
}
