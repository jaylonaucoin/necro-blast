using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{

    [Tooltip("Furthest distance bullet will look for target")]
    public float maxDistance = 1000000;
    RaycastHit hit;
    [Tooltip("Prefab of wall damange hit. The object needs 'LevelPart' tag to create decal on it.")]
    public GameObject decalHitWall;
    [Tooltip("Decal will need to be slightly in front of the wall so it doesn't cause rendering problems. Best feel is from 0.01-0.1.")]
    public float floatInfrontOfWall;
    [Tooltip("Blood prefab particle this bullet will create upon hitting enemy")]
    public GameObject bloodEffect;
    [Tooltip("Put Weapon layer and Player layer to ignore bullet raycast.")]
    public LayerMask ignoreLayer;
    public int bulletDamage = 5; // Damage that bullet will deal

    /*
    * Upon bullet creation with this script attached,
    * bullet creates a raycast which searches for corresponding tags.
    * If raycast finds something it will create a decal of corresponding tag.
    */
    void Update()
    {

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ~ignoreLayer))
        {
            if (decalHitWall)
            {
                if (hit.transform.CompareTag("LevelPart"))
                {
                    Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
                    Destroy(gameObject);
                }
                else if (hit.transform.CompareTag("Dummie"))
                {
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));

                    // Deal damage to enemy
                    EnemyMovement enemy = hit.transform.GetComponent<EnemyMovement>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(bulletDamage);
                    }

                    Destroy(gameObject);
                }
            }
            Destroy(gameObject);
        }
        Destroy(gameObject, 0.1f);
    }
}
