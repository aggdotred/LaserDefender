using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
    public float health = 100.0f;
    public GameObject projectile;
    public float projectileSpeed = 10.0f;
    public float shotsPerSecond = 0.5f;

    void Update()
    {
        float probability = shotsPerSecond * Time.deltaTime;
        if (Random.value < probability)
        {
            Fire();
        }
    }

    void Fire()
    {
        
        Vector3 startPosition = transform.position + new Vector3(0, -1, 0);
        GameObject missile = Instantiate(projectile, startPosition, Quaternion.identity) as GameObject;
        missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile missile = collider.gameObject.GetComponent<Projectile>();
        if (missile)
        {
            missile.Hit();
            health -= missile.GetDamage();
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

    }
}
