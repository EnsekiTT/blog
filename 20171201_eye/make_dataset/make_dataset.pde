import processing.video.*;

Capture cam;
int r = 30;
int col = 4;
int row = 6;
int xp = 0;
int yp = 0;
int count = 0;
int cam_w, cam_h;
int area_w = 210;
int area_h = 210;
int shot_frame = 60;
int wait_frame = 30;
int wait = 0;
String hash = "";
boolean pause = false;

void setup() {
  fullScreen();
  frameRate(10);
  String[] cams = Capture.list();
  cam = new Capture(this, cams[0]);
  cam.start();
  cam_w = cam.width;
  cam_h = cam.height;
  delay(int(random(100)));
  String yy = str(year());
  String mm = str(month());
  String dd = str(day());
  String HH = str(hour());
  String MM = str(minute());
  String SS = str(second());
  String mi = str(millis()%1000);
  String TS = yy+mm+dd+HH+MM+SS+mi;
  
  byte[] md5hash = messageDigest(TS,"MD5");
  for(int i=0; i<md5hash.length-10; i++) hash+=hex(md5hash[i],2);
  print(hash);
}

void draw() {
  if(pause){
    fill(20);
    rect(0, 0, width, height);
    fill(200,0,0);
    textSize(30);
    text("Pause! Restart => S|s", width/2-100, height/2, 200, 100);
    return; 
  }
  fill(255);
  noStroke();
  rect(0, 0, width, height);
  fill(10);
  noStroke();
  ellipse(width/(row) * xp, height/(col) * yp, r, r);
  if(count > shot_frame){
    count = 0;
    wait = 0;
    xp = xp + 1;
    if(xp > row){
      xp = 0;
      yp = yp + 1;
      if(yp > col+1){
        exit();
      }
    }
  }
  if(wait < wait_frame){
    wait = wait + 1;
    if(cam.available()){
      cam.read();
      PImage img = cam.copy();
      img.resize(img.width/2, img.height/2);
      img = img.get(img.width/2-area_w/2, img.height/2-area_h/2, area_w, area_h);
      set(width/2-img.width/2, height/2-img.height/2, img);
    }
    return;
  }
  if(cam.available()){
    cam.read();
    PImage img = cam.copy();
    img.resize(img.width/2, img.height/2);
    img = img.get(img.width/2-area_w/2, img.height/2-area_h/2, area_w, area_h);
    String pos = xp + "_" + yp + "/";
    String path  = "/Users/ensekitt/dev/blog/20171201_eye/datas/" + pos + hash + '_' + count + ".jpg";
    println(path);
    img.save(path);
    count++;
  }
}

void keyPressed() {
  if (key == 'P' || key == 'p') {
    pause = true;
  }  else if (key == 'S' || key == 's') {
    pause = false;
  }
}

byte[] messageDigest(String message, String algorithm) {
  try {
  java.security.MessageDigest md = java.security.MessageDigest.getInstance(algorithm);
  md.update(message.getBytes());
  return md.digest();
  } catch(java.security.NoSuchAlgorithmException e) {
    println(e.getMessage());
    return null;
  }
} 