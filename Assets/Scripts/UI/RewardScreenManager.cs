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
    public SpellUI spellUI;
    public SpellBuilder spellBuilder;
    public Spell builtSpell;
    public PlayerController playerController;
    public TextMeshProUGUI spellDescriptionText;
    public Button replaceButton;

    void Start()
    {
        rewardUI.SetActive(false);
        nextButton.onClick.AddListener(OnNextWavePressed);
        nextButton.onClick.AddListener(OnReplaceSpell);
        GameManager.Instance.OnWaveEnd += ShowRewardScreen;
        GameManager.Instance.OnGameWin += ShowWinScreen;

        if (playerController == null)
        {
            playerController = FindAnyObjectByType<PlayerController>();
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        GameManager.Instance.OnWaveEnd -= ShowRewardScreen;
        GameManager.Instance.OnGameWin -= ShowWinScreen;
    }

    void ShowRewardScreen()
    {
        rewardUI.SetActive(true);
        summaryText.text = GameManager.Instance.GetWaveSummary();
        descriptionText.text = "Select your reward before continuing.";
        builtSpell = spellBuilder.Build(playerController.spellcaster);
        spellUI.SetSpell(builtSpell);
        Time.timeScale = 0f;
        spellDescriptionText.text = spellBuilder.GetFullDescription();
    }

    void ShowWinScreen()
    {
        rewardUI.SetActive(false);
        GameEndText.onWin();
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
    //     {
    //         rewardUI.SetActive(true);
    //         summaryText.text = GameManager.Instance.GetWaveSummary();
    //         descriptionText.text = "";
    //     }
    //     else if(GameManager.Instance.state == GameManager.GameState.GAMEWIN)
    //     {
    //         GameEndText.onWin();
    //     }
    //     else
    //     {
    //         rewardUI.SetActive(false);
    //     }
    // }

    void OnNextWavePressed()
    {
        GameManager.Instance.ResetWaveStats();
        rewardUI.SetActive(false);
        Time.timeScale = 1f;
    }


    public void OnReplaceSpell()
    {
        playerController.spellcaster.setSpell(builtSpell);
    }
}
