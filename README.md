# WeAreNotNumbers 2
Physical pixels 100 servo motors controlled by an Unity3D Android app for Internet Festival 2016.
![alt tag](https://dl.dropboxusercontent.com/u/10907181/githubimages/wearenotnumbers/3.jpg)
# Concept
Starting from the idea to tell the emotions expressed on some social networks, We Are Not Numbers II is an interactive kinetic installation that creates a connection with the user who is invited to create a visual pattern through an Android application placed alongside the installation. The texture created will be displayed in real time on 100 physical pixels represented by the servo motors installed on it, through a hardware and open source software. The composition of the pattern is a grid of 100 inclined wooden pieces of 45 ° or -45 °. Each user then compose different views and then will be posted each account facebook We Are Not Numbers in real time.
[Internet Festival Link](http://www.internetfestival.it/en/eventi/we-are-not-numbers-ii/) 

# Tech Rider
* 1 Arduino UNO rev3
* 1 EthernetShield
* 7 ADAFRUIT 16-CHANNEL PWM/SERVO SHIELD
* 100 SERVO MOTORS
* 1 LINUX UBUNTU 16.04 LTS 
* 7 POWER SUPPLY 5V 16A

![alt tag](https://dl.dropboxusercontent.com/u/10907181/githubimages/wearenotnumbers/4.jpg)
![alt tag](https://dl.dropboxusercontent.com/u/10907181/githubimages/wearenotnumbers/5.jpg)
![alt tag](https://dl.dropboxusercontent.com/u/10907181/githubimages/wearenotnumbers/6.jpg)

# Unity3D Linux Editor
The project was developed by using Unity3D Linux Editor and basically I create a simple matrix of gameobjects which will be send later to the pyshical screen
```C#
void Start () 
	{
		grid = new GameObject[patternResolution,patternResolution];
		status = new bool[patternResolution,patternResolution];

		for(int i = 0; i < patternResolution; i++)
		{
			for(int j = 0; j < patternResolution; j++)
			{
				grid[i,j] = Instantiate(quad, new Vector3(i * gap, j * gap, 0), Quaternion.identity) as GameObject;
				grid[i,j].transform.localRotation = Quaternion.Euler(new Vector3(0,0,rot));
				grid[i,j].transform.parent = this.transform;
			}
		}
		this.transform.localRotation = Quaternion.Euler( new Vector3(0,0,-90));
		this.transform.localPosition = new Vector3(-3.61f,26.5f,0);
	}
```
Every time I touch each wood strip on the screen with the mouse left button with ScreenPointToRay I save the value of the rotation and I trigger to 45° -45°
```C#
ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
		{
			for(int i =0; i < patternResolution; i++)
			{
				for(int j=0; j < patternResolution; j++)
				{
					if(hit.collider.gameObject == grid[i,j].gameObject)
					{
						indexX = j;
						indexY = i;
					}
				}
			}
			if(hit.collider.gameObject.transform.localRotation.eulerAngles.z > 180)
			{
				rot=45;
				valueMax =0;
			}
			else
			{
				rot=-45;
				valueMax = 1;
			}
			
hit.collider.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0,0,(int)rot));
iShield = ( ( patternResolution * indexY) + indexX) / 16;
jServo = (((patternResolution * indexY) + indexX ) - (iShield * 16)) ;
```
Finally we are ready to send the value to Arduino by using the OSC protocol
```C#
OscSendReceiver.SingleServo(iShield,jServo,valueMax);
```
SingleServo is a public static funtion from OscSendReceiver.cs script. I send the message "/unity/single " and the 3 osc values oscM.Values.Add(iShield) , oscM.Values.Add(jServo) , oscM.Values.Add(MaxValue) which will be decoded by Arduino software.

```C#
	public static void SingleServo(int iShield, int jServo, int MaxValue)
	{
		string msg = "/unity/single ";
		OscMessage oscM = Osc.StringToOscMessage(msg);
		oscM.Values.Add(iShield);
		oscM.Values.Add(jServo);
		oscM.Values.Add(MaxValue);
		handler.Send(oscM);
	}

```
# Arduino
![alt tag](https://dl.dropboxusercontent.com/u/10907181/githubimages/wearenotnumbers/8.jpg)


# Mobile Android User Interface
![alt tag](https://dl.dropboxusercontent.com/u/10907181/githubimages/wearenotnumbers/1.jpg)
![alt tag](https://dl.dropboxusercontent.com/u/10907181/githubimages/wearenotnumbers/2.jpg)




