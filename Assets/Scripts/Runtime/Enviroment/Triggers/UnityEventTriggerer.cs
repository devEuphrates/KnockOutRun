using UnityEngine;
using UnityEngine.Events;

public class UnityEventTriggerer : MonoBehaviour
{
	public UnityEvent OnTrigger;

	public void Invoke() => OnTrigger?.Invoke();
}
