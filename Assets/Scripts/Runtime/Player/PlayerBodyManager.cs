using Euphrates;
using UnityEngine;

public class PlayerBodyManager : MonoBehaviour
{
    [SerializeReference] SkinnedMeshRenderer _mesh;
    [SerializeReference] Transform _scaled;

    [Header("Muscle"), Space]
    [SerializeReference] FloatSO _strength;
    [SerializeReference] FloatSO _maxMuscle;

    [Header("Scale"), Space]
    [SerializeReference] FloatSO _minScale;
    [SerializeReference] FloatSO _maxScale;

    void OnEnable()
    {
        _strength.OnChange += SetBody;
    }

    void OnDisable()
    {
        _strength.OnChange -= SetBody;
    }

    void Awake()
    {
        _strength.Value = 30;
        SetBody(0f);
    }

    void SetBody(float _)
    {
        SetMuscle();
        SetScale();
    }

    void SetMuscle()
    {
        float amount = Mathf.Clamp(_strength.Value, 0f, _maxMuscle);

        if (amount < 50f)
        {
            _mesh.SetBlendShapeWeight(0, 0);
            Tween.Lerp(_mesh.GetBlendShapeWeight(1), (50f - amount) * 2f, .5f,
                (object val) => _mesh.SetBlendShapeWeight(1, (float)val));
            //_mesh.SetBlendShapeWeight(1, (50f - amount) * 2f);
            return;
        }

        float newAmt = amount - 50f;
        newAmt *= 2f;

        _mesh.SetBlendShapeWeight(1, 0);
        Tween.Lerp(_mesh.GetBlendShapeWeight(0), newAmt, .5f,
                (object val) => _mesh.SetBlendShapeWeight(0, (float)val));
        _mesh.SetBlendShapeWeight(0, newAmt);
    }

    void SetScale()
    {
        float step = _strength / _maxMuscle;
        float newScale = Mathf.Lerp(_minScale, _maxScale, step);

        void StepFunc(object val)
        {
            Vector3 newScale = Vector3.one * (float)val;
            _scaled.localScale = newScale;
        }

        Tween.Lerp(_scaled.localScale.x, newScale, 0.5f, StepFunc);
    }
}
