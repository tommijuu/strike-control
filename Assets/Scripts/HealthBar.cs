using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //public DisplayScore displayScore;
    private int scoreOnDeath = 50;

    public EntityStats stats;
    private Color normalHealthColor;
    private Color middleHealthColor;
    private Color criticalHealthColor;

    private GameManager gm;

    [SerializeField] GameObject prefab;
    private GameObject healthBarObject;
    RawImage healthBarFill;

    GameObject canvas;
    Camera cam;

    public GameObject explosionPrefab;

    public GameObject HealthBarObject { get { return healthBarObject; } }

    public float health;

    void Start()
    {
        //displayScore = GameObject.Find("DynamicUI").GetComponent<DisplayScore>();

        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        cam = Camera.main;
        canvas = GameObject.FindGameObjectWithTag("HealthCanvas");
        healthBarObject = Instantiate(prefab, canvas.transform);

        healthBarFill = healthBarObject.transform.GetChild(2).GetComponent<RawImage>();

        normalHealthColor = Color.green;
        middleHealthColor = Color.yellow;
        criticalHealthColor = Color.red;

        health = stats.maxHealth;

        HealthBarInvisible();
    }

    void LateUpdate()
    {
        healthBarObject.transform.position = cam.WorldToScreenPoint(transform.position + new Vector3(0f, 2f, 0f));

        float healthRatio = health / stats.maxHealth;
        healthBarFill.transform.localScale = new Vector3(healthRatio, 1, 1);

        if (healthRatio < 1f && !healthBarObject.activeSelf)
        {
            HealthBarVisible();
        }

        if (healthRatio >= .65f)
            healthBarFill.color = normalHealthColor;
        else if (healthRatio >= .35f)
            healthBarFill.color = middleHealthColor;
        else
            healthBarFill.color = criticalHealthColor;

        if (health <= 0)
            Die();
    }

    void Die()
    {

        GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>().selectedUnits.Remove(transform);

        if (!gameObject.GetComponent<BuildingManager>())
        {
            if (!CompareTag("Headquarters"))
            {
                string unitType = gameObject.GetComponent<UnitController>().stats.unitType.ToString();
                if (unitType.Equals("infantry"))
                {
                    if (CompareTag("EnemyUnit"))
                    {
                        if (GetComponentInChildren<EnemyFOV>().visible)
                        {
                            GetComponentInChildren<UnitAudioManager>().UnitDeath(gameObject.GetComponent<UnitController>().stats.name, unitType, 1f);
                        }
                        else
                        {
                            Debug.Log(gameObject.name + " hidden death");
                        }
                    }
                    else
                    {
                        GetComponentInChildren<UnitAudioManager>().UnitDeath(gameObject.GetComponent<UnitController>().stats.name, unitType, 1f);
                    }

                }
                else
                {
                    GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                    explosion.GetComponentInChildren<ExplosionSoundManager>().PlaySoundExplosion(1f);
                    Destroy(explosion, 3);
                }
            }
            else if (CompareTag("Headquarters"))
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosion.GetComponentInChildren<ExplosionSoundManager>().PlaySoundExplosion(1f);
                Destroy(explosion, 3);
            }
        }
        else
        {
            if (CompareTag("EnemyUnit"))
            {
                if (GetComponentInChildren<EnemyFOV>().visible)
                {
                    GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                    explosion.GetComponentInChildren<ExplosionSoundManager>().PlaySoundExplosion(1f);
                    Destroy(explosion, 3);
                }
                else
                {
                    Debug.Log(gameObject.name + " hidden death");
                }
            }
            else
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosion.GetComponentInChildren<ExplosionSoundManager>().PlaySoundExplosion(1f);
                Destroy(explosion, 3);
            }
        }



        gm.score += scoreOnDeath;

        Destroy(gameObject);
        Destroy(healthBarObject);
    }

    public void HealthBarVisible()
    {
        healthBarObject.gameObject.SetActive(true);
    }

    public void HealthBarInvisible()
    {
        healthBarObject.gameObject.SetActive(false);
    }

    public void ShowOrHide()
    {
        if (healthBarObject.activeSelf)
        {
            HealthBarInvisible();
        }
        else
        {
            HealthBarVisible();
        }
    }

    void EnemyHealthBar()
    {
        if (gameObject.tag == "EnemyUnit")
        {
            healthBarObject.gameObject.SetActive(false);
        }
    }
}
