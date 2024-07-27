using Arch.Core;
using AvionEngine.Components;
using AvionEngine.OpenGL;
using Silk.NET.Windowing;
using System.Drawing;
using Tester;

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
    renderer.ClearColor = Color.Blue;
    engine = new AvionEngine.AvionEngine(renderer);

    var entity = engine.World.Create(new TransformComponent<float, float, float>(), new CameraComponent() { ProjectionShader = new ProjectionShader(renderer, projVert, projFrag)});

    window.Render += OnRender;

    void OnRender(double delta)
    {
        var query = new QueryDescription()
            .WithAny<CameraComponent>();

        engine.World.Query(in query, (ref CameraComponent camera) =>
        {
            camera.Render(delta);
        });
    }
}