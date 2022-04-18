using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FuelManager : MonoBehaviour
{
	#region Singelton
	public static FuelManager Instance;
	private void Awake()
	{
		Instance = this;
	}
	#endregion

	[Header("Fuel Indecators")]
	[SerializeField] private Slider _rocketFuelIndicator;
	[SerializeField] private Slider _methaneIndicator;
	[SerializeField] private Slider _oxygenIndicator;
	[SerializeField] private Slider _percentOfFuelToConvert;

	[Header("Fuel Using Rate")]
	[SerializeField] private float _rocketUsingFuelRate = 5f;
	[SerializeField] private float _spaceShipUsingFuelRate = 10f;
	
	//[SerializeField] private float _maxSmallShipFuel = 30;
	[Header("Set Fuel Tanks")]
	[SerializeField] private float _maxRocketFuel = 500;
	[SerializeField] private float _maxAmountOfMethane = 400;
	[SerializeField] private float _maxAmountOfOxygen = 200;

	[Header("GameOverScene")]
	[SerializeField] private SceneAsset GameOverScene;

	private float _amountOfMethane = 400f;
	private float _amountOfOxygen = 200f;
	private float _rocketFuel = 50f;

	[HideInInspector]
	public bool isSmallSpaceShipMove = false;
	[HideInInspector]
	public bool hasSpaceShipFuel = true;
	void Start()
	{
		_amountOfMethane = _maxAmountOfMethane;
		_amountOfOxygen = _maxAmountOfOxygen;
		_rocketFuel = _maxRocketFuel;
	}

	// Update is called once per frame
	void Update()
	{
		
		if (_rocketFuel <= 0)
		{
			GameOver();
			return;
		}
		if (isSmallSpaceShipMove)
		{
			SpaceShipUseFuel();
		}
		RocketUseFuel();
		Display();
	}
	private void RocketUseFuel()
	{
		_rocketFuel -= _rocketUsingFuelRate * Time.deltaTime;
	}
	private void SpaceShipUseFuel()
	{
		if (_amountOfMethane <= 0 || _amountOfOxygen <= 0)
		{
			hasSpaceShipFuel = false;
		}
		else 
		{
			hasSpaceShipFuel = true;
			_amountOfMethane -= _spaceShipUsingFuelRate * 2 * Time.deltaTime;
			_amountOfOxygen -= _spaceShipUsingFuelRate * Time.deltaTime;
		}
	}
	public float IncreseAmountOfFuel(float amount, int Tank)
	{
		switch (Tank)
		{
			case 0:
				if (_amountOfMethane < _maxAmountOfMethane)         //it may be separate method but I`m lazy 
				{
					float freeSpace = _maxAmountOfMethane - _amountOfMethane;
					if (freeSpace < amount)
					{
						_amountOfMethane += freeSpace;
						return freeSpace;
					}
					else
					{
						_amountOfMethane += amount;
						return amount;
					}
				}
				break;
			case 1:
				if (_amountOfOxygen < _maxAmountOfOxygen)         //it can be separate method but I an lazy 
				{
					float freeSpace = _maxAmountOfOxygen - _amountOfOxygen;
					if (freeSpace < amount)
					{
						_amountOfOxygen += freeSpace;
						return freeSpace;
					}
					else
					{
						_amountOfOxygen += amount;
						return amount;
					}
				}
				break;
		}
		return 0;
	}
	public void ConvertOxygenAndMethane()
	{
		float percentOfComponents = _percentOfFuelToConvert.value;
		float freeSpace = _maxRocketFuel - _rocketFuel;
		float methaneToConvert;

		if (_amountOfMethane / 2 < _amountOfOxygen)  //if Amount Of Methane not enough to react with oxygen
			methaneToConvert = _amountOfMethane * percentOfComponents;
		else										//if Amount Of OXYGEN not enough to react with methane
			methaneToConvert = _amountOfOxygen * percentOfComponents * 2; 

			if(methaneToConvert > freeSpace)		//All clear if not you are very stupid
				methaneToConvert = freeSpace;

			_rocketFuel += methaneToConvert;
			_amountOfMethane -= methaneToConvert;
			_amountOfOxygen -= methaneToConvert / 2;
		
	}
	private void Display()
	{
		float ratioOfFuel = _rocketFuel / _maxRocketFuel;
		float ratioOfMethane = _amountOfMethane / _maxAmountOfMethane;
		float ratioOfOxygen = _amountOfOxygen / _maxAmountOfOxygen;
		_rocketFuelIndicator.value = ratioOfFuel;
		_methaneIndicator.value = ratioOfMethane;
		_oxygenIndicator.value = ratioOfOxygen;
	}
	private void GameOver()
	{
		SceneManager.LoadScene(GameOverScene.name);
	}

}
