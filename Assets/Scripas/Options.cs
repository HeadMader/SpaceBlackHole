using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private Slider _volume;
	public AudioClip clip;
    public void ChangeVolume()
	{
        float volume = _volume.value;
        AudioListener.volume = volume;
	}
	private void Awake()
	{
		_volume.value = 1f;
	}

}
