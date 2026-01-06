using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;
    private float move = 0.0f;
    private float gravity;
    private bool grounded;
    private bool attacked;
    private bool slammed;

    public float speed = 10f;
    public float jump = 10f;
    public float slamGrav = 5f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gravity = body.gravityScale;
    }

    private void Update()
    {
        //float horizontal = 0.0f;
        //float vertical = 0.0f;
        bool horizontal = HorizontalMovement();
        attacked = false;
        slammed = false;


        if (Keyboard.current.spaceKey.IsPressed() && grounded)
        {
            Jump();
        }

        if (Mouse.current.leftButton.IsPressed() && grounded)
        {
            Attack();
        }

        if (Mouse.current.leftButton.IsPressed() && !grounded)
        {
            Slam();
        }

        body.linearVelocity = new Vector2(move * speed, body.linearVelocity.y);
        animator.SetBool("isRunning", horizontal);
        animator.SetBool("isGrounded", grounded);
        animator.SetBool("isAttacking", attacked);
        animator.SetBool("isSlamming", slammed);
    }

    public bool HorizontalMovement()
    {
        move = 0.0f;
        if (Keyboard.current.aKey.IsPressed())
        {
            move = -1f;
            transform.localScale = new Vector3(-1, 1, 1);
            return true;
        }
        else if (Keyboard.current.dKey.IsPressed())
        {
            move = 1f;
            transform.localScale = Vector3.one;
            return true;
        }
        return false;
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jump);
        grounded = false;
    }

    private void Attack()
    {
        attacked = true;
    }

    private void Slam()
    {
        slammed = true;
        body.gravityScale = slamGrav;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            body.gravityScale = gravity;
        }
    }
}
