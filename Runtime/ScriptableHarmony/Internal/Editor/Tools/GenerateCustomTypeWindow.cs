using UnityEngine;
using UnityEditor;
using System.IO;
using NuiN.ScriptableHarmony.Internal.Helpers;
using NuiN.ScriptableHarmony.ListVariable.Base;
using NuiN.ScriptableHarmony.RuntimeSet.Base;
using NuiN.ScriptableHarmony.RuntimeSet.Components.Base;
using NuiN.ScriptableHarmony.RuntimeSingle.Base;
using NuiN.ScriptableHarmony.RuntimeSingle.Components.Base;
using NuiN.ScriptableHarmony.Variable.Base;
using Object = UnityEngine.Object;

namespace NuiN.ScriptableHarmony.Core.Editor.Tools
{
#if UNITY_EDITOR
    internal class GenerateCustomTypeWindow : EditorWindow
    {
        const string SCRIPT_TEMPLATE =
@"using UnityEngine;
using NuiN.ScriptableHarmony.{SingularSuffix}.Base;

namespace NuiN.ScriptableHarmony.{SingularSuffix}.{CustomOrCommon}
{   
    [CreateAssetMenu(
        menuName = ""ScriptableHarmony/{CustomOrCommon}/{Suffix}/{Type}"",
        fileName = ""{FileName}"")]
    internal class {TypeWithSuffix}SO : {BaseClass}<{Type}> { }
}";
        
        const string COMPONENT_SCRIPT_TEMPLATE = 
@"using UnityEngine;
using NuiN.ScriptableHarmony.{SingularSuffix}.Components.Base;

namespace NuiN.ScriptableHarmony.{SingularSuffix}.Components.{CustomOrCommon}
{   
    public class {TypeWithSuffix}Item : {BaseClass}<{Type}> { }
}";
        
        static SOType _dataType;
        static string _type;
        static string _scriptPreview;
        static bool _lockPreview = true;
        static bool _overwriteExisting;

        static bool _isComponent;

        // remove this functionality when created all commons
        const bool IS_COMMON = true;

        SelectionPathController _pathController;
        
        static GenerateCustomTypeWindow _windowInstance;
        
        [MenuItem("ScriptableHarmony/Generate a Custom Type")]
        static void OpenWindow()
        {
            _windowInstance = GetWindow<GenerateCustomTypeWindow>();
            _windowInstance.titleContent = new GUIContent("Custom Type Generator");
            _windowInstance.Show();
        }

        Vector2 _scrollPosition;
        void OnGUI()
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
                _dataType = (SOType)EditorGUILayout.EnumPopup(_dataType, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                GUILayout.Label("Name", GUILayout.Width(50));
                _type = EditorGUILayout.TextField(_type, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
                
                _pathController.DisplayPathGUI();
                
                _overwriteExisting = EditorGUILayout.Toggle("Overwrite Existing", _overwriteExisting);
            }

            void DisplayScriptPreview()
            {
                EditorGUILayout.LabelField("Preview:");
                using (var scrollView = new EditorGUILayout.ScrollViewScope(_scrollPosition, GUILayout.Height(position.height - 200)))
                {
                    _scrollPosition = scrollView.scrollPosition;
                    if (_lockPreview) EditorGUI.BeginDisabledGroup(true);
                    _scriptPreview = EditorGUILayout.TextArea(_scriptPreview, GUILayout.ExpandHeight(true));
                    if (_lockPreview) EditorGUI.EndDisabledGroup();
                }
                _lockPreview = EditorGUILayout.Toggle("Lock Preview", _lockPreview);
                if (_lockPreview) _scriptPreview = ScriptPreview(SCRIPT_TEMPLATE);
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
                bool emptyType = string.IsNullOrEmpty(_type);
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
            _scriptPreview = ScriptPreview(SCRIPT_TEMPLATE);
            GenerateScriptFile($"{GetTypeWithSuffix()}SO", _scriptPreview);

            if (_dataType is not (SOType.RuntimeSet or SOType.RuntimeSingle)) return;

            _isComponent = true;
            _scriptPreview = ScriptPreview(COMPONENT_SCRIPT_TEMPLATE);
            GenerateScriptFile($"{GetTypeWithSuffix()}Item", _scriptPreview);
            _isComponent = false;
        }

        static string ScriptPreview(string template)
        {
            template = template.Replace("{Type}", _type);
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
            return _dataType switch
            {
                SOType.Variable => $"{_type}{GetSingularSuffix()}",
                _ => $"{_type}{GetSingularSuffix()}"
            };
        }
        
        void OnEnable() => _pathController = new SelectionPathController(this);
        void OnDisable() => _pathController?.Dispose();

        static string GetPluralSuffix() => GetSingularSuffix() + "s";

        static string GetFileName() => $"New {_type} {GetSingularSuffix()}";

        static string GetSingularSuffix()
        {
            return _dataType switch
            {
                SOType.Variable => "Variable",
                SOType.ListVariable => "ListVariable",
                SOType.RuntimeSet => "RuntimeSet",
                SOType.RuntimeSingle => "RuntimeSingle",
                _ => ""
            };
        }

        static string GetBaseClass()
        {
            if (_isComponent)
            {
                return _dataType switch
                {
                    SOType.RuntimeSet => nameof(RuntimeSetItemComponentBase<Object>),
                    SOType.RuntimeSingle => nameof(RuntimeSingleItemComponentBase<Object>),
                    _ => null
                };
            }
            return _dataType switch
            {
                SOType.Variable => nameof(ScriptableVariableBaseSO<object>),
                SOType.ListVariable => nameof(ScriptableListVariableBaseSO<object>),
                SOType.RuntimeSet => nameof(RuntimeSetBaseSO<Object>),
                SOType.RuntimeSingle => nameof(RuntimeSingleBaseSO<Object>),
                _ => ""
            };
        }

        void GenerateScriptFile(string fileName, string fileContents)
        {
            string folderName = _type;
            string folderPath = Path.Combine(_pathController.SelectionPath, folderName);

            if (_dataType is SOType.RuntimeSet or SOType.RuntimeSingle)
            {
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            }
            else
            {
                folderPath = _pathController.SelectionPath;
            }

            string filePath = Path.Combine(folderPath, $"{fileName}.cs");

            if (File.Exists(filePath) && !_overwriteExisting)
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
