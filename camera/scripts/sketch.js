// const {
//   jsonp
// } = require("express/lib/response");

var myCanvas = null;

// Declare kinectron 
var kinectron = null;

// drawHand variables
var start = 30;
var target = 100;
var diameter = start;
var light = 255;
var dark = 100;
var hueValue = light;
var lerpAmt = 0.3;
var state = 'ascending';

var leftFoot = {};
var rightFoot = {};

var lastTimeStamp = new Date().getTime();

function setup() {
  myCanvas = createCanvas(500, 500);
  background(0);
  noStroke();

  // Define and create an instance of kinectron
  var kinectronIpAddress = "127.0.0.1"; // FILL IN YOUR KINECTRON IP ADDRESS HERE
  kinectron = new Kinectron(kinectronIpAddress);

  // Connect with application over peer
  kinectron.makeConnection();

  // Request all tracked bodies and pass data to your callback
  kinectron.startTrackedBodies(bodyTracked);
}

function draw() {

}

function bodyTracked(body) {
  background(0, 20);

  // Get all the joints off the tracked body and do something with them
  kinectron.getJoints(drawJoint);

  // Get the hands off the tracked body and do somethign with them
  kinectron.getHands(drawHands);
}

// Draw skeleton
function drawJoint(joint) {
  // if (joint.jointType == 23 && joint.trackingState == 2) {
  //   console.table(joint)
  // }
  if (!lastTimeStamp) {
    lastTimeStamp = new Date().getTime();
  }
  var currentTimeStamp = new Date().getTime();

  if (joint.jointType == 15) {
    leftFoot = joint;
  }
  if (joint.jointType == 19) {
    rightFoot = joint;
  }
    
    if (lastTimeStamp + 1000 <= currentTimeStamp) {

      sendDataToQuiz(rightFoot);
      sendDataToQuiz(leftFoot);

      lastTimeStamp = currentTimeStamp;
    }
  
  // console.table(joint)

  fill(100);

  // Kinect location data needs to be normalized to canvas size
  ellipse(joint.depthX * myCanvas.width, joint.depthY * myCanvas.height, 15, 15);

  fill(200);

  // Kinect location data needs to be normalized to canvas size
  ellipse(joint.depthX * myCanvas.width, joint.depthY * myCanvas.height, 3, 3);
}

// Draw hands
function drawHands(hands) {

  //check if hands are touching 
  if ((Math.abs(hands.leftHand.depthX - hands.rightHand.depthX) < 0.01) && (Math.abs(hands.leftHand.depthY - hands.rightHand.depthY) < 0.01)) {
    hands.leftHandState = 'clapping';
    hands.rightHandState = 'clapping';
  }

  // draw hand states
  updateHandState(hands.leftHandState, hands.leftHand);
  updateHandState(hands.rightHandState, hands.rightHand);
}

// Find out state of hands
function updateHandState(handState, hand) {
  switch (handState) {
    case 'closed':
      drawHand(hand, 1, 255);
      break;

    case 'open':
      drawHand(hand, 0, 255);
      break;

    case 'lasso':
      drawHand(hand, 0, 255);
      break;

      // Created new state for clapping
    case 'clapping':
      drawHand(hand, 1, 'red');
  }
}

// Draw the hands based on their state
function drawHand(hand, handState, color) {

  if (handState === 1) {
    state = 'ascending';
  }

  if (handState === 0) {
    state = 'descending';
  }

  if (state == 'ascending') {
    diameter = lerp(diameter, target, lerpAmt);
    hueValue = lerp(hueValue, dark, lerpAmt);
  }

  if (state == 'descending') {
    diameter = lerp(diameter, start, lerpAmt);
    hueValue = lerp(hueValue, light, lerpAmt);
  }

  fill(color);

  // Kinect location needs to be normalized to canvas size
  ellipse(hand.depthX * myCanvas.width, hand.depthY * myCanvas.height, diameter, diameter);
}