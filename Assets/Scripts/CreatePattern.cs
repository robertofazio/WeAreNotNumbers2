using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Threading;

public class CreatePattern : MonoBehaviour 
{
	public static int patternResolution = 10;
	public static float gap = 3f;
	public GameObject quad;
	public static GameObject[,] grid;

	public float rot = 45.0f;
	public static bool[,] status;

	//public float rot = -45.0f;
	Ray ray;
	RaycastHit hit;

	int counterX = 0;
	int counterY= -1;

	public static string message;



	int indexX = 0;
	int indexY = 0;


	int iShield = 0;
	int jServo = 0;
	int valueMax = 0;

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
		
	void Update () 
	{
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

			OscSendReceiver.SingleServo(iShield,jServo,valueMax);

		}

	}









//		if(Input.GetKeyDown(KeyCode.Alpha0))
//		{
//			if(counterY != -1 )
//				grid[counterX,counterY].transform.localRotation = Quaternion.Euler(0,0,45);
//			counterY ++;
//			if(counterY > patternResolution-1)
//			{
//				counterY =0;
//				counterX ++;
//			}
//			if(counterX > patternResolution -1)
//				counterX=0;
//			grid[counterX,counterY].transform.localRotation = Quaternion.Euler(0,0,-45);
//			ComposeAndSend();
//		}




//	public void ComposeAndSend()
//	{
//		message = "";
//		for(int i = 0; i < patternResolution; i++)
//		{
//			for(int j = 0; j < patternResolution; j++)
//			{
//				if(grid[i,j].gameObject.transform.localRotation.eulerAngles.z > 180)
//					message+= "1";
//				else
//					message += "0";
//			}
//		}
//
//		print(message);
//		//OscSendReceiver.ResetServo();
//
//		if(message == "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")
//			OscSendReceiver.ResetServo();
//		else
//		{
//
//			//	Thread.Sleep(1000);
//
//
//			string firstPart = message.Substring (0, 70);
//			string secondPart = message.Substring (70, 30);
//
//			OscSendReceiver.PixelsPackageOSC(firstPart);
//			//HACK antepondo un carattere non numerico al secondo pacchetto. Mistero!
//			OscSendReceiver.PixelsPackageOSC("a" + secondPart);
//		}
//	}
		
	public void ResetButton()
	{
		for(int i = 0; i < patternResolution; i++)
		{
			for(int j = 0; j < patternResolution; j++)
			{
				grid[i,j].transform.localRotation = Quaternion.Euler(new Vector3(0,0,45));

			}
		}
		OscSendReceiver.ResetServo();
	}

}
