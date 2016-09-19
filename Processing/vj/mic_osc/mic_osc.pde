import ddf.minim.*;
import ddf.minim.analysis.*;
import oscP5.*;
import netP5.*;

FFT fft;
Minim minim;
AudioInput in;
int bands = 256;
float[] spectrum = new float[bands];
WindowFunction win = FFT.LANCZOS;

OscP5 osc;
NetAddress net;

void setup(){
  size(256,256);
  minim = new Minim(this);
  in = minim.getLineIn(minim.STEREO);
  fft = new FFT(in.bufferSize(),in.sampleRate());
  fft.window(win);
  
  osc = new OscP5(this,12000); 
  net = new NetAddress("127.0.0.1",32000);
}
void draw(){
  background(255);
  stroke(0);
  fft.forward(in.mix);
  OscMessage msg = new OscMessage ("/vj");
  for(int i = 0; i< 128;i++){
    float f = fft.getBand((i));
   // f+=fft.getBand((i+1)*4);
   // f*=.5;
    f*=i*.2;
    msg.add( f);
    line( i, 256, i, 256 - f );     
  }
  
  osc.send(msg,net);

}


