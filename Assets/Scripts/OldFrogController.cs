using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldFrogController : MonoBehaviour
{
    private GameManager GameManager;
    private Rigidbody Rigid;
    private Animator Anim;   

    //Flags
    public bool isSwimming = true;
    public bool isJumping = false;
    public bool isAttacking = false;
    public bool isSmashed = false;

    //Jump
    public Vector3 jumpPos; //Jump starting position
    public Vector3 landPos; //Jump landing position    
    public float reduceHeight = 1f; //center value of the jump parabola (higher the lower)
    public float journeyTime = 2f; //Duration from jumpPos to landPos (higher the slower)
    public float startTime;

    //Tongue
    bool firstAttack = true;
    float tongueTimer;
    int waitingTime;
    public GameObject Damage;

    //click
    private RaycastHit hit;
    public GameObject guts;

    private void Awake()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Rigid = gameObject.GetComponent<Rigidbody>();

    }

    void Start()
    {
        Anim = this.GetComponent<Animator>();

        tongueTimer = 0.0f;
        waitingTime = 2;
    }
    private void FixedUpdate()
    {
        if (isSwimming)
        {
            transform.Translate(0, 0, 0.01f);
        }
    }

    void Update()
    {
        
        if (!isSwimming && !isSmashed)
        {
            if (isJumping)
            {
                Debug.Log("jump");
                Jump();
            }

            else if (!isJumping)
            {
                Debug.Log("attack");
                transform.position = landPos;
                Tongue();

            }
            /*
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("click!");
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("HIT : "+ hit.collider.gameObject.name);
                    Debug.Log("HIT : " + hit.rigidbody.gameObject.name);
                    if (!hit.collider && hit.collider.gameObject.name == "Frog")
                    {
                        Debug.Log("hit! - Collider");
                        isSmashed = true;
                        Smashed();
                    }
                    else if (!hit.rigidbody && hit.rigidbody.gameObject.name == "Frog")
                    {
                        Debug.Log("hit! - Rigid");
                        isSmashed = true;
                        Smashed();
                    }
                }
            }*/
            

        }

    }

    private void OnMouseDown()
    {
        Debug.Log("touched me!");
    }

    public void Jump()
    {
        Anim.SetBool("Jump", true);
        Vector3 center = (jumpPos + landPos) * 0.5F; //gose up by center value
        center -= new Vector3(0, 1f * reduceHeight, 0); //higher y the lower
        Vector3 jumpCenter = jumpPos - center;
        Vector3 landCenter = landPos - center;
        float fracComplete = (Time.time - startTime) / journeyTime;
        transform.position = Vector3.Slerp(jumpCenter, landCenter, fracComplete);
        transform.position += center;

    }

    public void Tongue()
    {
        if (firstAttack)
        {
            Anim.SetBool("Idle", true);
            firstAttack = false;
        }
        tongueTimer += Time.deltaTime;
        if (tongueTimer > waitingTime)
        {
            Anim.SetTrigger("Tongue");
            StartCoroutine("RedFlick");
            tongueTimer = 0;
        }
    }

    IEnumerator RedFlick()
    {
        yield return new WaitForSeconds(0.3f);
        Damage.SetActive(true);
        GameManager.hp -= 20;
        
        yield return new WaitForSeconds(0.1f);
        Damage.SetActive(false);
    }

    public void Smashed()
    {
        Rigid.constraints = RigidbodyConstraints.FreezePosition;
        Anim.SetBool("Smashed", true);
        //Instantiate(guts, transform.position, transform.rotation);
        //Invoke("SpreadGuts", 0.1f);
    }
}
