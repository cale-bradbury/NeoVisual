using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class NeoMidi : EditorWindow
{
    MidiInput input;
    MidiManager manager;

    [MenuItem("CampCult/NeoMidi %_m")]
    static void Init()
    {
        NeoMidi m = EditorWindow.GetWindow<NeoMidi>();
        m.input = FindObjectOfType<MidiInput>();
        m.manager = FindObjectOfType<MidiManager>();
    }

    void OnGUI()
    {
        if (manager ==null)
        {
            input = FindObjectOfType<MidiInput>();
            manager = FindObjectOfType<MidiManager>();
        }

        if(GUILayout.Button("Add Stack"))
        {
            manager.stacks.Add(new MidiStack(manager));
            manager.stacks[manager.stacks.Count - 1].index = manager.stacks.Count - 1;
        }


        int stackWidth = 200;
        Rect stackRect = new Rect(0, 25, stackWidth, 300);
        for (int i = 0; i < manager.stacks.Count; i++)
        {
            manager.stacks[i].OnGUI(stackRect);
            stackRect.x += stackWidth+10;
        }

    }

}