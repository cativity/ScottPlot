using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace ScottPlot.OpenGL.GLPrograms;

public abstract class GLProgramBase : IGLProgram
{
    private readonly int _handle;

    protected virtual string? VertexShaderSource => null;

    protected virtual string? GeometryShaderSource => null;

    protected virtual string? FragmentShaderSource => null;

    public GLProgramBase()
    {
        GLShader? vertexShader = new GLShader(ShaderType.VertexShader, VertexShaderSource);
        GLShader? geometryShader = new GLShader(ShaderType.GeometryShader, GeometryShaderSource);
        GLShader? fragmentShader = new GLShader(ShaderType.FragmentShader, FragmentShaderSource);

        _handle = GL.CreateProgram();

        vertexShader.AttachToProgram(_handle);
        geometryShader.AttachToProgram(_handle);
        fragmentShader.AttachToProgram(_handle);

        GL.LinkProgram(_handle);

        GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out int success);

        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(_handle);
            Debug.WriteLine(infoLog);
        }

        vertexShader.DetachFromProgram(_handle);
        geometryShader.DetachFromProgram(_handle);
        fragmentShader.DetachFromProgram(_handle);
    }

    public void Use()
    {
        GL.UseProgram(_handle);
    }

    public int GetAttribLocation(string attribName) => GL.GetAttribLocation(_handle, attribName);

    public int GetUniformLocation(string attribName) => GL.GetUniformLocation(_handle, attribName);

    private bool _disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            GL.DeleteProgram(_handle);

            _disposedValue = true;
        }
    }

    public void GLFinish() => GL.Finish();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
