using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using static UnityEngine.InputSystem.LowLevel.InputEventTrace;

public class GameEndText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerController PlayController;
    public EnemySpawner EnemySpawner;
    public TextMeshProUGUI temp;

    //public EnemyDataLoader dataLoader;
    void Start()
    {
        //TextMeshProUGUI temp;
        //temp = GetComponent<TextMeshProUGUI>();
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
        string currentLevel = EnemySpawner.GetCurrentLevel();
        int currentWave = (int)EnemySpawner.GetCurrentWave();
        int maxWave = EnemyDataLoader.GetMaxWave(currentLevel);
        if(GameManager.Instance.state == GameManager.GameState.WAVEEND && currentWave == maxWave)
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
