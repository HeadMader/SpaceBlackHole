using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ChangeFuelToConvert : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _text;
    [SerializeField]
    private Slider _value;
    void Start()
    {
        _value.value = 1f;
    }
    public void UpdateValue()
	{
        _text.text = (int)(_value.value * 100) + " %";
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
