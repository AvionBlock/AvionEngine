using Arch.Core;
using AvionEngine.OpenGL;
using AvionEngine.Rendering;
using Silk.NET.Windowing;
using System.Drawing;
using Tester;
using Tester.Structures;
using Tester.Components;
using Silk.NET.Maths;

string projFrag = @"#version 330 core
out vec4 out_color;

void main() {
	out_color = vec4(1.0,0.5,0.2,1.0);
}";
string projVert = @"#version 330 core
layout (location = 0) in vec3 aPosition; //vertex coordinates

//uniform variables
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main() {
	gl_Position = vec4(aPosition, 1.0) * model * view * projection; //Coordinates
}";

AvionEngine.AvionEngine engine;

var window = Window.Create(WindowOptions.Default);
window.Load += OnLoad;

window.Run();

void OnLoad()
{
    var renderer = new Renderer(window);
    renderer.ClearColor = Color.Aqua;
    engine = new AvionEngine.AvionEngine(renderer);
    var mesh = engine.Renderer.CreateMesh();
    mesh.Set([
        new Vertex(-0.5f, -0.5f, 0), new Vertex(0.5f, -0.5f, 0), new Vertex(0.5f, 0.5f, 0), new Vertex(-0.5f, 0.5f, 0)],
        [
            0,1,2,
            2,3,0
        ]);

    engine.World.Create(new TransformComponent<float>(), new CameraComponent(new ProjectionShader(renderer, projVert, projFrag)) { AspectSize = window.Size });
    engine.World.Create(new TransformComponent<float>() { Scale = new Vector3D<float>(1f, 1f, 0), Rotation = Quaternion<float>.CreateFromAxisAngle(new Vector3D<float>(0,0,1),45f * (MathF.PI / 180f))}, new MeshComponent() { Mesh = new BaseMesh(mesh) });

    window.Render += OnRender;

    void OnRender(double delta)
    {
        var query = new QueryDescription()
            .WithAny<CameraComponent>();

        engine.World.Query(in query, (ref CameraComponent camera) =>
        {
            camera.Render(delta, engine.World);
        });
    }
}