using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Rigidbody2D body;
    public LayerMask playerMask;
    public LayerMask swordMask;
    public PlayerHealth playerHealth;
    public Vector2 localDirection;
    public ShooterBehaviour shooterBehaviour;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        localDirection = -shooterBehaviour.direction;

    }

    private void Update()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        shooterBehaviour = FindAnyObjectByType<ShooterBehaviour>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == playerMask)
        {
            playerHealth.health--;
            Debug.Log("player hit");
            //Destroy(gameObject);
        }
        else if (collision.gameObject.layer == swordMask)
        {
            body.AddForce(localDirection * shooterBehaviour.force);
        }
        else
        {
            Debug.Log("wall hit");
            //Destroy(gameObject);
        }
    }
}
