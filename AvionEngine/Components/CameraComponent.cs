using Arch.Core;
using AvionEngine.Rendering;
using Silk.NET.Maths;
using System;

namespace AvionEngine.Components
{
    public struct CameraComponent
    {
        public BaseShader ProjectionShader { get; set; }

        public void Render(double delta, World world)
        {
            var projectionShader = ProjectionShader;
            var query = new QueryDescription()
                .WithAll<MeshComponent>();

            world.Query(in query, (ref MeshComponent mesh) =>
            {
                projectionShader.Render(delta);
                //projectionShader.NativeShader.SetUniform4("model", Matrix4X4<float>.Identity);
                //NativeShader.SetUniform4(nameof(view), view);
                //NativeShader.SetUniform4(nameof(projection), projection);
                mesh.Render(delta);
            });
        }
    }
}
