using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

[ExecuteInEditMode]
public class GLSLFragmentConverter : EditorWindow
{

    string name = "ShaderName";
    string path = "Assets/CampCult/Shaders";
    string shaderPath = "Camp Cult/Generators";
    string glsl;

    [MenuItem("Camp Cult/GLSLFragmentConverter")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GLSLFragmentConverter));
    }
    Vector2 scroll;
    void OnGUI()
    {
        scroll = GUILayout.BeginScrollView(scroll);
        GUILayout.Label("GLSL Fragment -> Unity Shader", EditorStyles.boldLabel);
        name = EditorGUILayout.TextField("Name", name);
        path = EditorGUILayout.TextField("File Path", path);
        shaderPath = EditorGUILayout.TextField("Shader Path", shaderPath);
        glsl = EditorGUILayout.TextArea(glsl);
        if (GUILayout.Button("!CONVERT!"))
        {
            bool hasTex = glsl.IndexOf("iChannel0") != -1;

            string s = "Shader \"" + shaderPath + "/" + name + "\" {\n" +
                    "Properties {\n";
            if (hasTex)
                s += "_MainTex (\"Base (RGB)\", 2D) = \"white\" {}\n";
            s += " }\n" +
                "Category {\n" +
                "Blend SrcAlpha OneMinusSrcAlpha\n" +
                "Tags {\"Queue\"=\"Transparent\"}\n" +
                "SubShader {\n" +
                "Pass {\n" +
                "CGPROGRAM\n" +
                "#include \"UnityCG.cginc\"\n" +
                "#pragma vertex vert_img\n" +
                "#pragma fragment frag\n";
            if (hasTex)
            {
                s += "uniform sampler2D _MainTex;\n" +
                    "uniform float4 _MainTex_ST;\n";
            }
            s += Convert(glsl) + "\n" +
                "ENDCG\n" +
                "}\n" +
                "}\n" +
                "}\n" +
                "FallBack \"Unlit\"\n" +
                "}\n";
            Save(s);
        }
        GUILayout.EndScrollView();
    }

    string Convert(string s)
    {
        s = s.Replace("void mainImage( out vec4 fragColor, in vec2 fragCoord )", "fixed4 frag (v2f_img i) : COLOR");
        s = s.Replace("vec2 uv = fragCoord.xy / iResolution.xy", "float2 uv = i.uv.xy");
        s = s.Replace("iGlobalTime", "_Time.y");
        s = s.Replace("fragColor =", "return");
        s = s.Replace("vec", "float");
        s = s.Replace("texture2D", "tex2D");
        s = s.Replace("texture", "tex2D");
        s = s.Replace("iChannel0", "_MainTex");
        s = s.Replace("mat2", "float2x2");
        s = s.Replace("mat3", "float3x3");
        s = s.Replace("mat4", "float4x4");
        s = s.Replace("mod", "fmod");
        s = s.Replace("atan", "atan2");
        s = s.Replace("mix", "lerp");
        s = s.Replace("fract", "frac");

        return s;
    }

    void Save(string s)
    {
        string p = path + "/" + name + ".shader";
        bool write = false;
        if (File.Exists(p))
        {
            if (EditorUtility.DisplayDialog("overwrite?", "Hold up there buster, looks like a file exists at the specified path, \n\noverwrite?", "yes pls", "no thx"))
            {
                write = true;
            }
        }
        else
        {
            write = true;
        }

        if (!write)
            return;

        StreamWriter outFile = new StreamWriter(p);
        outFile.Write(s);
        outFile.Close();
        AssetDatabase.Refresh();
    }
}