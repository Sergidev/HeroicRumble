using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTL : MonoBehaviour
{
    public int seconds;
    void Start()
    {
        StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
}
