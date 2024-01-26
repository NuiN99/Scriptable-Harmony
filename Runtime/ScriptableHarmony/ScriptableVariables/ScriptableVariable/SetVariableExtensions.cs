using System;
using UnityEngine;

namespace NuiN.ScriptableHarmony
{
    public static class SetVariableExtensions
    {
        #region Add
        
        public static void Add(this SetScriptableVariable<float> reference, float value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetScriptableVariable<double> reference, double value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetScriptableVariable<decimal> reference, decimal value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetScriptableVariable<int> reference, int value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetScriptableVariable<long> reference, long value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetScriptableVariable<string> reference, string value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetScriptableVariable<Vector2> reference, Vector2 value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetScriptableVariable<Vector3> reference, Vector3 value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetScriptableVariable<Vector2Int> reference, Vector2Int value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetScriptableVariable<Vector3Int> reference, Vector3Int value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetScriptableVariable<Vector4> reference, Vector4 value) 
            => reference.Set(reference.Val + value);
        public static void Add(this SetScriptableVariable<Color> reference, Color value) 
            => reference.Set(reference.Val + value);
        
        public static void AddNoInvoke(this SetScriptableVariable<float> reference, float value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetScriptableVariable<double> reference, double value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetScriptableVariable<decimal> reference, decimal value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetScriptableVariable<int> reference, int value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetScriptableVariable<long> reference, long value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetScriptableVariable<string> reference, string value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetScriptableVariable<Vector2> reference, Vector2 value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetScriptableVariable<Vector3> reference, Vector3 value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetScriptableVariable<Vector2Int> reference, Vector2Int value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetScriptableVariable<Vector3Int> reference, Vector3Int value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetScriptableVariable<Vector4> reference, Vector4 value) 
            => reference.SetNoInvoke(reference.Val + value);
        public static void AddNoInvoke(this SetScriptableVariable<Color> reference, Color value) 
            => reference.SetNoInvoke(reference.Val + value);
        
        #endregion
        
        #region AddClamped
        
        public static void AddClamped(this SetScriptableVariable<float> reference, float value, float? min = null, float? max = null)
        {
            float newVal = reference.Val + value;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClamped(this SetScriptableVariable<double> reference, double value, double? min = null, double? max = null)
        {
            double newVal = reference.Val + value;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClamped(this SetScriptableVariable<decimal> reference, decimal value, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val + value;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClamped(this SetScriptableVariable<int> reference, int value, int? min = null, int? max = null)
        {
            int newVal = reference.Val + value;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClamped(this SetScriptableVariable<long> reference, long value, long? min = null, long? max = null)
        {
            long newVal = reference.Val + value;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClamped(this SetScriptableVariable<Vector2> reference, Vector2 value, float max)
        {
            Vector2 newVal = reference.Val + value;
            reference.Set(Vector2.ClampMagnitude(newVal, max));
        }
        public static void AddClamped(this SetScriptableVariable<Vector3> reference, Vector3 value, float max)
        {
            Vector3 newVal = reference.Val + value;
            reference.Set(Vector3.ClampMagnitude(newVal, max));
        }
        public static void AddClamped(this SetScriptableVariable<Vector2Int> reference, Vector2Int value, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val + value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        public static void AddClamped(this SetScriptableVariable<Vector3Int> reference, Vector3Int value, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val + value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        
        public static void AddClampedNoInvoke(this SetScriptableVariable<float> reference, float value, float? min = null, float? max = null)
        {
            float newVal = reference.Val + value;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClampedNoInvoke(this SetScriptableVariable<double> reference, double value, double? min = null, double? max = null)
        {
            double newVal = reference.Val + value;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClampedNoInvoke(this SetScriptableVariable<decimal> reference, decimal value, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val + value;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClampedNoInvoke(this SetScriptableVariable<int> reference, int value, int? min = null, int? max = null)
        {
            int newVal = reference.Val + value;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClampedNoInvoke(this SetScriptableVariable<long> reference, long value, long? min = null, long? max = null)
        {
            long newVal = reference.Val + value;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void AddClampedNoInvoke(this SetScriptableVariable<Vector2> reference, Vector2 value, float max)
        {
            Vector2 newVal = reference.Val + value;
            reference.SetNoInvoke(Vector2.ClampMagnitude(newVal, max));
        }
        public static void AddClampedNoInvoke(this SetScriptableVariable<Vector3> reference, Vector3 value, float max)
        {
            Vector3 newVal = reference.Val + value;
            reference.SetNoInvoke(Vector3.ClampMagnitude(newVal, max));
        }
        public static void AddClampedNoInvoke(this SetScriptableVariable<Vector2Int> reference, Vector2Int value, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val + value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }
        public static void AddClampedNoInvoke(this SetScriptableVariable<Vector3Int> reference, Vector3Int value, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val + value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }

        #endregion
        
        #region Subtract

        public static void Subtract(this SetScriptableVariable<float> reference, float value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetScriptableVariable<double> reference, double value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetScriptableVariable<decimal> reference, decimal value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetScriptableVariable<int> reference, int value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetScriptableVariable<long> reference, long value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetScriptableVariable<Vector2> reference, Vector2 value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetScriptableVariable<Vector3> reference, Vector3 value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetScriptableVariable<Vector2Int> reference, Vector2Int value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetScriptableVariable<Vector3Int> reference, Vector3Int value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetScriptableVariable<Vector4> reference, Vector4 value) 
            => reference.Set(reference.Val - value);
        public static void Subtract(this SetScriptableVariable<Color> reference, Color value) 
            => reference.Set(reference.Val - value);
        
        public static void SubtractNoInvoke(this SetScriptableVariable<float> reference, float value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetScriptableVariable<double> reference, double value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetScriptableVariable<decimal> reference, decimal value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetScriptableVariable<int> reference, int value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetScriptableVariable<long> reference, long value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetScriptableVariable<Vector2> reference, Vector2 value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetScriptableVariable<Vector3> reference, Vector3 value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetScriptableVariable<Vector2Int> reference, Vector2Int value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetScriptableVariable<Vector3Int> reference, Vector3Int value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetScriptableVariable<Vector4> reference, Vector4 value) 
            => reference.SetNoInvoke(reference.Val - value);
        public static void SubtractNoInvoke(this SetScriptableVariable<Color> reference, Color value) 
            => reference.SetNoInvoke(reference.Val - value);
        
        #endregion
        
        #region SubtractClamped
        
        public static void SubtractClamped(this SetScriptableVariable<float> reference, float value, float? min = null, float? max = null)
        {
            float newVal = reference.Val - value;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClamped(this SetScriptableVariable<double> reference, double value, double? min = null, double? max = null)
        {
            double newVal = reference.Val - value;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClamped(this SetScriptableVariable<decimal> reference, decimal value, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val - value;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClamped(this SetScriptableVariable<int> reference, int value, int? min = null, int? max = null)
        {
            int newVal = reference.Val - value;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClamped(this SetScriptableVariable<long> reference, long value, long? min = null, long? max = null)
        {
            long newVal = reference.Val - value;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClamped(this SetScriptableVariable<Vector2> reference, Vector2 value, float maxLength)
        {
            Vector2 newVal = reference.Val - value;
            reference.Set(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void SubtractClamped(this SetScriptableVariable<Vector3> reference, Vector3 value, float maxLength)
        {
            Vector3 newVal = reference.Val - value;
            reference.Set(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void SubtractClamped(this SetScriptableVariable<Vector2Int> reference, Vector2Int value, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val - value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        public static void SubtractClamped(this SetScriptableVariable<Vector3Int> reference, Vector3Int value, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val - value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        
        public static void SubtractClampedNoInvoke(this SetScriptableVariable<float> reference, float value, float? min = null, float? max = null)
        {
            float newVal = reference.Val - value;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClampedNoInvoke(this SetScriptableVariable<double> reference, double value, double? min = null, double? max = null)
        {
            double newVal = reference.Val - value;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClampedNoInvoke(this SetScriptableVariable<decimal> reference, decimal value, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val - value;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClampedNoInvoke(this SetScriptableVariable<int> reference, int value, int? min = null, int? max = null)
        {
            int newVal = reference.Val - value;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClampedNoInvoke(this SetScriptableVariable<long> reference, long value, long? min = null, long? max = null)
        {
            long newVal = reference.Val - value;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void SubtractClampedNoInvoke(this SetScriptableVariable<Vector2> reference, Vector2 value, float maxLength)
        {
            Vector2 newVal = reference.Val - value;
            reference.SetNoInvoke(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void SubtractClampedNoInvoke(this SetScriptableVariable<Vector3> reference, Vector3 value, float maxLength)
        {
            Vector3 newVal = reference.Val - value;
            reference.SetNoInvoke(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void SubtractClampedNoInvoke(this SetScriptableVariable<Vector2Int> reference, Vector2Int value, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val - value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }
        public static void SubtractClampedNoInvoke(this SetScriptableVariable<Vector3Int> reference, Vector3Int value, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val - value;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }

        #endregion
        
        #region Multiply

        public static void Multiply(this SetScriptableVariable<float> reference, float factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<double> reference, double factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<decimal> reference, decimal factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<int> reference, int factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<long> reference, long factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<Vector2> reference, Vector2 factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<Vector2> reference, float factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<Vector3> reference, Vector3 factor) 
            => reference.Set(Vector3.Scale(reference.Val, factor));
        public static void Multiply(this SetScriptableVariable<Vector3> reference, float factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<Vector2Int> reference, Vector2Int factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<Vector2Int> reference, int factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<Vector3Int> reference, Vector3Int factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<Vector3Int> reference, int factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<Vector4> reference, float factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<Color> reference, Color factor) 
            => reference.Set(reference.Val * factor);
        public static void Multiply(this SetScriptableVariable<Color> reference, float factor) 
            => reference.Set(reference.Val * factor);
        
        public static void MultiplyNoInvoke(this SetScriptableVariable<float> reference, float factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<double> reference, double factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<decimal> reference, decimal factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<int> reference, int factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<long> reference, long factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<Vector2> reference, Vector2 factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<Vector2> reference, float factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<Vector3> reference, Vector3 factor) 
            => reference.SetNoInvoke(Vector3.Scale(reference.Val, factor));
        public static void MultiplyNoInvoke(this SetScriptableVariable<Vector3> reference, float factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<Vector2Int> reference, Vector2Int factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<Vector2Int> reference, int factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<Vector3Int> reference, Vector3Int factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<Vector3Int> reference, int factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<Vector4> reference, float factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<Color> reference, Color factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        public static void MultiplyNoInvoke(this SetScriptableVariable<Color> reference, float factor) 
            => reference.SetNoInvoke(reference.Val * factor);
        
        #endregion
        
        #region MultiplyClamped
        
        public static void MultiplyClamped(this SetScriptableVariable<float> reference, float factor, float? min = null, float? max = null)
        {
            float newVal = reference.Val * factor;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClamped(this SetScriptableVariable<double> reference, double factor, double? min = null, double? max = null)
        {
            double newVal = reference.Val * factor;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClamped(this SetScriptableVariable<decimal> reference, decimal factor, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val * factor;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClamped(this SetScriptableVariable<int> reference, int factor, int? min = null, int? max = null)
        {
            int newVal = reference.Val * factor;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClamped(this SetScriptableVariable<long> reference, long factor, long? min = null, long? max = null)
        {
            long newVal = reference.Val * factor;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClamped(this SetScriptableVariable<Vector2> reference, Vector2 factor, float maxLength)
        {
            Vector2 newVal = reference.Val * factor;
            reference.Set(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClamped(this SetScriptableVariable<Vector2> reference, float factor, float maxLength)
        {
            Vector2 newVal = reference.Val * factor;
            reference.Set(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClamped(this SetScriptableVariable<Vector3> reference, Vector3 factor, float maxLength)
        {
            Vector3 newVal = Vector3.Scale(reference.Val, factor);
            reference.Set(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClamped(this SetScriptableVariable<Vector3> reference, float factor, float maxLength)
        {
            Vector3 newVal = reference.Val * factor;
            reference.Set(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClamped(this SetScriptableVariable<Vector2Int> reference, Vector2Int factor, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        public static void MultiplyClamped(this SetScriptableVariable<Vector3Int> reference, Vector3Int factor, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        public static void MultiplyClamped(this SetScriptableVariable<Vector2Int> reference, int factor, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        public static void MultiplyClamped(this SetScriptableVariable<Vector3Int> reference, int factor, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        
        
        public static void MultiplyClampedNoInvoke(this SetScriptableVariable<float> reference, float factor, float? min = null, float? max = null)
        {
            float newVal = reference.Val * factor;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClampedNoInvoke(this SetScriptableVariable<double> reference, double factor, double? min = null, double? max = null)
        {
            double newVal = reference.Val * factor;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClampedNoInvoke(this SetScriptableVariable<decimal> reference, decimal factor, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val * factor;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClampedNoInvoke(this SetScriptableVariable<int> reference, int factor, int? min = null, int? max = null)
        {
            int newVal = reference.Val * factor;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClampedNoInvoke(this SetScriptableVariable<long> reference, long factor, long? min = null, long? max = null)
        {
            long newVal = reference.Val * factor;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void MultiplyClampedNoInvoke(this SetScriptableVariable<Vector2> reference, Vector2 factor, float maxLength)
        {
            Vector2 newVal = reference.Val * factor;
            reference.SetNoInvoke(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClampedNoInvoke(this SetScriptableVariable<Vector2> reference, float factor, float maxLength)
        {
            Vector2 newVal = reference.Val * factor;
            reference.SetNoInvoke(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClampedNoInvoke(this SetScriptableVariable<Vector3> reference, Vector3 factor, float maxLength)
        {
            Vector3 newVal = Vector3.Scale(reference.Val, factor);
            reference.SetNoInvoke(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClampedNoInvoke(this SetScriptableVariable<Vector3> reference, float factor, float maxLength)
        {
            Vector3 newVal = reference.Val * factor;
            reference.SetNoInvoke(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void MultiplyClampedNoInvoke(this SetScriptableVariable<Vector2Int> reference, Vector2Int factor, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }
        public static void MultiplyClampedNoInvoke(this SetScriptableVariable<Vector3Int> reference, Vector3Int factor, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }
        public static void MultiplyClampedNoInvoke(this SetScriptableVariable<Vector2Int> reference, int factor, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }
        public static void MultiplyClampedNoInvoke(this SetScriptableVariable<Vector3Int> reference, int factor, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val * factor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }

        #endregion
        
        #region Divide

        public static void Divide(this SetScriptableVariable<float> reference, float divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetScriptableVariable<double> reference, double divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetScriptableVariable<decimal> reference, decimal divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetScriptableVariable<int> reference, int divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetScriptableVariable<long> reference, long divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetScriptableVariable<Vector2> reference, Vector2 divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetScriptableVariable<Vector2> reference, float divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetScriptableVariable<Vector3> reference, float divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetScriptableVariable<Vector2Int> reference, int divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetScriptableVariable<Vector3Int> reference, int divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetScriptableVariable<Vector4> reference, float divisor) 
            => reference.Set(reference.Val / divisor);
        public static void Divide(this SetScriptableVariable<Color> reference, float divisor) 
            => reference.Set(reference.Val / divisor);
        
        public static void DivideNoInvoke(this SetScriptableVariable<float> reference, float divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetScriptableVariable<double> reference, double divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetScriptableVariable<decimal> reference, decimal divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetScriptableVariable<int> reference, int divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetScriptableVariable<long> reference, long divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetScriptableVariable<Vector2> reference, Vector2 divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetScriptableVariable<Vector2> reference, float divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetScriptableVariable<Vector3> reference, float divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetScriptableVariable<Vector2Int> reference, int divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetScriptableVariable<Vector3Int> reference, int divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetScriptableVariable<Vector4> reference, float divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        public static void DivideNoInvoke(this SetScriptableVariable<Color> reference, float divisor) 
            => reference.SetNoInvoke(reference.Val / divisor);
        
        #endregion
        
        #region DivideClamped
        
        public static void DivideClamped(this SetScriptableVariable<float> reference, float divisor, float? min = null, float? max = null)
        {
            float newVal = reference.Val / divisor;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClamped(this SetScriptableVariable<double> reference, double divisor, double? min = null, double? max = null)
        {
            double newVal = reference.Val / divisor;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClamped(this SetScriptableVariable<decimal> reference, decimal divisor, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val / divisor;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClamped(this SetScriptableVariable<int> reference, int divisor, int? min = null, int? max = null)
        {
            int newVal = reference.Val / divisor;
            reference.Set(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClamped(this SetScriptableVariable<long> reference, long divisor, long? min = null, long? max = null)
        {
            long newVal = reference.Val / divisor;
            reference.Set(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClamped(this SetScriptableVariable<Vector2> reference, Vector2 divisor, float maxLength)
        {
            Vector2 newVal = reference.Val / divisor;
            reference.Set(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClamped(this SetScriptableVariable<Vector2> reference, float divisor, float maxLength)
        {
            Vector2 newVal = reference.Val / divisor;
            reference.Set(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClamped(this SetScriptableVariable<Vector3> reference, Vector3 divisor, float maxLength)
        {
            Vector3 newVal = Vector3.Scale(reference.Val, divisor);
            reference.Set(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClamped(this SetScriptableVariable<Vector3> reference, float divisor, float maxLength)
        {
            Vector3 newVal = reference.Val / divisor;
            reference.Set(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClamped(this SetScriptableVariable<Vector2Int> reference, int divisor, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val / divisor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        public static void DivideClamped(this SetScriptableVariable<Vector3Int> reference, int divisor, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val / divisor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.Set(newVal);
        }
        
        
        public static void DivideClampedNoInvoke(this SetScriptableVariable<float> reference, float divisor, float? min = null, float? max = null)
        {
            float newVal = reference.Val / divisor;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClampedNoInvoke(this SetScriptableVariable<double> reference, double divisor, double? min = null, double? max = null)
        {
            double newVal = reference.Val / divisor;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClampedNoInvoke(this SetScriptableVariable<decimal> reference, decimal divisor, decimal? min = null, decimal? max = null)
        {
            decimal newVal = reference.Val / divisor;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClampedNoInvoke(this SetScriptableVariable<int> reference, int divisor, int? min = null, int? max = null)
        {
            int newVal = reference.Val / divisor;
            reference.SetNoInvoke(Mathf.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClampedNoInvoke(this SetScriptableVariable<long> reference, long divisor, long? min = null, long? max = null)
        {
            long newVal = reference.Val / divisor;
            reference.SetNoInvoke(Math.Clamp(newVal, min ?? newVal, max ?? newVal));
        }
        public static void DivideClampedNoInvoke(this SetScriptableVariable<Vector2> reference, Vector2 divisor, float maxLength)
        {
            Vector2 newVal = reference.Val / divisor;
            reference.SetNoInvoke(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClampedNoInvoke(this SetScriptableVariable<Vector2> reference, float divisor, float maxLength)
        {
            Vector2 newVal = reference.Val / divisor;
            reference.SetNoInvoke(Vector2.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClampedNoInvoke(this SetScriptableVariable<Vector3> reference, Vector3 divisor, float maxLength)
        {
            Vector3 newVal = Vector3.Scale(reference.Val, divisor);
            reference.SetNoInvoke(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClampedNoInvoke(this SetScriptableVariable<Vector3> reference, float divisor, float maxLength)
        {
            Vector3 newVal = reference.Val / divisor;
            reference.SetNoInvoke(Vector3.ClampMagnitude(newVal, maxLength));
        }
        public static void DivideClampedNoInvoke(this SetScriptableVariable<Vector2Int> reference, int divisor, Vector2Int? min = null, Vector2Int? max = null)
        {
            Vector2Int newVal = reference.Val / divisor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }
        public static void DivideClampedNoInvoke(this SetScriptableVariable<Vector3Int> reference, int divisor, Vector3Int? min = null, Vector3Int? max = null)
        {
            Vector3Int newVal = reference.Val / divisor;
            newVal.Clamp(min ?? newVal, max ?? newVal);
            reference.SetNoInvoke(newVal);
        }

        #endregion
        
        #region Toggle
        
        public static void Toggle(this SetScriptableVariable<bool> reference) => reference.Set(!reference.Val);
        public static void ToggleNoInvoke(this SetScriptableVariable<bool> reference) => reference.SetNoInvoke(!reference.Val);
        
        #endregion
    }
}


