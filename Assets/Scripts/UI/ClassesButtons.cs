using System;
using UnityEngine;

public class ClassesButtons : MonoBehaviour
{
    public string className;
    public GameObject buttonsUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //className = string.Empty;
    }

    public void MageClass()
    {
        buttonsUI.SetActive(false);
        this.className = "mage";
        Debug.Log("Player selected mage class.");
    }
    public void WarlockClass()
    {
        buttonsUI.SetActive(false);
        this.className = "warlock";
        Debug.Log("Player selected warlock class.");

    }
    public void BattlemageClass()
    {
        buttonsUI.SetActive(false);
        this.className = "battlemage";
        Debug.Log("Player selected battlemage class.");

    }
    public string GetClassName()
    {
        return this.className;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
