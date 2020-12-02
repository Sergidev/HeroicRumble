using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionMagic : MonoBehaviour
{
    public Transform DerowingCircle;


    public SpriteRenderer circle;
    public SpriteRenderer circleDerowing;

    public float angle;

    public List<EnemyAI> EnemiesProtected;

    void Update()
    {
        this.transform.Rotate(0, 0, angle * Time.deltaTime);

        if (DerowingCircle.localScale.x > 0.0f)
        {
            DerowingCircle.localScale -= new Vector3(Time.deltaTime/3, Time.deltaTime/3, Time.deltaTime/3);
            if (DerowingCircle.localScale.x <= 0.0f)
                StartCoroutine(FadeField());
        }
        else
        {
            circle.color -= new Color(0, 0, 0, Time.deltaTime * 2);
            circleDerowing.color -= new Color(0, 0, 0, Time.deltaTime * 2);
        }
    }

    void OnTriggerStay(Collider Collider)
    {
        if (Collider.gameObject.tag == "Enemy")
        {
            EnemyAI colliderComp = Collider.GetComponent<EnemyAI>();

            if(colliderComp.EnemyTypeAI != EnemyAI.EnemyType.Bomber) colliderComp.isProtected = true;

            if(!EnemiesProtected.Contains(colliderComp))
            {
                EnemiesProtected.Add(colliderComp);
            }
        }
    }
    void OnTriggerExit(Collider Collider)
    {
        if (Collider.gameObject.tag == "Enemy")
        {
            EnemyAI colliderComp = Collider.GetComponent<EnemyAI>();

            colliderComp.isProtected = false;

            if (EnemiesProtected.Contains(colliderComp))
            {
                EnemiesProtected.Remove(colliderComp);
            }
        }
    }
    public IEnumerator FadeField()
    {
        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < EnemiesProtected.Count; i++)
        {
           if(EnemiesProtected[i] != null) EnemiesProtected[i].gameObject.GetComponent<EnemyAI>().isProtected = false;
        }

        yield return new WaitForSeconds(1.5f);

        Destroy(this.gameObject);
    }
}
