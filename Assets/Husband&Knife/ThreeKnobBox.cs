using UnityEngine;
using System.Collections;
using System.Reflection;

public class ThreeKnobBox : MonoBehaviour {
    
	string oscHost = "127.0.0.1";
	int SendToPort = 12001;
	int ListenerPort = 32001;
	string address = "/arduino";
	
	UDPPacketIO udp;
	Osc handler;

    public CCReflectFloat top;
    public Vector2 minMaxTop;
    public CCReflectFloat middle;
    public Vector2 minMaxMiddle;
    public CCReflectFloat bottom;
    public Vector2 minMaxBottom;

    void OnEnable(){
			udp = gameObject.AddComponent<UDPPacketIO> ();
			handler = gameObject.AddComponent<Osc> ();

			udp.init (oscHost, SendToPort, ListenerPort);
			handler.init (udp);
         handler.SetAddressHandler(address, getValue);
	}

	void getValue( OscMessage msg)
    {
        if (msg.Values.Count < 3)
            return;
        
        top.SetValue(Utils.Lerp(minMaxTop.x, minMaxTop.y, ((int)msg.Values[0]) / 1024f));
        middle.SetValue(Utils.Lerp(minMaxMiddle.x, minMaxMiddle.y, ((int)msg.Values[1]) / 1024f));
        bottom.SetValue(Utils.Lerp(minMaxBottom.x, minMaxBottom.y, ((int)msg.Values[2]) / 1024f));
    }


	
}