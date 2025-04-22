using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using TMPro;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using static UnityEngine.InputSystem.LowLevel.InputEventTrace;

public class GameEndText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerController PlayController;
    public EnemySpawner EnemySpawner;
    void Start()
    {
        TextMeshProUGUI temp;
        temp = GetComponent<TextMeshProUGUI>();
        PlayController.GetCurrentHp();
    }



    // Update is called once per frame
    void Update()
    {

        TextMeshProUGUI temp;
        temp = GetComponent<TextMeshProUGUI>();
        if (PlayController.GetCurrentHp() == 0)
        {
            temp.text = "You have run out of hp and lose.";
        }
        int currentWave = (int)EnemySpawner.GetCurrentWave();
        if(GameManager.Instance.state == GameManager.GameState.WAVEEND && currentWave == maxLevel)
        {
            temp.text = "You have beat all the waves and win!";
        }
        //Add a button to reset the game
    }
    public void RestartGame()
    {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }
}
