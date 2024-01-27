using UnityEngine;

namespace NuiN.NExtensions
{
    [System.Serializable]
    public class SerializedWaitForSeconds : ISerializationCallbackReceiver
    {
        float _prevSeconds;
        
        [SerializeField] float seconds;
        public float Seconds => seconds;
        public WaitForSeconds Wait { get; private set; }

        public SerializedWaitForSeconds(float defaultSeconds)
        {
            seconds = defaultSeconds;
        }
        
        public void Init()
        {
            _prevSeconds = seconds;
            Wait = new WaitForSeconds(seconds);
        }
        
        public void Set(float seconds)
        {
            this.seconds = seconds;
            Init();
        }
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (Application.isPlaying && !Mathf.Approximately(_prevSeconds, seconds)) Init();
        }
        void ISerializationCallbackReceiver.OnAfterDeserialize() { }
    }
}