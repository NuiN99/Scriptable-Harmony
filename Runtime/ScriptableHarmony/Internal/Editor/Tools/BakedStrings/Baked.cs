using UnityEngine;
namespace NuiN.NExtensions
{
    public static class Baked
    {
        public static class Tags
        {
            public static readonly string Untagged = "Untagged";
            public static readonly string Respawn = "Respawn";
            public static readonly string Finish = "Finish";
            public static readonly string EditorOnly = "EditorOnly";
            public static readonly string MainCamera = "MainCamera";
            public static readonly string Player = "Player";
            public static readonly string GameController = "GameController";
            public static readonly string ASDASD = "ASDASD";
            
        }

        public static class Layers
        {
            public static readonly int Default = LayerMask.NameToLayer("Default");
            public static readonly int TransparentFX = LayerMask.NameToLayer("TransparentFX");
            public static readonly int IgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
            public static readonly int Water = LayerMask.NameToLayer("Water");
            public static readonly int UI = LayerMask.NameToLayer("UI");
            public static readonly int Bullet = LayerMask.NameToLayer("Bullet");
            public static readonly int Swag = LayerMask.NameToLayer("Swag");
            
        }

        public static class Scenes
        {
            public static readonly string Demo_Simple = "Demo_Simple";
            public static readonly string Test = "Test";
            public static readonly string Test12 = "Test 12";
            
        }
    }
}