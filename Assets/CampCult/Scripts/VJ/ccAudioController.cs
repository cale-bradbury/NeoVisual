using UnityEngine;
using System.Collections;
using System.Reflection;

public class ccAudioController : MonoBehaviour {

	public static float value = 0;
	public bool oscMode = true;

	public string oscHost = "127.0.0.1";
	public int SendToPort = 12000;
	public int ListenerPort = 32000;
	public string address = "/vj";
    float max = 1;
	
	public AudioSource source;
	public int numSamples;
	public int channel = 0;
	public FFTWindow FFTWindow;
	public float lerp = .5f;
	public AnimationCurve spectrumCurve;
	public float spectrumMul = 1;
    public float spectrumPow = 2;
    public float falloffRate = .01f;

	UDPPacketIO udp;
	Osc handler;
	public static float[] FFT = new float[1];

	void OnEnable(){
		if (oscMode) {
			udp = gameObject.AddComponent<UDPPacketIO> ();
			handler = gameObject.AddComponent<Osc> ();

			udp.init (oscHost, SendToPort, ListenerPort);
			handler.init (udp);
			handler.SetAddressHandler (address, vjValue);
			FFT = new float[1];
		} else {
			FFT = new float[numSamples];
		}
	}

	void Update(){
		if (!oscMode) {
			float[] n = new float[FFT.Length];
			source.GetSpectrumData (n, channel, FFTWindow);
			for(int i = 0; i< FFT.Length;i++){
				FFT[i] = Mathf.Lerp(FFT[i],spectrumCurve.Evaluate((float)i/FFT.Length)*(n[i]) *spectrumMul,lerp);
			}
		}
	}

	void vjValue(OscMessage msg){

		if (FFT.Length != msg.Values.Count) {
			FFT = new float[msg.Values.Count];
			for(int i = 0; i<FFT.Length;i++){
				FFT[i] = 0;
			}
		}
        float f = 0;
        max = Mathf.Max(10, max-falloffRate);
        for (int i = 0; i<FFT.Length;i++){
            f = spectrumCurve.Evaluate((float)i / FFT.Length)*(float)msg.Values[i];
            max = Mathf.Max(f, max);
            FFT[i] = Mathf.Lerp(FFT[i], Mathf.Pow((f / max) * spectrumMul, spectrumPow), lerp);
        }

	}


	
}