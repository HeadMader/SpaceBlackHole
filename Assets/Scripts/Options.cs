using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    [SerializeField] private Slider _volume;
	[SerializeField] private Slider _musicVolume;
	[SerializeField] private Toggle _musicToggle;
	[SerializeField] private TMP_Dropdown _chooseMusic;
	[SerializeField] private OptionsSet optionsSet;
	[SerializeField] private AudioMixerGroup _mixerMusic;
	[SerializeField] private AudioSource _backgroudMusic;
	private float musicVolume = 0f;
	private bool isMusicMuted = false;
	private void Awake()
	{
		_volume.value = optionsSet.Volume;
		_musicVolume.value = optionsSet.MusicVolume;
		_musicToggle.isOn = optionsSet.ToggleMusic;
		_chooseMusic.value = optionsSet.CurrentMusic;

		ChangeMusic(optionsSet.CurrentMusic);
		ChangeVolume();
		ChangeMusicVolume();
	}
	private void Start()
	{
		ToggleMusic(optionsSet.ToggleMusic);
	}
	public void ChangeVolume()
	{
        float volume = _volume.value;
        AudioListener.volume = volume;
		optionsSet.Volume = volume;
	}
	public void ChangeMusicVolume()
	{
		float volume = _musicVolume.value;
		musicVolume = Mathf.Lerp(-80, 0, volume);
		if (!isMusicMuted)
		{
			_mixerMusic.audioMixer.SetFloat("MusicVolume", musicVolume);
		}
		optionsSet.MusicVolume = volume;
	}
	public void ToggleMusic(bool enabled)
	{
		if (enabled)
		{
			_mixerMusic.audioMixer.SetFloat("MusicVolume", musicVolume);
		}
		else
		{
			_mixerMusic.audioMixer.SetFloat("MusicVolume", -80f);
		}
		optionsSet.ToggleMusic = enabled;
		isMusicMuted = !enabled;
	}
	public void ChangeMusic(int number)
	{
		_backgroudMusic.clip = optionsSet.BackgroundMusics[number];
		optionsSet.CurrentMusic = number;
		_backgroudMusic.Play();
	}

}
