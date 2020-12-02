using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlwindController : MonoBehaviour
{
    public enum ForceType { Repulsion = -1, None = 0, Attraction = 1 }
    public ForceType Type;
    public Transform Pivot;
    public float Radius;
    public LayerMask Layers;
    public float angle;
    public float power;
    public float attack;
    public int seconds;
    private bool die = false;
    void Start()
    {
        StartCoroutine(Finish());
    }

    private void Update()
    {
        if (!die)
        {
            this.transform.Rotate(0, angle * Time.deltaTime * 3, 0);

            Collider[] colliders = Physics.OverlapSphere(Pivot.position, Radius, Layers);

            float signal = (float)Type;

            foreach (var collider in colliders)
            {
                Transform body = collider.GetComponent<Transform>();
                if (body != null && collider.tag == "Enemy")
                {
                    Vector3 direction = Pivot.position - body.position;
                    float distance = direction.magnitude;
                    float forceRate = (power / distance);

                    switch (Type)
                    {
                        case ForceType.Repulsion:
                            body.position = Vector3.MoveTowards(body.position, Pivot.position - body.position, forceRate * Time.deltaTime);
                            break;
                        case ForceType.Attraction:
                            body.position = Vector3.MoveTowards(body.position, Pivot.position, forceRate * Time.deltaTime);
                            break;

                        default: break;
                    }

                    if (GameManager.Instance.LevelWhirlwind > 1) collider.GetComponent<EnemyAI>().GetDamage(0, attack, 0, false, this.transform);
                }
            }
        }
        else
        {
            if (this.transform.localScale.x > 0)
            {
                this.transform.localScale -= new Vector3(Time.deltaTime * 1, Time.deltaTime * 1, Time.deltaTime * 1);
            }
            else
                Destroy(this.gameObject);
        }

        if (GameManager.Instance.LevelWhirlwind > 2) this.transform.position = Vector3.MoveTowards(this.transform.position, GameManager.Instance.player.transform.position, -power / 2 * Time.deltaTime);
    }

    public IEnumerator Finish()
    {
        yield return new WaitForSeconds(seconds);
        die = true;
    }
}