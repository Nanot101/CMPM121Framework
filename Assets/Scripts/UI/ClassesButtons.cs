using UnityEngine;

public class ClassesButtons : MonoBehaviour
{
    public string className;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        className = string.Empty;
    }

    public void MageClass()
    {
        className = "mage";
    }
    public void WarlockClass()
    {
        className = "warlock";
    }
    public void BattlemageClass()
    {
        className = "battlemage";
    }
    public string GetClassName()
    {
        return className;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
