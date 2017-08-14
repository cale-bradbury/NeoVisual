// Unity MIDI Input plug-in / C# interface
// By Keijiro Takahashi, 2013
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[ExecuteInEditMode]
public class MidiInput : MonoBehaviour
{
    #region Static interface
    // Common instance.
    public static MidiInput instance;
    public MidiMessage lastMessage;
    
    // Filter mode IDs.
    public enum Filter
    {
        Realtime,
        Fast,
        Slow
    }

    // Returns the key state (on: velocity, off: zero).
    public static float GetKey (int noteNumber)
    {
        var v = instance.notes [noteNumber];
        if (v > 1.0f) {
            return v - 1.0f;
        } else if (v > 0.0) {
            return v;
        } else {
            return 0.0f;
        }
    }

    // Returns true if the key was pressed down in this frame.
    public static bool GetKeyDown (int noteNumber)
    {
        return instance.notes [noteNumber] > 1.0f;
    }

    // Returns true if the key was released in this frame.
    public static bool GetKeyUp (int noteNumber)
    {
        return instance.notes [noteNumber] < 0.0f;
    }

    // Provides the channel list.
    /*public static int[] KnobChannels {
        get {
            int[] channels = new int[instance.knobs.Count];
            instance.knobs.Keys.CopyTo (channels, 0);
            return channels;
        }
    }*/

    // Get the Camp value.
    public static float GetKnob(int channel, Filter filter = Filter.Realtime)
    {
        return GetKnob(0xb0, channel, filter);
    }
    public static float GetKnob(int status, int channel, Filter filter = Filter.Realtime)
    {
        MidiLocation l = new MidiLocation(status, channel);
        if (instance.knobs.ContainsKey(l))
        {
            return instance.knobs[l].filteredValues[(int)filter];
        }
        else
        {
            return 0.0f;
        }
    }
    #endregion

    #region MIDI message sturcture
    public struct MidiMessage
    {
        // MIDI source (endpoint) ID.
        public uint source;
        
        // MIDI status byte.
        public byte status;
        
        // MIDI data bytes.
        public byte data1;
        public byte data2;
        
        public MidiMessage (ulong data)
        {
            source = (uint)(data & 0xffffffffUL);
            status = (byte)((data >> 32) & 0xff);
            data1 = (byte)((data >> 40) & 0xff);
            data2 = (byte)((data >> 48) & 0xff);
        }
        
        public override string ToString ()
        {
            return string.Format ("s({0:X2}) d({1:X2},{2:X2}) from {3:X8}", status, data1, data2, source);
        }
    }
    #endregion

    #region Internal data structure
    // Camp channel (knob) information.
    class Knob
    {
        public float[] filteredValues;

        public Knob (float initial)
        {
            filteredValues = new float[3];
            filteredValues [0] = filteredValues [1] = filteredValues [2] = initial;
        }

        public void Update (float value)
        {
            filteredValues [0] = value;
        }

        public void UpdateFilter (float fastFilterCoeff, float slowFilterCoeff)
        {
			filteredValues [1] = filteredValues [2] = filteredValues [0] - (filteredValues [0] - filteredValues [1]) * fastFilterCoeff;
            //filteredValues [2] = filteredValues [0] - (filteredValues [0] - filteredValues [2]) * slowFilterCoeff;
        }
    }

    // Note state array.
    // X<0    : Released on this frame.
    // X=0    : Off.
    // 0<X<=1 : On. X represents velocity.
    // 1<X<=2 : Triggered on this frame. (X-1) represents velocity.
    float[] notes;

    // Channel number to knob mapping.
    Dictionary<MidiLocation, Knob> knobs;
    #endregion

    #region Editor supports
    #if UNITY_EDITOR
    // Incoming message history.
    Queue<MidiMessage> messageHistory;
    public Queue<MidiMessage> History {
        get { return messageHistory; }
    }
    #endif
    #endregion

    #region Public properties
    public float sensibilityFast = 20.0f;
    public float sensibilitySlow = 8.0f;
    #endregion

    #region Monobehaviour functions
    void Awake ()
    {
        instance = this;
        notes = new float[128];
        knobs = new Dictionary<MidiLocation, Knob> ();
        #if UNITY_EDITOR
        messageHistory = new Queue<MidiMessage> ();
        #endif
    }

    void Update ()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            ProcessIncomingDataEditor();
            return;
        }
#endif
        // Update the note state array.
        for (var i = 0; i < 128; i++) {
            var x = notes [i];
            if (x > 1.0f) {
                // Key down -> Hold.
                notes [i] = x - 1.0f;
            } else if (x < 0) {
                // Key up -> Off.
                notes [i] = 0.0f;
            }
        }

        // Calculate the filter coefficients.
        var fastFilterCoeff = Mathf.Exp (-sensibilityFast * Time.deltaTime);
       // var slowFilterCoeff = Mathf.Exp (-sensibilitySlow * Time.deltaTime);

        // Update the filtered value.
        foreach (var k in knobs.Values) {
			k.UpdateFilter (fastFilterCoeff, fastFilterCoeff);
        }

        ProcessIncomingData();

        #if UNITY_EDITOR
        // Truncate the history.
        while (messageHistory.Count > 8) {
            messageHistory.Dequeue ();
        }
        #endif
    }

    void ProcessIncomingDataEditor()
    {
        while (true)
        {
            // Pop from the queue.
            var data = DequeueIncomingData();
            if (data == 0)
            {
                break;
            }

            // Parse the message.
            var message = new MidiMessage(data);
            if (message.data1 == 0)
                return;
            
            lastMessage = message;
#if UNITY_EDITOR
            // Record the message history.
            messageHistory.Enqueue(message);
#endif
        }
    }

        void ProcessIncomingData()
    {
        // Process the message queue.
        while (true)
        {
            // Pop from the queue.
            var data = DequeueIncomingData();
            if (data == 0)
            {
                break;
            }

            // Parse the message.
            var message = new MidiMessage(data);
            if (message.data1 == 0)
                return;

            // Note on message?
            if ((message.status >= 0x90 && message.status < 0xA0))
            {
                notes[message.data1] = 1.0f / 127 * message.data2 + 1.0f;
            }

            // Note off message?
            if ((message.status >= 0x80 && message.status < 0x90 )|| (message.status == 0x90 && message.data2 == 0))
            {
                notes[message.data1] = -1.0f;
            }

            // Camp message?
            if (message.status >= 0xb0 && message.status < 0xc0)
            {
                // Normalize the value.
                var value = 1.0f / 127 * message.data2;

                MidiLocation l = new MidiLocation(message.status, message.data1);
                // Update the channel if it already exists, or add a new channel.
                if (knobs.ContainsKey(l))
                {
                    knobs[l].Update(value);
                }
                else
                {
                    knobs[l] = new Knob(value);
                }
            }
            lastMessage = message;
#if UNITY_EDITOR
            // Record the message history.
            messageHistory.Enqueue(message);
#endif
        }
    }
    #endregion

    #region Native module interface
    [DllImport ("UnityMIDIReceiver", EntryPoint="UnityMIDIReceiver_CountEndpoints")]
    public static extern int CountEndpoints ();
    
    [DllImport ("UnityMIDIReceiver", EntryPoint="UnityMIDIReceiver_GetEndpointIDAtIndex")]
    public static extern uint GetEndpointIdAtIndex (int index);
    
    [DllImport ("UnityMIDIReceiver", EntryPoint="UnityMIDIReceiver_DequeueIncomingData")]
    public static extern ulong DequeueIncomingData ();
    
    [DllImport ("UnityMIDIReceiver")]
    private static extern System.IntPtr UnityMIDIReceiver_GetEndpointName (uint id);
    
    public static string GetEndpointName (uint id)
    {
        return Marshal.PtrToStringAnsi (UnityMIDIReceiver_GetEndpointName (id));
    }
    #endregion
}

struct MidiLocation
{
    public int note;
    public int status;
    public MidiLocation(int status, int note)
    {
        this.note = note;
        this.status = status;
    }
}
