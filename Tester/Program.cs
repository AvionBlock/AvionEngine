using Arch.Core;
using AvionEngine.Components;
using AvionEngine.OpenGL;
using AvionEngine.Rendering;
using AvionEngine.Structures;
using Silk.NET.Windowing;
using System.Drawing;
using Tester;

/*
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
*/
const string projVert = @"
#version 330 core

layout (location = 0) in vec3 aPosition;

void main()
{
    gl_Position = vec4(aPosition, 1.0);
}";
const string projFrag = @"
#version 330 core

out vec4 out_color;

void main()
{
    out_color = vec4(1.0, 0.5, 0.2, 1.0);
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
    mesh.Set([new Vertex(-0.5f, 0, 0), new Vertex(0.5f, 0, 0), new Vertex(0, 0.5f, 0)], [0,1,2]);

    engine.World.Create(new TransformComponent<float, float, float>(), new CameraComponent() { ProjectionShader = new ProjectionShader(renderer, projVert, projFrag)});
    engine.World.Create(new MeshComponent() { Mesh = new BaseMesh(mesh)});

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