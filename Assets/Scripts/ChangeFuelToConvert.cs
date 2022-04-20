using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private void Update()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
        }
    }
    public void UpdateValue()
	{
        _text.text = (int)(_value.value * 100) + " %";
	}
    public void ReturnTime()
	{
        Time.timeScale = 1f;
    }

}
