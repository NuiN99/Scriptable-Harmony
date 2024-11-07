#if UNITY_EDITOR

using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NuiN.NExtensions
{
	[CustomEditor( typeof( MonoBehaviour ), true ), CanEditMultipleObjects]
	public class AttributesEditor : UnityEditor.Editor
	{
		void OnEnable() => RuntimeHelper.SubOnUpdate(Repaint);
		void OnDisable() => RuntimeHelper.SubOnUpdate(Repaint);

		public override void OnInspectorGUI()
		{
			MonoBehaviour script = (MonoBehaviour)target;
			MethodInfo[] methods = script.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				.Where(method => method.GetCustomAttributes(typeof(MethodButtonAttribute), true).Length > 0)
				.ToArray();

			foreach (var method in methods)
			{
				MethodButtonAttribute attribute = (MethodButtonAttribute)method.GetCustomAttributes(typeof(MethodButtonAttribute), true)[0];
				string buttonLabel = attribute == null ? method.Name : attribute.label;

				if (attribute == null || (attribute.onlyShowInPlayMode && !Application.isPlaying)) continue;
				if (GUILayout.Button(buttonLabel)) method.Invoke(script, attribute?.parameters);
			}

			if (Application.isPlaying)
			{
				this.DrawShowInInspector();
			}
			
			base.OnInspectorGUI();
		}
	}
}

#endif