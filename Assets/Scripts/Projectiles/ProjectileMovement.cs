using UnityEngine;

public class ProjectileMovement
{
    public float speed;
    protected Vector3 direction;

    public ProjectileMovement(float speed)
    {
        this.speed = speed;
    }

    public virtual void Movement(Transform transform)
    {
        
    }

    public virtual Vector3 GetDirection()
    {
        return direction.normalized;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction.normalized;
    }
}
