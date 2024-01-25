using System;
using NuiN.ScriptableHarmony.References;
using UnityEngine;

namespace NuiN.ScriptableHarmony.Variable.References
{
    public static class SetVariableExtensions
    {
        #region Add
        
        public static void Add(this SetVariable<float> reference, float value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetVariable<double> reference, double value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetVariable<decimal> reference, decimal value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetVariable<int> reference, int value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetVariable<long> reference, long value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetVariable<string> reference, string value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetVariable<Vector2> reference, Vector2 value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetVariable<Vector3> reference, Vector3 value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetVariable<Vector2Int> reference, Vector2Int value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetVariable<Vector3Int> reference, Vector3Int value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetVariable<Vector4> reference, Vector4 value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetVariable<Color> reference, Color value) 
            => reference.Set(reference.Val + value);
        
        public static void AddNoInvoke(this SetVariable<float> reference, float value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetVariable<double> reference, double value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetVariable<decimal> reference, decimal value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetVariable<int> reference, int value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetVariable<long> reference, long value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetVariable<string> reference, string value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetVariable<Vector2> reference, Vector2 value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetVariable<Vector3> reference, Vector3 value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetVariable<Vector2Int> reference, Vector2Int value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetVariable<Vector3Int> reference, Vector3Int value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetVariable<Vector4> reference, Vector4 value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetVariable<Color> reference, Color value) 
            => reference.SetNoInvoke(reference.Val + value);
        
        #endregion
        
        #region AddClamped
        
        public static void AddClamped(this SetVariable<float> reference, float value, float? min = null, float? max = null)
        {
            float newVal = reference.Val + value;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClamped(this SetVariable<double> reference, double value, double? min = null, double? max = null)
        {
            double newVal = reference.Val + value;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClamped(this SetVariable<decimal> reference, decimal value, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val + value;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClamped(this SetVariable<int> reference, int value, int? min = null, int? max = null)
        {
            int newVal = reference.Val + value;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClamped(this SetVariable<long> reference, long value, long? min = null, long? max = null)
        {
            long newVal = reference.Val + value;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClamped(this SetVariable<Vector2> reference, Vector2 value, float max)
        {
            Vector2 newVal = reference.Val + value;
            reference.Set(Vector2.ClampMagnitude(newVal, max));
        }
        public static void AddClamped(this SetVariable<Vector3> reference, Vector3 value, float max)
        {
            Vector3 newVal = reference.Val + value;
            reference.Set(Vector3.ClampMagnitude(newVal, max));
        }
        public static void AddClamped(this SetVariable<Vector2Int> reference, Vector2Int value, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val + value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        public static void AddClamped(this SetVariable<Vector3Int> reference, Vector3Int value, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val + value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        
        public static void AddClampedNoInvoke(this SetVariable<float> reference, float value, float? min = null, float? max = null)
        {
            float newVal = reference.Val + value;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClampedNoInvoke(this SetVariable<double> reference, double value, double? min = null, double? max = null)
        {
            double newVal = reference.Val + value;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClampedNoInvoke(this SetVariable<decimal> reference, decimal value, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val + value;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClampedNoInvoke(this SetVariable<int> reference, int value, int? min = null, int? max = null)
        {
            int newVal = reference.Val + value;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClampedNoInvoke(this SetVariable<long> reference, long value, long? min = null, long? max = null)
        {
            long newVal = reference.Val + value;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClampedNoInvoke(this SetVariable<Vector2> reference, Vector2 value, float max)
        {
            Vector2 newVal = reference.Val + value;
            reference.SetNoInvoke(Vector2.ClampMagnitude(newVal, max));
        }
        public static void AddClampedNoInvoke(this SetVariable<Vector3> reference, Vector3 value, float max)
        {
            Vector3 newVal = reference.Val + value;
            reference.SetNoInvoke(Vector3.ClampMagnitude(newVal, max));
        }
        public static void AddClampedNoInvoke(this SetVariable<Vector2Int> reference, Vector2Int value, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val + value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }
        public static void AddClampedNoInvoke(this SetVariable<Vector3Int> reference, Vector3Int value, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val + value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }

        #endregion
        
        #region Subtract

        public static void Subtract(this SetVariable<float> reference, float value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetVariable<double> reference, double value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetVariable<decimal> reference, decimal value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetVariable<int> reference, int value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetVariable<long> reference, long value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetVariable<Vector2> reference, Vector2 value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetVariable<Vector3> reference, Vector3 value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetVariable<Vector2Int> reference, Vector2Int value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetVariable<Vector3Int> reference, Vector3Int value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetVariable<Vector4> reference, Vector4 value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetVariable<Color> reference, Color value) 
            => reference.Set(reference.Val - value);
        
        public static void SubtractNoInvoke(this SetVariable<float> reference, float value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetVariable<double> reference, double value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetVariable<decimal> reference, decimal value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetVariable<int> reference, int value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetVariable<long> reference, long value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetVariable<Vector2> reference, Vector2 value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetVariable<Vector3> reference, Vector3 value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetVariable<Vector2Int> reference, Vector2Int value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetVariable<Vector3Int> reference, Vector3Int value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetVariable<Vector4> reference, Vector4 value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetVariable<Color> reference, Color value) 
            => reference.SetNoInvoke(reference.Val - value);
        
        #endregion
        
        #region SubtractClamped
        
        public static void SubtractClamped(this SetVariable<float> reference, float value, float? min = null, float? max = null)
        {
            float newVal = reference.Val - value;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClamped(this SetVariable<double> reference, double value, double? min = null, double? max = null)
        {
            double newVal = reference.Val - value;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClamped(this SetVariable<decimal> reference, decimal value, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val - value;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClamped(this SetVariable<int> reference, int value, int? min = null, int? max = null)
        {
            int newVal = reference.Val - value;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClamped(this SetVariable<long> reference, long value, long? min = null, long? max = null)
        {
            long newVal = reference.Val - value;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClamped(this SetVariable<Vector2> reference, Vector2 value, float maxLength)
        {
            Vector2 newVal = reference.Val - value;
            reference.Set(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void SubtractClamped(this SetVariable<Vector3> reference, Vector3 value, float maxLength)
        {
            Vector3 newVal = reference.Val - value;
            reference.Set(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void SubtractClamped(this SetVariable<Vector2Int> reference, Vector2Int value, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val - value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        public static void SubtractClamped(this SetVariable<Vector3Int> reference, Vector3Int value, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val - value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        
        public static void SubtractClampedNoInvoke(this SetVariable<float> reference, float value, float? min = null, float? max = null)
        {
            float newVal = reference.Val - value;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClampedNoInvoke(this SetVariable<double> reference, double value, double? min = null, double? max = null)
        {
            double newVal = reference.Val - value;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClampedNoInvoke(this SetVariable<decimal> reference, decimal value, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val - value;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClampedNoInvoke(this SetVariable<int> reference, int value, int? min = null, int? max = null)
        {
            int newVal = reference.Val - value;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClampedNoInvoke(this SetVariable<long> reference, long value, long? min = null, long? max = null)
        {
            long newVal = reference.Val - value;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClampedNoInvoke(this SetVariable<Vector2> reference, Vector2 value, float maxLength)
        {
            Vector2 newVal = reference.Val - value;
            reference.SetNoInvoke(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void SubtractClampedNoInvoke(this SetVariable<Vector3> reference, Vector3 value, float maxLength)
        {
            Vector3 newVal = reference.Val - value;
            reference.SetNoInvoke(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void SubtractClampedNoInvoke(this SetVariable<Vector2Int> reference, Vector2Int value, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val - value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }
        public static void SubtractClampedNoInvoke(this SetVariable<Vector3Int> reference, Vector3Int value, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val - value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }

        #endregion
        
        #region Multiply

        public static void Multiply(this SetVariable<float> reference, float factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<double> reference, double factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<decimal> reference, decimal factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<int> reference, int factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<long> reference, long factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<Vector2> reference, Vector2 factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<Vector2> reference, float factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<Vector3> reference, Vector3 factor) 
            => reference.Set(Vector3.Scale(reference.Val, factor));
        public static void Multiply(this SetVariable<Vector3> reference, float factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<Vector2Int> reference, Vector2Int factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<Vector2Int> reference, int factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<Vector3Int> reference, Vector3Int factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<Vector3Int> reference, int factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<Vector4> reference, float factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<Color> reference, Color factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetVariable<Color> reference, float factor) 
            => reference.Set(reference.Val * factor);
        
        public static void MultiplyNoInvoke(this SetVariable<float> reference, float factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<double> reference, double factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<decimal> reference, decimal factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<int> reference, int factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<long> reference, long factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<Vector2> reference, Vector2 factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<Vector2> reference, float factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<Vector3> reference, Vector3 factor) 
            => reference.SetNoInvoke(Vector3.Scale(reference.Val, factor));
        public static void MultiplyNoInvoke(this SetVariable<Vector3> reference, float factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<Vector2Int> reference, Vector2Int factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<Vector2Int> reference, int factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<Vector3Int> reference, Vector3Int factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<Vector3Int> reference, int factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<Vector4> reference, float factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<Color> reference, Color factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetVariable<Color> reference, float factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        
        #endregion
        
        #region MultiplyClamped
        
        public static void MultiplyClamped(this SetVariable<float> reference, float factor, float? min = null, float? max = null)
        {
            float newVal = reference.Val * factor;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClamped(this SetVariable<double> reference, double factor, double? min = null, double? max = null)
        {
            double newVal = reference.Val * factor;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClamped(this SetVariable<decimal> reference, decimal factor, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val * factor;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClamped(this SetVariable<int> reference, int factor, int? min = null, int? max = null)
        {
            int newVal = reference.Val * factor;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClamped(this SetVariable<long> reference, long factor, long? min = null, long? max = null)
        {
            long newVal = reference.Val * factor;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClamped(this SetVariable<Vector2> reference, Vector2 factor, float maxLength)
        {
            Vector2 newVal = reference.Val * factor;
            reference.Set(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClamped(this SetVariable<Vector2> reference, float factor, float maxLength)
        {
            Vector2 newVal = reference.Val * factor;
            reference.Set(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClamped(this SetVariable<Vector3> reference, Vector3 factor, float maxLength)
        {
            Vector3 newVal = Vector3.Scale(reference.Val, factor);
            reference.Set(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClamped(this SetVariable<Vector3> reference, float factor, float maxLength)
        {
            Vector3 newVal = reference.Val * factor;
            reference.Set(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClamped(this SetVariable<Vector2Int> reference, Vector2Int factor, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        public static void MultiplyClamped(this SetVariable<Vector3Int> reference, Vector3Int factor, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        public static void MultiplyClamped(this SetVariable<Vector2Int> reference, int factor, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        public static void MultiplyClamped(this SetVariable<Vector3Int> reference, int factor, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        
        
        public static void MultiplyClampedNoInvoke(this SetVariable<float> reference, float factor, float? min = null, float? max = null)
        {
            float newVal = reference.Val * factor;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClampedNoInvoke(this SetVariable<double> reference, double factor, double? min = null, double? max = null)
        {
            double newVal = reference.Val * factor;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClampedNoInvoke(this SetVariable<decimal> reference, decimal factor, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val * factor;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClampedNoInvoke(this SetVariable<int> reference, int factor, int? min = null, int? max = null)
        {
            int newVal = reference.Val * factor;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClampedNoInvoke(this SetVariable<long> reference, long factor, long? min = null, long? max = null)
        {
            long newVal = reference.Val * factor;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClampedNoInvoke(this SetVariable<Vector2> reference, Vector2 factor, float maxLength)
        {
            Vector2 newVal = reference.Val * factor;
            reference.SetNoInvoke(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClampedNoInvoke(this SetVariable<Vector2> reference, float factor, float maxLength)
        {
            Vector2 newVal = reference.Val * factor;
            reference.SetNoInvoke(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClampedNoInvoke(this SetVariable<Vector3> reference, Vector3 factor, float maxLength)
        {
            Vector3 newVal = Vector3.Scale(reference.Val, factor);
            reference.SetNoInvoke(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClampedNoInvoke(this SetVariable<Vector3> reference, float factor, float maxLength)
        {
            Vector3 newVal = reference.Val * factor;
            reference.SetNoInvoke(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClampedNoInvoke(this SetVariable<Vector2Int> reference, Vector2Int factor, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }
        public static void MultiplyClampedNoInvoke(this SetVariable<Vector3Int> reference, Vector3Int factor, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }
        public static void MultiplyClampedNoInvoke(this SetVariable<Vector2Int> reference, int factor, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }
        public static void MultiplyClampedNoInvoke(this SetVariable<Vector3Int> reference, int factor, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }

        #endregion
        
        #region Divide

        public static void Divide(this SetVariable<float> reference, float divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetVariable<double> reference, double divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetVariable<decimal> reference, decimal divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetVariable<int> reference, int divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetVariable<long> reference, long divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetVariable<Vector2> reference, Vector2 divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetVariable<Vector2> reference, float divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetVariable<Vector3> reference, float divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetVariable<Vector2Int> reference, int divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetVariable<Vector3Int> reference, int divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetVariable<Vector4> reference, float divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetVariable<Color> reference, float divisor) 
            => reference.Set(reference.Val / divisor);
        
        public static void DivideNoInvoke(this SetVariable<float> reference, float divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetVariable<double> reference, double divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetVariable<decimal> reference, decimal divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetVariable<int> reference, int divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetVariable<long> reference, long divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetVariable<Vector2> reference, Vector2 divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetVariable<Vector2> reference, float divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetVariable<Vector3> reference, float divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetVariable<Vector2Int> reference, int divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetVariable<Vector3Int> reference, int divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetVariable<Vector4> reference, float divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetVariable<Color> reference, float divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        
        #endregion
        
        #region DivideClamped
        
        public static void DivideClamped(this SetVariable<float> reference, float divisor, float? min = null, float? max = null)
        {
            float newVal = reference.Val / divisor;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClamped(this SetVariable<double> reference, double divisor, double? min = null, double? max = null)
        {
            double newVal = reference.Val / divisor;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClamped(this SetVariable<decimal> reference, decimal divisor, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val / divisor;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClamped(this SetVariable<int> reference, int divisor, int? min = null, int? max = null)
        {
            int newVal = reference.Val / divisor;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClamped(this SetVariable<long> reference, long divisor, long? min = null, long? max = null)
        {
            long newVal = reference.Val / divisor;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClamped(this SetVariable<Vector2> reference, Vector2 divisor, float maxLength)
        {
            Vector2 newVal = reference.Val / divisor;
            reference.Set(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClamped(this SetVariable<Vector2> reference, float divisor, float maxLength)
        {
            Vector2 newVal = reference.Val / divisor;
            reference.Set(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClamped(this SetVariable<Vector3> reference, Vector3 divisor, float maxLength)
        {
            Vector3 newVal = Vector3.Scale(reference.Val, divisor);
            reference.Set(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClamped(this SetVariable<Vector3> reference, float divisor, float maxLength)
        {
            Vector3 newVal = reference.Val / divisor;
            reference.Set(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClamped(this SetVariable<Vector2Int> reference, int divisor, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val / divisor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        public static void DivideClamped(this SetVariable<Vector3Int> reference, int divisor, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val / divisor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        
        
        public static void DivideClampedNoInvoke(this SetVariable<float> reference, float divisor, float? min = null, float? max = null)
        {
            float newVal = reference.Val / divisor;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClampedNoInvoke(this SetVariable<double> reference, double divisor, double? min = null, double? max = null)
        {
            double newVal = reference.Val / divisor;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClampedNoInvoke(this SetVariable<decimal> reference, decimal divisor, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val / divisor;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClampedNoInvoke(this SetVariable<int> reference, int divisor, int? min = null, int? max = null)
        {
            int newVal = reference.Val / divisor;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClampedNoInvoke(this SetVariable<long> reference, long divisor, long? min = null, long? max = null)
        {
            long newVal = reference.Val / divisor;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClampedNoInvoke(this SetVariable<Vector2> reference, Vector2 divisor, float maxLength)
        {
            Vector2 newVal = reference.Val / divisor;
            reference.SetNoInvoke(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClampedNoInvoke(this SetVariable<Vector2> reference, float divisor, float maxLength)
        {
            Vector2 newVal = reference.Val / divisor;
            reference.SetNoInvoke(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClampedNoInvoke(this SetVariable<Vector3> reference, Vector3 divisor, float maxLength)
        {
            Vector3 newVal = Vector3.Scale(reference.Val, divisor);
            reference.SetNoInvoke(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClampedNoInvoke(this SetVariable<Vector3> reference, float divisor, float maxLength)
        {
            Vector3 newVal = reference.Val / divisor;
            reference.SetNoInvoke(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClampedNoInvoke(this SetVariable<Vector2Int> reference, int divisor, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val / divisor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }
        public static void DivideClampedNoInvoke(this SetVariable<Vector3Int> reference, int divisor, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val / divisor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }

        #endregion
        
        #region Toggle
        
        public static void Toggle(this SetVariable<bool> reference) => reference.Set(!reference.Val);
        public static void ToggleNoInvoke(this SetVariable<bool> reference) => reference.SetNoInvoke(!reference.Val);
        
        #endregion
    }
}


