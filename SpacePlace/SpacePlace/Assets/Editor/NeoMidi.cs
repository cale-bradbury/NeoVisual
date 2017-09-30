using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class NeoMidi : EditorWindow
{
    MidiInput input;
    NeoMidiManager manager;
    Vector2 scrollPos = Vector2.zero;

    [MenuItem("CampCult/NeoMidi %_m")]
    static void Init()
    {
        NeoMidi m = EditorWindow.GetWindow<NeoMidi>();
        m.input = FindObjectOfType<MidiInput>();
        m.manager = FindObjectOfType<NeoMidiManager>();
        if (m.manager == null)
            Debug.LogWarning("NO NeoMidiManager found in scene");
    }

    void OnGUI()
    {
        if (manager ==null)
        {
            input = FindObjectOfType<MidiInput>();
            manager = FindObjectOfType<NeoMidiManager>();
        }

        if (GUILayout.Button("Add Stack"))
        {
            manager.stacks.Add(new MidiStack(manager));
            manager.stacks[manager.stacks.Count - 1].index = manager.stacks.Count - 1;
        }

        int stackWidth = 200;
        scrollPos = GUI.BeginScrollView(new Rect(0,0,position.width, position.height), scrollPos, new Rect(0, 0, manager.stacks.Count * (stackWidth+10), 200));

        Rect stackRect = new Rect(0, 25, stackWidth, 300);
        for (int i = 0; i < manager.stacks.Count; i++)
        {
            stackRect.x+=manager.stacks[i].OnGUI(stackRect) + 10;
        }
        GUI.EndScrollView();
    }

}