#include <dht.h>

dht DHT;

const int Rain1=A0;
const int Led=12;
const int Gas=A1;
const int Temp=11;
const int LDR=A2;
const int PIR=10;
const int Flood1=A3;
const int Flood2=A4;
const int ManSig=9;

int PIRVal=0;
int LDRVal=0;
int TempVal=0;
int HumidityVal=0;
int Rain1Val=0;
int GasVal=0;
int RainBool=0;
int FloodVal=0;
int ManSigVal=0;

void setup() {
  Serial.begin(9600);
  pinMode(Led,OUTPUT);
  pinMode(Rain1,INPUT);
  pinMode(Gas,INPUT);
  pinMode(LDR,INPUT);
  pinMode(PIRVal,INPUT);
  pinMode(Flood1,INPUT);
  pinMode(Flood2,INPUT);
  pinMode(ManSig,INPUT);
  DHT.read11(Temp);
}

void loop() {
  
  // put your main code here, to run repeatedly:
  if(Serial.available()){
    String Read=Serial.readString();
    if(Read=="dta" || true){
      DHT.read11(Temp);
      ReadSenVals();
      Serial.println("<"+String(LDRVal)+","+String(TempVal)+","+String(HumidityVal)+","+String(Rain1Val)+","+String(GasVal)+","+String(PIRVal)+","+String(RainBool)+","+String(FloodVal)+","+String(ManSigVal)+">");
    }
    if(Read=="LON"){
      DigSwitch(Led,true);
    }
    if(Read=="LOF"){
      DigSwitch(Led,false);
    }
  }  
  ReadSenVals();
  //Serial.println(Rain1Val);
  if(Rain1Val<900){
    DigSwitch(Led,true);
    RainBool=1;
    //Serial.println("Error rain");
  }
  else{
    DigSwitch(Led,false);
    RainBool=0;
  }
}

void ReadSenVals(){
  Rain1Val=analogRead(Rain1);
  GasVal=analogRead(Gas);
  TempVal=DHT.temperature;
  HumidityVal=DHT.humidity;
  LDRVal=analogRead(LDR);
  PIRVal=digitalRead(PIR);
  ManSigVal=digitalRead(ManSig);
  FloodVal=getFloodVal();
}

void DigSwitch(int pin,bool state){
  if(state){
    digitalWrite(pin,0);
  }
  else{
    digitalWrite(pin,1);
  }
}

int getFloodVal(){
  int TempF1=analogRead(Flood1);
  int TempF2=analogRead(Flood2);
  //Serial.println(String(TempF2)+":"+String(TempF1));
  if(TempF1<900 && TempF2<900){
    //Serial.print("Flood alert :");
    //Serial.print(TempF1);
    //Serial.println(TempF2);
    return 1;
  }
  return 0;
}
