using Euphrates;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class EnemyStrengthIndicator : MonoBehaviour
{
    [SerializeReference] Enemy _enemy;
    [SerializeReference] TextMeshProUGUI _text;

    [Header("Animation"), Space]
    [SerializeField] float _duration = .5f;
    [SerializeReference] AnimationCurveSO _alphaAnim;
    [SerializeReference] AnimationCurveSO _scaleAnim;

    CanvasGroup _cg;

    private void Awake() => _cg = GetComponent<CanvasGroup>();

    void OnEnable() => _enemy.OnDefeat.AddListener(Defeat);

    void OnDisable() => _enemy.OnDefeat.RemoveListener(Defeat);
    void Start() => _text.text = _enemy.Strength.ToString();

    void Defeat() => Tween.Lerp(1, 0, .5f, (object val) => _cg.alpha = (float)val);
}
