using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFuelToConvertValue : MonoBehaviour
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

}
