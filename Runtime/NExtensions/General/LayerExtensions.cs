using UnityEngine;

namespace NuiN.NExtensions
{
    public static class LayerExtensions
    {
        public static bool ContainsLayer(this LayerMask layerMask, int layer)
            => layerMask == (layerMask | (1 << layer));
        public static bool ContainsLayer(this LayerMask layerMask, GameObject layerObject)
            => layerMask.ContainsLayer(layerObject.layer);
        
        public static bool ContainsLayer(this LayerMask layerMask, Component layerObject)
            => layerMask.ContainsLayer(layerObject.gameObject.layer);
        
        public static bool ContainsLayer(this LayerMask layerMask, Collision layerObject)
            => layerMask.ContainsLayer(layerObject.gameObject.layer);
    }

}