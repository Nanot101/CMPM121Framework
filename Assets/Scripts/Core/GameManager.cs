using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager 
{
    public enum GameState
    {
        PREGAME,
        INWAVE,
        WAVEEND,
        COUNTDOWN,
        GAMEOVER,
        GAMEWIN
    }
    public GameState state;

    public int countdown;
    private static GameManager theInstance;
    public static GameManager Instance {  get
        {
            if (theInstance == null)
                theInstance = new GameManager();
            return theInstance;
        }
    }

    public GameObject player;
    
    public ProjectileManager projectileManager;
    public SpellIconManager spellIconManager;
    public EnemySpriteManager enemySpriteManager;
    public PlayerSpriteManager playerSpriteManager;
    public RelicIconManager relicIconManager;

    private List<GameObject> enemies;
    public int enemy_count { get { return enemies.Count; } }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public GameObject GetClosestEnemy(Vector3 point)
    {
        if (enemies == null || enemies.Count == 0) return null;
        if (enemies.Count == 1) return enemies[0];
        return enemies.Aggregate((a,b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    }


    // ==========================
    // Wave Summary Tracking
    // ==========================

    public int damageTaken;
    public int zombiesKilled;
    public int skeletonsKilled;
    public int warlocksKilled;
    public int roguesKilled;
    public float waveStartTime;
    public float waveEndTime;

    public void ResetWaveStats()
    {
        damageTaken = 0;
        zombiesKilled = 0;
        skeletonsKilled = 0;
        warlocksKilled = 0;
        roguesKilled = 0;
        waveStartTime = Time.time;
    }

    public void EndWave()
    {
        waveEndTime = Time.time;
    }

    public float WaveDuration => waveEndTime - waveStartTime;

    public void ShowSummary()
    {
        float duration = waveEndTime - waveStartTime;
        Debug.Log("----- Wave Summary -----");
        Debug.Log("Damage Taken: " + damageTaken);
        Debug.Log("Zombies Killed: " + zombiesKilled);
        Debug.Log("Skeletons Killed: " + skeletonsKilled);
        Debug.Log("Warlocks Killed: " + warlocksKilled);
        Debug.Log("Rogues killed: " + roguesKilled);
        Debug.Log("Time to Beat the Wave: " + duration.ToString("F2") + "s");
        Debug.Log("------------------------");
        //ResetWaveStats();
    }

    public string GetWaveSummary()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        float duration = waveEndTime - waveStartTime;
        sb.AppendLine($"<b>Wave Summary</b>");
        sb.AppendLine($"Time to complete wave: {duration:F2} seconds");
        sb.AppendLine($"Damage taken: {damageTaken}");
        sb.AppendLine($"Zombies killed: {zombiesKilled}");
        sb.AppendLine($"Skeletons killed: {skeletonsKilled}");
        sb.AppendLine($"Warlocks killed: {warlocksKilled}");
        sb.AppendLine($"Rogues killed:  + {roguesKilled}");
        return sb.ToString();
    }

    private GameManager()
    {
        enemies = new List<GameObject>();
    }
}