using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationarySword : MonoBehaviour
{
    public float angle;
    public float power;
    public float attack;
    public Vector3 target;
    public Transform boss;
    public bool followBoss = false;
    public float speed;

    void Start()
    {
        attack = attack + (attack * GameManager.Instance.percentageScaleEnemies);
        target = GameManager.Instance.player.transform.position;
        this.transform.parent = GameManager.Instance.Trash.transform;
    }
    private void Update()
    {
        this.transform.Rotate(0, 0, angle * Time.deltaTime * 10);

        if (followBoss)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, boss.position, speed * Time.deltaTime);
        
            if(this.transform.position == boss.transform.position) Destroy(this.gameObject);
        }
        else
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
    }
    public void SetBossPath(Transform bossPos)
    {
        boss = bossPos;
        followBoss = true;
        speed = speed * 2;
    }
    void OnTriggerStay(Collider Collider)
    {
        if (Collider.gameObject.tag == "Player")
        {
            Collider.GetComponent<PlayerController>().GetDamage(attack, true);
        }
    }
}