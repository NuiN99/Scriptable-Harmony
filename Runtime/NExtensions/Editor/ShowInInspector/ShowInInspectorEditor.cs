using UnityEditor;

namespace NuiN.NExtensions
{
	// modified from https://github.com/BrainswitchMachina/Show-In-Inspector
	public class ShowInInspectorEditor : UnityEditor.Editor
	{
		void OnEnable() => RuntimeHelper.SubOnUpdate(Repaint);
		void OnDisable() => RuntimeHelper.SubOnUpdate(Repaint);

		void DrawGUI()
		{
			this.DrawDefaultInspector();
			this.DrawShowInInspector();
		}

		public override void OnInspectorGUI() => DrawGUI();
	}
}