using UnityEngine;
using System.Collections;
using System.Xml;

public class XmlConfig : MonoBehaviour 
{
	private string				pathXml;
	public XmlDocument 			config;
	public static XmlNode 		ip;
	public static XmlNode 		port;
	public static XmlNode       timer;
	public static float         _timer;

	void Awake()
	{
		config = new XmlDocument();
		pathXml = Application.dataPath + "/Resources/config.xml";
		config.Load(pathXml);
		// print(pathXml);

		// <network> primo nodo
		XmlElement root = config.DocumentElement;
		// <ip> primo nodo
		ip = root.FirstChild;
		// <port> successivo a ip
		port = ip.NextSibling;

		timer = port.NextSibling;
		_timer = float.Parse(timer.InnerXml.ToString());

	}
		

}
