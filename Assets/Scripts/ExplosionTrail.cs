using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrail : MonoBehaviour
{
    public float power;
    public float attack;

    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 3);

        foreach (var collider in colliders)
        {
            Transform body = collider.GetComponent<Transform>();
            if (body != null && collider.tag == "Enemy")
            {
                collider.GetComponent<EnemyAI>().GetDamage(power, attack, 0, false, this.transform);
            }
        }
    }
}
