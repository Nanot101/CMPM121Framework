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
        temp.enabled = false;
        restartButton.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        temp.enabled = false;
        restartButton.SetActive(false);
        artPanel.SetActive(false);
        GameManager.Instance.Reset();
        GameManager.Instance.Initialize();
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
        artPanel.SetActive(true);
    }
}
