using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultSpecialPower: MonoBehaviour
{
    private ParticleSystem ps;
    public GameObject CultParticleEffect;
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
        Audiomanager.instance.PlaySpecialSound(6,1f);
        Destroy(explosion, 3);
    }

    void Update()
    {
        if (start)
        {
            if (radius < maxRadius)
            {
                radius += maxRadius / 2 * Time.deltaTime;

            }
            else if (radius >= maxRadius)
            {
                Destroy(gameObject);
            }
            TargetCheck();
        }
    }

    public void TargetCheck()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        int i = 0;

        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.CompareTag("EnemyUnit"))
            {
                HealthBar targetHealth = hitColliders[i].GetComponent<HealthBar>();
                targetHealth.health -= damage;
            }
            if (hitColliders[i].gameObject.CompareTag("PlayerUnit"))
            {
                if (!hitColliders[i].gameObject.GetComponent<ResourceGatherer>())
                {
                    HealthBar targetHealth = hitColliders[i].GetComponent<HealthBar>();
                    targetHealth.health -= damage;
                }
            }
            i++;
        }
    }


    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        start = true;
    }
}