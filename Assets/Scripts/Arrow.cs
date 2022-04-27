using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Arrow : MonoBehaviour
{

	[SerializeField] private Transform _rocket;
	[SerializeField] private RectTransform _arrow;
	[SerializeField] private Camera _arrowCamera;

	[Header("Offset")]
	[SerializeField] private float _offset = 50f;

	private float _halfScreenWidthWithOffset;
	private float _halfScreenHeightWithOffset;
	private Rect _screenRect;
	private Vector2 _centerOfScreen;

	void Start()
	{

		_screenRect = new Rect(0, 0, Screen.width, Screen.height);
	}

	// Update is called once per frame
	void Update()
	{
		PointToTarget();
		_centerOfScreen = new Vector2((float)Screen.width / 2, (float)Screen.height / 2);
		_halfScreenWidthWithOffset = (float)Screen.width / 2 - _offset;
		_halfScreenHeightWithOffset = (float)Screen.height / 2 - _offset;
	}

	private void PointToTarget()
	{
		Vector2 targetOnScreen = _arrowCamera.WorldToScreenPoint(_rocket.position);

		if (_screenRect.Contains(targetOnScreen))
		{
			_arrow.gameObject.SetActive(false);
			return;
		}
		else
			_arrow.gameObject.SetActive(true);

		Vector2 vectorToTarget = targetOnScreen - _centerOfScreen;
		_arrow.rotation = Quaternion.FromToRotation(Vector3.up, vectorToTarget);

		//_arrow.position = vectorToTarget.normalized * 200 + new Vector2((float)Screen.width / 2, (float)Screen.height / 2);
		Vector2 normVectorToTarget;

		float screenRatio = _halfScreenWidthWithOffset / _halfScreenHeightWithOffset;
		float vectorRatio = Mathf.Abs(vectorToTarget.x / vectorToTarget.y);

		if (screenRatio <= vectorRatio)
			normVectorToTarget = Vector2.ClampMagnitude(vectorToTarget, vectorToTarget.magnitude * (_halfScreenWidthWithOffset / Mathf.Abs(vectorToTarget.x)));
		else
			normVectorToTarget = Vector2.ClampMagnitude(vectorToTarget, vectorToTarget.magnitude * (_halfScreenHeightWithOffset / Mathf.Abs(vectorToTarget.y)));

		_arrow.position = normVectorToTarget + _centerOfScreen;

	}
}

