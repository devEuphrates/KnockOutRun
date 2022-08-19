using Euphrates;
using UnityEngine;

public class FailDetector : MonoBehaviour
{
    [SerializeReference] TriggerChannelSO _fail;
    [SerializeReference] FloatSO _strength;

    bool _failed = false;

    void OnEnable()
    {
        _strength.OnChange += StrengthChange;
    }

    void OnDisable()
    {
        _strength.OnChange -= StrengthChange;
    }

    private void StrengthChange(float change)
    {
        if (_failed || _strength.Value > 0f)
            return;

        Fail();
    }

    void Fail()
    {
        _failed = true;
        _fail.Invoke();
        _fail.Silent = true;
    }
}
