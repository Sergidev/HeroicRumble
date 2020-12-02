using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemy : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider bc;
    SphereCollider sc;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        bc = this.GetComponent<BoxCollider>();
        sc = this.GetComponent<SphereCollider>();
    }
    public void Drop(float power)
    {
        rb.isKinematic = false;
        this.transform.SetParent(null);
        if (bc != null) bc.isTrigger = false; else if (sc != null) { /*sc.isTrigger = false;*/ Destroy(this.gameObject); }
        rb.AddForce((this.transform.position - GameManager.Instance.player.transform.position + new Vector3(0, 0.5f, 0)).normalized * power, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), Random.Range(-30, 30)), ForceMode.Impulse);
        this.transform.SetParent(GameManager.Instance.Trash.transform);
    }
}
