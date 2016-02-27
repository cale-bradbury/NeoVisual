import cc.arduino.*;
import org.firmata.*;

import oscP5.*;
import netP5.*;
 
import processing.serial.*;
 
OscP5 osc;
NetAddress net;
//Arduino ad;
Serial s1;
Arduino a1;
int[] val = new int[4];
void setup(){
  size(320,320);
 
  println(Arduino.list());
 println("***");
  a1 = new Arduino(this, Arduino.list()[0]);
  for(int i = 0; i<4;i++){
    a1.pinMode(i, Arduino.INPUT);
    val[i] = 0;
  }
  //s1 = new Serial(this, Serial.list()[0], 9600);
  //s2 = new Serial(this, Serial.list()[1], 9600);
  osc = new OscP5(this,12001);
  net = new NetAddress("127.0.0.1",32001);
}
float c = 0;
 
void draw(){
  OscMessage msg = new OscMessage ("/arduino");
 
  background(100);
  stroke(0);
  val =  getData(a1,val);
  /*if(mousePressed){
    if(mouseY<100){
     val[0] = mouseX/200.0*256.0;
    }else if(mouseY<200){
     val[1] = mouseX/200.0*256.0;
    }else if(mouseY<300){
     val[3] = mouseX/400.0;
    }
  }
  //println(val[1]);
  c+=val[3];
  val[2] = sin(c)*512+552;
 
  line(0, 100,200,100);
  line(0, 200,200,200);
  line(0, 300,200,300);*/
  msg.add(val[0]);
  msg.add(val[1]);
  msg.add(val[2]);
 
 
  showData(val, 10);
 
 
  osc.send(msg,net);
 
}

int[] getData(Arduino a, int[] val){
  /* while ( s.available() > 0) {  
       String v  = s.readStringUntil('\n');
       if(v!=null){
         v = fix(trim(v));
         if(v.length()>0){
           int j = Integer.parseInt(v);
           if(j==9999){
              index2 = -1;
           }
           else
             val[index2] =  j;
          index2++;
          index2 %= 4;
         }
     }
  }*/
  for(int i = 0; i<3;i++){
   val[i] = a.analogRead(i);
  }
  return val;
}
 
void showData(int[] val, int y){
  for(int i = 0; i<3;i++){
    int j = i*10+y;
    line(0, j,val[i]*.1, j);
  }
}