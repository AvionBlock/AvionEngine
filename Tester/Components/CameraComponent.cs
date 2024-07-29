using Arch.Core;
using AvionEngine.Rendering;
using Silk.NET.Maths;

namespace Tester.Components
{
    public struct CameraComponent
    {
        public BaseShader ProjectionShader;
        public Vector2D<int> AspectSize;

        private Vector3D<float> CameraPosition;
        private Vector3D<float> CameraFront;
        private Vector3D<float> CameraUp;
        private Vector3D<float> CameraDirection;
        private float CameraYaw;
        private float CameraPitch;
        private float CameraZoom;

        public CameraComponent(BaseShader projectionShader)
        {
            ProjectionShader = projectionShader;
            CameraPosition = new Vector3D<float>(0.0f, 0.0f, 2.0f);
            CameraFront = new Vector3D<float>(0.0f, 0.0f, -1.0f);
            CameraUp = Vector3D<float>.UnitY;
            CameraDirection = Vector3D<float>.Zero;
            CameraYaw = -90f;
            CameraPitch = 0f;
            CameraZoom = 60f;
        }

        public void Render(double delta, World world)
        {
            CameraDirection.X = MathF.Cos(CameraPitch * (MathF.PI / 180)) * MathF.Cos(CameraYaw * (MathF.PI / 180));
            CameraDirection.Y = MathF.Sin(CameraPitch * (MathF.PI / 180));
            CameraDirection.Z = MathF.Cos(CameraPitch * (MathF.PI / 180)) * MathF.Sin(CameraYaw * (MathF.PI / 180));
            CameraFront = Vector3D.Normalize(CameraDirection);

            var projectionShader = ProjectionShader;
            var query = new QueryDescription()
                .WithAll<MeshComponent, TransformComponent<float>>();

            var view = Matrix4X4.CreateLookAt(CameraPosition, CameraPosition + CameraFront, CameraUp);
            //Note that the apsect ratio calculation must be performed as a float, otherwise integer division will be performed (truncating the result).
            var projection = Matrix4X4.CreatePerspectiveFieldOfView(CameraZoom * MathF.PI / 180f, AspectSize.X / (float)AspectSize.Y, 0.1f, 100.0f);

            world.Query(in query, (ref MeshComponent mesh, ref TransformComponent<float> transform) =>
            {
                projectionShader.Render(delta);
                projectionShader.NativeShader.SetUniform4("model", transform.Model);
                projectionShader.NativeShader.SetUniform4("view", view);
                projectionShader.NativeShader.SetUniform4("projection", projection);
                mesh.Render(delta);
            });
        }
    }
}
