using UnityEngine;
using UnityEditor;
using System.IO;
using NuiN.ScriptableHarmony.Core;
using Object = UnityEngine.Object;

namespace NuiN.ScriptableHarmony.Editor
{
#if UNITY_EDITOR
    internal class GenerateCustomTypeGUI
    {
        const string SCRIPT_TEMPLATE =
@"using UnityEngine;
using NuiN.ScriptableHarmony.Core;

namespace NuiN.ScriptableHarmony
{   
    [CreateAssetMenu(
        menuName = ""ScriptableHarmony/{CustomOrCommon}/{Suffix}/{Type}"",
        fileName = ""{FileName}"")]
    internal class {TypeWithSuffix}SO : {BaseClass}<{Type}> { }
}";
        
        const string COMPONENT_SCRIPT_TEMPLATE = 
@"using UnityEngine;
using NuiN.ScriptableHarmony.Core;

namespace NuiN.ScriptableHarmony
{   
    public class {TypeWithSuffix}Item : {BaseClass}<{Type}> { }
}";
        
        static SOType dataType;
        static string type;
        static string scriptPreview;
        static bool lockPreview = true;
        static bool overwriteExisting;

        static bool isComponent;

        // remove this functionality when created all commons
        const bool IS_COMMON = true;

        SelectionPathController _pathController;
        
        Vector2 _scrollPosition;

        public GenerateCustomTypeGUI(SelectionPathController pathController)
        {
            _pathController = pathController;
        }
        
        public void DrawGUI(EditorWindow window)
        {
            GUILayout.Space(10);
            
            DisplayOptions();
            
            GUILayout.Space(10);

            DisplayScriptPreview();

            DisplayGenerateButton();

            return;

            void DisplayOptions()
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Type:", GUILayout.Width(50));
                dataType = (SOType)EditorGUILayout.EnumPopup(dataType, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                GUILayout.Label("Name", GUILayout.Width(50));
                type = EditorGUILayout.TextField(type, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
                
                _pathController.DisplayPathGUI();
                
                overwriteExisting = EditorGUILayout.Toggle("Overwrite Existing", overwriteExisting);
            }

            void DisplayScriptPreview()
            {
                EditorGUILayout.LabelField("Preview:");
                using (var scrollView = new EditorGUILayout.ScrollViewScope(_scrollPosition, GUILayout.Height(window.position.height - 225)))
                {
                    _scrollPosition = scrollView.scrollPosition;
                    if (lockPreview) EditorGUI.BeginDisabledGroup(true);
                    scriptPreview = EditorGUILayout.TextArea(scriptPreview, GUILayout.ExpandHeight(true));
                    if (lockPreview) EditorGUI.EndDisabledGroup();
                }
                lockPreview = EditorGUILayout.Toggle("Lock Preview", lockPreview);
                if (lockPreview) scriptPreview = ScriptPreview(SCRIPT_TEMPLATE);
            }

            void DisplayGenerateButton()
            {
                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    normal = { textColor = new Color(1, 0.3f, 0f, 1f) },
                    fontSize = 17,
                    fontStyle = FontStyle.Bold,
                    fixedHeight = 55 
                };
                Color ogColor = GUI.backgroundColor;
                GUI.backgroundColor = new Color(0.6f, 0.9f, 1f, 1f);
                if (GUILayout.Button("Generate Script", buttonStyle, GUILayout.ExpandWidth(true)))
                {
                    TryGenerateScript();
                }
                GUI.backgroundColor = ogColor;
            }

            void TryGenerateScript()
            {
                bool emptyType = string.IsNullOrEmpty(type);
                if (!_pathController.EmptyPath && !emptyType)
                {
                    GenerateScript();
                }
                else
                {
                    string warningMessage = "Empty {string} when attempting to create a new Custom Type Script";
                    warningMessage = _pathController.EmptyPath switch
                    {
                        true when emptyType => warningMessage.Replace("{string}", "Path and Type Name"),
                        true => warningMessage.Replace("{string}", "Path"),
                        _ => warningMessage.Replace("{string}", "Type Name")
                    };

                    Debug.LogWarning(warningMessage);
                }
            }
        }

        public void GenerateScript()
        {
            scriptPreview = ScriptPreview(SCRIPT_TEMPLATE);
            GenerateScriptFile($"{GetTypeWithSuffix()}SO", scriptPreview);

            if (dataType is not (SOType.RuntimeSet or SOType.RuntimeSingle)) return;

            isComponent = true;
            scriptPreview = ScriptPreview(COMPONENT_SCRIPT_TEMPLATE);
            GenerateScriptFile($"{GetTypeWithSuffix()}Item", scriptPreview);
            isComponent = false;
        }

        static string ScriptPreview(string template)
        {
            template = template.Replace("{Type}", type);
            template = template.Replace("{TypeWithSuffix}", GetTypeWithSuffix());
            template = template.Replace("{BaseClass}", GetBaseClass());
            template = template.Replace("{Suffix}", GetPluralSuffix());
            template = template.Replace("{SingularSuffix}", GetSingularSuffix());
            template = template.Replace("{FileName}", GetFileName());
            template = template.Replace("{CustomOrCommon}", IS_COMMON ? "Common" : "Custom");
        
            return template;
        }

        static string GetTypeWithSuffix()
        {
            return $"{type}{GetSingularSuffix()}";
        }
        
        static string GetPluralSuffix() => GetSingularSuffix() + "s";

        static string GetFileName() => $"New {type} {GetSingularSuffix()}";

        static string GetSingularSuffix()
        {
            return dataType switch
            {
                SOType.Variable => "Variable",
                SOType.List => "List",
                SOType.Dictionary => "Dictionary",
                SOType.RuntimeSet => "RuntimeSet",
                SOType.RuntimeSingle => "RuntimeSingle",
                _ => ""
            };
        }

        static string GetBaseClass()
        {
            if (isComponent)
            {
                return dataType switch
                {
                    SOType.RuntimeSet => nameof(RuntimeSetComponent<Object>),
                    SOType.RuntimeSingle => nameof(RuntimeSingleComponent<Object>),
                    _ => null
                };
            }
            return dataType switch
            {
                SOType.Variable => nameof(ScriptableVariableSO<object>),
                SOType.List => nameof(ScriptableListSO<object>),
                SOType.RuntimeSet => nameof(RuntimeSetSO<Object>),
                SOType.RuntimeSingle => nameof(RuntimeSingleSO<Object>),
                SOType.Dictionary => nameof(ScriptableDictionarySO<object, object>),
                _ => ""
            };
        }

        void GenerateScriptFile(string fileName, string fileContents)
        {
            string folderName = type;
            string folderPath = Path.Combine(_pathController.SelectionPath, folderName);

            if (dataType is SOType.RuntimeSet or SOType.RuntimeSingle)
            {
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            }
            else
            {
                folderPath = _pathController.SelectionPath;
            }

            string filePath = Path.Combine(folderPath, $"{fileName}.cs");

            if (File.Exists(filePath) && !overwriteExisting)
            {
                Debug.Log("Script already exists and 'Overwrite Existing' is disabled");
                return;
            }

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(fileContents);
            }

            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }
    }
#endif
}
