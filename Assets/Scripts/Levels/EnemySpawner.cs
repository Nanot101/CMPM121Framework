using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public Image level_selector;
    public GameObject button;
    public GameObject enemy;
    public SpawnPoint[] SpawnPoints;
    public GameEndText GameEndText;
    [SerializeField] private Level currentLevel;
    private int currentWave = 1;
    public PlayerController playerController;

    void Start()
    {
        // Load data from JSON
        EnemyDataLoader.LoadAll();

        // Generate a button for each level
        float yOffset = 130f;
        float spacing = -40f;

        for (int i = 0; i < EnemyDataLoader.levels.Count; i++)
        {
            Level level = EnemyDataLoader.levels[i];

            GameObject selector = Instantiate(button, level_selector.transform);
            selector.transform.localPosition = new Vector3(0, yOffset + spacing * i);
            selector.GetComponent<MenuSelectorController>().spawner = this;
            selector.GetComponent<MenuSelectorController>().SetLevel(level.name);
            if (playerController == null)
            {
                playerController = GameObject.FindAnyObjectByType<PlayerController>();
            }
        }
    }

    public void StartLevel(string levelname)
    {
        currentLevel = EnemyDataLoader.levels.Find(l => l.name == levelname);
        currentWave = 1;
        GameManager.Instance.CurrentWave = currentWave;

        level_selector.gameObject.SetActive(false);
        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();
        StartCoroutine(SpawnWave());
        
    }


    public void NextWave()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().updateHP();
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        if (currentLevel == null)
        {
            Debug.LogError("No level selected!");
            yield break;
        }

        GameManager.Instance.state = GameManager.GameState.COUNTDOWN;
        GameManager.Instance.countdown = 3;
        for (int i = 3; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            GameManager.Instance.countdown--;
        }

        GameManager.Instance.state = GameManager.GameState.INWAVE;
        GameManager.Instance.waveStartTime = Time.time;
        EventBus.Instance.DoNewWave();

        var rpn = new RpnEvaluator();

        foreach (Spawn spawn in currentLevel.spawns)
        {
            EnemyDefinition enemyData = EnemyDataLoader.GetEnemy(spawn.enemy);
            if (enemyData == null)
            {
                Debug.LogError($"Enemy type {spawn.enemy} not found");
                continue;
            }

            Dictionary<string, float> vars = new Dictionary<string, float>
            {
                { "wave", (float)currentWave },
                { "base", (float)enemyData.hp }
            };

            int count = Mathf.RoundToInt(rpn.EvaluateRPN(spawn.count, vars));
            int hp = Mathf.RoundToInt(rpn.EvaluateRPN(spawn.hp, vars));
            int damage = enemyData.damage;

            if (!string.IsNullOrEmpty(spawn.damage))
            {
                vars["base"] = (float)enemyData.damage;
                damage = Mathf.RoundToInt(rpn.EvaluateRPN(spawn.damage, vars));
            }


            for (int i = 0; i < count; ++i)
            {
                yield return SpawnEnemy(enemyData, hp, damage);
            }
        }

        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        GameManager.Instance.waveEndTime = Time.time;
        if (currentWave >= currentLevel.waves)
        {
            GameManager.Instance.state = GameManager.GameState.GAMEWIN;
        }
        else
        {
            GameManager.Instance.state = GameManager.GameState.WAVEEND;
            GameManager.Instance.EndWave();
            currentWave++;
            GameManager.Instance.CurrentWave = currentWave;
        }

    }

    IEnumerator SpawnEnemy(EnemyDefinition enemyData, int hp, int damage)
    {
        SpawnPoint spawn_point = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        Vector2 offset = Random.insideUnitCircle * 1.8f;
        Vector3 position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);

        GameObject new_enemy = Instantiate(enemy, position, Quaternion.identity);
        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(enemyData.sprite);

        EnemyController en = new_enemy.GetComponent<EnemyController>();
        en.hp = new Hittable(hp, Hittable.Team.MONSTERS, new_enemy);
        en.speed = enemyData.speed;
        en.damage = damage;
        en.enemyType = enemyData.name.ToLower();
        GameManager.Instance.AddEnemy(new_enemy);

        yield return new WaitForSeconds(1.0f);
    }
    public int GetCurrentWave()
    {
        return currentWave;
    }
    public string GetCurrentLevel()
    {
        return currentLevel.name;
    }
}