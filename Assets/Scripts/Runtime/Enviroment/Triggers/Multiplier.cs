using Euphrates;
using TMPro;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    [SerializeReference] MeshRenderer _mesh;
    [SerializeReference] TextMeshProUGUI _text;
    [SerializeReference] FloatSO _strength;
    [SerializeReference] LayerMaskSO _champLayer;
    [SerializeReference] TriggerChannelSO _endGameTrigger;
    [SerializeReference] FloatSO _gameMultiplier;

    void OnEnable() => _endGameTrigger.AddListener(OnEnd);

    void OnDisable() => _endGameTrigger.RemoveListener(OnEnd);

    float _multiplier = 1f;
    bool _isEnabled = true;

    public void Init(float multiplier)
    {
        float randR = Random.Range(0f, 1f);

        Color randCol = Color.HSVToRGB(randR, 1, 1);
        Init(multiplier, randCol);
    }

    public void Init(float multiplier, Color color)
    {
        _multiplier = multiplier;
        _mesh.materials[0].color = color;
        _text.text = $"x{_multiplier.ToString("0.##")}";
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!_isEnabled)
            return;

        int pow = (int)Mathf.Pow(2, collision.gameObject.layer);

        if ((_champLayer.Value & pow) != pow)
            return;

        Champ champ = collision.gameObject.transform.GetFirstParentsComponent<Champ>();
        if (champ == null)
            return;

        champ.Stop();

        if (_gameMultiplier < _multiplier)
            _gameMultiplier.Value = _multiplier;

        var pos = transform.localPosition;
        Tween.Lerp(pos.y, pos.y + 5f, 1f, (object val) => transform.localPosition = new Vector3(pos.x, (float) val, pos.z));

        _endGameTrigger.Invoke();
    }

    void OnEnd()
    {
        _isEnabled = false;
    }
}
