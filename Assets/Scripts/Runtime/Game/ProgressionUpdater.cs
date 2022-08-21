using Euphrates;
using UnityEngine;

public class ProgressionUpdater : MonoBehaviour
{
    [SerializeReference] FloatSO _progression;
    [SerializeReference] Transform _player;

    Transform _transform;

    private void Awake() => _transform = transform;

    private void Start()
    {
        _progression.Value = 0;
    }

    private void Update()
    {
        float maxDist = _transform.position.z;
        _progression.Value = Mathf.Clamp01(_player.position.z / maxDist);
    }
}
