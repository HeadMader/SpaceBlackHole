using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rocket : Interactable
{
	[SerializeField] private GameObject _convertToFuelMenu; 
	
	protected override void Interact()
	{
		_convertToFuelMenu.SetActive(true);
		Time.timeScale = 0f;
	}
	
}
