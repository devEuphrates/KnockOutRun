using Euphrates;
using System;
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

    [Header("Enemies"), Space]
    [SerializeReference] EnemyHolderSO _enemyHolder;
    [SerializeReference] IntSO _baseMaxLevel;
    [SerializeReference] IntSO _maxLevelStep;


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

        SetEnemyLevels();

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

        // Set Enemy levels have sum of max.
        SetEnemyLevels();

        _levelGenerated.Invoke();
    }

    void SetEnemyLevels()
    {
        if (_enemyHolder.Enemies.Count < 2)
            return;

        int levelMax = _baseMaxLevel + _maxLevelStep * _currentLevel;
        int minBeatableEnemyCount = Mathf.CeilToInt(_enemyHolder.Enemies.Count * 0.6f);

        List<Enemy> presetRandomEnemies = _enemyHolder.Enemies.FindAll(e => e.EnemyPresetType == EnemyType.Random);
        List<Enemy> presetBeatableEnemies = _enemyHolder.Enemies.FindAll(e => e.EnemyPresetType == EnemyType.GiveStrength);

        int totalBeatable = Mathf.CeilToInt(_enemyHolder.Enemies.Count * 0.6f);
        totalBeatable = totalBeatable > presetBeatableEnemies.Count ? totalBeatable : presetBeatableEnemies.Count;

        // Create int array of size of enemies we can beat.
        int[] rndNums = new int[totalBeatable];

        // Randomly populate the array.
        for (int i = 0; i < totalBeatable; i++)
            rndNums[i] = UnityEngine.Random.Range(5, 95);

        // Sort the array in ascending order.
        Array.Sort(rndNums);

        // Create the array that will hold the real values.
        int[] rndSumToMax = new int[totalBeatable];

        rndSumToMax[0] = rndNums[0];

        for (int i = 1; i < totalBeatable - 1; i++)
            rndSumToMax[i] = rndNums[i] - rndNums[i - 1];

        rndSumToMax[totalBeatable - 1] = levelMax - rndSumToMax[totalBeatable - 2];

        List<Enemy> unSelectedBeatableEnemies = new List<Enemy>(presetBeatableEnemies);
        List<Enemy> unSelectedRandom = new List<Enemy>(presetRandomEnemies);

        int randomBeatable = totalBeatable > presetBeatableEnemies.Count ? totalBeatable - presetBeatableEnemies.Count : 0;
        for (int i = 0; i < randomBeatable; i++)
        {
            Enemy sel = unSelectedRandom.GetRandomItem();
            unSelectedRandom.Remove(sel);
            unSelectedBeatableEnemies.Add(sel);
        }

        List<int> randomUnbeatable = new List<int>() {100, 110, 120, 130, 140, 150, 200, 210, 220, 230, 240, 250, 260, 270, 280, 290, 300 };
        foreach (var enemy in _enemyHolder.Enemies)
            enemy.StrengthChange = randomUnbeatable.GetRandomItem() * -1f;

        for (int i = 0; i < totalBeatable; i++)
        {
            Enemy sel = unSelectedBeatableEnemies.GetRandomItem();
            unSelectedBeatableEnemies.Remove(sel);

            sel.StrengthChange = rndSumToMax[i];
        }
    }
}
