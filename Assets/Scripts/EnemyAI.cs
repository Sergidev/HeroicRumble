using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyType { Knight, Mage, Bomber, Priest, Boss };

    public bool isProtected = false;
    public float speed;
    public float speedReal;

    public float attack;

    private Transform Player;
    public Animator anim;

    public SwordEnemy sword;

    public MeshRenderer[] content;

    public ParticleSystem ParticlesAttack;
    public ParticleSystem StunParticles;
    public ParticleSystem ParticlesCircularAttack;

    public GameObject ExplosionBomb;
    public GameObject ExplosionPoison;

    public SwordController swordHitbox;
    public CircularController CircularAttack;

    Rigidbody rb;
    NavMeshAgent navemeshagent;
    bool attacking = false;
    bool canAttack = true;

    bool died = false;

    public GameObject IceMagicPrefab;

    public GameObject StationarySwordPrefab;

    public GameObject ProtectionMagicPrefab;

    [SerializeField]
    public EnemyType EnemyTypeAI;

    public bool canReceiveDamage = true;

    private int secondsPosioned = 0;
    private bool isPoisoned = false;
    public bool stunned = false;
    public bool vulnerable = false;

    private Renderer weapon;
    int bossAttackCounter;
    int bossSwordsCounter;
    bool canSpawnSord = true;

    public LifeCylinder Life;

    PlayerController playerScript;
    void Start()
    {
        playerScript = GameManager.Instance.player.GetComponent<PlayerController>();
        weapon = sword.gameObject.GetComponent<Renderer>();
        navemeshagent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        Player = GameManager.Instance.player.transform;
        speedReal = speed;
        StartCoroutine(Fadeprot());
        StartCoroutine(FadeVulnerable());

        this.Life.TotalLife = this.Life.TotalLife + ((this.Life.TotalLife/100) * GameManager.Instance.percentageScaleEnemies);
        this.Life.CurrentLife = this.Life.TotalLife;

        if (CircularAttack != null) CircularAttack.attack = this.attack + ((this.attack * (GameManager.Instance.percentageScaleEnemies / 100) + 0.15f));
        if (swordHitbox != null) swordHitbox.attack = this.attack + (this.attack * (GameManager.Instance.percentageScaleEnemies / 100));

        if (this.EnemyTypeAI == EnemyType.Bomber) StartCoroutine(StartBomb());
    }

    void Update()
    {
        if (!stunned && playerScript.Life.CurrentLife > 0)
        {
            switch (EnemyTypeAI)
            {
                case (EnemyType.Knight):

                    if (speedReal < speed)
                        speedReal += Time.deltaTime * speed;
                    else
                    {
                        speedReal = speed;
                        rb.velocity = new Vector3(0, 0, 0);
                    }

                    Vector3 movement = new Vector3(Player.position.x - this.transform.position.x, 0.0f, Player.position.z - this.transform.position.z);

                    if (canAttack && Vector3.Distance(Player.position, this.transform.position) < 3.0f)
                    {
                        this.gameObject.transform.rotation = Quaternion.LookRotation(movement);
                        StartCoroutine(AttackCoroutine());
                    }
                    if (!attacking)
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position, Player.position, speedReal * Time.deltaTime);

                        anim.SetBool("Walking", true);
                        this.gameObject.transform.rotation = Quaternion.LookRotation(movement);
                    }

                    break;

                case (EnemyType.Mage):

                    Vector3 movement2 = new Vector3(Player.position.x - this.transform.position.x, 0.0f, Player.position.z - this.transform.position.z);

                    if (speedReal < speed)
                        speedReal += Time.deltaTime * speed;
                    else
                    {
                        speedReal = speed;
                        rb.velocity = new Vector3(0, 0, 0);
                    }

                    if (Vector3.Distance(Player.position, this.transform.position) < 15.0f && Vector3.Distance(Player.position, this.transform.position) > 10.0f)
                    {
                        anim.SetBool("Walking", false);

                        if (canAttack)
                        {
                            this.gameObject.transform.rotation = Quaternion.LookRotation(movement2);
                            StartCoroutine(FireMagicCoroutine());
                        }
                    }
                    else if (!attacking)
                    {
                        if (Vector3.Distance(Player.position, this.transform.position) > 15.0f)
                        {

                            this.transform.position = Vector3.MoveTowards(this.transform.position, Player.position, speedReal * Time.deltaTime);
                            this.gameObject.transform.rotation = Quaternion.LookRotation(movement2);
                        }
                        else
                        {
                            this.transform.position = Vector3.MoveTowards(this.transform.position, Player.position, -speedReal * Time.deltaTime);
                            this.gameObject.transform.rotation = Quaternion.LookRotation(-movement2);
                        }

                        anim.SetBool("Walking", true);
                    }

                    break;

                case (EnemyType.Bomber):

                    anim.SetBool("Bomb", true);
                    this.transform.position = Vector3.MoveTowards(this.transform.position, Player.position, speedReal * Time.deltaTime);
                    Vector3 movement3 = new Vector3(Player.position.x - this.transform.position.x, 0.0f, Player.position.z - this.transform.position.z);
                    this.gameObject.transform.rotation = Quaternion.LookRotation(movement3);

                    if (speedReal < speed)
                        speedReal += Time.deltaTime * 3;
                    else
                    {
                        speedReal = speed;
                        rb.velocity = new Vector3(0, 0, 0);
                    }

                    if (Vector3.Distance(Player.position, this.transform.position) < 2.0f)
                    {
                        StartCoroutine(BombCoroutine());
                    }

                    break;

                case (EnemyType.Priest):

                    Vector3 movement4 = new Vector3(Player.position.x - this.transform.position.x, 0.0f, Player.position.z - this.transform.position.z);

                    if (speedReal < speed)
                        speedReal += Time.deltaTime * speed;
                    else
                    {
                        speedReal = speed;
                        rb.velocity = new Vector3(0, 0, 0);
                    }

                    if (Vector3.Distance(Player.position, this.transform.position) < 15 && Vector3.Distance(Player.position, this.transform.position) > 10.0f)
                    {
                        anim.SetBool("Walking", false);

                        if (canAttack)
                        {
                            this.gameObject.transform.rotation = Quaternion.LookRotation(movement4);
                            StartCoroutine(ProtectionMagicCoroutine());
                        }
                    }
                    else if (!attacking)
                    {
                        if (Vector3.Distance(Player.position, this.transform.position) > 15.0f)
                        {

                            this.transform.position = Vector3.MoveTowards(this.transform.position, Player.position, speedReal * Time.deltaTime);
                            this.gameObject.transform.rotation = Quaternion.LookRotation(movement4);
                        }
                        else
                        {
                            this.transform.position = Vector3.MoveTowards(this.transform.position, Player.position, -speedReal * Time.deltaTime);
                            this.gameObject.transform.rotation = Quaternion.LookRotation(-movement4);
                        }

                        anim.SetBool("Walking", true);
                    }

                    break;

                case (EnemyType.Boss):

                    if (speedReal < speed)
                        speedReal += Time.deltaTime * speed;
                    else
                    {
                        speedReal = speed;
                        rb.velocity = new Vector3(0, 0, 0);
                    }

                    Vector3 movement5 = new Vector3(Player.position.x - this.transform.position.x, 0.0f, Player.position.z - this.transform.position.z);

                    if (canAttack && Vector3.Distance(Player.position, this.transform.position) < 4.0f)
                    {
                        this.gameObject.transform.rotation = Quaternion.LookRotation(movement5);                      

                        if (bossAttackCounter < 3)
                        {
                            bossAttackCounter++;
                            StartCoroutine(AttackCoroutine());
                        }
                        else
                        {
                            bossAttackCounter = 0;
                            StartCoroutine(CircularCoroutine());
                        }

                    }
                    if (!attacking)
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position, Player.position, speedReal * Time.deltaTime);

                        if(canSpawnSord && this.Life.CurrentLife < this.Life.TotalLife / 2)
                        {                            
                            StartCoroutine(SpawnSwordCoroutine());
                        }                    

                        anim.SetBool("Walking", true);
                        this.gameObject.transform.rotation = Quaternion.LookRotation(movement5);
                    }

                    break;

                default: break;
            }
        }
        else
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Attack", false);
            anim.SetBool("FireMagic", false);
            anim.SetBool("Venom", false);
            anim.SetBool("Circular", false);
        }
    }
    public IEnumerator AttackCoroutine()
    {
        canAttack = false;
        attacking = true;
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Attack", true);
        anim.SetBool("Walking", false);
        yield return new WaitForSeconds(0.55f);
        ParticlesAttack.Play();
        swordHitbox.activated = true;
        yield return new WaitForSeconds(0.2f);
        swordHitbox.activated = false;
        anim.SetBool("Attack", false);
        yield return new WaitForSeconds(0.75f);
        canAttack = true;
        attacking = false;
    }
    public IEnumerator FireMagicCoroutine()
    {
        attacking = true;
        canAttack = false;
        anim.SetBool("Walking", false);
        anim.SetBool("FireMagic", true);
        Instantiate(IceMagicPrefab, GameManager.Instance.player.transform.position, Quaternion.Euler(90, 0, 0));
        yield return new WaitForSeconds(2.0f);
        anim.SetBool("FireMagic", false);
        yield return new WaitForSeconds(2.25f);
        canAttack = true;
        attacking = false;
    }
    public IEnumerator ProtectionMagicCoroutine()
    {
        attacking = true;
        canAttack = false;
        anim.SetBool("Walking", false);
        anim.SetBool("FireMagic", true);
        Instantiate(ProtectionMagicPrefab, GameManager.Instance.player.transform.position, Quaternion.Euler(90, 0, 0));
        yield return new WaitForSeconds(2.0f);
        anim.SetBool("FireMagic", false);
        yield return new WaitForSeconds(6.25f);
        canAttack = true;
        attacking = false;
    }
    public IEnumerator BombCoroutine()
    {
        if(sword != null)
        {
            Instantiate(ExplosionBomb, sword.transform.position, Quaternion.identity);
            if (Vector3.Distance(Player.position, this.transform.position) < 2.0f) GameManager.Instance.player.GetComponent<PlayerController>().GetDamage(attack + (attack * (GameManager.Instance.percentageScaleEnemies / 100)), false);
            GetDamage(5, this.Life.TotalLife, 0, false, sword.transform);
            yield return new WaitForSeconds(0.0f);
        }       
    }
    public IEnumerator StartBomb()
    {
        yield return new WaitForSeconds(15);

        StartCoroutine(BombCoroutine());
    }
    public void GetDamage(float power, float damage, int venom, bool sparks, Transform hit)
    {
        if(canReceiveDamage && !isProtected)
        {
            if (vulnerable) damage = damage * 2;

            Life.CurrentLife -= damage;
            if (venom > 0 && this.Life.CurrentLife > 0)
            {
                if (!isPoisoned)
                    StartCoroutine(Poisoned());
                else
                    secondsPosioned += GameManager.Instance.LevelVenomIvyBlade;
            }

            if (Life.CurrentLife <= 0)
            {
                Die(power, hit);
            }                
            else
            {
                if(power > 0) speedReal = 0;
                rb.AddForce((this.transform.position - GameManager.Instance.player.transform.position).normalized * 2 * power, ForceMode.Impulse);                               
            }
            
            if(sparks) Instantiate(GameManager.Instance.player.GetComponent<PlayerController>().hitParticles, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            canReceiveDamage = false;
            StartCoroutine(CanReceiveDamageAgain());
        }       
    }
    public IEnumerator CircularCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("Circular", true);
        yield return new WaitForSeconds(0.5f);
        CircularAttack.activated = true;
        yield return new WaitForSeconds(0.1f);
        ParticlesCircularAttack.Play();
        yield return new WaitForSeconds(0.4f);
        CircularAttack.activated = false;
        anim.SetBool("Circular", false);       
    }
    public IEnumerator SpawnSwordCoroutine()
    {
        attacking = true;
        canAttack = false;
        this.speedReal = 0;
        canSpawnSord = false;
        anim.SetBool("Venom", true);
        yield return new WaitForSeconds(0.15f);
        if(bossSwordsCounter < 5)
        {
            Instantiate(StationarySwordPrefab, this.transform.position, Quaternion.Euler(90, 90, 90));
            bossSwordsCounter++;
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.Trash.transform.childCount; i++)
            {
                if(GameManager.Instance.Trash.transform.GetChild(i).GetComponent<StationarySword>() != null) GameManager.Instance.Trash.transform.GetChild(i).GetComponent<StationarySword>().SetBossPath(this.transform);
            }
            bossSwordsCounter = 0;
        }
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Venom", false);
        attacking = false;
        canAttack = true;

        yield return new WaitForSeconds(5.0f);
        canSpawnSord = true;
    }
    public void Die(float power, Transform hit)
    {
        if (ParticlesAttack != null) ParticlesAttack.gameObject.SetActive(false);
        if (CircularAttack != null) CircularAttack.gameObject.SetActive(false);

        anim.enabled = false;

        if(!died)
            sword.Drop(power);

        anim.SetBool("Walking", false);        
        navemeshagent.enabled = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(((((this.transform.position - hit.position + new Vector3(0, 1, 0)).normalized * power) * 10) + (new Vector3(0, 0, 10))), ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(-power * 10, power * 10), Random.Range(-power * 10, power * 10), Random.Range(-power * 10, power * 10)), ForceMode.Impulse);
        
        if(isPoisoned && GameManager.Instance.LevelVenomIvyBlade > 2)
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, 10);
            Instantiate(ExplosionPoison, this.transform.position, Quaternion.identity);

            foreach (var collider in colliders)
            {
                if (collider.tag == "Enemy")
                {
                    collider.GetComponent<EnemyAI>().StartCoroutine(Poisoned());
                }
            }
        }

        if (this.EnemyTypeAI == EnemyType.Boss) GameManager.Instance.activeClock = true;

        died = true;
        StartCoroutine(Disappear());
        this.enabled = false;
    }
    public IEnumerator CanReceiveDamageAgain()
    {
        for (int j = 0; j < content.Length; j++)
        {
            content[j].materials[0].color = new Color(1, 0, 0, 1.0f);
        }
        yield return new WaitForSeconds(0.05f);
        for (int j = 0; j < content.Length; j++)
        {
            content[j].materials[0].color = new Color(1, 1, 1, 1.0f);
        }

        yield return new WaitForSeconds(0.4f);
        canReceiveDamage = true;
    }
    public IEnumerator Poisoned()
    {
        isPoisoned = true;
        secondsPosioned = 2 + GameManager.Instance.LevelVenomIvyBlade;
        if (GameManager.Instance.LevelVenomIvyBlade > 1) speedReal = speed / 2;

        while (secondsPosioned > 0)
        {
            yield return new WaitForSeconds(1.0f);
            secondsPosioned--;
            GetDamage(0, secondsPosioned, 0, false, this.transform);
        }

        speedReal = speed;
        isPoisoned = false;
    }
    public IEnumerator Fadeprot()
    {
        isProtected = false;
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Fadeprot());
    }
    public IEnumerator FadeVulnerable()
    {
        vulnerable = false;
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(FadeVulnerable());
    }
    public void StunEnemy()
    {
        StartCoroutine(StunEnemyCoroutine());
    }
    public IEnumerator StunEnemyCoroutine()
    {
        stunned = true;
        anim.enabled = false;
        StunParticles.Play();
        ParticlesAttack.gameObject.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        ParticlesAttack.gameObject.SetActive(true);
        anim.enabled = true;
        stunned = false;
    }
    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(this.gameObject);
    }
}
