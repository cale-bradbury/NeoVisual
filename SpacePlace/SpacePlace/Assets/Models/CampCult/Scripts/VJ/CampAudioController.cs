using UnityEngine;
using System.Collections;
using System.Reflection;

public class CampAudioController : MonoBehaviour {

	public static float value = 0;
	public bool oscMode = true;

	public string oscHost = "127.0.0.1";
	public int SendToPort = 12000;
	public int ListenerPort = 32000;
	public string address = "/vj";
    public static float max = 1;
	
	public AudioSource source;
	public int numSamples;
	public int channel = 0;
	public FFTWindow FFTWindow;
	public float lerp = .5f;
	public AnimationCurve spectrumCurve;
	public float spectrumMul = 1;
    public float spectrumPow = 2;
    public float falloffRate = .01f;

    public float peakFalloff = .01f;
    public float peakMin = .1f;

	UDPPacketIO udp;
	Osc handler;
    public static float[] FFT = new float[1];
    static float[] peak = new float[1];
    public static int largestIndex;
    public static float largestValue;


    void OnEnable(){
		if (oscMode) {
			udp = gameObject.AddComponent<UDPPacketIO> ();
			handler = gameObject.AddComponent<Osc> ();

			udp.init (oscHost, SendToPort, ListenerPort);
			handler.init (udp);
			handler.SetAddressHandler (address, vjValue);
			FFT = new float[1];
            peak = new float[1];
		} else
        {
            FFT = new float[numSamples];
            peak = new float[numSamples];
            for (int i = 0; i < FFT.Length; i++)
            {
                FFT[i] = 0;
                peak[i] = 0;
            }
        }
	}

	void Update(){
		if (!oscMode) {
			float[] n = new float[FFT.Length];
			source.GetSpectrumData (n, channel, FFTWindow);
			for(int i = 0; i< FFT.Length;i++){
                float f = (float)n[i];
                peak[i] = Mathf.Max(Mathf.Max(peakMin, peak[i] - peakFalloff), f, 0);
                f /= peak[i];
                FFT[i] = Mathf.Max(FFT[i] - falloffRate, Mathf.Lerp(FFT[i], Mathf.Pow(f * spectrumMul, spectrumPow) * spectrumCurve.Evaluate((float)i / FFT.Length), lerp));
            }
		}
	}

	void vjValue(OscMessage msg){

		if (FFT.Length != msg.Values.Count)
        {
            FFT = new float[msg.Values.Count];
            peak = new float[msg.Values.Count];
            for (int i = 0; i<FFT.Length;i++){
				FFT[i] = 0;
                peak[i] = 0;
			}
		}
        largestIndex = 0;
        largestValue = 0;
        for (int i = 0; i < FFT.Length; i++)
        {
            float f = (float)msg.Values[i];
            peak[i] = Mathf.Max(Mathf.Max(peakMin,peak[i]-peakFalloff), f, 0);
            f /= peak[i];
            FFT[i] = Mathf.Max(FFT[i]-falloffRate,Mathf.Lerp(FFT[i], Mathf.Pow(f  * spectrumMul, spectrumPow), lerp));
            if (FFT[i] > largestValue)
            {
                largestValue = FFT[i];
                largestIndex = i;
            }
        }
    }


	
}