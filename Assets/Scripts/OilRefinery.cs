using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilRefinery : MonoBehaviour
{
    private GameObject gathererPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gathererPrefab = FactionManager.instance.PlayerGatherer;

        Instantiate(gathererPrefab, transform.position, Quaternion.identity);
    }

}
