using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrail : MonoBehaviour
{
    public float power;
    public float attack;
    bool die = false;

    void Start()
    {
        StartCoroutine(Finish());
    }

    void Update()
    {
        if(die)
        {
            if (this.transform.localScale.x > 0)
            {
                this.transform.localScale -= new Vector3(Time.deltaTime * 1, Time.deltaTime * 1, Time.deltaTime * 1);
            }
            else
                Destroy(this.gameObject);
        }
    }

    void OnTriggerStay(Collider Collider)
    {
        if (Collider.gameObject.tag == "Enemy")
        {
            Collider.GetComponent<EnemyAI>().GetDamage(power, GameManager.Instance.player.GetComponent<PlayerController>().power * 0.2f, 0, false, this.transform);
        }
    }
    public IEnumerator Finish()
    {
        yield return new WaitForSeconds(3);
        die = true;
    }
}
