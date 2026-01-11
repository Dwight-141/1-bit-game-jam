using UnityEngine;

public class GolemMovement : MonoBehaviour
{
    public Transform leftEdge;
    public Transform rightEdge;
    public Transform enemy;
    public float speed;
    private Vector3 initScale;
    private bool movingLeft;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                movingLeft = !movingLeft;
            }
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                movingLeft = !movingLeft;
            }
        }
    }

    private void MoveInDirection(int direction)
    {
        //Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * -direction, initScale.y, initScale.z);

        //Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed, enemy.position.y, enemy.position.z);
    }
}
