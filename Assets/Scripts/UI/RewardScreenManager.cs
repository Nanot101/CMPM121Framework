using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    // public TextMeshProUGUI summaryText;
    public TextMeshProUGUI descriptionText;
    public Button nextButton;
    public GameEndText GameEndText;
    public SpellUI spellUI;
    public SpellBuilder spellBuilder;
    public Spell builtSpell;
    public PlayerController playerController;
    public TextMeshProUGUI spellDescriptionText;
    public Button replaceButton;
    public RelicUI relicUI;

    void Start()
    {
        rewardUI.SetActive(false);
        nextButton.onClick.AddListener(OnNextWavePressed);
        replaceButton.onClick.AddListener(OnReplaceSpell);
        GameManager.Instance.OnWaveEnd += ShowRewardScreen;
        GameManager.Instance.OnGameWin += ShowWinScreen;

        if (playerController == null)
        {
            playerController = FindAnyObjectByType<PlayerController>();
        }
    }

    void OnDestroy()
    {
        GameManager.Instance.OnWaveEnd -= ShowRewardScreen;
        GameManager.Instance.OnGameWin -= ShowWinScreen;
    }

    void ShowRewardScreen()
    {
        rewardUI.SetActive(true);
        // summaryText.text = GameManager.Instance.GetWaveSummary();
        descriptionText.text = "Drop one of your current spells to replace it with this one.";
        builtSpell = spellBuilder.Build(playerController.spellcaster);
        spellUI.SetSpell(builtSpell);
        spellDescriptionText.text = spellBuilder.description;
        if (GameManager.Instance.CurrentWave % 3 == 0)
        {
            relicUI.ShowRelicChoices();
        }
        Time.timeScale = 0f;

    }

    void ShowWinScreen()
    {
        rewardUI.SetActive(false);
        GameEndText.onWin();
    }

    void OnNextWavePressed()
    {
        GameManager.Instance.ResetWaveStats();
        rewardUI.SetActive(false);
        Time.timeScale = 1f;
    }


    public void OnReplaceSpell()
    {
        playerController.spellcaster.setSpell(builtSpell);
        for (int i = 0; i < 4; i++)
        {
            // Debug.Log("rewardScreen: in for loop");
            if (playerController.spellcaster.getSpellAtIndex(i) != null)
            {
                Debug.Log("rewardScreen: in if statement");
                // Debug.Log("rewardScreen: in for loop");
                if (playerController.spellcaster.getSpellAtIndex(i) != null)
                {
                    // Debug.Log("rewardScreen: in if statement");
                    playerController.spellUIs[i].SetSpell(playerController.spellcaster.getSpellAtIndex(i));
                }
            }
        }
    }
}
