using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    public GameObject Damage;
    private GameObject Guts;
    private Rigidbody Rigid;
    private PlayerScript PlayerScript;
    private Animator Anim;

    private float attackTimer;
    private int waitingTime;

    //Flags
    public bool isCrawling = true;
    public bool isFlying = false;
    public bool isBoosting = false;
    public bool isLanding = false;
    private bool isAttacking = false;
    private bool isSmashed = false;

    // Start is called before the first frame update
    void Start()
    {
        Anim = this.GetComponent<Animator>();
        Rigid = gameObject.GetComponent<Rigidbody>();
        PlayerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        Guts = this.transform.GetChild(5).gameObject;
        Guts.SetActive(false);
        attackTimer = 0.0f;
        waitingTime = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (isSmashed)
        {
            Smashed();
        }
        else if (isLanding)
        {
            Land();
        }
        else if (isAttacking)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (isCrawling)
        {
            Crawl();
        }
        else if (isFlying)
        {
            Fly();
        }
        else if (isBoosting)
        {
            Boost();
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("frog touched");
        isSmashed = true;
    }

    private void Crawl()
    {
        transform.Translate(0, 0, 0.005f);
    }
    private void Fly()
    {
        Anim.SetBool("Fly", true);
        transform.Translate(0, 0.02f, 0.008f);
    }
    private void Boost()
    {
        Anim.SetBool("Boost", true);
        transform.Translate(0, 0, 0.05f);
    }
    private void Land()
    {
        Anim.SetBool("Land", true);
        transform.position = transform.position;
        isAttacking = true;
        isLanding = false;
        
    }
    private void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > waitingTime)
        {
            Debug.Log("attack!");
            Anim.SetTrigger("Attack");
            StartCoroutine("RedFlick");
            attackTimer = 0;
        }
    }
    IEnumerator RedFlick()
    {
        yield return new WaitForSeconds(0.3f);
        Damage.SetActive(true);
        PlayerScript.hp -= 20;
        yield return new WaitForSeconds(0.1f);
        Damage.SetActive(false);
    }

    private void Smashed()
    {
        isCrawling = false;
        isFlying = false;
        isBoosting = false;
        isLanding = false;
        isAttacking = false;
        Anim.SetBool("Smashed", true);
        Guts.SetActive(true);
        Rigid.constraints = RigidbodyConstraints.FreezeRotation;
        Rigid.useGravity = true;
    }

}
