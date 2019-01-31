var maxCircle;
var noSleep;

function setup()
{
  createCanvas(displayWidth, displayHeight);
  maxCircle = 0;
  noSleep = new NoSleep();
  textSize(45);
  text("touch to start", 30, 80);
}

function draw()
{
  fill('black');
  ellipse(width / 2, height / 2, 280);
  maxCircle = max(maxCircle, 100 + rotationX);
  if (maxCircle > 250)
  {
    fill('blue');
  }
  else
  {
    fill('red');
  }
  ellipse(width / 2, height / 2, 100 + rotationX);
}

// one-time user interaction (first touch) to enable fullscreen and lock screen sleep
document.addEventListener('click', function enableNoSleep()
{
  document.removeEventListener('click', enableNoSleep, false);
  noSleep.enable();
  fullscreen(true);
  background('cyan');
  if (navigator.geolocation)
  {
    background('yellow');
    navigator.geolocation.getCurrentPosition(geoSuccess, geoError, {
      timeout: 5 * 1000
    });
  }
  else
  {
    fill('black');
    text("no geo support", 30, 80);
  }
}, false);

function geoSuccess(position)
{
  fill('black');
  background('green');
  text('Lat: ' + nf(position.coords.latitude, 2, 2), 30, 80);
  text('Lon: ' + nf(position.coords.longitude, 2, 2), 30, 200);
}

function geoError(error)
{
  background('magenta');
  text(error.code, 30, 80);
  navigator.vibrate([100, 50, 100, 50]);
}
