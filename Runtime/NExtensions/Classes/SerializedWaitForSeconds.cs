using UnityEngine;

namespace NuiN.NExtensions
{
    [System.Serializable]
    public class SerializedWaitForSeconds
    {
        [SerializeField] float seconds;
        public float Seconds => seconds;
        public WaitForSeconds Wait { get; private set; }
        
        public SerializedWaitForSeconds(float defaultSeconds)
        {
            seconds = defaultSeconds;
        }
        
        public void Init()
        {
            Wait = new WaitForSeconds(seconds);
        }
        
        public void Set(float seconds)
        {
            this.seconds = seconds;
            Init();
        }
    }
}