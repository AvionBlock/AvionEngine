using Arch.Core;
using AvionEngine.Rendering;
using Silk.NET.Maths;
using System.Numerics;
using Tester.Structures;

namespace Tester.Components
{
    public class CameraComponent
    {
        public BaseShader ProjectionShader;
        public Vector2D<int> AspectSize;

        private Vector3D<float> CameraUp;
        private float CameraYaw;
        private float CameraPitch;
        private float CameraZoom;
        private Vector2 LastMousePosition;

        public CameraComponent(BaseShader projectionShader)
        {
            ProjectionShader = projectionShader;
            CameraUp = Vector3D<float>.UnitY;
            CameraYaw = -90f;
            CameraPitch = 0f;
            CameraZoom = 60f;
        }

        public void Render(double delta, World world, ref TransformComponent transform)
        {
            var projectionShader = ProjectionShader;
            var query = new QueryDescription()
                .WithAll<MeshComponent<Vertex>, TransformComponent>();

            var front = Vector3D.Normalize(transform.EulerAngles);
            var view = Matrix4X4.CreateLookAt(transform.Position, transform.Position + front, CameraUp);
            //Note that the apsect ratio calculation must be performed as a float, otherwise integer division will be performed (truncating the result).
            var projection = Matrix4X4.CreatePerspectiveFieldOfView(CameraZoom * MathF.PI / 180f, AspectSize.X / (float)AspectSize.Y, 0.1f, 100.0f);

            world.Query(in query, (ref MeshComponent<Vertex> mesh, ref TransformComponent transform) =>
            {
                projectionShader.Render(delta);
                projectionShader.NativeShader.SetUniform4("model", transform.Model);
                projectionShader.NativeShader.SetUniform4("view", view);
                projectionShader.NativeShader.SetUniform4("projection", projection);
                mesh.Render(delta);
            });
        }

        public void UpdateLook(Vector2 position, ref TransformComponent transform)
        {
            var lookSensitivity = 0.1f;
            if (LastMousePosition == default)
            {
                LastMousePosition = position;
            }
            else
            {
                var xOffset = (position.X - LastMousePosition.X) * lookSensitivity;
                var yOffset = (position.Y - LastMousePosition.Y) * lookSensitivity;
                LastMousePosition = position;

                CameraYaw += xOffset;
                CameraPitch -= yOffset;

                //We don't want to be able to look behind us by going over our head or under our feet so make sure it stays within these bounds
                CameraPitch = Math.Clamp(CameraPitch, -89.0f, 89.0f);

                var direction = transform.EulerAngles;
                direction.X = MathF.Cos(CameraPitch * (MathF.PI / 180)) * MathF.Cos(CameraYaw * (MathF.PI / 180));
                direction.Y = MathF.Sin(CameraPitch * (MathF.PI / 180));
                direction.Z = MathF.Cos(CameraPitch * (MathF.PI / 180)) * MathF.Sin(CameraYaw * (MathF.PI / 180));
                transform.EulerAngles = direction;
            }
        }
    }
}
