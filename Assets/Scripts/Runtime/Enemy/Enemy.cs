using Euphrates;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [Header("Strength")]
    [SerializeField] float _strengthDifference;
    public event Action OnStrengthSet;
    float _strength = 0f;
    public float Strength { get { return _strength; } }
    [Space]
    [SerializeReference] UnityEventTriggerCollider _trigger;
    [SerializeReference] FloatSO _playerStrength;
    [SerializeReference] IntSO _coin;
    [SerializeReference] CharacterAnimationControls _anim;
    [SerializeReference] RagdollEnabler _ragdoll;
    [SerializeReference] Rigidbody _rb;
    public UnityEvent OnDefeat;

    [Header("Channels"), Space]
    [SerializeReference] EnemyChannelSO _playerPunch;
    [SerializeReference] EnemyChannelSO _enemyHit;
    [SerializeReference] TriggerChannelSO _playerFail;

    bool _finished = false;

    private void Start() => SetStrength(0f);

    private void OnEnable()
    {
        _trigger.OnEnter.AddListener(Fight);
        _enemyHit.AddListener(LoseFight);
        _playerFail.AddListener(OnFinish);
        _playerStrength.OnChange += SetStrength;
    }

    private void OnDisable()
    {
        _trigger.OnEnter.RemoveListener(Fight);
        _enemyHit.RemoveListener(LoseFight);
        _playerFail.RemoveListener(OnFinish);
        _playerStrength.OnChange -= SetStrength;
    }

    private void Fight(GameObject _)
    {
        if (_finished)
            return;

        float change = _playerStrength.Value - _strength;
        bool lost = change > 0;

        if (lost)
        {
            _playerPunch.Invoke(this);
            _playerStrength.Value += Mathf.Floor(_strength);
            return;
        }

        _playerStrength.Value += change;

        _anim.Punch();
    }

    void LoseFight(Enemy enemy)
    {
        if (enemy != this)
            return;

        _anim.Disable();
        _ragdoll.Enable();

        _coin.Value++;

        float randX = UnityEngine.Random.Range(-5f, -3f);
        _rb.AddForce(new Vector3(randX, 2f, 1f) * 20f, ForceMode.Impulse);

        OnDefeat?.Invoke();
    }

    void OnFinish()
    {
        _finished = true;
    }

    void SetStrength(float _)
    {
        _strength = Mathf.Clamp(_playerStrength.Value + _strengthDifference, 5f, float.MaxValue);
        OnStrengthSet?.Invoke();
    }
}
