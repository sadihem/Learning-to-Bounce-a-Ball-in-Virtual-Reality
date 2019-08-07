using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
public class physics : MonoBehaviour {
public float thrust;
public Rigidbody rb;
public Rigidbody PaddleRb;
public GameObject tcpNetworking;
public GameObject myPaddle;
public GameObject bubble;
public GameObject my2DWall;
public GameObject diamond;
public GameObject AIMiddle;
public GameObject AIPaddle;
public GameObject AITop;
Vector3 startpos;
public float ylimit = -11;
public float startYvel;
Vector3 ballV;
Vector3 relBallV;
float magnitude;
public float fixedMagnitude;
public bool useFixedMag;
public float angle = 0.0F;
public AudioSource[] audio;
private Vector3 relative;
private Vector3 flipped;
public Vector3 startVelocity;
public float myGravity;
Transform oldTransform;
Transform newTransform;
Vector3 paddleVel;
TrailRenderer myTrail;
public float trailTime;
tcp tcpScript;
bubbleColider bubbleScript;
getRoll myRoll;
int score;
int lastScore;
int highScore;
int level;
int countdown;
public int countDownSpeed;
public int hitsPerLevel;
public float minHitTime;
float hitTime;
int totalHits;
bool restart;
public Material paddleMat;
public Material ballMat;
public Camera myCamera;
Color[] colorArray = new Color[12];
float colorLerp;
public float colorLerpValue;
public TextMesh levelDisplay;
public TextMesh scoreDisplay;
public TextMesh lastScoreDisplay;
public TextMesh highScoreDisplay;
public TextMesh ballDisplay;
int ballsDropped;
public int levelDownBallDrops;
int totalBallsDropped;
public int totalBallLives;
public float startheight = .65f;
public float myTimeScale = 1f;
float roll;
// Start is called at first frame
void Start()
{
initializeSave ();
Time.timeScale = myTimeScale;
level = 1;
colorLerp = 1;
#region COLOR
colorArray[0] = new Color(1, 0, 0);
colorArray[1] = new Color(1, .5f, 1);
colorArray[2] = new Color(1, 1, 0);
colorArray[3] = new Color(.5f, 1, 0);
colorArray[4] = new Color(0, 1, 0);
colorArray[5] = new Color(0, 1, .5f);
colorArray[6] = new Color(0, 1, 1);
colorArray[7] = new Color(0, .5f, 1);
colorArray[8] = new Color(0, 0, 1);
colorArray[9] = new Color(.5f, 0, 1);
colorArray[10] = new Color(1, 0, 1);
colorArray[11] = new Color(1, 0, .5f);
#endregion
restart = true;
rb = GetComponent<Rigidbody>();
PaddleRb = myPaddle.GetComponent<Rigidbody>();
audio = GetComponents<AudioSource>();
myTrail = GetComponent<TrailRenderer>();
tcpScript = tcpNetworking.GetComponent<tcp>();
bubbleScript = bubble.GetComponent<bubbleColider>();
myRoll = myPaddle.GetComponent<getRoll>();
//myCamera = GetComponent<Camera>();
trailTime = myTrail.time;
startpos = transform.position;
//rb.velocity = startVelocity;
newTransform = myPaddle.transform;
totalHits = 0;
hitTime = Time.time;
ballsDropped = 0;

LevelChange (0);
updateBalls (0);
highScore = 0;
}
// Update is called once per frame
void Update ()
{
Vector3 pos = ProjectPointOnPlane(Vector3.up, Vector3.zero, transform.forward);
roll = SignedAngle(myPaddle.transform.right, pos, myPaddle.transform.forward);
Time.timeScale = myTimeScale;
//Temporary color lerp
if (colorLerp <= 1)
{
paddleMat.color = Color. Lerp (colorArray[(level - 1 + 1) % 12], colorArray[(level - 1 + 1 + 1)
% 12], colorLerp);
ballMat.color = Color. Lerp (colorArray[(level - 1 + 4) % 12], colorArray[(level - 1 + 4 + 1) %
12], colorLerp);
myCamera.backgroundColor = Color. Lerp (colorArray[(level - 1 + 8) % 12], colorArray[(level - 1
+ 8 + 1) % 12], colorLerp);
colorLerp = colorLerp + colorLerpValue;
}
//Ball reset
if (transform.position.y < ylimit)
ballreset();
if (Input. GetMouseButtonDown (2))
{
Debug. Log ("saving output");
outputSave();
// ballreset ();
}
if (Input. GetButtonDown ("Set"))
setStartPos(myPaddle.transform.position + Vector3.up * .2f);
if (Input. GetMouseButtonDown (1))
setStartPos(myPaddle.transform.position + Vector3.up * .2f);
}
Vector3 ProjectPointOnPlane (Vector3 planeNormal, Vector3 planePoint, Vector3 point)
{
planeNormal. Normalize ();
float distance = -Vector3. Dot (planeNormal.normalized, (point - planePoint));
return point + planeNormal * distance;
}
float SignedAngle (Vector3 v1, Vector3 v2, Vector3 normal)
{
Vector3 perp = Vector3. Cross (normal, v1);
float angle = Vector3. Angle (v1, v2);
angle *= Mathf. Sign (Vector3. Dot (perp, v2));
return angle;
}
void FixedUpdate ()
{
//Gravity
if (!restart)
rb. AddForce (transform.up * myGravity);
}
void LevelChange (int addLevel)

{
if (level + addLevel < 1)
level = 1;
else
{
if (level < level + addLevel)
{
audio[1]. Play ();
}
else if (level > level + addLevel)
{
audio[2]. Play ();
}
level = level + addLevel;
}
myGravity = -9.81f; //myGravity = -(1 + level / 2);
//startVelocity = Vector3.up * -(myGravity / 1.42f);
startVelocity = Vector3.up * Mathf. Sqrt (Mathf. Abs (2 * myGravity * startheight));
levelDisplay.text = "LEVEL: " + level. ToString ();
colorLerp = 0;
ballsDropped = 0;
}
void LateUpdate ()
{
oldTransform = newTransform;
newTransform = myPaddle.transform;
//Debug. Log ("old : " + oldTransform.position);
//Debug. Log ("new : " + newTransform.position);
paddleVel = (newTransform.position - oldTransform.position) / Time.deltaTime;
//Debug. Log ("PaddleVel : " + paddleVel);
//Debug. Log ("RB PaddlVel: " + PaddleRb.velocity);
}
Vector3 myPaddleVel;
Vector3 myPaddleAng;
public float YballElasticity = 1 ;
public float XZballElasticity = 1 ;
public float YpaddleTransfer = 1 ;
public float XZpaddleTransfer = 1 ;
void OnCollisionEnter (Collision collision)
{
Debug. Log ("Collision entered");
}
Vector3 relativePaddleVel;
void OnTriggerEnter (Collider collision)
{
if (collision. CompareTag ("Diamond"))
{
diamondHit();
}
if (collision. CompareTag ("Paddle"))
{
// Debug. Log ("Time between hits: " + (Time.time - hitTime). ToString ());
if (Time.time - hitTime > minHitTime)
{
totalHits = totalHits + 1;
updateScore(10 * (1 + level / 3));

}
if (totalHits >= (hitsPerLevel + level))
{
totalHits = 0;
LevelChange(1);
}
hitTime = Time.time;
// Debug. Log ("Trigger entered");
// Debug. Log ("PaddleVel : " + paddleVel);
myPaddleVel = tcpScript.paddleVel;
// Debug. Log ("TCP vel :" + myPaddleVel);
//Debug. Break ();
ballV = rb.velocity;
if (useFixedMag)
magnitude = fixedMagnitude;
else
magnitude = ballV.magnitude;
//Flip the velocity relative to the paddle
relative = myPaddle.transform. InverseTransformDirection (ballV);
relativePaddleVel = myPaddle.transform. InverseTransformDirection (myPaddleVel);
//Debug. Log ("Rel P vel :" + relativePaddleVel);
flipped = new Vector3((relative.x * XZballElasticity) + (relativePaddleVel.x *
XZpaddleTransfer), (-relative.y * YballElasticity) + (relativePaddleVel.y * YpaddleTransfer), (relative.z
* XZballElasticity) + (relativePaddleVel.x * XZpaddleTransfer));
#region DEBUG
/*
Debug. Log ("relative : " + relative);
Debug. Log ("unflipped : " + myPaddle.transform. TransformDirection (flipped));
*/
#endregion
audio[0].volume = (.2f + (flipped.magnitude / rb.velocity.magnitude)) * .2f;
//Debug. Log ("change in magnitude ratio " + flipped.magnitude / rb.velocity.magnitude);
string[] collisionInfo = new string[14];
myPaddleAng = tcpScript.paddleAng;
//Debug. Log ("Xang: " + myPaddleAng.x * Mathf. Sign (myPaddle.transform.up.x) + " Xang2: " +
Mathf. Atan (myPaddle.transform.up.y / myPaddle.transform.up.x) * Mathf.Rad2Deg);
collisionInfo[0] = (startpos.x - transform.position.x). ToString (); // "Xball";
collisionInfo[1] = (startpos.z - transform.position.z). ToString ();// "ZBall";
collisionInfo[2] = (rb.velocity.x). ToString ();// "BallVX";
collisionInfo[3] = (rb.velocity.z). ToString ();// "BallVZ";
collisionInfo[4] = (rb.velocity.y). ToString ();// "BallVY";
collisionInfo[5] = (myPaddleAng.x * Mathf. Sign (myPaddle.transform.up.x) - 90). ToString ();//
"PaddleAngX";
collisionInfo[6] = (myPaddleAng.x * Mathf. Sign (myPaddle.transform.up.z) - 90). ToString ();//
"PaddleAngZ";
collisionInfo[7] = (myPaddleVel.x). ToString ();// "PaddleVX";
collisionInfo[8] = (myPaddleVel.z). ToString ();// "PaddleVZ";
collisionInfo[9] = (myPaddleVel.y). ToString ();// "PaddleVY";
collisionInfo[10] = (Mathf. Atan (myPaddle.transform.up.y / myPaddle.transform.up.x) *
Mathf.Rad2Deg). ToString ();// "PaddleAngXv2";
collisionInfo[11] = (myPaddle.transform. TransformDirection (flipped).x). ToString (); //
"BallVXOut";
collisionInfo[12] = (myPaddle.transform. TransformDirection (flipped).z). ToString (); //
"BallVZOut";
collisionInfo[13] = (myPaddle.transform. TransformDirection (flipped).y). ToString (); //
"BallVYOut";
saveData(collisionInfo);
Debug. Log ("PADDLE");
Debug. Log ("Ball X Pos " + (transform.position.x - AIMiddle.transform.position.x));

Debug. Log ("Ball X Vel " + rb.velocity.x);
Debug. Log ("Ball Y Vel " + rb.velocity.y);
Debug. Log ("Ball X Pos Discrete #" + getDiscretePosition((transform.position.x -
AIMiddle.transform.position.x)));
Debug. Log ("Ball X Vel Discrete #" + getDiscreteXVelocity(rb.velocity.x, (transform.position.x
- AIMiddle.transform.position.x)));
Debug. Log ("Ball Y Vel Discrete #" + getDiscreteYVelocity(rb.velocity.y));
rb.velocity = myPaddle.transform. TransformDirection (flipped);
//rb. AddForce (bounceForce * thrust);
//Debug. Log ("audio volume" + audio[0].volume);
// Debug. Log ("hit velocity magnitude" + rb.velocity.magnitude);
//Debug. Log ("hit velocity magnitude / g" + .5f*(rb.velocity.magnitude*rb.velocity.magnitude)
/myGravity);
audio[0]. Play ();
//Get distance to center
//Get change in angle
//Apply angular momentum
//Apply
}
if (collision. CompareTag ("AI"))
{
Debug. Log ("--AI--");
Debug. Log ("Ball X Pos " + (transform.position.x - AIMiddle.transform.position.x));
Debug. Log ("Ball X Vel " + rb.velocity.x);
Debug. Log ("Ball Y Vel " + rb.velocity.y);
int xpos = getDiscretePosition((transform.position.x - AIMiddle.transform.position.x));
int xvel = getDiscreteXVelocity(rb.velocity.x, (transform.position.x -
AIMiddle.transform.position.x));
int yvel = getDiscreteYVelocity(rb.velocity.y);
Debug. Log ("Ball X Pos Discrete #" + getDiscretePosition((transform.position.x -
AIMiddle.transform.position.x)));
Debug. Log ("Ball X Vel Discrete #" + getDiscreteXVelocity(rb.velocity.x, (transform.position.x
- AIMiddle.transform.position.x)));
Debug. Log ("Ball Y Vel Discrete #" + getDiscreteYVelocity(rb.velocity.y));
String actions = getAction(xpos, xvel, yvel);
Debug. Log ("Optimal Actions" + actions);
float[] paddleAngles = new float[5] {0, 3, 7, 10, 30};
float[] paddleXVelocities = new float[5] { -.3f, -.1f, 0, .1f, .3f };
float[] paddleYVelocities = new float[5] { 0, .1f, .15f, .3f, 1 };
//float paddleAng = paddleAngles[actions[0]].
Debug. Log (actions.Length);
Debug. Log ("Action to take " + paddleAngles[actions[1] - 1 - '0'] + " " +
paddleXVelocities[actions[2] - 1 - '0'] + " " + paddleYVelocities[actions[3] - 1 - '0']);

AIPaddle.transform.position = new Vector3(transform.position.x, AIPaddle.transform.position.y,
AIPaddle.transform.position.z);
//bubble.transform.position = new Vector3(startpos.x, myPaddle.transform.position.y + .2f,
startpos.z)
AITop.transform.localRotation = Quaternion. Euler (new Vector3(AITop.transform.rotation.x,
AITop.transform.rotation.y, paddleAngles[actions[1] - 1 - '0'] * Math. Sign ((transform.position.x -
AIMiddle.transform.position.x))));
float AIxvel = paddleXVelocities[actions[2] - 1 - '0'];
float AIyvel = paddleYVelocities[actions[3] - 1 - '0'];
ballV = rb.velocity;
//Flip the velocity relative to the paddle
relative = AITop.transform. InverseTransformDirection (ballV);
relativePaddleVel = myPaddle.transform. InverseTransformDirection (myPaddleVel);
//Debug. Log ("Rel P vel :" + relativePaddleVel);
flipped = new Vector3((relative.x * XZballElasticity) + (AIxvel * XZpaddleTransfer),
(-relative.y * YballElasticity) + (AIyvel * YpaddleTransfer), (relative.z * XZballElasticity) + (AIxvel *
XZpaddleTransfer));
rb.velocity = myPaddle.transform. TransformDirection (flipped);
}
}
void updateScore (int addScore)
{
score = score + addScore;
scoreDisplay.text = "SCORE: " + score. ToString ();
}
void updateBalls (int addBalls)
{
totalBallsDropped = totalBallsDropped - addBalls;
int ballsLeft = (totalBallLives - totalBallsDropped);
ballDisplay.text = "BALLS: " + ballsLeft. ToString ();
}
public void playPop ()
{
audio[3]. Play ();
}
//Sets the ball back to its starting pos after it falls
void ballreset ()
{
totalHits = 0;
bubbleScript. createBubble ();
restart = true;
ballsDropped = ballsDropped + 1;
updateBalls(-1);
if (ballsDropped > levelDownBallDrops)
{
LevelChange(-1);
}
if (totalBallsDropped >= totalBallLives)
{
gameOver();
46
}
myTrail.time = 0;
transform.position = new Vector3(startpos.x, myPaddle.transform.position.y + .2f, startpos.z);
bubble.transform.position = new Vector3(startpos.x, myPaddle.transform.position.y + .2f,
startpos.z);
rb.velocity = Vector3.zero;
//
myTrail.time = trailTime;
}
public void ballstart ()
{
rb.velocity = startVelocity;
restart = false;
setDiamondLoc();
}
void gameOver ()
{
audio[4]. Play ();
lastScore = score;
lastScoreDisplay.text = "LAST\nSCORE\n" + lastScore. ToString ();
score = 0;
scoreDisplay.text = "SCORE: " + score. ToString ();
if (highScore < lastScore)
{
audio[5]. Play ();
highScore = lastScore;
highScoreDisplay.text = "HIGH\nSCORE\n" + highScore. ToString ();
}
totalBallsDropped = 0;
ballDisplay.text = "BALLS: " + totalBallLives. ToString ();
level = 1;
myGravity = -9.81f;// myGravity = -(1 + level / 2);
startVelocity = Vector3.up * Mathf. Sqrt (Mathf. Abs (2 * myGravity * startheight));
levelDisplay.text = "LEVEL: " + level. ToString ();
}
void setStartPos (Vector3 location)
{
//Debug. Log ("Start location set: " + location);
bubble.transform.position = location;
my2DWall.transform.position = location;
transform.position = location;
startpos = location;
}
void diamondHit ()
{
updateScore(15 * (1 + level));
audio[6]. Play ();
setDiamondLoc();
}
void setDiamondLoc ()
{
float randMag;
float randTheta;
randMag = UnityEngine.Random. Range (.05F, .13F);
randTheta = UnityEngine.Random. Range (0, 360F);
float randX = randMag * Mathf. Cos (randTheta);

float randZ = randMag * Mathf. Sin (randTheta);
float randY = UnityEngine.Random. Range (.13F, .4F);
diamond.transform.position = new Vector3(startpos.x + randX, startpos.y + randY, startpos.z +
randZ);
}
private List<string[]> rowData = new List<string[]>();
string[] rowDataTemp = new string[ 14 ];
void initializeSave ()
{
// Creating First row of titles manually..
rowDataTemp[0] = "Xball";
rowDataTemp[1] = "ZBall";
rowDataTemp[2] = "BallVX";
rowDataTemp[3] = "BallVZ";
rowDataTemp[4] = "BallVY";
rowDataTemp[5] = "PaddleAngX";
rowDataTemp[6] = "PaddleAngZ";
rowDataTemp[7] = "PaddleVX";
rowDataTemp[8] = "PaddleVZ";
rowDataTemp[9] = "PaddleVY";
rowDataTemp[10] = "PaddleAngXv2";
rowDataTemp[11] = "BallVXOut";
rowDataTemp[12] = "BallVZOut";
rowDataTemp[13] = "BallVYOut";
rowData. Add (rowDataTemp);
}
void saveData (string[] input) {
rowData. Add (input);
}
void outputSave ()
{
// You can add up the values in as many cells as you want.
string[][] output = new string[rowData.Count][];
for (int i = 0; i < output.Length; i++)
{
output[i] = rowData[i];
}
int length = output. GetLength (0);
string delimiter = ",";
StringBuilder sb = new StringBuilder();
for (int index = 0; index < length; index++)
sb. AppendLine (string. Join (delimiter, output[index]));
string filePath = getPath();
Debug. Log ("Output File Path: " + filePath);
StreamWriter outStream = System.IO.File. CreateText (filePath);
outStream. WriteLine (sb);
outStream. Close ();
}
// Following method is used to retrive the relative path as device platform
private string getPath ()
{

# if UNITY_EDITOR
return Application.dataPath + "/CSV/" + "Saved_data.csv";
#elif UNITY_ANDROID
return Application.persistentDataPath+"Saved_data.csv";
#elif UNITY_IPHONE
return Application.persistentDataPath+"/"+"Saved_data.csv";
#else
return Application.dataPath +"/"+"Saved_data.csv";
# endif
}
/*
float getRoll(Vector3 originalTransform)
{
GameObject tempGO = new GameObject();
Transform t = tempGO.transform;
t.localRotation = Quaternion.Euler(new Vector3(originalTransform.x, originalTransform.y,
originalTransform.z));
t.Rotate(0, 0, t.localEulerAngles.z * -1);
GameObject.Destroy(tempGO);
return t.localEulerAngles.x;
}
*/
int getDiscretePosition (float xballhit)
{
xballhit = 5 + xballhit * 10;
int Sxballhit = 6;
if (0.5 > xballhit && xballhit >= 0)
Sxballhit = 5;
else if (1.5 > xballhit && xballhit >= 0.5)
Sxballhit = 4;
else if (2.5 > xballhit && xballhit >= 1.5)
Sxballhit = 3;
else if (3.5 > xballhit && xballhit >= 2.5)
Sxballhit = 2;
else if (4.5 > xballhit && xballhit >= 3.5)
Sxballhit = 1;
else if (5.5 > xballhit && xballhit >= 4.5)
Sxballhit = 6;
else if (6.5 > xballhit && xballhit >= 5.5)
Sxballhit = 1;
else if (7.5 > xballhit && xballhit >= 6.5)
Sxballhit = 2;
else if (8.5 > xballhit && xballhit >= 7.5)
Sxballhit = 3;
else if (9.5 > xballhit && xballhit >= 8.5)
Sxballhit = 4;
else if (10 > xballhit && xballhit >= 9.5)
Sxballhit = 5;
return Sxballhit;
}
int getDiscreteXVelocity (float ballvx, float xballhit)
{
int Sballvx = 11;
float leftlimit = 0;
float rightlimit = 10;
float ballvxTemp;
if (xballhit >= (leftlimit + rightlimit) / 2)
49
{
ballvxTemp = ballvx * 5;
if (ballvxTemp < -4.5)
Sballvx = 11;
else if (-4.5 <= ballvxTemp && ballvxTemp < -3.5)
Sballvx = 10;
else if (-3.5 <= ballvxTemp && ballvxTemp < -2.5)
Sballvx = 9;
else if (-2.5 <= ballvxTemp && ballvxTemp < -1.5)
Sballvx = 8;
else if (-1.5 <= ballvxTemp && ballvxTemp < -.5)
Sballvx = 7;
else if (-.5 <= ballvxTemp && ballvxTemp < .5)
Sballvx = 6;
else if (.5 <= ballvxTemp && ballvxTemp < 1.5)
Sballvx = 5;
else if (1.5 <= ballvxTemp && ballvxTemp < 2.5)
Sballvx = 4;
else if (2.5 <= ballvxTemp && ballvxTemp < 3.5)
Sballvx = 3;
else if (3.5 <= ballvxTemp && ballvxTemp < 4.5)
Sballvx = 2;
else if (4.5 < ballvxTemp || ballvxTemp == 4.5)
Sballvx = 1;
}
if (xballhit < (leftlimit + rightlimit) / 2)
{
ballvxTemp = ballvx * 10;
if (ballvxTemp < -4.5)
Sballvx = 1;
else if (-4.5 <= ballvxTemp && ballvxTemp < -3.5)
Sballvx = 2;
else if (-3.5 <= ballvxTemp && ballvxTemp < -2.5)
Sballvx = 3;
else if (-2.5 <= ballvxTemp && ballvxTemp < -1.5)
Sballvx = 4;
else if (-1.5 <= ballvxTemp && ballvxTemp < -.5)
Sballvx = 5;
else if (-.5 <= ballvxTemp && ballvxTemp < .5)

Sballvx = 6;
else if (.5 <= ballvxTemp && ballvxTemp < 1.5)
Sballvx = 7;
else if (1.5 <= ballvxTemp && ballvxTemp < 2.5)
Sballvx = 8;
else if (2.5 <= ballvxTemp && ballvxTemp < 3.5)
Sballvx = 9;
else if (3.5 <= ballvxTemp && ballvxTemp < 4.5)
Sballvx = 10;
else if (4.5 < ballvxTemp || ballvxTemp == 4.5)
Sballvx = 11;
}
return Sballvx;
}
int getDiscreteYVelocity (float ballvy)
{
int Sballvy = 6;
float ballvyTemp;
ballvyTemp = ballvy * 2;
if (-1.5 <= ballvyTemp && ballvyTemp < -.5)
Sballvy = 5;
else if (-2.5 <= ballvyTemp && ballvyTemp < -1.5)
Sballvy = 4;
else if (-3.5 <= ballvyTemp && ballvyTemp < -2.5)
Sballvy = 3;
else if (-4.5 <= ballvyTemp && ballvyTemp < -3.5)
Sballvy = 2;
else if (ballvyTemp< -4.5)
Sballvy = 1;
return Sballvy;
}
String getAction (int xpos, int xvel, int yvel)
{
string[] actions = new string[] { " 515", " 515", " 515", " 515", " 515", " 455", " 415", " 415",
" 415", " 515", " 515", " 455", " 415", " 415", " 415", " 515", " 515", " 455", " 415", " 415", " 415", "
415", " 515", " 455", " 315", " 315", " 315", " 415", " 415", " 445", " 215", " 215", " 215", " 315", "
415", " 415", " 215", " 115", " 115", " 115", " 115", " 345", " 115", " 115", " 115", " 115", " 115", "
315", " 115", " 115", " 115", " 115", " 115", " 315", " 115", " 115", " 115", " 115", " 115", " 315", "
115", " 115", " 115", " 115", " 115", " 555", " 515", " 515", " 515", " 515", " 515", " 455", " 415", "
415", " 415", " 515", " 515", " 455", " 415", " 415", " 415", " 515", " 515", " 455", " 415", " 415", "
415", " 415", " 515", " 455", " 315", " 315", " 315", " 415", " 415", " 445", " 215", " 215", " 215", "
315", " 415", " 415", " 215", " 115", " 115", " 115", " 115", " 345", " 115", " 115", " 115", " 115", "
115", " 315", " 115", " 115", " 115", " 115", " 115", " 315", " 115", " 115", " 115", " 115", " 115", "
315", " 115", " 115", " 115", " 115", " 115", " 555", " 515", " 515", " 515", " 515", " 515", " 455", "
51
415", " 415", " 415", " 515", " 515", " 455", " 415", " 415", " 415", " 515", " 515", " 455", " 415", "
415", " 415", " 415", " 515", " 455", " 315", " 315", " 315", " 415", " 415", " 445", " 215", " 215", "
215", " 315", " 415", " 415", " 215", " 115", " 115", " 115", " 115", " 345", " 115", " 115", " 115", "
115", " 115", " 315", " 115", " 115", " 115", " 115", " 115", " 315", " 115", " 115", " 115", " 115", "
115", " 315", " 115", " 115", " 115", " 115", " 115", " 555", " 515", " 515", " 515", " 515", " 515", "
455", " 415", " 415", " 415", " 515", " 515", " 455", " 415", " 415", " 415", " 515", " 515", " 455", "
415", " 415", " 415", " 415", " 515", " 455", " 315", " 315", " 315", " 415", " 415", " 445", " 215", "
215", " 215", " 315", " 415", " 415", " 215", " 115", " 115", " 115", " 115", " 345", " 115", " 115", "
115", " 115", " 115", " 315", " 115", " 115", " 115", " 115", " 115", " 315", " 115", " 115", " 115", "
115", " 115", " 315", " 115", " 115", " 115", " 115", " 115", " 515", " 515", " 515", " 515", " 515", "
111", " 455", " 415", " 515", " 515", " 515", " 515", " 455", " 415", " 415", " 515", " 515", " 515", "
455", " 415", " 415", " 415", " 515", " 515", " 455", " 315", " 315", " 315", " 415", " 415", " 445", "
215", " 215", " 215", " 315", " 415", " 415", " 215", " 115", " 115", " 115", " 115", " 345", " 115", "
115", " 115", " 115", " 115", " 315", " 115", " 115", " 115", " 115", " 115", " 315", " 115", " 115", "
115", " 115", " 115", " 215", " 115", " 115", " 115", " 115", " 115", " 444", " 515", " 515", " 515", "
515", " 515", " 355", " 415", " 415", " 415", " 515", " 515", " 455", " 315", " 415", " 415", " 415", "
515", " 455", " 315", " 315", " 415", " 415", " 515", " 455", " 215", " 215", " 315", " 315", " 415", "
445", " 135", " 135", " 135", " 215", " 315", " 355", " 155", " 155", " 155", " 155", " 155", " 345", "
155", " 155", " 155", " 155", " 155", " 315", " 155", " 155", " 155", " 155", " 155", " 255", " 155", "
155", " 155", " 155", " 155", " 255", " 155", " 155", " 155", " 155", " 155" };
int myindex = (xpos - 1) * 11 * 6 + (xvel - 1) * 6 + yvel - 1;
return actions[myindex];
}
}