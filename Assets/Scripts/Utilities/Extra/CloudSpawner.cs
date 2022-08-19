using System.Collections.Generic;
using UnityEngine;
using Euphrates;

public class CloudSpawner : MonoBehaviour
{
    readonly int MAX_ITERATION = 10000;

    [SerializeField] List<GameObject> _prefabs = new List<GameObject>();
    List<GameObject> _clouds = new List<GameObject>();

    [Space]
    [SerializeField] List<CloudZone> _zones = new List<CloudZone>();

    public void Spawn()
    {
        DeleteAll();

        foreach (var zone in _zones)
        {
            float minSep = zone.MinSeperation == -1 ? float.MaxValue : zone.MinSeperation;
            float maxSep = zone.MaxSeperation == -1 ? float.MaxValue : zone.MaxSeperation;

            Vector3 realMin = zone.Min();
            Vector3 realMax = zone.Max();

            Vector3 lastPos = new Vector3(Random.Range(realMin.x, realMax.x), Random.Range(realMin.y, realMax.y), Random.Range(realMin.z, realMax.z));
            GameObject go = Instantiate(_prefabs.GetRandomItem(), transform);
            go.transform.position = lastPos;
            _clouds.Add(go);

            for (int i = 1; i < zone.Count; i++)
            {
                int j = 0;
                while (j++ < MAX_ITERATION)
                {
                    float sep = Random.Range(minSep, maxSep);
                    Vector3 dir = Random.insideUnitSphere;

                    Vector3 pos = lastPos + (dir * sep);
                    if (!zone.InZone(pos))
                        continue;

                    lastPos = pos;
                    GameObject spawned = Instantiate(_prefabs.GetRandomItem(), transform);
                    spawned.transform.position = pos;
                    _clouds.Add(spawned);
                    break;
                }
            }
        }
    }

    public void DeleteAll()
    {
        if (_clouds.Count < 1)
            return;

        for (int i = _clouds.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(_clouds[i]);
            _clouds.RemoveAt(i);
        }
    }
}

[System.Serializable]
struct CloudZone
{
    public int Count;
    public Vector3 Corner1;
    public Vector3 Corner2;
    [Tooltip("Put -1 for no minimum")]
    public float MinSeperation;
    [Tooltip("Put -1 for no maximum")]
    public float MaxSeperation;

    public Vector3 Max()
    {
        float maxX = Mathf.Max(Corner1.x, Corner2.x);
        float maxY = Mathf.Max(Corner1.y, Corner2.y);
        float maxZ = Mathf.Max(Corner1.z, Corner2.z);

        return new Vector3(maxX, maxY, maxZ);
    }

    public Vector3 Min()
    {
        float minX = Mathf.Min(Corner1.x, Corner2.x);
        float minY = Mathf.Min(Corner1.y, Corner2.y);
        float minZ = Mathf.Min(Corner1.z, Corner2.z);

        return new Vector3(minX, minY, minZ);
    }

    public bool InZone(Vector3 point)
    {
        Vector3 min = Min();
        Vector3 max = Max();

        return point.x > min.x && point.x < max.x && point.y > min.y && point.y < max.y && point.z > min.z && point.z < max.z;
    }

    public Vector3 GetRandomPoint()
    {
        Vector3 min = Min();
        Vector3 max = Max();

        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }
}