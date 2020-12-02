using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningShot : MonoBehaviour
{
    Vector3 dir;
    public float speed;
    public float damage;
    public float power;
    public GameObject hitParticles;
    public GameObject LightningRotPrefab;
    public bool Rotative;
    public Transform pos;
    void Start()
    {
        PlayerController pc = GameManager.Instance.player.GetComponent<PlayerController>();
        dir = new Vector3(this.transform.position.x - pc.transform.position.x, 0, this.transform.position.z - pc.transform.position.z);
        if (Rotative) StartCoroutine(EndOrb());
    }

    void Update()
    {
        if(!Rotative)
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider Collider)
    {
        if (Collider.gameObject.tag == "Enemy")
        {
            if(GameManager.Instance.LevelLightningStrike > 1 && !this.Rotative) 
                Instantiate(LightningRotPrefab, GameManager.Instance.player.transform.position, Quaternion.identity);

            Collider.GetComponent<EnemyAI>().GetDamage(power, GameManager.Instance.player.GetComponent<PlayerController>().power * 1.5f, 0, false, this.transform);
            Instantiate(hitParticles, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public IEnumerator EndOrb()
    {
        yield return new WaitForSeconds(12.0f);
        Instantiate(hitParticles, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
