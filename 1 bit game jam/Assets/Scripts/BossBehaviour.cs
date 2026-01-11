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

    //spinning variables 
    public bool spinning;
    public float knockbackStartDistance;

    //throwing variables
    public GameObject bullet;
    public Transform startPoint;
    public Vector2 direction;
    public float force;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float attackPattern = Random.Range(1, 3);
        sword = GameObject.Find("Sword");
        Vector2 playerPos = playerHealth.gameObject.transform.position;
        direction = playerPos - (Vector2)transform.position;
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
        {
            if (attackPattern == 1)
            {
                animator.SetTrigger("isSpinning");
            }
            else if (attackPattern == 2)
            {
                animator.SetTrigger("isThrowing");
            }
            cooldownTimer = 0;
        }

        if (spinning)
        {
            DamagePlayer();
        }
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
        if (collision.gameObject.tag == "Player" && sword.GetComponent<BoxCollider2D>().isActiveAndEnabled)
        {
            Destroy(gameObject);
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
        if (PlayerInSight())
        {
            playerHealth.health--;
            if (IsLeft())
            {
                playerHealth.gameObject.GetComponent<Transform>().position = new Vector3(playerHealth.gameObject.GetComponent<Transform>().position.x - knockbackStartDistance, playerHealth.gameObject.GetComponent<Transform>().position.y + knockbackStartDistance, playerHealth.gameObject.GetComponent<Transform>().position.z);
                //playerHealth.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-knockback, knockback / 2));
                Debug.Log("got here left");
            }
            else
            {
                playerHealth.gameObject.GetComponent<Transform>().position = new Vector3(playerHealth.gameObject.GetComponent<Transform>().position.x + knockbackStartDistance, playerHealth.gameObject.GetComponent<Transform>().position.y + knockbackStartDistance, playerHealth.gameObject.GetComponent<Transform>().position.z);
                //playerHealth.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback, knockback / 2));
                Debug.Log("got here right");
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
    }

    public void Shoot()
    {
        GameObject bulletIns = Instantiate(bullet, startPoint.position, Quaternion.identity);
        bulletIns.GetComponent<Rigidbody2D>().AddForce(direction * force);
    }
}
