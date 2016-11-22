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



	void Update()
	{
		if (CreatePattern.message != null) 
		{
			

			if (Input.GetKeyDown (KeyCode.R)) 
			{
				ResetServo ();
			}

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

		if(File.Exists("WeAreNotNumbers.log"))
			File.Delete("WeAreNotNumbers.log");
	}



}