using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace ScottPlot.OpenGL.GLPrograms;

public class GLShader
{
    private readonly int _handle;
    private bool _active;

    public GLShader(ShaderType type, string? sourceCode)
    {
        if (sourceCode is null)
        {
            return;
        }

        _handle = GL.CreateShader(type);
        _active = true;
        Compile(sourceCode);
    }

    public void AttachToProgram(int programHandle)
    {
        if (!_active)
        {
            return;
        }

        GL.AttachShader(programHandle, _handle);
    }

    public void DetachFromProgram(int programHandle)
    {
        if (!_active)
        {
            return;
        }

        GL.DetachShader(programHandle, _handle);
        GL.DeleteShader(_handle);
        _active = false;
    }

    private void Compile(string sourceCode)
    {
        GL.ShaderSource(_handle, sourceCode);
        GL.CompileShader(_handle);
        GL.GetShader(_handle, ShaderParameter.CompileStatus, out int successVertex);

        if (successVertex == 0)
        {
            string infoLog = GL.GetShaderInfoLog(_handle);
            Debug.WriteLine(infoLog);
            _active = false;
        }
    }
}
