using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordController : MonoBehaviour
{
    public float power;
    public float attack;
    public bool activated = false;
    public int VenomDamage;

    public Attacker AttackerType;
    public enum Attacker { Player, Enemy };

    void OnTriggerStay(Collider Collider)
    {
        if (AttackerType == Attacker.Player && activated && Collider.gameObject.tag == "Enemy")
        {
            Collider.GetComponent<EnemyAI>().GetDamage(power, GameManager.Instance.player.GetComponent<PlayerController>().power * 1.0f, VenomDamage, true, GameManager.Instance.player.transform);
        }
        else if (AttackerType == Attacker.Enemy && activated && Collider.gameObject.tag == "Player")
        {
            Collider.GetComponent<PlayerController>().GetDamage(attack, true);
        }
    }
}
