using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using static UnityEngine.InputSystem.LowLevel.InputEventTrace;

public class GameEndText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerController PlayerController;
    public EnemySpawner EnemySpawner;
    public TextMeshProUGUI temp;
    public GameObject restartButton;
    public GameObject artPanel;

    //public EnemyDataLoader dataLoader;
    void Start()
    {
       // TextMeshProUGUI temp;
        //temp = GetComponent<TextMeshProUGUI>();
        temp.enabled = false;
        restartButton.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        string currentLevel = EnemySpawner.GetCurrentLevel();
        int currentWave = (int)EnemySpawner.GetCurrentWave();
        int maxWave = EnemyDataLoader.GetMaxWave(currentLevel);
        //Add a button to reset the game
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
        restartButton.SetActive(false);
        temp.enabled = false;
        Time.timeScale = 1;
    }
    public void onDie()
    {
        temp.text = "You have run out of hp and lose.";
        restartButton.SetActive(true);
        temp.enabled = true;
        Time.timeScale = 0;
        artPanel.SetActive(true);
    }
    public void onWin()
    {
        temp.text = "You have beat all the waves and win!";
        restartButton.SetActive(true);
        temp.enabled = true;
        Time.timeScale = 0;
    }
}
