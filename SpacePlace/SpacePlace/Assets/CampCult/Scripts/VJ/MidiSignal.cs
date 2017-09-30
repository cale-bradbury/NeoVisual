using UnityEngine;
using System.Collections;

[System.Serializable]
public class MidiSignal {

    public System.Action<float> OnChange;
    public int channel;
    public int status;
    float value;
	
    public MidiSignal(int channel, int signal)
    {
        this.channel = channel;
        this.status = signal;
    }

    public void SetToLastPressed()
    {
        MidiInput.MidiMessage m = MidiInput.instance.lastMessage;
        channel = m.data1;
        status = m.status;
    }

    public void Check()
    {
        float f = MidiInput.GetKnob(status, channel);
        if (f != value)
        {
            value = f;
            OnChange(value);
        }
    }
}
