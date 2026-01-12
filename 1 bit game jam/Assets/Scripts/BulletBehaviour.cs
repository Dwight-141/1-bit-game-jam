using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Rigidbody2D body;
    public PlayerHealth playerHealth;
    public Vector2 localDirection;
    public Vector2 bossLocalDirection;
    public ShooterBehaviour shooterBehaviour;
    public BossBehaviour bossBehaviour;
    public GameObject sword;
    public GameObject player;
    public bool swordHit;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        shooterBehaviour = FindAnyObjectByType<ShooterBehaviour>();
        bossBehaviour = FindAnyObjectByType<BossBehaviour>();
        sword = GameObject.Find("Sword");
        player = GameObject.Find("Player");
        if (shooterBehaviour != null)
        {
            localDirection = -shooterBehaviour.direction;
        }
        if (bossBehaviour != null)
        {
            bossLocalDirection = -bossBehaviour.direction;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionCheck();
        //Debug.Log(sword.transform.position.x);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !sword.GetComponent<BoxCollider2D>().isActiveAndEnabled)
        {
            playerHealth.health--;
            //Debug.Log("player hit");
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && sword.GetComponent<BoxCollider2D>().isActiveAndEnabled && swordHit == true)
        {
            if (shooterBehaviour != null)
            {
                body.AddForce(localDirection * shooterBehaviour.force);
            }
            else
            {
                body.AddForce(localDirection * bossBehaviour.force);
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Shooter"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else
        {
            //Debug.Log("wall hit");
            Destroy(gameObject);
        }

        //
    }

    public void CollisionCheck()
    {
        if (shooterBehaviour.isLeftSide == true)
        {
            //looking left
            if (player.transform.localScale.x > 0)
            {
                swordHit = true;
            }
            else
            {
                swordHit = false;
            }
        }
        else if (shooterBehaviour.isLeftSide == false)
        {
            //looking right
            if (player.transform.localScale.x < 0)
            {
                swordHit = true;
            }
            else
            {
                swordHit = false;
            }
        }
    }
}
