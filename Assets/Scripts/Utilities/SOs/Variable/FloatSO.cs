using System;
using UnityEngine;

namespace Euphrates
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Float SO", menuName = "SO Variables/Float SO")]
    public class FloatSO : SOVariable<float>
    {

        public static implicit operator float(FloatSO so) => so.Value;
        public static explicit operator FloatSO(float value)
        {
            FloatSO rval = ScriptableObject.CreateInstance<FloatSO>();
            rval.Value = value;
            return rval;
        }

        protected override float Subtract(float x, float y) => x - y;

        public static FloatSO operator +(FloatSO var1, FloatSO var2) => DoOp(() => var1.Value + var2.Value);
        public static FloatSO operator +(FloatSO var1, int var2) => DoOp(() => var1.Value + var2);
        public static FloatSO operator +(int var1, FloatSO var2) => DoOp(() => var1 + var2);
        public static FloatSO operator -(FloatSO var1, FloatSO var2) => DoOp(() => var1.Value - var2.Value);
        public static FloatSO operator -(FloatSO var1, int var2) => DoOp(() => var1.Value - var2);
        public static FloatSO operator -(int var1, FloatSO var2) => DoOp(() => var1 - var2.Value);
        public static FloatSO operator *(FloatSO var1, FloatSO var2) => DoOp(() => var1.Value * var2.Value);
        public static FloatSO operator *(FloatSO var1, int var2) => DoOp(() => var1.Value * var2);
        public static FloatSO operator *(int var1, FloatSO var2) => DoOp(() => var1 * var2.Value);
        public static FloatSO operator /(FloatSO var1, FloatSO var2) => DoOp(() => var1.Value / var2.Value);
        public static FloatSO operator /(FloatSO var1, int var2) => DoOp(() => var1.Value / var2);
        public static FloatSO operator /(int var1, FloatSO var2) => DoOp(() => var1 / var2.Value);
        public static FloatSO operator %(FloatSO var1, FloatSO var2) => DoOp(() => var1.Value % var2.Value);
        public static FloatSO operator %(FloatSO var1, int var2) => DoOp(() => var1.Value % var2);
        public static FloatSO operator %(int var1, FloatSO var2) => DoOp(() => var1 % var2.Value);

        static FloatSO DoOp(Func<float> op)
        {
            FloatSO rval =  CreateInstance<FloatSO>();
            rval.Value = op.Invoke();
            return rval;
        }
    }
}