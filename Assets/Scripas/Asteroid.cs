using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Interactable
{
	public FuelComponents FuelComponent;
	public float AmountOfFuelComponet = 100;

	protected override void Interact()
	{
		float usedAmountOfFuelComponet = FuelManager.Instance.IncreseAmountOfFuel(AmountOfFuelComponet, (int)FuelComponent);
		if (usedAmountOfFuelComponet == AmountOfFuelComponet)
		{
			Destroy(this.gameObject);
		}
		else
		{
			AmountOfFuelComponet -= usedAmountOfFuelComponet;
		}
	}
	
	public enum FuelComponents
	{
		 Methane, Oxygen
	}
}
