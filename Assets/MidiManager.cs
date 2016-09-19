using UnityEngine;
using System;
using System.Collections.Generic;

public class MidiManager : MonoBehaviour {
    public List<MidiStack> stacks = new List<MidiStack>();

	// Use this for initialization
	void Start () {
        LinkStacks();
        for (int i = 0; i<stacks.Count; i++)
        {
            Messenger.AddListener<float>(stacks[i].eventName, stacks[i].OnEvent);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        Messenger.Broadcast<float>("k0", Input.mousePosition.x / Screen.width);
        Messenger.Broadcast<float>("k1", Input.mousePosition.y / Screen.height);
    }

    public void LinkStacks()
    {
        foreach(MidiStack s in stacks)
        {
            foreach (MidiModule m in s.modules)
                m.stack = s;
        }
    }
}

[System.Serializable]
public class MidiStack
{
    public MidiManager manager;
    public string friendlyName = "";
    public string eventName = "";
    public List<MidiModule> modules = new List<MidiModule>();
    public float value = 0;

    public MidiStack(MidiManager manager)
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
        Rect moduleRect = new Rect(r.x, r.y, r.width, 20);
        for (int i = 0; i < modules.Count; i++)
        {
            moduleRect.y += modules[i].OnGUI(moduleRect);
        }
        moduleRect.height = 20;
        moduleRect.width *= .5f;
        if (GUI.Button(moduleRect, "Add Module"))
        {
            modules.Add(new MidiModule(MidiModule.MidiModuleType.ReflectFloat, this));
        }
        moduleRect.x += moduleRect.width;
        if (GUI.Button(moduleRect, "Remove Stack"))
        {
            if (manager == null)
                manager = GameObject.FindObjectOfType<MidiManager>();
            manager.stacks.Remove(this);
        }
    }
}

[System.Serializable]
public class MidiModule
{
    public enum MidiModuleType {
        ReflectFloat
    }
    public MidiModuleType type;
    [NonSerialized]
    public MidiStack stack;
    Func<Rect, MidiModule, float> guiAction;
    Action<MidiModule> callAction;
    float editorLastHeight = 10;


    public bool hasCurve = true;
    public AnimationCurve curve = new AnimationCurve();
    public CCReflectFloat output = new CCReflectFloat();

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
    }

    public void OnEvent()
    {
        if (callAction == null)
            GetActions();
        callAction(this);
    }

    public float OnGUI(Rect r)
    {
        if (guiAction == null)
            GetActions();
        if (stack == null)
            GameObject.FindObjectOfType<MidiManager>().LinkStacks();

        r.height = editorLastHeight;
        GUIHelper.DrawRect(r, Color.gray);
        r.x += 2;
        r.y += 2;
        r.width -= 4;
        float f = guiAction(r, this);
        editorLastHeight = f+4; 
        return f+10;
    }

    static float ReflectFloatDraw(Rect r, MidiModule m)
    {
        float startHeight = r.y;
        r.height = 20;
        GUI.Label(new Rect(r.x, r.y, r.width-20, 20), "Float Reflect");
        if(GUI.Button(new Rect(r.x+r.width-20, r.y, 20, 20), "x"))
        {
            m.stack.modules.Remove(m);
            return 0;
        }
        r.y += 20;
        m.hasCurve = GUI.Toggle(r, m.hasCurve,"hasCurve");
        if (m.hasCurve)
        {
            r.y += 20;
            m.curve = UnityEditor.EditorGUI.CurveField(r, m.curve);
        }
        r.y += 20;
        m.output.Draw(r);
        
        return r.y-startHeight+20;
    }

    static void ReflectFloatCall(MidiModule m)
    {
        float f = m.stack.value;
        if (m.hasCurve)
            f = m.curve.Evaluate(f);
        m.output.SetValue(f);
    }
}
