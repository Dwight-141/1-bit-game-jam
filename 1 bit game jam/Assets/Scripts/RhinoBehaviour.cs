using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class RhinoBehaviour : MonoBehaviour
{
    //private Rigidbody2D rhinoBody;
    private Animator animator;
    public bool attack;
    public bool stunned;
    public bool running;
    private float move = -1f;
    private float stunTimer = 0;

    public float distance = 5f;
    public float speed = 10f;
    public LayerMask playerMask;
    public float stunCooldown = 1f;

    private void Awake()
    {
        //rhinoBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //attack = false;
        MoveDirection();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, distance, playerMask);

        if (Time.time >= stunTimer)
        {
            stunned = false;
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                //Debug.Log("hit");

                attack = true;
            }
        }




        animator.SetBool("isAttacking", attack);
        animator.SetBool("isStunned", stunned);
    }

    private void FixedUpdate()
    {
        if (attack && !stunned && running)
        {
            transform.position = new Vector3(transform.position.x + Time.deltaTime * -transform.localScale.x * speed, transform.position.y, transform.position.z);
            //rhinoBody.linearVelocity = new Vector2(move * speed, rhinoBody.linearVelocity.y);
            //Debug.Log("move");
        }
    }

    public void StartRun()
    {
        running = true;
    }

    public float MoveDirection()
    {
        // looking left
        if (transform.localScale.x > 0f)
        {
            move = -1f;
        }
        // looking right
        else if (transform.localScale.x < 0f)
        {
            move = 1f;
        }
        return move;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            // looking left
            if (transform.localScale.x > 0f)
            {
                transform.localScale = new Vector3(-1.5f, 1.5f, 1);
            }
            // looking right
            else if (transform.localScale.x < 0f)
            {
                transform.localScale = new Vector3(1.5f, 1.5f, 1);
            }
            stunned = true;
            attack = false;
            running = false;
            stunTimer = Time.time + stunCooldown;
        }

        if (collision.gameObject.tag == "Player")
        {
            //gameObject.SetActive(false);
            Debug.Log("move");
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.left * distance * transform.localScale.x);
    }
}
