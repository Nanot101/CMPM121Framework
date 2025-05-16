using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public TextMeshProUGUI summaryText;
    public TextMeshProUGUI descriptionText;
    public Button nextButton;
    public GameEndText GameEndText;

    void Start()
    {
        rewardUI.SetActive(false);
        nextButton.onClick.AddListener(OnNextWavePressed);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
        {
            rewardUI.SetActive(true);
            summaryText.text = GameManager.Instance.GetWaveSummary();
            descriptionText.text = "";
        }
        else if(GameManager.Instance.state == GameManager.GameState.GAMEWIN)
        {
            GameEndText.onWin();
        }
        else
        {
            rewardUI.SetActive(false);
        }
    }

    void OnNextWavePressed()
    {
        GameManager.Instance.ResetWaveStats();
    }
}
