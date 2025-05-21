using UnityEngine;

[System.Serializable]
public class RelicDef
{
    public string name;
    public int sprite;

    public Trigger trigger;
    public Effect effect;

    [System.Serializable]
    public class Trigger
    {
        public string description;
        public string type;
        public string amount;
    }

    [System.Serializable]
    public class Effect
    {
        public string description;
        public string type;
        public string amount;
        public string until;
    }
}
