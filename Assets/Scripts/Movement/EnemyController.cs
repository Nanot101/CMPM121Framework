using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Transform target;
    public int speed;
    public Hittable hp;
    public int damage;
    public HealthBar healthui;
    public bool dead;

    public float last_attack;
    public string enemyType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameManager.Instance.player.transform;
        hp.OnDeath += Die;
        healthui.SetHealth(hp);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        if (direction.magnitude < 2f)
        {
            DoAttack();
        }
        else
        {
            GetComponent<Unit>().movement = direction.normalized * speed;
        }
    }
    
    void DoAttack()
    {
        if (last_attack + 2 < Time.time)
        {
            last_attack = Time.time;
            target.gameObject.GetComponent<PlayerController>().hp.Damage(new Damage(damage, Damage.Type.PHYSICAL));
            GameManager.Instance.damageTaken += damage;
        }
    }


    void Die()
    {
        if (!dead)
        {
            dead = true;
            // Track enemy kills by type
            switch (enemyType.ToLower())
            {
                case "zombie":
                    GameManager.Instance.zombiesKilled++;
                    break;
                case "skeleton":
                    GameManager.Instance.skeletonsKilled++;
                    break;
                case "warlock":
                    GameManager.Instance.warlocksKilled++;
                    break;
                case "rogue":
                    GameManager.Instance.roguesKilled++;
                    break;
            }

            GameManager.Instance.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }
    }
}
