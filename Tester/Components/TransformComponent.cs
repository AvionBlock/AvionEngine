using Silk.NET.Maths;
using System.Numerics;

namespace Tester.Components
{
    public struct TransformComponent
    {
        public Vector3D<float> Position;
        public Quaternion<float> Rotation;
        public Vector3D<float> Scale;

        //Credits: https://stackoverflow.com/questions/70462758/c-sharp-how-to-convert-quaternions-to-euler-angles-xyz
        public Vector3D<float> EulerAngles
        { 
            get
            {
                var angles = new Vector3D<float>();

                // roll (x-axis rotation)
                float sinr_cosp = 2 * (Rotation.W * Rotation.X + Rotation.Y * Rotation.Z);
                float cosr_cosp = 1 - 2 * (Rotation.X * Rotation.X + Rotation.Y * Rotation.Y);
                angles.X = MathF.Atan2(sinr_cosp, cosr_cosp);

                // pitch (y-axis rotation)
                float sinp = 2 * (Rotation.W * Rotation.Y - Rotation.Z * Rotation.X);
                if (Math.Abs(sinp) >= 1)
                {
                    angles.Y = MathF.CopySign(MathF.PI / 2, sinp);
                }
                else
                {
                    angles.Y = MathF.Asin(sinp);
                }

                // yaw (z-axis rotation)
                float siny_cosp = 2 * (Rotation.W * Rotation.Z + Rotation.X * Rotation.Y);
                float cosy_cosp = 1 - 2 * (Rotation.Y * Rotation.Y + Rotation.Z * Rotation.Z);
                angles.Z = MathF.Atan2(siny_cosp, cosy_cosp);

                return angles;
            }
            set
            {
                float cy = MathF.Cos(value.Z * 0.5f);
                float sy = MathF.Sin(value.Z * 0.5f);
                float cp = MathF.Cos(value.Y * 0.5f);
                float sp = MathF.Sin(value.Y * 0.5f);
                float cr = MathF.Cos(value.X * 0.5f);
                float sr = MathF.Sin(value.X * 0.5f);

                Rotation.W = cr * cp * cy + sr * sp * sy;
                Rotation.X = sr * cp * cy - cr * sp * sy;
                Rotation.Y = cr * sp * cy + sr * cp * sy;
                Rotation.Z = cr * cp * sy - sr * sp * cy;
            }
        }

        public TransformComponent()
        {
            Position = Vector3D<float>.Zero;
            Rotation = Quaternion<float>.Identity;
            Scale = Vector3D<float>.One;
        }

        public Matrix4X4<float> Model => Matrix4X4.CreateScale(Scale) * Matrix4X4.CreateFromQuaternion(Rotation) * Matrix4X4.CreateTranslation(Position);
    }
}
