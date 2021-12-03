using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperOilResource : MonoBehaviour
{
    [SerializeField] private GameObject fovObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisableFov());
    }

    private IEnumerator DisableFov()
    {
        yield return new WaitForSeconds(0.1f);
        fovObject.gameObject.SetActive(false);
    }
}
