using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Option Asset", menuName = "Option Asset")]
public class OptionsSet : ScriptableObject
{
	public float Volume = 1f;
	public float MusicVolume = 1f;
	public bool ToggleMusic = true;
	public int CurrentMusic = 0;
	public AudioClip[] BackgroundMusics;

}
