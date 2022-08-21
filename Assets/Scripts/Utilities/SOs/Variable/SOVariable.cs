﻿using UnityEngine;
using UnityEngine.Events;

namespace Euphrates
{
	[System.Serializable]
    public abstract class SOVariable<T> : ScriptableObject
	{
		protected virtual T Subtract(T x, T y)
        {
			T result = default(T);
			return result;
        }

		public T Value
		{
			get
			{
				return _value;
			}
			set
			{
				T change = Subtract(value, _value);
				_value = value;
				OnChange?.Invoke(change);
			}
		}

		[SerializeField] T _value;
		public event UnityAction<T> OnChange;

		public override string ToString() => _value.ToString();
    }
}