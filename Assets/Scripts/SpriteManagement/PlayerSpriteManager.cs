using UnityEngine;
using UnityEngine.UI;

public class PlayerSpriteManager : IconManager
{

    public GameObject playerShown;
    public int chosenSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void setSprite( int numSprite)
    {
        chosenSprite = numSprite;
        playerShown.GetComponent<SpriteRenderer>().sprite = Get(chosenSprite);

    }

    void Start()
    {
        GameManager.Instance.playerSpriteManager = this;
    }


}
