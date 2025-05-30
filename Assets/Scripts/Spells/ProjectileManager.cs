using UnityEngine;
using System;

public class ProjectileManager : MonoBehaviour
{
    public GameObject[] projectiles;

    void Start()
    {
        GameManager.Instance.projectileManager = this;
    }

    // public void CreateProjectile(int which, string trajectory, Vector3 where, Vector3 direction, float speed, Action<Hittable,Vector3> onHit)
    // {
    //     GameObject new_projectile = Instantiate(projectiles[which], where + direction.normalized*1.1f, Quaternion.Euler(0,0,Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg));
    //     new_projectile.GetComponent<ProjectileController>().movement = MakeMovement(trajectory, speed);
    //     new_projectile.GetComponent<ProjectileController>().OnHit += onHit;
    // }

    public void CreateProjectile(int which, string trajectory, Vector3 where, Vector3 direction, float speed, Action<Hittable, Vector3> onHit, float lifetime)
    {
        GameObject new_projectile = Instantiate(projectiles[which], where + direction.normalized * 1.1f, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        new_projectile.GetComponent<ProjectileController>().movement = MakeMovement(trajectory, speed);
        new_projectile.GetComponent<ProjectileController>().OnHit += onHit;
        new_projectile.GetComponent<ProjectileController>().SetLifetime(lifetime);
    }

    public void CreateProjectile(int which, string trajectory, Vector3 where, Vector3 direction, float speed, Action<Hittable, Vector3> onHit, float lifetime, float size)
    {
        GameObject new_projectile = Instantiate(projectiles[which], where + direction.normalized * 1.1f, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        new_projectile.GetComponent<ProjectileController>().movement = MakeMovement(trajectory, speed);
        new_projectile.GetComponent<ProjectileController>().OnHit += onHit;
        new_projectile.GetComponent<ProjectileController>().SetLifetime(lifetime);
        new_projectile.transform.localScale = new Vector3(size, size, size);
    }

    public GameObject CreateProjectileWithReference(int which, string trajectory, Vector3 where, Vector3 direction, float speed, Action<Hittable, Vector3> onHit, float lifetime)
    {
        GameObject new_projectile = Instantiate(projectiles[which], where + direction.normalized * 1.1f, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        new_projectile.GetComponent<ProjectileController>().movement = MakeMovement(trajectory, speed);
        new_projectile.GetComponent<ProjectileController>().movement.SetDirection(direction);
        new_projectile.GetComponent<ProjectileController>().OnHit += onHit;
        new_projectile.GetComponent<ProjectileController>().SetLifetime(lifetime);

        return new_projectile;
    }

    public ProjectileMovement MakeMovement(string name, float speed)
    {
        if (name == "straight")
        {
            return new StraightProjectileMovement(speed);
        }
        if (name == "homing")
        {
            return new HomingProjectileMovement(speed);
        }
        if (name == "spiraling")
        {
            return new SpiralingProjectileMovement(speed);
        }
        return null;
    }

}
