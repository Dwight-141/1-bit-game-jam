using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Rigidbody2D body;
    public PlayerHealth playerHealth;
    public Vector2 localDirection;
    public ShooterBehaviour shooterBehaviour;
    public GameObject sword;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        

    }

    private void Update()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        shooterBehaviour = FindAnyObjectByType<ShooterBehaviour>();
        sword = GameObject.Find("Sword");
        localDirection = -shooterBehaviour.direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(sword.transform.position.x);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !sword.GetComponent<BoxCollider2D>().isActiveAndEnabled)
        {
            playerHealth.health--;
            Debug.Log("player hit");
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && sword.GetComponent<BoxCollider2D>().isActiveAndEnabled && sword.transform.position.x >= 0.4)
        {
            body.AddForce(localDirection * shooterBehaviour.force);
        }
        else
        {
            Debug.Log("wall hit");
            Destroy(gameObject);
        }

        //
    }
}
