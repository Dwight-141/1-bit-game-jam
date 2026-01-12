using TMPro;
using UnityEngine;


public class BossBehaviour : MonoBehaviour
{
    public float attackCooldown;
    public float range;
    public float colliderDistance;
    public BoxCollider2D boxCollider;
    public LayerMask playerLayer;
    [SerializeField] private float cooldownTimer;
    public Animator animator;
    public PlayerHealth playerHealth;
    public GameObject sword;
    public float knockback;
    public bool wasHit;

    //spinning variables 
    public bool spinning;
    public float knockbackStartDistance;
    public float spinningSpeed;

    //throwing variables
    public GameObject bullet;
    public Transform startPoint;
    public Vector2 direction;
    public float force;

    //charge variables
    public bool charging;
    public bool stunned;
    public bool running;
    public float chargeSpeed;

    //health
    public float health;
    public TextMeshProUGUI lives;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sword = GameObject.Find("Sword");
    }

    private void Update()
    {
        Vector2 playerPos = playerHealth.gameObject.transform.position;
        direction = playerPos - (Vector2)transform.position;
        cooldownTimer += Time.deltaTime;
        //FacePlayer();

        if (cooldownTimer >= attackCooldown)
        {
            stunned = false;
            float attackPattern = Random.Range(1, 4);
            if (attackPattern == 1 && !stunned)
            {
                animator.SetTrigger("isSpinning");
                attackCooldown = 8;
            }
            else if (attackPattern == 2 && !stunned)
            {
                FacePlayer();
                animator.SetTrigger("isThrowing");
                attackCooldown = 4;
            }
            else if (attackPattern == 3 && !stunned)
            {
                FacePlayer();
                charging = true;
                attackCooldown = 10;
            }
            cooldownTimer = 0;
        }

        if (spinning)
        {
            DamagePlayer();
        }

        animator.SetBool("isCharging", charging);
        animator.SetBool("isStunned", stunned);
        lives.text = "Boss: " + health.ToString();
    }

    private void FixedUpdate()
    {
        if (charging && !stunned && running)
        {
            transform.position = new Vector3(transform.position.x + Time.deltaTime * -transform.localScale.x * chargeSpeed, transform.position.y, transform.position.z);
        }

        if (spinning)
        {
            transform.position = new Vector3(transform.position.x + Time.deltaTime * -transform.localScale.x * spinningSpeed, transform.position.y, transform.position.z);
        }
    }

    public void StartRun()
    {
        running = true;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("hit wall");
            // looking left
            if (transform.localScale.x > 0f)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                Debug.Log("looking right");
            }
            // looking right
            else if (transform.localScale.x < 0f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                Debug.Log("looking left");
            }
            stunned = true;
            charging = false;
            running = false;
        }
        if (collision.gameObject.tag == "Player" && sword.GetComponent<BoxCollider2D>().isActiveAndEnabled)
        {
            if (health > 1)
            {
                health--;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.tag == "Player" && charging)
        {
            playerHealth.health--;
            //stunned = true;
            //attack = false;
            //running = false;
            //stunTimer = Time.time + stunCooldown;
            //playerMovement.move = move;
            collision.rigidbody.AddForce(new Vector2(0, knockback));
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public void DamagePlayer()
    {
        if (PlayerInSight() && wasHit == false)
        {
            playerHealth.health--;
            if (IsLeft())
            {
                //playerHealth.gameObject.GetComponent<Transform>().position = new Vector3(playerHealth.gameObject.GetComponent<Transform>().position.x - knockbackStartDistance, playerHealth.gameObject.GetComponent<Transform>().position.y + knockbackStartDistance, playerHealth.gameObject.GetComponent<Transform>().position.z);
                playerHealth.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-knockback, knockback / 2));
                Debug.Log("got here left");
                wasHit = true;
            }
            else
            {
                //playerHealth.gameObject.GetComponent<Transform>().position = new Vector3(playerHealth.gameObject.GetComponent<Transform>().position.x + knockbackStartDistance, playerHealth.gameObject.GetComponent<Transform>().position.y + knockbackStartDistance, playerHealth.gameObject.GetComponent<Transform>().position.z);
                playerHealth.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback, knockback / 2));
                Debug.Log("got here right");
                wasHit = true;
            }

        }
    }

    public bool IsLeft()
    {
        if (playerHealth.gameObject.GetComponent<Transform>().position.x <= transform.position.x)
        {
            return true;
        }
        return false;
    }

    public void SpinningOn()
    {
        spinning = true;
    }

    public void SpinningOff()
    {
        spinning = false;
        wasHit = false;
    }

    public void Shoot()
    {
        GameObject bulletIns = Instantiate(bullet, startPoint.position, Quaternion.identity);
        bulletIns.GetComponent<Rigidbody2D>().AddForce(direction * force);
    }

    public void FacePlayer()
    {
        if (playerHealth.gameObject.GetComponent<Transform>().position.x < transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            Debug.Log("face player left");
        }
        else if (playerHealth.gameObject.GetComponent<Transform>().position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            Debug.Log("face player right");
        }
    }

    /*
    private void FixedUpdate()
    {
        if (attack && !stunned && running)
        {
            transform.position = new Vector3(transform.position.x + Time.deltaTime * -transform.localScale.x * speed, transform.position.y, transform.position.z);
        }
    }

    public void StartRun()
    {
        running = true;
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

        if (collision.gameObject.tag == "Player" && collision.rigidbody.gravityScale >= 3)
        {
            //Debug.Log("move");
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Player" && collision.rigidbody.gravityScale <= 3)
        {
            playerHealth.health--;
            //stunned = true;
            //attack = false;
            //running = false;
            //stunTimer = Time.time + stunCooldown;
            //playerMovement.move = move;
            collision.rigidbody.AddForce(new Vector2(0,knockback));
        }
    }*/
}
