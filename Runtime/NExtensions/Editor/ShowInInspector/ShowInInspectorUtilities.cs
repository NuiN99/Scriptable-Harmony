#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NuiN.NExtensions
{
    // modified from https://github.com/BrainswitchMachina/Show-In-Inspector
    public static class ShowInInspectorUtilities
    {
        static MethodInfo gradientField = typeof(EditorGUILayout).GetMethod("GradientField", BindingFlags.Static | BindingFlags.NonPublic, null, new[] { typeof(string), typeof(Gradient), typeof(GUILayoutOption[]) }, null);
        public static Gradient GradientField(string label, Gradient gradient)
        {
            return (Gradient)gradientField.Invoke(null, new object[] { label, gradient, null });
        }

        static Dictionary<UnityEditor.Editor, EditorData> editors = new();

        class EditorData
        {
            public BaseData[] Properties { get; }

            public BaseData[] Fields { get; }

            public EditorData(UnityEditor.Editor editor)
            {
                Properties = GetProperties(editor.target);
                Fields = GetFields(editor.target);
            }
        }

        public static void DrawShowInInspector(this UnityEditor.Editor editor)
        {
            EditorData editorData;
            if (!editors.ContainsKey(editor))
            {
                editorData = new EditorData(editor);
                editors.Add(editor, editorData);
            }
            editorData = editors[editor];

            // Fields first. Todo: Other colour?
            Draw(editorData.Fields);

            // Then properties
            if (editorData.Properties.Length > 0)
            {
                Draw(editorData.Properties);
            }
        }

        static void Draw(BaseData[] showInInspectorDatas)
        {
            GUILayoutOption[] emptyOptions = Array.Empty<GUILayoutOption>();

            EditorGUILayout.BeginVertical(emptyOptions);

            foreach (BaseData field in showInInspectorDatas)
            {
                EditorGUILayout.BeginHorizontal(emptyOptions);

                switch (field.InspectorType)
                {
                    case SerializedPropertyType.Integer:
                        field.SetValue(EditorGUILayout.IntField(field.Name, (int)field.GetValue()));
                        break;

                    case SerializedPropertyType.Float:
                        field.SetValue(EditorGUILayout.FloatField(field.Name, (float)field.GetValue()));
                        break;

                    case SerializedPropertyType.Boolean:
                        field.SetValue(EditorGUILayout.Toggle(field.Name, (bool)field.GetValue()));
                        break;

                    case SerializedPropertyType.String:
                        field.SetValue(EditorGUILayout.TextField(field.Name, (String)field.GetValue()));
                        break;

                    case SerializedPropertyType.Color:
                        field.SetValue(EditorGUILayout.ColorField(field.Name, (Color)field.GetValue()));
                        break;

                    case SerializedPropertyType.ObjectReference:
                        var objRef = field.GetValue();
                        field.SetValue(EditorGUILayout.ObjectField(field.Name, (Object)objRef, objRef.GetType(), true));
                        break;
                    
                    case SerializedPropertyType.ExposedReference:
                        var exposedRef = field.GetValue();
                        field.SetValue(EditorGUILayout.ObjectField(field.Name, (Object)exposedRef, exposedRef.GetType(), true));
                        break;

                    case SerializedPropertyType.LayerMask:
                        field.SetValue(EditorGUILayout.LayerField(field.Name, (LayerMask)field.GetValue()));
                        break;

                    case SerializedPropertyType.Enum:
                        field.SetValue(EditorGUILayout.EnumPopup(field.Name, (Enum)field.GetValue()));
                        break;

                    case SerializedPropertyType.Vector2:
                        field.SetValue(EditorGUILayout.Vector2Field(field.Name, (Vector2)field.GetValue()));
                        break;

                    case SerializedPropertyType.Vector3:
                        field.SetValue(EditorGUILayout.Vector3Field(field.Name, (Vector3)field.GetValue()));
                        break;

                    case SerializedPropertyType.AnimationCurve:
                        field.SetValue(EditorGUILayout.CurveField(field.Name, (AnimationCurve)field.GetValue()));
                        break;

                    case SerializedPropertyType.Bounds:
                        field.SetValue(EditorGUILayout.BoundsField(field.Name, (Bounds)field.GetValue()));
                        break;

                    // Haven't been able to get Gradients to work since the Editor for Gradients rely on SerializedProperty, todo: create my own gradient editor...
                    //case SerializedPropertyType.Gradient:
                    //    field.SetValue(GradientField(field.name, (Gradient)field.GetValue()));
                    //    break;

#if hasSerializedPropertyTypeQuaternion
                case SerializedPropertyType.Quaternion:
                    Vector3 eulerAngles = ((Quaternion)field.GetValue()).eulerAngles;
                    field.SetValue(Quaternion.Euler(EditorGUILayout.Vector3Field(field.name, eulerAngles)));
                    break;
#endif

                    case SerializedPropertyType.Rect:
                        field.SetValue(EditorGUILayout.RectField(field.Name, (Rect)field.GetValue()));
                        break;

                    case SerializedPropertyType.Generic:
                        object genericRef = field.GetValue();
                        if (genericRef == null) break;
                        field.SetValue(EditorGUILayout.ObjectField(field.Name, (Object)genericRef, genericRef.GetType(), true));
                        break;

                    default:
                        Debug.Log(field.InspectorType);
                        Debug.LogError("ShowInInspector: Failed drawing " + field.Name + ", " + field.Instance);
                        break;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

        }

        public static BaseData[] GetProperties(object obj)
        {
            List<PropertyData> properties = new List<PropertyData>();
            Type type = obj.GetType();

            while (type != null)
            {
                PropertyInfo[] infos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                
                foreach (PropertyInfo info in infos)
                {
                    Attribute[] attributes = Attribute.GetCustomAttributes(info, typeof(ShowInInspectorAttribute));
                    if (attributes.Length > 0)
                    {
                        PropertyData data = new PropertyData(obj, info);
                        properties.Add(data);
                    }
                }

                if (type.IsGenericType)
                {
                    foreach (var genericArgument in type.GetGenericArguments())
                    {
                        PropertyInfo[] genericProperties = genericArgument.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                        foreach (PropertyInfo genericProperty in genericProperties)
                        {
                            Attribute[] genericAttributes = Attribute.GetCustomAttributes(genericProperty, typeof(ShowInInspectorAttribute));
                            if (genericAttributes.Length > 0)
                            {
                                PropertyData data = new PropertyData(obj, genericProperty);
                                properties.Add(data);
                            }
                        }
                    }
                }

                type = type.BaseType;
            }

            return properties.ToArray();
        }

        public static BaseData[] GetFields(object obj)
        {
            List<FieldData> fields = new List<FieldData>();
            Type type = obj.GetType();

            while (type != null)
            {
                FieldInfo[] infos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                
                foreach (FieldInfo info in infos)
                {
                    Attribute[] attributes = Attribute.GetCustomAttributes(info, typeof(ShowInInspectorAttribute));
                    if (attributes.Length > 0)
                    {
                        FieldData data = new FieldData(obj, info);
                        fields.Add(data);
                    }
                }

                if (type.IsGenericType)
                {
                    foreach (var genericArgument in type.GetGenericArguments())
                    {
                        FieldInfo[] genericFields = genericArgument.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                        foreach (FieldInfo genericField in genericFields)
                        {
                            Attribute[] genericAttributes = Attribute.GetCustomAttributes(genericField, typeof(ShowInInspectorAttribute));
                            if (genericAttributes.Length > 0)
                            {
                                FieldData data = new FieldData(obj, genericField);
                                fields.Add(data);
                            }
                        }
                    }
                }

                type = type.BaseType;
            }

            return fields.ToArray();
        }

        public abstract class BaseData
        {
            public System.Object Instance { get; protected set; }
            public SerializedPropertyType InspectorType { get; protected set; }
            protected readonly MemberInfo memberInfo;

            public string Name => ObjectNames.NicifyVariableName(memberInfo.Name);

            public abstract bool HasSetter { get; }

            protected BaseData(object instance, MemberInfo info)
            {

                this.Instance = instance;
                memberInfo = info;
            }

            public abstract System.Object GetValue();

            public abstract void SetValue(System.Object value);

        }

        public class PropertyData : BaseData
        {
            PropertyInfo Info => memberInfo as PropertyInfo;

            MethodInfo _getMethodInfo;
            MethodInfo _setMethodInfo;

            public override bool HasSetter => _setMethodInfo != null;

            public PropertyData(System.Object instance, PropertyInfo info)
                : base(instance, info)
            {
                InspectorType = GetInspectorType(info.PropertyType);

                _getMethodInfo = Info.GetGetMethod();
                _setMethodInfo = Info.GetSetMethod();
            }

            public override System.Object GetValue()
            {
                return _getMethodInfo.Invoke(Instance, null);
            }

            public override void SetValue([NotNull] object value)
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if ((_setMethodInfo == null))
                    return;
                _setMethodInfo.Invoke(Instance, new[] { value });
            }

        }

        public class FieldData : BaseData
        {
            FieldInfo Info => memberInfo as FieldInfo;


            public override bool HasSetter
            {
                get
                {
                    if (Application.isPlaying)
                        return true;
                    return false;
                }
            }

            public FieldData(System.Object instance, FieldInfo info)
                : base(instance, info)
            {
                InspectorType = GetInspectorType(info.FieldType);
            }

            public override System.Object GetValue()
            {
                return Info.GetValue(Instance);
            }

            public override void SetValue(System.Object value)
            {
                Info.SetValue(Instance, value);
            }

        }

        static SerializedPropertyType GetInspectorType(Type type)
        {
            if (type == typeof(int))
            {
                return SerializedPropertyType.Integer;
            }

            if (type == typeof(bool))
            {
                return SerializedPropertyType.Boolean;
            }

            if (type == typeof(float))
            {
                return SerializedPropertyType.Float;
            }

            if (type == typeof(string))
            {
                return SerializedPropertyType.String;
            }

            if (type == typeof(Color))
            {
                return SerializedPropertyType.Color;
            }

            if (type == typeof(LayerMask))
            {
                return SerializedPropertyType.LayerMask;
            }

            if (type.IsEnum)
            {
                return SerializedPropertyType.Enum;
            }

            if (type == typeof(Vector2))
            {
                return SerializedPropertyType.Vector2;
            }

            if (type == typeof(Vector3))
            {
                return SerializedPropertyType.Vector3;
            }

            if (type == typeof(Rect))
            {
                return SerializedPropertyType.Rect;
            }

            if (type == typeof(AnimationCurve))
            {
                return SerializedPropertyType.AnimationCurve;
            }

            if (type == typeof(Bounds))
            {
                return SerializedPropertyType.Bounds;
            }

            if (type == typeof(Gradient))
            {
                return SerializedPropertyType.Gradient;
            }

#if hasSerializedPropertyTypeQuaternion
        if (type == typeof(Quaternion))
        {
            return SerializedPropertyType.Quaternion;
        }
#endif

            if (type == typeof(Material))
            {
                return SerializedPropertyType.ObjectReference;
            }
            return type == typeof(Object) ? SerializedPropertyType.ObjectReference : SerializedPropertyType.Generic;
        }
    }
}
#endif