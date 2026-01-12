using TMPro;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public Transform previousRoom;
    public Transform nextRoom;
    public TextMeshProUGUI lives;
    public CameraController cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Vector2 delta = collision.transform.position - transform.position;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x < 0)
                {
                    cam.MoveToNewRoom(nextRoom);
                }
                else
                {
                    cam.MoveToNewRoom(previousRoom);
                }
            }
            else
            {
                if (delta.y < 0)
                {
                    cam.MoveToNewRoom(previousRoom);
                }
                else
                {
                    if (nextRoom.gameObject.name == "Room 7")
                    {
                        lives.gameObject.SetActive(true);
                    }
                    cam.MoveToNewRoom(nextRoom);

                }
            }
        }
    }
}
