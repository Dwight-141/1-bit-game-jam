using UnityEngine;

public class RhinoBehaviour : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;
    public bool attack;
    public bool stunned;
    //private bool running;
    private float move = -1f;
    private float stunTimer = 0;

    public float distance = 5f;
    public float speed = 10f;
    public LayerMask playerMask;
    public float stunCooldown = 1f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //attack = false;
        MoveDirection();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * -1, distance, playerMask);
        Debug.Log(transform.right);


        if (hit.collider != null && hit.collider.CompareTag("Player") && Time.time >= stunTimer)
        {
            //Debug.Log("hit");
            stunned = false;
            attack = true;
        }



        animator.SetBool("isAttacking", attack);
        animator.SetBool("isStunned", stunned);
    }

    private void FixedUpdate()
    {
        if (attack && !stunned)
        {
            body.linearVelocity = new Vector2(move * speed, body.linearVelocity.y);
            //Debug.Log("move");
        }
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
            stunTimer = Time.time + stunCooldown;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * distance * -1);
    }
}
