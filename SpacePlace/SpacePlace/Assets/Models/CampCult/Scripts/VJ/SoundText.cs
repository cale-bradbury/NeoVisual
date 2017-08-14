using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundText : MonoBehaviour {

    public int fft = 12;
    public string[] lines;
    public Text text;
    public int maxVisible = 6;
    public float falloff = .01f;
    public float increaceForBeat = .5f;
    float lastMax;
    float c = 0;
    string t;
    int v = 0;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (CampAudioController.FFT[fft] > c + increaceForBeat)
        {
            NewLine();
            c = CampAudioController.FFT[fft];
        }
        else
        {
            c -= falloff;
            c = Mathf.Max(c, 0);
        }
	}

    void NewLine()
    {
        t = lines[Mathf.FloorToInt(Random.value*lines.Length)]+"\n" + t;
        v++;
        if (v > maxVisible)
            t = t.Substring(0, t.LastIndexOf("\n"));
        text.text = t;
    }
}
