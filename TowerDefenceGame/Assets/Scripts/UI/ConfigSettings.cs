using UnityEngine;
using System.Collections;

public class ConfigSettings
{
	private static ConfigSettings m_instance;

	public static ConfigSettings Instance{
		get{ 
			if (m_instance == null)
				m_instance = new ConfigSettings ();
			return m_instance;
		}
	}
	
	public int sensitivity = 40;

	public bool invertXAxis = true;
	public bool invertYAxis = true;
}
