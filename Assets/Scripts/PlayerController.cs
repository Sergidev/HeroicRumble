using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform PlayerPosition;
    public Transform ShotPosition1;
    public Transform ShotPosition2;
    public Transform ShotPosition3;
    public Animator anim;

    public Joystick joystick;

    public SwordController NormalAttack;
    public CircularController CircularAttack;
    public DivineSmiteController DivineSmiteAttack;

    public ParticleSystem ParticlesAttack;
    public ParticleSystem ParticlesVenomAttack;
    public ParticleSystem ParticlesCircularAttack;
    public ParticleSystem ParticlesDivineSmiteAttack;
    public ParticleSystem ParticlesDivineSmashAttack;
    public ParticleSystem ParticlesFireFeetAttack;
    public ParticleSystem ParticlesLightningStrikeAttack;
    public ParticleSystem ParticlesHealing;

    public MeshRenderer[] content;

    public GameObject hitParticles;

    public GameObject ParticlesDashAttack;
    public GameObject WhirlwindPrefab;
    public GameObject HealingPrefab;
    public GameObject LightningOrbPrefab;
    public GameObject TrailPrefab;
    public GameObject ExplosionPrefab;

    public GameObject SwordGameObject;

    public float speed;
    public float power;

    private bool canReceiveDamage = true;

    public LifeBar Life;

    public Image AttackButton;
    public Image DivineSmiteButton;
    public Image VenomButton;
    public Image SpinButton;
    public Image WhirlwindButton;
    public Image DashButton;
    public Button DashButtonClick;
    public Image FireFeetButton;
    public Image LightningStrikeButton;
    public Image HealingFieldButton;

    public bool lightningBlade = false;
    public bool TrailActive = false;
    public bool ExplosionActive = false;
    public bool quiet = false;

    bool died = false;

    void Start()
    {
        if(Life == null) Life = GameManager.Instance.LifePlayerObject;
        Life.TotalLife = 100;
        Life.CurrentLife = 100;
        power = 10;
        speed = 6;
    }
    void Update()
    {
        if (this.Life.CurrentLife > 1)
        {
            Vector3 movement = new Vector3(0, 0.0f, 0);

            //Mobile
            if (Application.platform == RuntimePlatform.Android)
            {
                movement = Vector3.Normalize(Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical); movement = new Vector3(movement.x, 0, movement.y);
            }
            //PC
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                movement = (Vector3.right * Input.GetAxis("Horizontal") + Vector3.up * Input.GetAxis("Vertical")); movement = new Vector3(movement.x, 0, movement.y);
            }
            //Editor Unity
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                movement = (Vector3.right * Input.GetAxis("Horizontal") + Vector3.up * Input.GetAxis("Vertical")); movement = new Vector3(movement.x, 0, movement.y);
            }

            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

            if (movement != Vector3.zero && !quiet)
            {
                anim.SetBool("Walking", true);

                this.gameObject.transform.rotation = Quaternion.LookRotation(movement);
                transform.Translate(movement.normalized * speed * Time.deltaTime, Space.World);

                DashButtonClick.interactable = true;
            }
            else
            {
                DashButtonClick.interactable = false;

                anim.SetBool("Walking", false);
            }
        }
        else
        {
            if (!died)
            {
                anim.enabled = false;

                SwordGameObject.GetComponent<Rigidbody>().isKinematic = false;
                SwordGameObject.transform.SetParent(null);
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                this.GetComponent<Rigidbody>().AddForce((this.transform.position + new Vector3(1, 0.25f, 1)).normalized * power, ForceMode.Impulse);
                this.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20)), ForceMode.Impulse);
                SwordGameObject.transform.SetParent(GameManager.Instance.Trash.transform);

                GameManager.Instance.EndGame();

                died = true;
            }
        }
    }

    public void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }
    public void SpinToWin()
    {
        StartCoroutine(CircularCoroutine());
    }
    public void DivineSmite()
    {
        StartCoroutine(DivineSmiteCoroutine());
    }
    public void VenomIvyBlade()
    {
        StartCoroutine(VenomCoroutine());
    }
    public void Whirlwind()
    {
        StartCoroutine(WhirlwindCoroutine());
    }
    public void Dash()
    {
        StartCoroutine(DashCoroutine());
    }
    public void FireFeet()
    {
        StartCoroutine(FireFeetCoroutine());
    }
    public void HealingField()
    {
        StartCoroutine(HealingFieldCoroutine());
    }
    public void LightningStrike()
    {
        StartCoroutine(LightningStrikeCoroutine());
    }

    public IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.15f);
        if (AttackButton.fillAmount == 1)
        {
            AttackButton.fillAmount = 0;

            anim.SetBool("Attack", true);
            yield return new WaitForSeconds(0.4f);
            NormalAttack.activated = true;

            if (lightningBlade)
            {
                GameObject go = Instantiate(LightningOrbPrefab, ShotPosition2.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                go.GetComponent<LightningShot>().pos = ShotPosition2;

                if(GameManager.Instance.LevelLightningStrike > 2)
                {
                    go = Instantiate(LightningOrbPrefab, ShotPosition1.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                    go.GetComponent<LightningShot>().pos = ShotPosition1;

                    go = Instantiate(LightningOrbPrefab, ShotPosition3.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                    go.GetComponent<LightningShot>().pos = ShotPosition3;
                }
            }

            ParticlesAttack.Play();
            yield return new WaitForSeconds(0.3f);
            NormalAttack.activated = false;
            anim.SetBool("Attack", false);

            while (AttackButton.fillAmount < 1)
            {
                AttackButton.fillAmount += Time.deltaTime * 1.5f;
                yield return new WaitForSeconds(0.01f);
            }

            if (AttackButton.fillAmount > 1) AttackButton.fillAmount = 1;
        }
    }
    public IEnumerator DivineSmiteCoroutine()
    {       
        if (DivineSmiteButton.fillAmount == 1)
        {
            quiet = true;
            DivineSmiteButton.fillAmount = 0;
            yield return new WaitForSeconds(0.15f);
            anim.SetBool("Divine", true);
            yield return new WaitForSeconds(0.1f);
            ParticlesDivineSmiteAttack.Play();
            yield return new WaitForSeconds(0.75f);
            ParticlesDivineSmashAttack.Play();
            DivineSmiteAttack.activated = true;
            yield return new WaitForSeconds(0.25f);
            anim.SetBool("Divine", false);
            yield return new WaitForSeconds(0.15f);
            DivineSmiteAttack.activated = false;
            quiet = false;

            while (DivineSmiteButton.fillAmount < 1)
            {
                DivineSmiteButton.fillAmount += Time.deltaTime / 10;
                yield return new WaitForSeconds(0.01f);
            }

            if (DivineSmiteButton.fillAmount > 1) DivineSmiteButton.fillAmount = 1;
        }
    }
    public IEnumerator VenomCoroutine()
    {
        if (VenomButton.fillAmount == 1)
        {
            VenomButton.fillAmount = 0;

            anim.SetBool("Venom", true);
            yield return new WaitForSeconds(0.15f);
            ParticlesVenomAttack.Play();
            this.NormalAttack.VenomDamage = 1;
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Venom", false);
            yield return new WaitForSeconds(10.0f);
            ParticlesVenomAttack.Stop();
            this.NormalAttack.VenomDamage = 0;

            while (VenomButton.fillAmount < 1)
            {
                VenomButton.fillAmount += Time.deltaTime / 15;
                yield return new WaitForSeconds(0.01f);
            }

            if (VenomButton.fillAmount > 1) VenomButton.fillAmount = 1;
        }
    }
    public IEnumerator WhirlwindCoroutine()
    {
        if (WhirlwindButton.fillAmount == 1)
        {
            WhirlwindButton.fillAmount = 0;

            anim.SetBool("Venom", true);
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Venom", false);
            Instantiate(WhirlwindPrefab, this.transform.position, Quaternion.identity);

            while (WhirlwindButton.fillAmount < 1)
            {
                WhirlwindButton.fillAmount += Time.deltaTime / 10;
                yield return new WaitForSeconds(0.01f);
            }

            if (WhirlwindButton.fillAmount > 1) WhirlwindButton.fillAmount = 1;
        }
    }
    public IEnumerator DashCoroutine()
    {
        if (DashButton.fillAmount == 1)
        {
            DashButton.fillAmount = 0;

            Instantiate(ParticlesDashAttack, this.transform.position +  new Vector3(0, 1, 0), this.transform.rotation);

            this.speed += 45;
            yield return new WaitForSeconds(0.074f);

            if (GameManager.Instance.LevelDash > 1)
            {
                Collider[] colliders = Physics.OverlapSphere(this.transform.position, 3);

                foreach (var collider in colliders)
                {
                    Transform body = collider.GetComponent<Transform>();
                    if (body != null && collider.tag == "Enemy")
                    {
                        collider.GetComponent<EnemyAI>().GetDamage(3, 10, 0, true, this.transform);
                        if (GameManager.Instance.LevelDash > 2) collider.GetComponent<EnemyAI>().StunEnemy();
                    }
                }
            }

            yield return new WaitForSeconds(0.074f);
            this.speed -= 45;

            while (DashButton.fillAmount < 1)
            {
                DashButton.fillAmount += Time.deltaTime * 8;
                yield return new WaitForSeconds(0.01f);
            }

            if (DashButton.fillAmount > 1) DashButton.fillAmount = 1;
        }
    }
    public IEnumerator FireFeetCoroutine()
    {
        if (FireFeetButton.fillAmount == 1)
        {
            if (GameManager.Instance.LevelFireFeet > 1) { StartCoroutine(LendTrail());  TrailActive = true; }
            if (GameManager.Instance.LevelFireFeet > 2) { StartCoroutine(ExplosionTrail()); ExplosionActive = true; }

            FireFeetButton.fillAmount = 0;
            ParticlesFireFeetAttack.Play();
            this.speed += 8;
            yield return new WaitForSeconds(5.0f);
            ParticlesFireFeetAttack.Stop();
            this.speed -= 8;

            TrailActive = false;
            ExplosionActive = false;

            while (FireFeetButton.fillAmount < 1)
            {
                FireFeetButton.fillAmount += Time.deltaTime / 10;
                yield return new WaitForSeconds(0.01f);
            }

            if (FireFeetButton.fillAmount > 1) FireFeetButton.fillAmount = 1;
        }
    }
    public IEnumerator CircularCoroutine()
    {
        if (SpinButton.fillAmount == 1)
        {
            SpinButton.fillAmount = 0;

            if (GameManager.Instance.LevelSpinToWin == 1)
            {
                anim.SetBool("Circular", true);
                yield return new WaitForSeconds(0.4f);
                CircularAttack.activated = true;
                yield return new WaitForSeconds(0.1f);
                ParticlesCircularAttack.Play();
                yield return new WaitForSeconds(0.4f);
                CircularAttack.activated = false;
                anim.SetBool("Circular", false);
            }
            else if (GameManager.Instance.LevelSpinToWin == 2)
            {
                anim.SetBool("CircularTriple", true);
                yield return new WaitForSeconds(0.4f);
                CircularAttack.activated = true;
                yield return new WaitForSeconds(0.1f);
                ParticlesCircularAttack.Play();
                yield return new WaitForSeconds(0.4f);
                ParticlesCircularAttack.Play();
                yield return new WaitForSeconds(0.4f);
                ParticlesCircularAttack.Play();
                yield return new WaitForSeconds(0.4f);
                CircularAttack.activated = false;
                anim.SetBool("CircularTriple", false);
            }
            else if (GameManager.Instance.LevelSpinToWin == 3)
            {
                this.speed += 5;
                anim.SetBool("CircularTriple", true);
                yield return new WaitForSeconds(0.4f);
                CircularAttack.activated = true;
                yield return new WaitForSeconds(0.1f);
                ParticlesCircularAttack.Play();
                yield return new WaitForSeconds(0.4f);
                ParticlesCircularAttack.Play();
                yield return new WaitForSeconds(0.4f);
                ParticlesCircularAttack.Play();
                yield return new WaitForSeconds(0.4f);
                ParticlesCircularAttack.Play();
                yield return new WaitForSeconds(0.4f);
                CircularAttack.activated = false;
                anim.SetBool("CircularTriple", false);
                this.speed -= 5;
            }

            while (SpinButton.fillAmount < 1)
            {
                SpinButton.fillAmount += Time.deltaTime / 10;
                yield return new WaitForSeconds(0.01f);
            }

            if (SpinButton.fillAmount > 1) SpinButton.fillAmount = 1;
        }
    }
    public IEnumerator HealingFieldCoroutine()
    {
        if (HealingFieldButton.fillAmount == 1)
        {
            HealingFieldButton.fillAmount = 0;

            anim.SetBool("Venom", true);
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Venom", false);
            GameObject go = Instantiate(HealingPrefab, this.transform.position, Quaternion.Euler(90, 0, 0));
            go.transform.SetParent(this.gameObject.transform);

            while (HealingFieldButton.fillAmount < 1)
            {
                HealingFieldButton.fillAmount += Time.deltaTime / 15;
                yield return new WaitForSeconds(0.01f);
            }

            if (HealingFieldButton.fillAmount > 1) HealingFieldButton.fillAmount = 1;
        }
    }
    public IEnumerator LightningStrikeCoroutine()
    {
        if (LightningStrikeButton.fillAmount == 1)
        {
            LightningStrikeButton.fillAmount = 0;
          
            lightningBlade = true;
            anim.SetBool("Venom", true);
            yield return new WaitForSeconds(0.25f);
            ParticlesLightningStrikeAttack.Play();
            yield return new WaitForSeconds(0.35f);
            anim.SetBool("Venom", false);
            yield return new WaitForSeconds(10.0f);
            ParticlesLightningStrikeAttack.Stop();
            lightningBlade = false;

            while (LightningStrikeButton.fillAmount < 1)
            {
                LightningStrikeButton.fillAmount += Time.deltaTime / 15;
                yield return new WaitForSeconds(0.01f);
            }

            if (LightningStrikeButton.fillAmount > 1) LightningStrikeButton.fillAmount = 1;
        }
    }
    public void GetDamage(float damage, bool sparks)
    {
        if(canReceiveDamage)
        {
            Life.CurrentLife -= damage;
            canReceiveDamage = false;
            if (sparks) Instantiate(hitParticles, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            StartCoroutine(CanReceiveDamageAgain());
        }       
    }
    public IEnumerator CanReceiveDamageAgain()
    {
        for (int i = 0; i <= 5; i++)
        {
            for (int j = 0; j < content.Length; j++)
            {
                content[j].materials[0].color = new Color(0.4f, 0.1f, 0.1f, 1.0f);
            }

            yield return new WaitForSeconds(0.05f);

            for (int j = 0; j < content.Length; j++)
            {
                content[j].materials[0].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }

            yield return new WaitForSeconds(0.05f);
        }

        canReceiveDamage = true;
    }
    public IEnumerator LendTrail()
    {
        Instantiate(TrailPrefab, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);

        if (TrailActive) StartCoroutine(LendTrail());
    }
    public IEnumerator ExplosionTrail()
    {
        Instantiate(ExplosionPrefab, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.65f);

        if (ExplosionActive) StartCoroutine(ExplosionTrail());
    }
}
