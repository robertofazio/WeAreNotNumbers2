/* ***************************************************************************
 * Roberto Fazio Studio 2016 
 * Last Update: 28.06.2016
 * 
 * ***************************************************************************
 */
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;

public class OscSendReceiver : MonoBehaviour 
{
	public string	        	RemoteIP;
	public static int 			SendToPort; // Sendo To Arduino UNO Ethernet 
	public static int 			ListenerPort = 12000;
	public static Osc 			handler;
	public static UDPPacketIO 	udp;
	public  string 				address;
	public  string    			values;

	void Awake()
	{
		udp = gameObject.AddComponent<UDPPacketIO>() as UDPPacketIO;
		handler = gameObject.AddComponent<Osc>() as Osc;
	}

	void Start () 
	{	
		RemoteIP = "192.168.31.141";
		SendToPort = 10000;

		udp.init(RemoteIP, SendToPort, ListenerPort);
		handler.init(udp);

		// handler for a specific event
	//	handler.SetAddressHandler("/ard/aaa", FromArduino);

		// set a handler for all incoming messages
		handler.SetAllMessageHandler(ListenAllEvent);

		print("remote ip: " + RemoteIP + " port: " + SendToPort);

	}

//	public void FromArduino(OscMessage oscMessage)
//	{	
//		address = oscMessage.Address;
//
//		if(address == "/ard/aaa")
//		{
//			print(oscMessage.Address + " : " + oscMessage.Values[0]);
//		}
//
//	} 

	// Send 100 pixels encoded to Arduino
//	public static void PixelsPackageOSC(string EncodedPixels)
//	{
//		if (CreatePattern.message != null) 
//		{
//			string msg = "/unity/values " + EncodedPixels;
//			OscMessage oscM = Osc.StringToOscMessage(msg);
//			handler.Send(oscM);
//			//print("osc msg  sended: " + msg);
//
//		}
//	}

	public static void SingleServo(int iShield, int jServo, int MaxValue)
	{
			string msg = "/unity/single ";
			OscMessage oscM = Osc.StringToOscMessage(msg);
			oscM.Values.Add(iShield);
			oscM.Values.Add(jServo);
			oscM.Values.Add(MaxValue);
			handler.Send(oscM);
	}

	public static void ResetServo()
	{
		OscMessage oscM = Osc.StringToOscMessage ("/unity/reset ");
		handler.Send(oscM);
		print ("OSC sent: ResetServo");
	}
//
//	public static void AllServo()
//	{
//		OscMessage oscM = Osc.StringToOscMessage ("/unity/allOn ");
//		handler.Send(oscM);
//		print ("OSC sent: All ON Servo");
//	}

//	public static void HardcodeToArduino()
//	{
//		int rnd = UnityEngine.Random.Range (0, 13);
//		string myMess = "/unity/hardcode " + rnd;
//
//		OscMessage oscM = Osc.StringToOscMessage (myMess);
//
//		handler.Send (oscM);
//		print(oscM.Address + oscM.Values[0]);
//
//	}


	void Update()
	{
		if (CreatePattern.message != null) 
		{
			// Test sender check frame
//			if (Input.GetKeyDown (KeyCode.Alpha1)) 
//			{
//				string prima = "1111111111111111111111111111111111111111111111111111111111111111111111";
//				string seconda = "111111111111111111111111111111";
//				OscSendReceiver.PixelsPackageOSC (prima);
//				OscSendReceiver.PixelsPackageOSC ("a" + seconda);
//				print ("lenght: " + prima.Length + " e " + seconda.Length); 
//
//			}

			if (Input.GetKeyDown (KeyCode.R)) 
			{
				ResetServo ();
			}

		/*	
		  if (Input.GetKeyDown (KeyCode.Alpha0)) 
			{
				string prima = "0000000000000000000000000000000000000000000000000000000000000000000000";
				string seconda = "000000000000000000000000000000";
			    OscSendReceiver.PixelsPackageOSC (prima);
			    OscSendReceiver.PixelsPackageOSC ("a" + seconda);
				print ("lenght: " + prima.Length + " e " + seconda.Length); 


			}

			if (Input.GetKeyDown (KeyCode.C)) 
			{
				string prima = "0000000000000000000000000000000000000000000000000000000000000000000000";
				string seconda = "000000000000000000000000000000";

				//char[] _prima = prima.ToCharArray ();
				//char[] _seconda = seconda.ToCharArray ();

				if (LoadEncode.data != null) 
				{
					//char[] send = _prima + _seconda;

					//string msg = "/unity/values " + EncodedPixels;
					//OscMessage oscM = Osc.StringToOscMessage(msg);

				
					//handler.Send(oscM);
					//print("osc msg  sended: " + msg);

				}

			}
				

		*/

		}
			

	}






	public void ListenAllEvent(OscMessage oscMessage)
	{
		address = oscMessage.Address.ToString();
		values = oscMessage.Values[0].ToString();

		try { print(oscMessage.Address + " : " + oscMessage.Values[0]); } 
		catch (System.Exception ex) { print(ex.Message); }

	}

	void OnApplicationQuit()
	{
		ResetServo ();
		//WriteConsoleSend.WriteLogFile ("Quit Application WeAreNotNumbers");
		//WriteConsoleSend.SendEmail ();

		if(File.Exists("WeAreNotNumbers.log"))
			File.Delete("WeAreNotNumbers.log");
	}



}