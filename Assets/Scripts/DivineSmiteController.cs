using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivineSmiteController : MonoBehaviour
{
    public float power;
    public float attack;
    public bool activated = false;

    void OnTriggerStay(Collider Collider)
    {
        if (activated && Collider.gameObject.tag == "Enemy")
        {           
            if (GameManager.Instance.LevelDivineSmite > 1 && Collider.GetComponent<EnemyAI>().canReceiveDamage) Collider.GetComponent<EnemyAI>().StunEnemy();
            if (GameManager.Instance.LevelDivineSmite > 2 && Collider.GetComponent<EnemyAI>().canReceiveDamage)
            {
                GameManager.Instance.player.GetComponent<PlayerController>().Life.CurrentLife += (GameManager.Instance.player.GetComponent<PlayerController>().Life.TotalLife * 0.03f);
                GameManager.Instance.player.GetComponent<PlayerController>().ParticlesHealing.Play();
            }
            Collider.GetComponent<EnemyAI>().GetDamage(power, GameManager.Instance.player.GetComponent<PlayerController>().power * 2.0f, 0, false, GameManager.Instance.player.transform);
        }
    }
}
