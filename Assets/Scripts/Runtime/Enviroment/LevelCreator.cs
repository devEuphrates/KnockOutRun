using Euphrates;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeReference] FloatSO _multiplierAmt;
    [SerializeReference] Transform _multiplierParent;
    [SerializeReference] GameObject _multiplierPrefab;
    [SerializeField] float _multiplierSize = 1f;
    [SerializeField] float _multiplierMax = 10f;
    [SerializeField] float _multiplierStep = .1f;
    private void Start()
    {
        _multiplierAmt.Value = 0;
        SetTilesAndObjects();
        SetMultipliers();
    }

    void SetTilesAndObjects()
    {

    }

    void SetMultipliers()
    {
        if (_multiplierParent == null || _multiplierPrefab == null)
            return;

        float curMult = 1f;
        int i = 0;

        while (curMult <= _multiplierMax)
        {
            // Instantiate multiplier.
            Multiplier multiplier = Instantiate(_multiplierPrefab, _multiplierParent).GetComponent<Multiplier>();

            // Set multiplier position.
            multiplier.transform.localPosition += Vector3.forward * _multiplierSize * i++;

            multiplier.Init(curMult);

            curMult += _multiplierStep;
        }
    }
}
