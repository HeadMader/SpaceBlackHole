using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void QuitGame()
	{
		Application.Quit(); 
	}
	private void OnEnable()
	{
		Time.timeScale = 0f;
	}
	private void OnDisable()
	{
		Time.timeScale = 1f;
	}
}
