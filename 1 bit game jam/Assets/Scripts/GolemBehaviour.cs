using UnityEngine;

public class GolemBehaviour : MonoBehaviour
{
    public float attackCooldown;
    public float range;
    public float colliderDistance;
    public BoxCollider2D boxCollider;
    public LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    public Animator animator;
    public PlayerHealth playerHealth;
    public GameObject sword;
    public GolemMovement golemMovement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        golemMovement = GetComponentInParent<GolemMovement>();
    }

    private void Update()
    {
        sword = GameObject.Find("Sword");
        cooldownTimer += Time.deltaTime;
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                animator.SetTrigger("attack");
            }
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
            Destroy(golemMovement.gameObject);
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
        }
    }
}
