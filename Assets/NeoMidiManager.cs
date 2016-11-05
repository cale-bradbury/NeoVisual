using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class NeoMidiManager : MonoBehaviour {
    public List<MidiStack> stacks = new List<MidiStack>();
    
    string path ;
	// Use this for initialization
	void Start ()
    {
        path = Application.dataPath + "/neomidi.dat";
        if (File.Exists(path))
            Load();
        LinkStacks();
        for (int i = 0; i<stacks.Count; i++)
        {
            Messenger.AddListener<float>(stacks[i].eventName, stacks[i].OnEvent);
            stacks[i].OnEvent(stacks[i].value);
        }
	}

    void OnDestroy()
    {
        Save();
    }

    public void Save()
    {
        string save = "";
        for (int i = 0; i < stacks.Count; i++)
            save += stacks[i].value + "\n";
        File.WriteAllText(path, save);
        Debug.Log("wrote neomidi to " + path);
    }
    public void Load()
    {
        string[] save = File.ReadAllText(path).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries );
        for (int i = 0; i < Mathf.Min(stacks.Count, save.Length); i++)
            stacks[i].value = float.Parse(save[i]);
    }

    public void LinkStacks()
    {
        foreach(MidiStack s in stacks)
        {
            foreach (MidiModule m in s.modules)
                m.stack = s;
        }
    }

    public void SwapStacks(int i, int j)
    {
        MidiStack s = stacks[i];
        stacks[i] = stacks[j];
        stacks[j] = s;
    }
}

[System.Serializable]
public class MidiStack
{
    public NeoMidiManager manager;
    public string friendlyName = "";
    public string eventName = "";
    public List<MidiModule> modules = new List<MidiModule>();
    public float value = 0;
    public int index;

    public MidiStack(NeoMidiManager manager)
    {
        this.manager = manager;
    }

    public void OnEvent(float f)
    {
        value = f;
        for (int i = 0; i < modules.Count; i++)
        {
            modules[i].OnEvent();
        }
    }

    public void OnGUI(Rect r)
    {
        GUI.Label(new Rect(r.x, r.y, r.width * .5f, 20), "name");
        friendlyName = GUI.TextField(new Rect(r.x+ r.width * .5f, r.y, r.width*.5f, 20), friendlyName);
        r.y += 25;
        GUI.Label(new Rect(r.x, r.y, r.width * .5f, 20), "event");
        eventName = GUI.TextField(new Rect(r.x+ r.width * .5f, r.y, r.width * .5f, 20), eventName);
        r.y += 25;
        index = manager.stacks.IndexOf(this);
        if (GUI.Button(new Rect(r.x,r.y,r.width*.5f, 20), "<-") && index != 0)
            manager.SwapStacks(index, index - 1);
        if (GUI.Button(new Rect(r.x+r.width*.5f, r.y, r.width * .5f, 20), "->") && index != manager.stacks.Count - 1)
            manager.SwapStacks(index, index + 1);

        r.y += 25;
        Rect moduleRect = new Rect(r.x, r.y, r.width, 20);
        for (int i = 0; i < modules.Count; i++)
        {
            moduleRect.y += modules[i].OnGUI(moduleRect);
        }
        moduleRect.height = 15;
        moduleRect.width *= .5f;

        MidiModule.MidiModuleType moduleType = (MidiModule.MidiModuleType)UnityEditor.EditorGUI.EnumPopup(moduleRect, MidiModule.MidiModuleType.AddModule);
        if(moduleType!= MidiModule.MidiModuleType.AddModule) { 
            modules.Add(new MidiModule(moduleType, this));
            modules[modules.Count - 1].index = modules.Count - 1;
        }

        moduleRect.x += moduleRect.width;
        if (GUI.Button(moduleRect, "Remove Stack"))
        {
            if (manager == null)
                manager = GameObject.FindObjectOfType<NeoMidiManager>();
            manager.stacks.Remove(this);
            for (int i = 0; i < manager.stacks.Count; i++)
            {
                manager.stacks[i].index = i;
            }
        }

    }
}
[System.Serializable]
public class MidiModule 
{

    public enum MidiModuleType {
        AddModule,
        ReflectFloat,
        ReflectGradient
    }
    public MidiModuleType type;
    [NonSerialized]
    public MidiStack stack;
    Func<Rect, MidiModule, float> guiAction;
    Action<MidiModule> callAction;
    float editorLastHeight = 10;
    public int index;

    public bool hasCurve = true;
    public AnimationCurve curve = new AnimationCurve();
    public Vector2 minmax = new Vector2(0, 1);
    public Gradient gradient;
    public CCReflectFloat floatOutput = new CCReflectFloat();
    public CCReflectColor colorOutput = new CCReflectColor();

    public MidiModule(MidiModuleType type, MidiStack stack) 
    {
        this.type = type;
        this.stack = stack;
        GetActions(); 
    }

    public void GetActions()
    {
        if(type== MidiModuleType.ReflectFloat)
        {
            guiAction = ReflectFloatDraw;
            callAction = ReflectFloatCall;
        }
        else if (type == MidiModuleType.ReflectGradient)
        {
            guiAction = ReflectGradientDraw;
            callAction = ReflectGradientCall;
        }
        else if (type == MidiModuleType.AddModule)
        {
            guiAction = ReflectFloatDraw;
            callAction = ReflectFloatCall;
        }
    }

    public void OnEvent()
    {
        Debug.Log(callAction);
        if (callAction == null)
            GetActions();
        callAction(this);
    }

    public float OnGUI(Rect r)
    {
        if (guiAction == null)
            GetActions();
        if (stack == null)
            GameObject.FindObjectOfType<NeoMidiManager>().LinkStacks();

        r.height = editorLastHeight;
        GUIHelper.DrawRect(r, Color.gray);
        r.x += 2;
        r.y += 2;
        r.width -= 4;
        float f = guiAction(r, this);
        editorLastHeight = f+4; 
        return f+10;
    }

    static bool DrawHeader(ref Rect r, MidiModule m, string text)
    {
        r.height = 15;
        GUI.Label(new Rect(r.x, r.y, r.width - 20, 20),text);
        if (GUI.Button(new Rect(r.x + r.width - 15, r.y, 15, 15), "x"))
        {
            m.stack.modules.Remove(m);
            for (int i = 0; i < m.stack.modules.Count; i++)
            {
                m.stack.modules[i].index = i;
            }
            return true;
        }
        r.y += 20;
        return false;
    }

    static float ReflectFloatDraw(Rect r, MidiModule m)
    {
        float startHeight = r.y;
        if (DrawHeader(ref r, m, "Float Reflect"))
            return 0;

        m.hasCurve = GUI.Toggle(r, m.hasCurve, "hasCurve");
        if (m.hasCurve)
        {
            r.y += 20;
            m.curve = UnityEditor.EditorGUI.CurveField(r, m.curve);
        }
        r.y += 20;
        m.minmax = UnityEditor.EditorGUI.Vector2Field(r, "min/max", m.minmax);
        r.y += 40;
        m.floatOutput.Draw(r);

        return r.y - startHeight + 20;
    }

    static void ReflectFloatCall(MidiModule m)
    {
        float f = m.stack.value;
        if (m.hasCurve)
            f = m.curve.Evaluate(f);
        m.floatOutput.SetValue(Mathf.Lerp(m.minmax.x, m.minmax.y,f));
    }
    
    static float ReflectGradientDraw(Rect r, MidiModule m)
    {
        float startHeight = r.y;
        if (DrawHeader(ref r, m, "Gradient Reflect"))
            return 0;
        UnityEditor.SerializedObject so = new UnityEditor.SerializedObject(m.stack.manager);
        UnityEditor.SerializedProperty gradient = so.FindProperty("stacks").GetArrayElementAtIndex(m.stack.index).FindPropertyRelative("modules").GetArrayElementAtIndex(m.index).FindPropertyRelative("gradient");
        UnityEditor.EditorGUI.BeginChangeCheck();
        UnityEditor.EditorGUI.PropertyField(r, gradient, true);
        if (UnityEditor.EditorGUI.EndChangeCheck())
            so.ApplyModifiedProperties();
        
        r.y += 20;
        m.colorOutput.Draw(r);

        return r.y - startHeight + 20;
    }

    static void ReflectGradientCall(MidiModule m)
    {
        m.colorOutput.SetValue(m.gradient.Evaluate(m.stack.value));
    }
}
