using Euphrates;
using UnityEngine;

public class HitForceManager : MonoBehaviour
{
    [SerializeReference] FloatSO _force;
    [SerializeReference] FloatSO _maxForce;
    [SerializeReference] FloatSO _forceAdd;
    [SerializeReference] EnemyChannelSO _hit;
    [SerializeReference] TriggerChannelSO _secondPhase;

    void OnEnable()
    {
        _hit.AddListener(OnHit);
        _secondPhase.AddListener(SecondPhase);
    }

    void OnDisable()
    {
        _hit.RemoveListener(OnHit);
        _secondPhase.RemoveListener(SecondPhase);
    }

    private void Start()
    {
        CreateReduceTimer();
        _force.Value = 0f;
    }

    void CreateReduceTimer()
    {
        GameTimer.CreateTimer("Force Reduce", 3600, CreateReduceTimer, ReduceTick);
    }

    void ReduceTick(TickInfo tick)
    {
        _force.Value -= tick.DeltaTime * 5f;
        _force.Value = Mathf.Clamp(_force, 0f, _maxForce);
    }

    void OnHit(Enemy enemy)
    {
        _force.Value += _forceAdd;
    }

    void SecondPhase()
    {
        GameTimer.CancleTimer("Force Reduce");
    }
}
