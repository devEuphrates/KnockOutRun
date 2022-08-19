using System.Collections.Generic;
using UnityEngine;
using Euphrates;
using Cinemachine;

public class CharacterAnimationControls : MonoBehaviour
{
    [SerializeReference] protected Animator _animator;
    [SerializeField] List<string> _punchAnimations = new List<string>();

    CinemachineImpulseSource _impulse;

    private void Awake() => _impulse = GetComponent<CinemachineImpulseSource>();

    public void Disable() => _animator.enabled = false;

    public void Punch()
    {
        if (_animator == null || _punchAnimations.Count == 0)
            return;

        // Get a random animation name from the list and pass it to the animator.
        _animator.Play(_punchAnimations.GetRandomItem());

        // Generate the punch hit shake.
        _impulse.GenerateImpulse(Vector3.forward);

        // Play the sound.
        SoundManager.Play("hit", 0, true, 1f);
    }
}
