using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultSpecialPower2 : MonoBehaviour
{
    private ParticleSystem ps;
    public GameObject CultParticleEffect;
    public GameObject CultExplosion;
    public bool start;
    public float radius;
    public float maxRadius;
    public float damage;

    private void Start()
    {
        ps = CultParticleEffect.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startSize = maxRadius * 2.5f;
        StartCoroutine(ExecuteAfterTime(1));
        GameObject explosion = Instantiate(CultParticleEffect, transform.position, CultParticleEffect.transform.rotation);
        Audiomanager.instance.PlaySpecialSound(6, 1f);
        Destroy(explosion, 3);
    }

    void Update()
    {
        if (start)
        {
            if (radius < maxRadius*2f)
            {
                radius += maxRadius * Time.deltaTime;
                transform.localScale = new Vector3(radius, 0.1f, radius);
            }
            else if (radius >= maxRadius*2f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        
        if (collider.CompareTag("EnemyUnit"))
        {
            if (collider.GetComponentInChildren<EnemyFOV>().visible)
            {
                string unitType = collider.GetComponent<UnitController>().stats.unitType.ToString();
                if (unitType.Equals("infantry"))
                {
                    GameObject explosion = Instantiate(CultExplosion, collider.transform.position, CultExplosion.transform.rotation);
                    Destroy(explosion, 3);
                }
            }

            Debug.Log("enemy: "+collider.name+" hit by cult special power");
            HealthBar targetHealth = collider.GetComponent<HealthBar>();
            targetHealth.health -= damage;
        }
        if (collider.CompareTag("PlayerUnit"))
        {
            if (!collider.GetComponent<ResourceGatherer>())
            {
                string unitType = collider.GetComponent<UnitController>().stats.unitType.ToString();
                if (unitType.Equals("infantry"))
                {
                    GameObject explosion = Instantiate(CultExplosion, collider.transform.position, CultExplosion.transform.rotation);
                    Destroy(explosion, 3);
                }
                    
                Debug.Log("friendly: "+collider.name+" hit by cult special power");
                HealthBar targetHealth = collider.GetComponent<HealthBar>();
                targetHealth.health -= damage;
            }
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        start = true;
    }
}
