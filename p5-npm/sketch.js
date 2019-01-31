var p5 = require("p5");
var NoSleep = require("nosleep.js");
var Tone = require("tone");
var screenLock;
var osc;

let sketch = function (p)
{
  p.setup = function ()
  {
    p.createCanvas(p.displayWidth, p.displayHeight);
    maxCircle = 0;
    screenLock = new NoSleep();
    p.textSize(45);
    p.text("touch to start", 30, 80);

  };

  p.draw = function ()
  {
    p.fill('black');
    p.ellipse(p.width / 2, p.height / 2, 280);
    maxCircle = p.max(maxCircle, 100 + p.rotationX);
    if (maxCircle > 250)
    {
      p.fill('blue');
    }
    else
    {
      p.fill('red');
    }
    p.ellipse(p.width / 2, p.height / 2, 100 + p.rotationX);
  };

  p.touchStarted = function (event)
  {
    //console.log(event);
    if (event.type == "touchstart" || event.type == "mousedown")
    {
      //console.log(Tone.context.currentTime);
      osc.triggerAttackRelease(p.random(60, 120), 1, Tone.context.currentTime);
      p.background(p.random(0, 255), 0, 0);
      return false;
    }
    return true;
  }

  // one-time user interaction (first touch) to enable fullscreen and lock screen sleep
  document.addEventListener('click', function enableNoSleep()
  {
    document.removeEventListener('click', enableNoSleep, false);
    screenLock.enable();
    //p.fullscreen(true);
    osc = new Tone.FMSynth().toMaster();
    osc.envelope.attack = 0.001;
    p.background('cyan');
    if (navigator.geolocation)
    {
      p.background('yellow');
      navigator.geolocation.getCurrentPosition(geoSuccess, geoError, {
        timeout: 5 * 1000
      });
    }
    else
    {
      p.fill('black');
      p.text("no geo support", 30, 80);
    }
  }, false);

  function geoSuccess(position)
  {
    p.fill('black');
    p.background('green');
    p.text('Lat: ' + nf(position.coords.latitude, 2, 2), 30, 80);
    p.text('Lon: ' + nf(position.coords.longitude, 2, 2), 30, 200);
  }

  function geoError(error)
  {
    p.background('magenta');
    p.text(error.code, 30, 80);
    navigator.vibrate([100, 50, 100, 50]);
  }
};

let instance = new p5(sketch);