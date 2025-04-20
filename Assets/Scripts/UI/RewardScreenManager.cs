using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public TextMeshProUGUI summaryText;
    public Button nextButton;


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
