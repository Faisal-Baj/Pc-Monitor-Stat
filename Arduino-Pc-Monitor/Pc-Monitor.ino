#include <MCUFRIEND_kbv.h>
#include <Adafruit_GFX.h>
#include <SPI.h>
#include <Wire.h>

MCUFRIEND_kbv tft;

const int headerOffset = 30;             // Space reserved for the header at the top

// Timeouts:
const unsigned long NA_TIMEOUT    = 3000;    // 3 seconds until we update the display with "N/A"
const unsigned long SLEEP_TIMEOUT = 300000;  // 5 minutes (300000 ms) until we put the display to sleep

unsigned long lastReceiveTime = 0;       // Time stamp of the last received serial data
bool displayAwake = true;                // Global flag to track whether the display is awake

void displaySleep() {
  tft.pushCommand(0x10, (uint8_t *)0, 0); // Sleep In command
  delay(120);
}

void displayWake() {
  tft.pushCommand(0x11, (uint8_t *)0, 0); // Sleep Out command
  delay(120);
}

void setup() {
  Serial.begin(9600);
  tft.begin(tft.readID());
  tft.setRotation(1);
  tft.fillScreen(0x4208); 
  tft.invertDisplay(1);
  drawLayout();
  lastReceiveTime = millis();
}

void loop() {
  if (Serial.available() > 0) {
    // If the display is asleep, wake it up and redraw the layout.
    if (!displayAwake) {
      displayWake();
      displayAwake = true;
      tft.fillScreen(0x4208);
      tft.invertDisplay(1);
      drawLayout();
    }
    
    String data = Serial.readStringUntil('\n');
    lastReceiveTime = millis();
    
    // Extract values
    float cpuTemp = extractValue(data, 0);
    float cpuLoad = extractValue(data, 1);
    float gpuTemp = extractValue(data, 2);
    float gpuLoad = extractValue(data, 3);
    float ramUsage = extractValue(data, 4);

    // Update the displayed values.
    updateValue(cpuTemp, 95, headerOffset + 100, " C");
    updateValue(cpuLoad, 95, headerOffset + 140, " %");
    updateValue(gpuTemp, 320, headerOffset + 100, " C");
    updateValue(gpuLoad, 320, headerOffset + 140, " %");
    updateValue(ramUsage, 210, headerOffset + 235, " %");
  }
  else {
    unsigned long elapsed = millis() - lastReceiveTime;

    // If no data for more than NA_TIMEOUT but less than SLEEP_TIMEOUT, update values to "N/A".
    if (elapsed > NA_TIMEOUT && elapsed <= SLEEP_TIMEOUT) {
      updateValue(-1, 95, headerOffset + 100, " C");
      updateValue(-1, 95, headerOffset + 140, " %");
      updateValue(-1, 320, headerOffset + 100, " C");
      updateValue(-1, 320, headerOffset + 140, " %");
      updateValue(-1, 210, headerOffset + 235, " %");
    }
    // If no data for longer than SLEEP_TIMEOUT and the display is still awake, put it to sleep.
    if (elapsed > SLEEP_TIMEOUT && displayAwake) {
      displaySleep();
      displayAwake = false;
    }
  }
}


void drawHeader() {
  tft.setTextSize(3);
  tft.setTextColor(0xFFE0);
  int16_t screenWidth = tft.width();
  int textWidth = 15 * 18;  
  int x = (screenWidth - textWidth) / 2;
  tft.setCursor(x, 5);
  tft.print("Pc Stat Monitor");
}

void drawLayout() {
  drawHeader();

  // Draw CPU section
  tft.drawRect(30, headerOffset + 10, 200, 200, 0x0000);
  tft.setCursor(40, headerOffset + 30);
  tft.setTextColor(0xFFFF);
  tft.setTextSize(3);
  tft.print("CPU");
  tft.setCursor(114, headerOffset + 38);
  tft.setTextSize(1);
  tft.setTextColor(0x1d3d);
  tft.print("Intel i7-9700K");

  // Draw GPU section
  tft.drawRect(250, headerOffset + 10, 200, 200, 0x0000);
  tft.setCursor(260, headerOffset + 30);
  tft.setTextColor(0xFFFF);
  tft.setTextSize(3);
  tft.print("GPU");
  tft.setCursor(334, headerOffset + 38);
  tft.setTextSize(1);
  tft.setTextColor(0x1d3d);
  tft.print("RTX 2070 SUPER");

  // Draw RAM section
  tft.drawRect(30, headerOffset + 215, 420, 70, 0x0000);
  tft.setCursor(120, headerOffset + 230);
  tft.setTextColor(0xFFFF);
  tft.setTextSize(3);
  tft.print("RAM");
}

// Updates a value on the display. If value is negative, "N/A" is printed.
void updateValue(float value, int x, int y, String unit) {
  tft.setTextSize(2);
  tft.setTextColor(0x001F);
  // Clear the area
  tft.fillRect(x, y, 80, 20, 0x4208);  
  tft.setCursor(x, y);
  if (value >= 0) {
    tft.print(value, 1);
    tft.print(unit);
  } 
  else {
    tft.print("N/A");
  }
}

float extractValue(String data, int index) {
  int startIndex = 0;
  for (int i = 0; i < index; i++) {
    startIndex = data.indexOf(' ', startIndex) + 1;
    if (startIndex == 0)
      return -1;
  }
  int endIndex = data.indexOf(' ', startIndex);
  if (endIndex == -1)
    endIndex = data.length();

  return data.substring(startIndex, endIndex).toFloat();
}
