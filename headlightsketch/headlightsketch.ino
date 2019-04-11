#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
  #include <avr/power.h>
#endif

#define PIN 6

// Parameter 1 = number of pixels in strip
// Parameter 2 = Arduino pin number (most are valid)
// Parameter 3 = pixel type flags, add together as needed:
//   NEO_KHZ800  800 KHz bitstream (most NeoPixel products w/WS2812 LEDs)
//   NEO_KHZ400  400 KHz (classic 'v1' (not v2) FLORA pixels, WS2811 drivers)
//   NEO_GRB     Pixels are wired for GRB bitstream (most NeoPixel products)
//   NEO_RGB     Pixels are wired for RGB bitstream (v1 FLORA pixels, not v2)
//   NEO_RGBW    Pixels are wired for RGBW bitstream (NeoPixel RGBW products)
Adafruit_NeoPixel strip = Adafruit_NeoPixel(11, PIN, NEO_GRB + NEO_KHZ400);

// IMPORTANT: To reduce NeoPixel burnout risk, add 1000 uF capacitor across
// pixel power leads, add 300 - 500 Ohm resistor on first pixel's data input
// and minimize distance between Arduino and first pixel.  Avoid connecting
// on a live circuit...if you must, connect GND first.

void setup() {
  // This is for Trinket 5V 16MHz, you can remove these three lines if you are not using a Trinket
  #if defined (__AVR_ATtiny85__)
    if (F_CPU == 16000000) clock_prescale_set(clock_div_1);
  #endif
  // End of trinket special code

  Serial.begin(9600);
  strip.begin();
  strip.show(); // Initialize all pixels to 'off'
}

int cmd=-1;
int val=-1;

void showPower(int val)
{
  int targetR=val;    
  int targetG=0;
  int targetB=255-val;
  int fadePos=val;
  int curFadePos=0;
  int nextFadePos=0;
  for(int i=0;i<6;i++)
  {
    int r,g,b;
    curFadePos=i*42;
    nextFadePos=(i+1)*42;
    if(nextFadePos<fadePos)
    {
      r=targetR;
      g=targetG;
      b=targetB;
    }else if(curFadePos>fadePos)
    {
      r=0;
      g=0;
      b=0;        
    }else
    {
      int fadeAmount=(fadePos-curFadePos);
      r=(targetR*fadeAmount)>>7;
      g=(targetG*fadeAmount)>>7;
      b=(targetB*fadeAmount)>>7;
    }
    if(i==0)
    {
      strip.setPixelColor(5,r,g,b);
    }else
    {
      strip.setPixelColor(5-i,r,g,b);
      strip.setPixelColor(i+5,r,g,b);        
    }
  }
  strip.show() ;  
}

int rLeft=128;
int rRight=128;
int gLeft=128;
int gRight=128;
int bLeft=128;
int bRight=128;
int i=0,j=2;


void showIntroAnimation(int val)
{
  while(Serial.available()==0)
  {
    // val =0 nothing held
    if((val&0x1)==0x01)
    {
      // left is held - fade towards red
      if(rLeft<252)rLeft+=4;
      if(gLeft>0)gLeft-=4;
      if(bLeft>0)bLeft-=4;
    }else
    {
      if(rLeft>128)rLeft-=4;
      if(gLeft<128)gLeft+=4;
      if(bLeft<128)bLeft+=4;
      
    }
    if((val&0x2)==0x02)
    {
      // right is held - fade towards red
      if(rRight<252)rRight+=4;
      if(gRight>0)gRight-=4;
      if(bRight>0)bRight-=4;
    }else
    {
      if(rRight>128)rRight-=4;
      if(gRight<128)gRight+=4;
      if(bRight<128)bRight+=4;
      
    }
    // left hand sweep out
    // right hand sweep out
    // centre point
    if(i==0)
    {
      strip.setPixelColor(5,(rLeft+rRight)>>1,(gLeft+gRight)>>1,(bLeft+bRight)>>1);
    }else
    {
      strip.setPixelColor(5-i,rRight,gRight,bRight);
      strip.setPixelColor(i+5,rLeft,gLeft,bLeft);        
    }
    if(j==0)
    {
      strip.setPixelColor(5,0,0,0);
    }else
    {
      strip.setPixelColor(5-j,0,0,0);
      strip.setPixelColor(j+5,0,0,0);
    }
    j+=1;
    if(j>=6)j=0;
    i+=1;
    if(i>=6)i=0;
    strip.show();
    delay(50);
  }
}

void showDeadAnimation(int val)
{
  int maxVal=255;
  int counter=0;
  while(Serial.available()==0 && maxVal>0)
  {
    int bright=0;
    counter++;
    if((counter&0x7)<2)
    {
      bright=0;
    }else
    {
      bright=maxVal;
    }
    // flash white
    for(int i=0;i<11;i++)
    {
      strip.setPixelColor(i,bright,bright,bright);
    }
    strip.show();
    delay(5);
    maxVal-=1;
    Serial.println(".");
  }
  showIntroAnimation(0);
}



void showHighScoreAnimation(int val)
{
  int fade=2047;
  int counter=0;
  while(Serial.available()==0 && fade>0)
  {
    int bright=0;
    counter++;
    if((counter&0x7)<2)
    {
      bright=0;
    }else
    {
      bright=fade>>3;
    }
    // flash white
    for(int i=0;i<11;i++)
    {
      strip.setPixelColor(i,bright,bright,bright);
    }
    strip.show();
    delay(50);
    fade-=1;
  }
  showIntroAnimation(0);
}


void loop() {
  if(Serial.available()>0)
  {
    
    int data=Serial.read();
    if(data=='\n')
    {
//      Serial.print(val);
//      Serial.print(":");
//      Serial.print(cmd);
//      Serial.println("");
      // run command
      switch(cmd)
      {
        case 't': // title screen animation
          // val = 0 - nothing held
          // val = 1 - left held
          // val = 2 - right held
          showIntroAnimation(val);
          break;
        case 'd': //dead animation
          showDeadAnimation(val);
          break;
        case 'h': //high score animation
          showHighScoreAnimation(val);
          break;
        case 'p': // show power level
          if(val!=-1)
          {
            showPower(val);            
          }else
          {
            showPower(0);
          }
          break;
      }
      cmd=-1;
      val=-1;
    }else if(cmd==-1)
    {
      cmd=data;
    }else if(val==-1)
    {
      val=data;
    }
  }
}

// Fill the dots one after the other with a color
void colorWipe(uint32_t c, uint8_t wait) {
  for(uint16_t i=0; i<strip.numPixels(); i++) {
    strip.setPixelColor(i, c);
    strip.show();
    delay(wait);
  }
}

void rainbow(uint8_t wait) {
  uint16_t i, j;

  for(j=0; j<256; j++) {
    for(i=0; i<strip.numPixels(); i++) {
      strip.setPixelColor(i, Wheel((i+j) & 255));
    }
    strip.show();
    delay(wait);
  }
}

// Slightly different, this makes the rainbow equally distributed throughout
void rainbowCycle(uint8_t wait) {
  uint16_t i, j;

  for(j=0; j<256*5; j++) { // 5 cycles of all colors on wheel
    for(i=0; i< strip.numPixels(); i++) {
      strip.setPixelColor(i, Wheel(((i * 256 / strip.numPixels()) + j) & 255));
    }
    strip.show();
    delay(wait);
  }
}

//Theatre-style crawling lights.
void theaterChase(uint32_t c, uint8_t wait) {
  for (int j=0; j<10; j++) {  //do 10 cycles of chasing
    for (int q=0; q < 3; q++) {
      for (uint16_t i=0; i < strip.numPixels(); i=i+3) {
        strip.setPixelColor(i+q, c);    //turn every third pixel on
      }
      strip.show();

      delay(wait);

      for (uint16_t i=0; i < strip.numPixels(); i=i+3) {
        strip.setPixelColor(i+q, 0);        //turn every third pixel off
      }
    }
  }
}

//Theatre-style crawling lights with rainbow effect
void theaterChaseRainbow(uint8_t wait) {
  for (int j=0; j < 256; j++) {     // cycle all 256 colors in the wheel
    for (int q=0; q < 3; q++) {
      for (uint16_t i=0; i < strip.numPixels(); i=i+3) {
        strip.setPixelColor(i+q, Wheel( (i+j) % 255));    //turn every third pixel on
      }
      strip.show();

      delay(wait);

      for (uint16_t i=0; i < strip.numPixels(); i=i+3) {
        strip.setPixelColor(i+q, 0);        //turn every third pixel off
      }
    }
  }
}

// Input a value 0 to 255 to get a color value.
// The colours are a transition r - g - b - back to r.
uint32_t Wheel(byte WheelPos) {
  WheelPos = 255 - WheelPos;
  if(WheelPos < 85) {
    return strip.Color(255 - WheelPos * 3, 0, WheelPos * 3);
  }
  if(WheelPos < 170) {
    WheelPos -= 85;
    return strip.Color(0, WheelPos * 3, 255 - WheelPos * 3);
  }
  WheelPos -= 170;
  return strip.Color(WheelPos * 3, 255 - WheelPos * 3, 0);
}
