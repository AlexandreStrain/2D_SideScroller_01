using UnityEngine;

public static class AttackPatterns
{
    public static void KeepShooting(EnemyController ec)
    {
        //Instantiate and store a clone of the Bullet Prefab from "_stats" 
        GameObject bulletPrefab = GameObject.Instantiate(ec._stats._bullet);
        //get the Projectile script on the bulletPrefab
        Projectile bulletScript = bulletPrefab.GetComponent<Projectile>();

        //fire the bullet some offset away from the center of the sprite
        Vector3 offset = new Vector3(ec._stats._shotOffset.x * ec._direction, ec._stats._shotOffset.y, 0f);

        //launch projectile in the proper "_direction"
        bulletScript.Init(ec.transform.position + offset, Vector2.right * ec._direction, ec.gameObject);
    }

    public static void TargetEntity(EnemyController ec)
    {
        //if EnemyController doesn't have a target, exit early
        if (!ec._target)
        {
            return;
        }

        /* Find the displacement vector between enemy and its target,
        normalize the vector to values between 0 and 1 */
        Vector2 directionToTarget = (ec._target.position - ec.transform.position).normalized;

        /* This line is a simplification of an if statement using 
        a conditional operator -- (condition) ? true side : false side  -- */
        ec._movement.x = (ec._target.position.x >= ec.transform.position.x) ? 1f : -1f;


        //Instantiate and store a clone of the Bullet Prefab from "_stats" 
        GameObject bulletPrefab = GameObject.Instantiate(ec._stats._bullet);
        //get the Projectile script on the bulletPrefab
        Projectile bulletScript = bulletPrefab.GetComponent<Projectile>();

        //fire the bullet some offset away from the center of the sprite
        Vector3 offset = new Vector3(ec._stats._shotOffset.x * ec._movement.x, ec._stats._shotOffset.y, 0f);

        //launch projectile in the proper "_direction"
        bulletScript.Init(ec.transform.position + offset, directionToTarget, ec.gameObject);
    }
}
