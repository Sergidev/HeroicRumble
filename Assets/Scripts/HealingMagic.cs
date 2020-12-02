using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingMagic : MonoBehaviour
{
    public Transform GrowingCircle;

    public bool activated = true;

    public SpriteRenderer circle;
    public SpriteRenderer circleGrowing;

    public Transform Pivot;
    public float Radius;
    public LayerMask Layers;

    public float angle;

    void Update()
    {
        this.transform.Rotate(0, 0, angle * Time.deltaTime);

        if (activated)
        {
            Collider[] colliders = Physics.OverlapSphere(Pivot.position, Radius, Layers);

            int amount = 0;

            foreach (var collider in colliders)
            {
                Transform body = collider.GetComponent<Transform>();
                if (body != null && collider.tag == "Enemy")
                {
                    if (GameManager.Instance.LevelHealingField > 1) body.gameObject.GetComponent<EnemyAI>().speedReal = body.gameObject.GetComponent<EnemyAI>().speed / 4;
                    if (GameManager.Instance.LevelHealingField > 2) body.gameObject.GetComponent<EnemyAI>().vulnerable = true;
                }
            }

            if (GrowingCircle.localScale.x < 2.05f)
            {
                GrowingCircle.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
                if (GrowingCircle.localScale.x >= 2.05f && activated)
                {
                    colliders = Physics.OverlapSphere(Pivot.position, Radius, Layers);

                    amount = 0;

                    foreach (var collider in colliders)
                    {
                        Transform body = collider.GetComponent<Transform>();
                        if (body != null && collider.tag == "Enemy")
                        {
                            amount++;
                        }
                    }

                    GameManager.Instance.player.GetComponent<PlayerController>().Life.CurrentLife += (GameManager.Instance.player.GetComponent<PlayerController>().Life.TotalLife/100) * 5 * amount;
                    GameManager.Instance.player.GetComponent<PlayerController>().ParticlesHealing.Play();
                    activated = false;
                    StartCoroutine(FadeExplosion());
                }
            }
        }
        else
        {
            circle.color -= new Color(0, 0, 0, Time.deltaTime * 2);
            circleGrowing.color -= new Color(0, 0, 0, Time.deltaTime * 2);
        }
    }
    public IEnumerator FadeExplosion()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(this.gameObject);
    }
}
