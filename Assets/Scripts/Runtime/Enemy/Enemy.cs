using Euphrates;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float Strength = 0f;
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

    private void OnEnable()
    {
        _trigger.OnEnter.AddListener(Fight);
        _enemyHit.AddListener(LoseFight);
        _playerFail.AddListener(OnFinish);
    }

    private void OnDisable()
    {
        _trigger.OnEnter.RemoveListener(Fight);
        _enemyHit.RemoveListener(LoseFight);
        _playerFail.RemoveListener(OnFinish);
    }

    private void Fight(GameObject _)
    {
        if (_finished)
            return;

        float change = _playerStrength.Value - Strength;
        bool lost = change > 0;

        if (lost)
        {
            _playerPunch?.Invoke(this);
            _playerStrength.Value += Mathf.Floor(Strength);
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
}
