using Euphrates;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeReference] IntSO _currentLevel;
    [SerializeReference] LevelsDataSO _levelsData;
    [SerializeReference] Transform _endPiece;
    [SerializeReference] IntSO _setPieceCount;
    [SerializeField] float _startOffset = 0f;

    [Header("Triggers"), Space]
    [SerializeReference] TriggerChannelSO _generateLevel;
    [SerializeReference] TriggerChannelSO _levelGenerated;

    [Header("Multipliers"), Space]
    [SerializeReference] Transform _multiplierParent;
    [SerializeReference] GameObject _multiplierPrefab;
    [SerializeReference] GameObject _multiplierEndPrefab;
    [SerializeField] float _multiplierSize = 1f;
    [SerializeField] float _multiplierMax = 10f;
    [SerializeField] float _multiplierStep = .1f;

    void OnEnable() => _generateLevel.AddListener(SetLevel);
    void OnDisable() => _generateLevel.RemoveListener(SetLevel);

    void SetLevel()
    {
        GenerateLevelEnviroment();
        GenerateMultipliers();
    }

    void GenerateLevelEnviroment()
    {
        foreach (var pml in _levelsData.PremadeLevels)
        {
            if (pml.Level != _currentLevel)
                continue;

            GeneratePremadeLevel(pml);
            return;
        }

        GenerateRandomLevel();
    }

    void GenerateMultipliers()
    {
        if (_multiplierParent == null || _multiplierPrefab == null)
            return;

        float curMult = 1f;
        int i = 0;

        while (curMult < _multiplierMax)
        {
            // Instantiate multiplier.
            Multiplier multiplier = Instantiate(_multiplierPrefab, _multiplierParent).GetComponent<Multiplier>();

            // Set multiplier position.
            multiplier.transform.localPosition += Vector3.forward * _multiplierSize * i++;

            multiplier.Init(curMult);

            curMult += _multiplierStep;
        }

        Multiplier endMultiplier = Instantiate(_multiplierEndPrefab, _multiplierParent).GetComponent<Multiplier>();

        // Set multiplier position.
        endMultiplier.transform.localPosition += Vector3.forward * _multiplierSize * i;

        endMultiplier.Init(curMult);
    }

    void GeneratePremadeLevel(PremadeLevel level)
    {
        GameObject go = Instantiate(level.LevelPrefab, transform);
        _endPiece.position = new Vector3(0f, 0f, level.ZEnd);

        _levelGenerated.Invoke();
    }

    void GenerateRandomLevel()
    {
        var usableSetPieces = new List<SetPieceSO>();

        foreach (var sp in _levelsData.SetPieces)
        {
            int maxLevel = sp.MaxLevel < 0 ? int.MaxValue : sp.MaxLevel;

            if (sp.MinLevel < _currentLevel && maxLevel > _currentLevel)
                usableSetPieces.Add(sp);
        }

        if (usableSetPieces.Count == 0)
            return;

        float curZ = _startOffset;

        // This iscreated to make sure to not to spawn same set pieces before going through all set pieces.
        var unUsed = new List<SetPieceSO>(usableSetPieces);

        for (int i = 0; i < _setPieceCount; i++)
        {
            // Reset the unused list if we went through all the set pieces.
            if (unUsed.Count == 0)
                unUsed = usableSetPieces.ToList();

            // Select from un used set pieces.
            var sel = unUsed.GetRandomItem();

            var go = Instantiate(sel.Prefab, transform);

            // Remove the spawned set piece from unused since it's recently used.
            unUsed.Remove(sel);

            // Set the position of set piece to the end of the previous one.
            var pos = Vector3.forward * curZ;
            go.transform.position = pos;

            // Increment current z position with the size of spawned set piece.
            curZ += sel.ZScale;
        }

        // Set the end boss position to the most end.
        _endPiece.position = Vector3.forward * curZ;

        _levelGenerated.Invoke();
    }
}
