﻿using Arch.Core;
using AvionEngine.OpenGL;
using AvionEngine.Rendering;
using Silk.NET.Windowing;
using System.Drawing;
using Tester;
using Tester.Structures;
using Tester.Components;
using Silk.NET.Maths;
using Silk.NET.Input;
using System.Numerics;

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
IKeyboard primaryKeyboard;

var window = Window.Create(WindowOptions.Default);
window.Load += OnLoad;

window.Run();

void OnLoad()
{
    var renderer = new Renderer(window);
    renderer.ClearColor = Color.Aqua;
    engine = new AvionEngine.AvionEngine(renderer);
    var mesh = engine.CreateMesh([
        new Vertex(-0.5f, -0.5f, 0), new Vertex(0.5f, -0.5f, 0), new Vertex(0.5f, 0.5f, 0)
        ],
        [
            0,1,2,
        ]);

    var camera = new CameraComponent(new ProjectionShader(renderer, projVert, projFrag)) { AspectSize = window.Size };
    engine.World.Create(new TransformComponent<float>(), camera);
    engine.World.Create(new TransformComponent<float>() { Position = new Vector3D<float>(0, 0f, 0f) }, new MeshComponent<Vertex>(mesh));

    IInputContext input = window.CreateInput();
    primaryKeyboard = input.Keyboards.FirstOrDefault();
    for (int i = 0; i < input.Mice.Count; i++)
    {
        input.Mice[i].Cursor.CursorMode = CursorMode.Raw;
        input.Mice[i].MouseMove += OnMouseMove;
    }

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

    void OnMouseMove(IMouse mouse, Vector2 position)
    {
        mesh.Set([
        new Vertex(-0.5f, -1f, 0), new Vertex(0.5f, -0.5f, 0), new Vertex(0.5f, 1f, 0)
        ],
        [
            0,1,2,
        ]);
        camera.UpdateLook(position);
    }
}