using Euphrates;
using UnityEngine;

public class Champ : MonoBehaviour
{
    [SerializeReference] FloatSO _strength;
    [SerializeReference] FloatSO _force;

    public void Throw()
    {
        SetChildRbVelocity(transform, new Vector3(0f, 20f, _strength * .25f + _force));
        SoundManager.Play("hit", 0, true, 1f);
    }

    public void Stop() => SetChildRbVelocity(transform, Vector3.forward * 3f);

    void SetChildRbVelocity(Transform parent, Vector3 velocity)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            SetChildRbVelocity(child, velocity);

            if (!child.TryGetComponent<Rigidbody>(out var rb))
                continue;

            rb.velocity = velocity;
        }
    }
}
