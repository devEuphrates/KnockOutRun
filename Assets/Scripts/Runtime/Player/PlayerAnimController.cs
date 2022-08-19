using Euphrates;
using UnityEngine;

public class PlayerAnimController : CharacterAnimationControls
{
    [SerializeReference] FloatSO _strength;

    [Header("Animation Names"), Space]
    [SerializeField] string _knockedOut;
    [SerializeField] string _stumble;
    [SerializeField] string _finishPunch;

    [Header("Animation Names"), Space]
    [SerializeReference] EnemyChannelSO _punchTrigger;
    [SerializeReference] EnemyChannelSO _enemyHit;
    [SerializeReference] TriggerChannelSO _failTrigger;
    [SerializeReference] TriggerChannelSO _finishPunchTrigger;

    private void OnEnable()
    {
        _strength.OnChange += OnDamage;
        _punchTrigger.OnTrigger += PunchEnemy;
        _finishPunchTrigger.AddListener(Finisher);
        _failTrigger.AddListener(Fail);
    }

    private void OnDisable()
    {
        _strength.OnChange -= OnDamage;
        _punchTrigger.OnTrigger -= PunchEnemy;
        _finishPunchTrigger.RemoveListener(Finisher);
        _failTrigger.RemoveListener(Fail);
    }

    void OnDamage(float change)
    {
        if (change > 0f)
            return;

        if (_strength <= 0f)
        {
            return;
        }

        _animator.Play(_stumble);
    }

    Enemy _selEnemy = null;
    void PunchEnemy(Enemy enemy)
    {
        _selEnemy = enemy;
        base.Punch();
    }


    public void EnemyHit()
    {
        if (_selEnemy == null)
            return;

        _enemyHit?.Invoke(_selEnemy);
    }

    void Finisher()
    {
        _animator.Play(_finishPunch);
    }
     
    void Fail()
    {
        _animator.enabled = false;
    }
}