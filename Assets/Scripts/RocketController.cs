using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RocketController : MonoBehaviour
{
	#region Veriables
	[Header("Movement")]
	[SerializeField] private float _moveSpeed = 1;
	[SerializeField] private float _rotationSpeed = 5;
	[SerializeField] private float _stopDistanceToDistinationPoint = 0.1f;
	[SerializeField] private float _stopDistanceToTaraget = 2f;
	[SerializeField] private float _chackForObsticleDistance = 1f;
	[SerializeField] private float _sphereRadius = 0.2f;

	[Header("BlackHole")]
	[SerializeField] private Transform _blackHole = null;
	[SerializeField] private float _blackHoleEffect = 5f;
	[SerializeField] private float distanceToDie = 30f;

	[Header("Camera")]
	[SerializeField] private Camera _playerCamera = null;

	[Header("Marker")]
	[SerializeField] private Transform _markPrefab = null;

	[Header("Layers")]
	[SerializeField] private LayerMask _interactacable;
	[SerializeField] private LayerMask _markers;

	[Header("PlayerMenu")]
	[SerializeField] private GameObject _menu = null;

	[Header("Flame")]
	[SerializeField] private ParticleSystem _flame = null;
	
	[Header("Audio")]
	[SerializeField] private AudioSource _flameSound;
	[SerializeField] private float _flameSoundMaxVolume;
	[SerializeField] private AudioMixerGroup _audioEffectsMixer;

	[Header("GameOver")]
	[SerializeField] private GameObject _gameOver;
	private Vector3 _moveVector { get; set; }
	private Vector3 _pointToMove;
	private Vector3 _directionToTarget;
	private float _stopDistance;
	private bool _canMove = true;
	private bool _hasArrived = true;
	private GameObject _mark = null;
	private Interactable _focus = null;
	private bool _isMove = false;

	#endregion
	void Start()
	{
		_pointToMove = transform.position;
		_stopDistance = _stopDistanceToDistinationPoint;
		_flameSoundMaxVolume = _flameSound.volume;
		_flameSound.volume = 0f;
	}
	void Update()
	{
		AudioEffectsOnOff();
		HoleEffect();   //black hole gravitation
		if (FuelManager.Instance._rocketFuel == 0)
			GameOver();

		if (Input.GetKeyDown(KeyCode.Escape))
			EnableDisableMenu();
		
		if (!EventSystem.current.IsPointerOverGameObject()) // Chack if pointer over UI element
		{
			if (Input.GetMouseButtonDown(0))
				SetPointToMove();                   //Set point to which ship will go
			if (Input.GetMouseButtonDown(1))
				SetTarget();                        //Set target object to which ship will go
		}

		if (FuelManager.Instance.hasSpaceShipFuel)          // If fuel is ended we cant move
		{
			ChackForObsticle(); //chack if in front of ship is obsticle
			if (!_hasArrived)   //if ship is arrived stop move
			{
				Move();
				RototeShip();
			}
		}
		else
		{
			_isMove = false;
		}
		FuelManager.Instance.isSmallSpaceShipMove = _isMove;
		FlameControll(_isMove);
	}
	#region Movement
	private void Move()
	{
		_directionToTarget = transform.position - _pointToMove;

		if (_canMove)
		{
			if (Vector3.Distance(transform.position, _pointToMove) > _stopDistance)
			{
				float distanceToTarget = Vector3.Distance(transform.position, _pointToMove);
				_moveVector = -transform.forward * _moveSpeed * Time.deltaTime;

				if (_moveVector.magnitude > distanceToTarget)
					_moveVector = _directionToTarget; //directionToTarget also represents how vector must change;

				transform.Translate(_moveVector, Space.World);
				_isMove = true;
			}
			else
			{
				Destroy(_mark);
				_hasArrived = true;
				_isMove = false;
			}
		}
		else
		{
			_isMove = false;
		}
	}

	private void RototeShip()
	{
		float addativeRotation = 180 / _directionToTarget.magnitude;

		Quaternion rotationToTarget = Quaternion.LookRotation(_directionToTarget);
		transform.rotation = Quaternion.RotateTowards(transform.rotation,
			rotationToTarget, (_rotationSpeed + addativeRotation) * Time.deltaTime);


	}
	#endregion
	#region SetMethods
	private void SetPointToMove()
	{
		Vector2 mousePos = Input.mousePosition;     //Get position of mouse on the screen
		Ray ray = _playerCamera.ScreenPointToRay(mousePos); //Create ray from the screen
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 200f, _markers, QueryTriggerInteraction.Collide))
		{
			Debug.Log(hit.collider.name);
			if (_focus != null)
				RemoveFocus();

			_pointToMove = hit.point;
			_stopDistance = _stopDistanceToDistinationPoint;
			_hasArrived = false;
			CreateMark(_pointToMove);       // create mark at the point to which ship is moving
		}
	}

	private void SetTarget()
	{
		Vector2 mousePos = Input.mousePosition;
		Ray ray = _playerCamera.ScreenPointToRay(mousePos);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 200f, _interactacable))
		{
			Interactable interactable = hit.collider.GetComponent<Interactable>();
			if (interactable != null)
			{
				Destroy(_mark);
				SetFocus(interactable);
				_stopDistance = _stopDistanceToTaraget;
				_pointToMove = hit.collider.transform.position;
				_hasArrived = false;
			}
		}
	}
	private void ChackForObsticle()
	{
		if (Physics.SphereCast(new Ray(transform.position, -transform.forward), _sphereRadius, _chackForObsticleDistance, _interactacable))
			_canMove = false;
		else _canMove = true;
	}
	#endregion
	private void CreateMark(Vector3 markPosition)
	{
		if (_mark != null)
			Destroy(_mark);
		_mark = Instantiate(_markPrefab.gameObject, markPosition, Quaternion.identity);
	}
	private void HoleEffect()
	{
		float distanceBetweenHoleAndPlayer = Vector3.Distance(_blackHole.position, transform.position);
		if(distanceBetweenHoleAndPlayer < distanceToDie)
		{
			GameOver();
		}
		Vector3 holeEffect = (_blackHole.position - transform.position).normalized * _blackHoleEffect / Mathf.Pow(distanceBetweenHoleAndPlayer, 2f);
		transform.Translate(holeEffect * Time.deltaTime, Space.World);
	}
	private void SetFocus(Interactable newFocus)
	{
		if (newFocus != _focus)
		{
			if (_focus != null)
				_focus.OnDefocused();

			_focus = newFocus;
		}
		newFocus.OnFocused(transform);
	}
	private void RemoveFocus()
	{
		if (_focus != null)
			_focus.OnDefocused();
		_focus = null;
	}
	private void EnableDisableMenu()
	{
		if (_menu.activeSelf)
			_menu.SetActive(false);
		else
			_menu.SetActive(true);

	}
	private void FlameControll(bool isWorking)
	{
		var main = _flame.main;
		float particalsSpeed;
		float flameSoundVolume;
		if (isWorking)
		{
			particalsSpeed = 5f;
			flameSoundVolume = _flameSoundMaxVolume;
		}
		else
		{
			particalsSpeed = flameSoundVolume = 0f;

		}
		float lerpedSpeed = Mathf.Lerp(main.startSpeed.constant, particalsSpeed, 0.5f);
		float lerpedFlameSound = Mathf.Lerp(_flameSound.volume, flameSoundVolume, 0.5f);
		main.startSpeed = lerpedSpeed;
		_flameSound.volume = lerpedFlameSound;
		
	}
	private void AudioEffectsOnOff()
	{
		if(Time.timeScale == 1f)
		{
			_audioEffectsMixer.audioMixer.SetFloat("EffectsVolume", 0);
		}
		else
		{
			_audioEffectsMixer.audioMixer.SetFloat("EffectsVolume", -80);
		}
	}
	public void GameOver()
	{
		_gameOver.SetActive(true);
		StartCoroutine(LoadScene());
	}
	IEnumerator LoadScene()
	{
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene(0);
	}
}
