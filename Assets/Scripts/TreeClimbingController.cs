using Unity.VisualScripting;
using UnityEngine;

public class TreeClimbingController : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _slingshotReleaseSound;
    [SerializeField] CharacterFacade _character;
    [SerializeField] KeyCode _startClimbingKey;
    [SerializeField] KeyCode _endClimbingKey;
    [SerializeField] KeyCode _holdTrunkKey;
    [SerializeField] CameraRaycaster _cameraRaycaster;
    [SerializeField] PlayerMovement _standartMovement;
    [SerializeField] float _climbingSpeed;
    [SerializeField] float _slingshotVerticalMult;
    bool _isClimbing = false;
    Transform _currentClimbingTree;
    Transform _minPoint;
    Transform _maxPoint;
    SlingshotController _slingshot;
    private void Start()
    {
        if (_character == null)
            _character = GetComponent<CharacterFacade>();
    }
    void Update()
    {
        if (_isClimbing)
        {
            HandleVerticalMovement();

            float verticalProgress = CalculateVerticalProgress();
            _slingshot.Bend(verticalProgress);

            if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(_endClimbingKey))
            {
                StopClimbing();
            }
        }
        else
        {
            if (Input.GetKeyDown(_startClimbingKey))
            {
                if (_cameraRaycaster.currentHitted != null && _cameraRaycaster.currentHitted.tag == "Tree")
                {
                    StartClimbing();
                }
            }
        }
    }
    public void StartClimbing()
    {
        _isClimbing = true;

        _standartMovement.enabled = false;

        _currentClimbingTree = _cameraRaycaster.currentHitted.transform;
        _minPoint = _currentClimbingTree.parent.Find("MinClimbingPoint");
        _maxPoint = _currentClimbingTree.parent.Find("MaxClimbingPoint");
        _slingshot = _currentClimbingTree.GetComponentInParent<SlingshotController>();
        _slingshot.SetDir(_currentClimbingTree.position - transform.position);
    }
    public void StopClimbing()
    {
        _isClimbing = false;

        _standartMovement.enabled = true;

        _currentClimbingTree = null;
        _minPoint = null;
        _maxPoint = null;  

        _slingshot.Release();
        _audioSource.PlayOneShot(_slingshotReleaseSound);
    }
    void HandleVerticalMovement()
    {
        float yDir = Input.GetAxis("Vertical");
        Transform t = _character.GetMainTransform();
        if (_currentClimbingTree)
        {
            t.position += _currentClimbingTree.up * yDir * Time.deltaTime * _climbingSpeed;
            if (t.position.y < _minPoint.position.y)
            {
                //t.position = new Vector3(t.position.x, _minPoint.position.y, t.position.z);
                StopClimbing();
                return;
            }
            if (t.position.y > _maxPoint.position.y)
            {
                //t.position = new Vector3(t.position.x, _maxPoint.position.y, t.position.z);
                
                StopClimbing();
                return;
            }
        }
    }
    private float CalculateVerticalProgress()
    {
        return (_character.GetMainTransform().position.y - _minPoint.position.y) / (_maxPoint.position.y - _minPoint.position.y);
    }
}
