using Euphrates;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    Transform _transform;

    [Header("References")]
    [SerializeReference] InputReaderSO _inputs;
    CharacterController _controller;

    [SerializeReference] FloatSO _speed;
    [SerializeReference] FloatSO _lerp;
    [SerializeReference] FloatSO _maxInputRadius;
    [SerializeReference] Transform _target;
    
    [Header("Constraints"), Space]
    [SerializeField, Tooltip("Minimum X position character can run to.")] float _minX = -3f;
    [SerializeField, Tooltip("Maximum X position character can run to.")] float _maxX = 3f;

    [Header("Triggers"), Space]
    [SerializeReference] TriggerChannelSO _start;
    [SerializeReference] TriggerChannelSO _fail;
    [SerializeReference] TriggerChannelSO _phaseTwo;
    [SerializeReference] TriggerChannelSO _endPunch;


    bool _isActive = false;
    bool _controlActive = false;

    void Awake()
    {
        Application.targetFrameRate = 60;
        _transform = transform;
        _controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        _inputs.OnTouchDown += TouchDown;
        _inputs.OnTouchMove += TouchMove;

        _start.AddListener(OnStart);
        _phaseTwo.AddListener(EndPhase);
        _fail.AddListener(Fail);
        _endPunch.AddListener(FinishPunch);
    }

    void OnDisable()
    {
        _inputs.OnTouchDown -= TouchDown;
        _inputs.OnTouchMove -= TouchMove;
        
        _start.RemoveListener(OnStart);
        _phaseTwo.RemoveListener(EndPhase);
        _fail.RemoveListener(Fail);
        _endPunch.RemoveListener(FinishPunch);
    }

    void FixedUpdate()
    {
        if (!_isActive)
            return;

        Vector3 realTarget = new Vector3(_target.position.x, 0f, _transform.position.z + _lerp);

        _transform.forward = (realTarget - _transform.position).normalized;

        Vector3 velocity = _speed * Time.fixedDeltaTime * _transform.forward;
        velocity.y = Physics.gravity.y * Time.fixedDeltaTime;

        _controller.Move(velocity);
    }

    void OnStart()
    {
        _isActive = true;
        _controlActive = true;
    }

    Vector2 _initialTouch = Vector2.zero;

    void TouchDown(Vector2 pos)
    {
        if (!_controlActive)
            return;

        _initialTouch = pos;
    }

    void TouchMove(Vector2 pos)
    {
        if (!_controlActive)
            return;

        if (Vector2.Distance(pos, _initialTouch) > _maxInputRadius)
            _initialTouch = pos + (_initialTouch - pos).normalized * _maxInputRadius;

        Vector2 moveInput = pos - _initialTouch;
        float moveX = moveInput.x;

        if (float.IsNaN(moveX))
            return;

        float targetX = Mathf.Clamp(_target.position.x + moveX * 0.003f, _minX, _maxX);
        _target.position = new Vector3(targetX, 0f, 0f);
    }

    void Fail()
    {
        _isActive = false;
    }

    void EndPhase()
    {
        _controlActive = false;
        _isActive = true;
        _target.position = Vector3.zero;
    }

    void FinishPunch()
    {
        _isActive = false;
    }
}
