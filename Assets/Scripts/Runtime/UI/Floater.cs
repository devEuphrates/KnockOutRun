using Euphrates;
using TMPro;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeReference] TextMeshProUGUI _text;
    [SerializeReference] CanvasGroup _group;

    float _duration = 0f;
    float _fadeOut = 0f;

    public void UpdateText(string text) => _text.text = text;

    public void Show(float duration, float fadeIn = 0.2f, float fadeOut = 0.2f)
    {
        _duration = duration;
        _fadeOut = fadeOut;
        Tween.Lerp(0f, 1f, fadeIn, (object val) => _group.alpha = (float)val, OnFadeIn);
    }

    void OnFadeIn() => GameTimer.CreateTimer("Floater Timer - " + gameObject.GetNamePath(), _duration, 
        () => Tween.Lerp(1f, 0f, _fadeOut, (object val) => _group.alpha = (float)val, null));
}
