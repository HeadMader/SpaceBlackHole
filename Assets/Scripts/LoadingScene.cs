using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
	[SerializeField] private Slider _loading;
	[SerializeField] private TMP_Text _loadingPercent;

	public void LoadScene()
	{
		StartCoroutine(LoadSceneCoroutine());
	}
	IEnumerator LoadSceneCoroutine()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(1);
		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);
			_loading.value = progress;
			_loadingPercent.text =(int)(progress * 100) + "%";
			yield return null;
		}
	}
}
