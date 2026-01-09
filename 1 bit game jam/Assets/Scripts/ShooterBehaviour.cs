using Unity.VisualScripting;
using UnityEngine;

public class ShooterBehaviour : MonoBehaviour
{
    public Transform player;
    public GameObject head;
    public Rigidbody2D body;
    public GameObject bullet;
    public Transform startPoint;
    public Vector2 direction;
    public float shotCooldown;
    private float nextShotTimer = 0;
    public float force;


    private void Awake()
    {
        //body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 playerPos = player.position;
        direction = playerPos - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        body.rotation = -angle;

        if(head.transform.rotation.z >= 0)
        {
            //Debug.Log("left side");
            head.transform.localScale = Vector3.one  ;
        }
        else if(head.transform.rotation.z < 0)
        {
            //Debug.Log("right side");
            head.transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Time.time > nextShotTimer)
        {
            nextShotTimer = Time.time + shotCooldown;
            Shoot();
        }
        
    }

    public void Shoot()
    {
        GameObject bulletIns = Instantiate(bullet, startPoint.position, Quaternion.identity);
        bulletIns.GetComponent<Rigidbody2D>().AddForce(direction * force);
    }

}
