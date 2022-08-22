using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Enemy Holder", menuName = "SO Channels/Enemy Holder")]
public class EnemyHolderSO : ScriptableObject
{
    public List<Enemy> Enemies { get => new List<Enemy>(_sceneEnemies); }
    List<Enemy> _sceneEnemies = new List<Enemy>();
    public UnityEvent<Enemy> OnEnemyAdded;
    public UnityEvent<Enemy> OnEnemyRemoved;

    public void AddEnemy(Enemy enemy)
    {
        OnEnemyAdded.Invoke(enemy);
        _sceneEnemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        bool cehck = _sceneEnemies.Exists(p => p == enemy);

        if (!cehck)
            return;
        
        Enemies.Remove(enemy);
        OnEnemyRemoved.Invoke(enemy);
    }
}
