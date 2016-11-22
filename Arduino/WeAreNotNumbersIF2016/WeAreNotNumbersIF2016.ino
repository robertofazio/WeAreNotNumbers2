/* WE ARE NOT NUMBERS IF2016
   Working Copy Master
   Last Update 04.07.2016
*/

#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>
#include <SPI.h>
#include <Ethernet.h>
#include <ArdOSC.h>

#define SERVOMIN  210                   // this is the 'minimum' pulse length count (out of 4096)
#define SERVOMAX  430                   // this is the 'maximum' pulse length count (out of 4096)
#define NUM_PWM 7                       // numero delle Servo Shield adafruit
#define SERVOS_PER_PWM 16               // servo per ogni scheda
#define TOT_PIXELS 100                  // pixels totali ( + 1 )  
#define PIX_BLOCK_1 70                  //pixels nel primo messaggio
#define PIX_BLOCK_2 30                  //pixels nel secondo messaggio

Adafruit_PWMServoDriver pwm[NUM_PWM];

OSCServer server;
OSCClient client;

// Mac Address Ethernet Shield
byte myMac[] = { 0x90, 0xA2, 0xDA, 0x0F, 0x07, 0xEE };
byte myIp[]  = { 192, 168, 31, 141 };   // SET ARDUINO IP same in Unity3d config.XML
int  serverPort  = 10000;               // Unity3d incoming port
int  destPort = 12345;

//facciamo un array che contenga i 100 valori del nostro frame
uint16_t oldFrame[TOT_PIXELS];
uint16_t frame[TOT_PIXELS];

//facciamo una variabile che ci dice se il frame e' nuovo
//la accendiamo quando inseriamo nuovi dati, la spegnamo dopo aver mandato ai SERVOMIN
bool isFrameNew = false;

void setup()
{
  InitPWM();

  Serial.begin(9600);
  Serial.println("WE ARE NOT NUMBERS II - IF2016 - OSC From Unity 7 x 16 channel Servo!");

  Serial.println("\nI2C Scanner");
  I2CDetect();

  yield();

  Ethernet.begin(myMac , myIp);
  server.begin(serverPort);

  PrintIP();
  resetServos();

  server.addCallback("/unity/reset", &Reset);
  server.addCallback("/unity/single", &UnitySingle);


}

void loop()
{
  if (server.availableCheck() > 0)
  {
    //Serial.println("alive! ");
  }
  // Controllo se ho un nuovo frame aggiorno
  //updateServos();
}


void updateSingleServo(int iShield, int jServo, int maxValues)
{
  if(maxValues == 1)
  {
   pwm[iShield].setPWM(jServo,0,SERVOMAX);
  }
  else
  {
      pwm[iShield].setPWM(jServo,0,SERVOMIN);

  } 
}

void resetServos()
{
  //mando tutti i servi al valore minimo
  for (int i = 0; i < NUM_PWM; i++)
  {
    for (int j = 0; j < SERVOS_PER_PWM; j++)
    {
      pwm[i].setPWM(j, 0, SERVOMIN); // SERVOMIN
      delay(20);                  // waits for a second
    }
  }
  isFrameNew = false;
}


// Callback che riceve da Unity il RESET di tutti i servi
void Reset(OSCMessage *_msg)
{
  Serial.println("OSC from Unity RESET ALL SERVO");

  String address = _msg->getOSCAddress();
  if (address == "/unity/reset")
  {
    Serial.println("Reset!");
    resetServos();
  }
}


  void UnitySingle(OSCMessage * _msg)
{
  
  int iShield = _msg->getArgInt32(0);
  int jServo = _msg->getArgInt32(1);
  int MaxValue = _msg->getArgInt32(2);

  Serial.println("shield n");
  Serial.println(iShield);
  Serial.println("servo n");
  Serial.println(jServo);
  Serial.println("to max");
  Serial.println(MaxValue);

  updateSingleServo(iShield,jServo, MaxValue);
  
}


void logOscAddress(OSCMessage *_msg)
{
  Serial.println(_msg->getOSCAddress());
}


void logIp(OSCMessage *_msg)
{
  byte *ip = _msg->getIpAddress();
  Serial.print("IP:");
  Serial.print(ip[0], DEC);
  Serial.print(".");
  Serial.print(ip[1], DEC);
  Serial.print(".");
  Serial.print(ip[2], DEC);
  Serial.print(".");
  Serial.print(ip[3], DEC);
  Serial.print(" ");
  Serial.println("");
}

void I2CDetect()
{
  byte error, address;
  int nDevices;

  Serial.println("Scanning...");


  nDevices = 0;
  for (address = 1; address < 127; address++ )
  {
    // The i2c_scanner uses the return value of
    // the Write.endTransmisstion to see if
    // a device did acknowledge to the address.
    Wire.beginTransmission(address);
    error = Wire.endTransmission();

    if (error == 0)
    {
      Serial.print("I2C device found at address 0x");
      if (address < 16)
        Serial.print("0");
      Serial.print(address, HEX);
      Serial.println("  !");

      nDevices++;
    }
    else if (error == 4)
    {
      Serial.print("Unknow error at address 0x");
      if (address < 16)
        Serial.print("0");
      Serial.println(address, HEX);
    }
  }
  if (nDevices == 0)
    Serial.println("No I2C devices found\n");
  else
  {
    Serial.println("done\n");
  }

}

void InitPWM()
{
  Wire.begin();

  pwm[0] = Adafruit_PWMServoDriver(0x40);
  pwm[1] = Adafruit_PWMServoDriver(0x41);
  pwm[2] = Adafruit_PWMServoDriver(0x42);
  pwm[3] = Adafruit_PWMServoDriver(0x43);
  pwm[4] = Adafruit_PWMServoDriver(0x44);
  pwm[5] = Adafruit_PWMServoDriver(0x45);
  pwm[6] = Adafruit_PWMServoDriver(0x46);

  pwm[0].begin();
  pwm[1].begin();
  pwm[2].begin();
  pwm[3].begin();
  pwm[4].begin();
  pwm[5].begin();
  pwm[6].begin();

  pwm[0].setPWMFreq(60);
  pwm[1].setPWMFreq(60);
  pwm[2].setPWMFreq(60);
  pwm[3].setPWMFreq(60);
  pwm[4].setPWMFreq(60);
  pwm[5].setPWMFreq(60);
  pwm[6].setPWMFreq(60);

}

void PrintIP()
{
  // print your local IP address:
  Serial.print("Arduino IP address: ");
  for (byte thisByte = 0; thisByte < 4; thisByte++)
  {
    // print the value of each byte of the IP address:
    Serial.print(Ethernet.localIP()[thisByte], DEC);
    Serial.print(".");
  }
}
