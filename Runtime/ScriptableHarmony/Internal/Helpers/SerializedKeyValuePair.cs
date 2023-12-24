using System;

[Serializable]
internal class SerializedKeyValuePair<TKey,TValue>
{
    public TKey key;
    public TValue value;

    public SerializedKeyValuePair(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }
}
