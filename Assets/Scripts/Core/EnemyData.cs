using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

// Enemy Configs
[System.Serializable]
public class EnemyDefinition
{
    public string name;
    public int sprite;
    public int hp;
    public int speed;
    public int damage;
}

// Spawn Configs
[System.Serializable]
public class Spawn
{
    public string enemy;
    public string count;
    public string hp;
    public string damage;
    public string delay;
    public List<int> sequence;
    public string location;
}

// Level Configs
[System.Serializable]
public class Level
{
    public string name;
    public int waves;
    public List<Spawn> spawns;
    public int maxWaves;
}




public static class EnemyDataLoader
{
    public static Dictionary<string, EnemyDefinition> enemyDict;
    public static List<Level> levels;
    public static string levelName;

    public static void LoadAll()
    {
        TextAsset enemiesText = Resources.Load<TextAsset>("enemies");
        TextAsset levelsText = Resources.Load<TextAsset>("levels");
        int maxWaves = 10;

        if (enemiesText == null)
        {
            Debug.LogError("Failed to load enemies.json from Resources!");
            return;
        }

        if (levelsText == null)
        {
            Debug.LogError("Failed to load levels.json from Resources!");
            return;
        }

        List<EnemyDefinition> enemies = JsonConvert.DeserializeObject<List<EnemyDefinition>>(enemiesText.text);
        enemyDict = enemies.ToDictionary(e => e.name);

        levels = JsonConvert.DeserializeObject<List<Level>>(levelsText.text);
        //levels.waves = (int)maxWaves;
    }

    // Get enemy name
    public static EnemyDefinition GetEnemy(string name)
    {
        if (enemyDict != null && enemyDict.ContainsKey(name))
            return enemyDict[name];
        else
            Debug.LogWarning($"Enemy '{name}' not found in enemyDict.");
        return null;
    }

    // Get level name
    public static Level GetLevel(string name)
    {
        return levels?.FirstOrDefault(l => l.name == name);
    }
    public static int GetMaxWave(string levelName)
    {
        Level level = GetLevel(levelName);
        if (level != null)
            return level.waves;

        Debug.LogWarning($"Level '{levelName}' not found.");
        return 0;
    }
}

