using Euphrates;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [Header("Strength")]
    public EnemyType EnemyPresetType = EnemyType.Random;

    float _strengthChange;
    public float StrengthChange
    {
        get => _strengthChange;
        set
        {
            if (value >= 0)
                _strengthChange = Mathf.Clamp(value, 5f, float.MaxValue);
            else
                _strengthChange = value;
        }
    }

    public event Action OnStrengthSet;
    float _strength = 0f;
    public float Strength { get => _strength; }
    [Space]
    [SerializeReference] EnemyHolderSO _enemyHolder;
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
    bool _fought = false;

    private void OnEnable()
    {
        _enemyHolder.AddEnemy(this);
        _trigger.OnEnter.AddListener(Fight);
        _enemyHit.AddListener(LoseFight);
        _playerFail.AddListener(OnFinish);
    }

    private void OnDisable()
    {
        _enemyHolder.RemoveEnemy(this);
        _trigger.OnEnter.RemoveListener(Fight);
        _enemyHit.RemoveListener(LoseFight);
        _playerFail.RemoveListener(OnFinish);
    }

    public void Fight(GameObject _)
    {
        if (_finished || _fought)
            return;

        _fought = true;

        float change = _playerStrength.Value - _strength;
        bool lost = change > 0;

        if (lost)
        {
            _playerPunch.Invoke(this);
            _playerStrength.Value += _strength;
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

    public void SetStrength(GameObject _)
    {
        if (_strengthChange < 0)
        {
            _strength = _playerStrength - _strengthChange;
            OnStrengthSet?.Invoke();
            return;
        }

        _strength = _playerStrength > _strengthChange ?
            _strengthChange
            : Mathf.CeilToInt(Mathf.Clamp(_playerStrength - 5, 1, float.MaxValue));

        OnStrengthSet?.Invoke();
    }
}

public enum EnemyType { Random, GiveStrength, RemoveStrength}