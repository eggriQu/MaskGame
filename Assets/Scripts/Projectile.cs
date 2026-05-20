using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float lifespan = 10f;
    
    void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
    }
}
