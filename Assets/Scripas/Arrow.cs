using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	[SerializeField] private			Transform			_rocket;
	[SerializeField] private RectTransform _arrow;
	[SerializeField] private Camera _playerCamera;

	[Header("Offset")]
	[SerializeField] private float _offset = 50f;

	private float _halfScreenWidth;
	private float _halfScreenHeight;
	private Rect _screenRect;
	void Start()
	{

		_screenRect = new Rect(0, 0, Screen.width, Screen.height);
	}

	// Update is called once per frame
	void Update()
	{

		PointToTarget();
		_halfScreenWidth = (float)Screen.width / 2 - _offset;
		_halfScreenHeight = (float)Screen.height / 2 - _offset;
	}
	private void PointToTarget()
	{
		Vector2 targetOnScreen = _playerCamera.WorldToScreenPoint(_rocket.position);
		if (_screenRect.Contains(targetOnScreen))
		{
			_arrow.gameObject.SetActive(false);
			return;
		}
		else
			_arrow.gameObject.SetActive(true);
		Vector2 vectorToTarget = targetOnScreen - new Vector2((float)Screen.width / 2, (float)Screen.height / 2);
		_arrow.rotation = Quaternion.FromToRotation(Vector3.up, vectorToTarget);
		//  transform.position = vectorToTarget.normalized * 200 + new Vector2(halfScreenWidth, halfScreenHeight);
		Vector2 normVectorToTarget;


		float screenRatio = _halfScreenWidth / _halfScreenHeight;
		float vectorRatio = Mathf.Abs(vectorToTarget.x / vectorToTarget.y);

		if (screenRatio <= vectorRatio)
			normVectorToTarget = Vector2.ClampMagnitude(vectorToTarget, vectorToTarget.magnitude * (_halfScreenWidth / Mathf.Abs(vectorToTarget.x)));
		else
			normVectorToTarget = Vector2.ClampMagnitude(vectorToTarget, vectorToTarget.magnitude * (_halfScreenHeight / Mathf.Abs(vectorToTarget.y)));

		_arrow.position = normVectorToTarget + new Vector2((float)Screen.width / 2, (float)Screen.height / 2);

	}
}
