using Arch.Core;
using AvionEngine.OpenGL;
using Silk.NET.Windowing;
using System.Drawing;
using Tester.Structures;
using Tester.Components;
using Silk.NET.Maths;
using Silk.NET.Input;
using System.Numerics;
using Arch.Core.Extensions;
using StbImageSharp;

string projFrag = @"#version 330 core
out vec4 FragColor;
  
in vec3 ourNormal;
in vec2 TexCoord;

uniform sampler2D ourTexture;

void main()
{
    FragColor = texture(ourTexture, TexCoord);
}";
string projVert = @"#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

out vec3 ourNormal;
out vec2 TexCoord;

void main()
{
    gl_Position = vec4(aPos, 1.0);
    ourNormal = aNormal;
    TexCoord = aTexCoord;
}
";

AvionEngine.AvionEngine engine;
IKeyboard primaryKeyboard;

var window = Window.Create(WindowOptions.Default);
var stream = File.OpenRead("./awesomeface.png");
window.Load += OnLoad;

window.Run();

void OnLoad()
{
    var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
    var renderer = new Renderer(window);
    renderer.SetClearColor(Color.Aqua);
    engine = new AvionEngine.AvionEngine(renderer);
    var img = renderer.CreateTexture(new AvionEngine.Structures.TextureInfo() { Data = image.Data, Height = (uint)image.Height, Width = (uint)image.Width }, formatMode: AvionEngine.Enums.TextureFormatMode.RGBA);
    var mesh = engine.CreateMesh<Vertex>(
        [new Vertex(0.5f, 0.5f, 0.0f) { TexLoc = new Vector2(1.0f, 1.0f) },
         new Vertex(0.5f, -0.5f, 0.0f) { TexLoc = new Vector2(1.0f, 0.0f) },
         new Vertex(-0.5f, -0.5f, 0.0f) { TexLoc = new Vector2(0.0f, 0.0f) },
         new Vertex(-0.5f, 0.5f, 0.0f) { TexLoc = new Vector2(0.0f, 1.0f) }], 
        [0,1,2,
        2,3,0],
        AvionEngine.Enums.UsageMode.Static);

    var camera = engine.World.Create(new TransformComponent() { Position = new Vector3D<float>(0,0,1), Rotation = Quaternion<float>.CreateFromAxisAngle(Vector3D<float>.UnitX, -90 * (MathF.PI / 180)) }, new CameraComponent(engine.CreateShader(projVert, projFrag)) { AspectSize = window.Size });
    engine.World.Create(new TransformComponent() { Position = new Vector3D<float>(0, 0f, 0f) }, new MeshComponent(mesh) { Texture = new AvionEngine.Rendering.BaseTexture(img) });

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
        camera.Get<CameraComponent>().Render(delta, engine.World, ref camera.Get<TransformComponent>());
    }

    void OnMouseMove(IMouse mouse, Vector2 position)
    {
        camera.Get<CameraComponent>().UpdateLook(position, ref camera.Get<TransformComponent>());
    }
}