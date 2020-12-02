using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMagic : MonoBehaviour
{ 
    public Transform GrowingCircle;

    public bool activated = false;
    public float damage;

    public SpriteRenderer circle;
    public SpriteRenderer circleGrowing;

    public float angle;
    void Start()
    {
        damage = damage + (damage * (GameManager.Instance.percentageScaleEnemies / 100));
    }
    void Update()
    {
        this.transform.Rotate(0, 0, angle * Time.deltaTime);

        if (GrowingCircle.localScale.x < 2.05f)
        {
            GrowingCircle.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
            if (GrowingCircle.localScale.x >= 2.05f)
                StartCoroutine(FadeExplosion());
        }  
        else
        {
            circle.color -= new Color(0, 0, 0, Time.deltaTime * 2);
            circleGrowing.color -= new Color(0, 0, 0, Time.deltaTime * 2);
        }
    }

    void OnTriggerStay(Collider Collider)
    {
        if (activated && Collider.gameObject.tag == "Player")
        {
            Collider.GetComponent<PlayerController>().GetDamage(damage, false);
            activated = false;
        }
    }
    public IEnumerator FadeExplosion()
    {
        activated = true;  
        yield return new WaitForSeconds(0.05f);
        activated = false;       
        yield return new WaitForSeconds(3.0f);
        Destroy(this.gameObject);
    }
}
