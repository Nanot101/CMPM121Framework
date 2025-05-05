using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using static UnityEngine.InputSystem.LowLevel.InputEventTrace;

public class GameEndText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    public void RestartGame()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
        // restartButton.SetActive(false);
        // temp.enabled = false;
        // Time.timeScale = 1;
        temp.enabled = false;
        restartButton.SetActive(false);
        Time.timeScale = 1; // Resume time before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads scene completely
    }
    public void onDie()
    {
        temp.text = "You lost.";
        restartButton.SetActive(true);
        temp.enabled = true;
        Time.timeScale = 0;
        artPanel.SetActive(true);
    }
    public void onWin()
    {
        temp.text = "You won!";
        restartButton.SetActive(true);
        temp.enabled = true;
        Time.timeScale = 0;
    }
}
