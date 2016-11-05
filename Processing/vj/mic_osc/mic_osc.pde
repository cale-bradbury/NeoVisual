import ddf.minim.*;
import ddf.minim.analysis.*;
import oscP5.*;
import netP5.*;

FFT fft;
Minim minim;
AudioInput in;
int bands = 128;
float[] highest = new float[bands];
WindowFunction[] win = { FFT.GAUSS, FFT.NONE, FFT.TRIANGULAR, FFT.LANCZOS, FFT.HANN, FFT.HAMMING, FFT.COSINE, FFT.BLACKMAN, FFT.BARTLETTHANN, FFT.BARTLETT};
int windowIndex = 0;

OscP5 osc;
NetAddress net;

void setup(){
  size(256,256);
  
  osc = new OscP5(this,12000); 
  net = new NetAddress("127.0.0.1",32000);
  
  minim = new Minim(this);
  in = minim.getLineIn(minim.STEREO);
  fft = new FFT(in.bufferSize(),in.sampleRate());
  fft.window(win[windowIndex]);
  
}
void draw(){  
  background(255);
  stroke(0);
  fft.linAverages(bands*2);
  fft.forward(in.mix);
  OscMessage msg = new OscMessage ("/vj");
  for(int i = 0; i< bands;i++){
    float f = fft.getAvg((i));
    highest[i] = max(highest[i], f);
    f = f/highest[i];
    highest[i]-=.000001;
   // f+=fft.getBand((i+1)*4);
   // f*=.5;
    //f*=i;
    msg.add( f);
    line( i, 256, i, 256 - f*100 );     
  }  
  osc.send(msg,net);
}

void keyPressed(){
 if(keyCode == 37){
   windowIndex -= 1;
   if(windowIndex ==-1)
     windowIndex = win.length-1;
   fft.window(win[windowIndex]);
 }else if(keyCode == 39){
   windowIndex++;
   windowIndex %= win.length; 
   fft.window(win[windowIndex]);
 }
 println(win[windowIndex]);
}