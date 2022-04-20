using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	[SerializeField] private float _radius = 3f;
	internal Transform _player;
	private bool _hasInteracted;
	internal bool isFocus = false;
	protected virtual void Interact()
	{
		//Some...
	}
	void Start()
	{

	}
	void Update()
	{
		if (isFocus && !_hasInteracted)
		{
			float distanceToPlayer = Vector3.Distance(_player.position, transform.position);
			if (distanceToPlayer < _radius)
			{
				if (isFocus)
				{
					Interact();
					_hasInteracted = true;
				}
			}
		}
	}
	public void OnFocused(Transform playerTransform)
	{
		isFocus = true;
		_player = playerTransform;
		_hasInteracted = false;
	}

	public void OnDefocused()
	{
		isFocus = false;
		_player = null;
		_hasInteracted = false;
	}

}
