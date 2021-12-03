using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float speed = 2f;

    public UnitController unitController;

    void Start()
    {
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    public void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosion.GetComponentInChildren<ExplosionSoundManager>().PlaySoundExplosion(1f);
        Destroy(explosion, 3);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach(string tag in unitController.enemyTags)
        {
            if(collision.gameObject.CompareTag(tag))
            {
                unitController.DoDamage();
                Explode();
            }
        }
    }
}