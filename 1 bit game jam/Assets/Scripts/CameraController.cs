using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;
    private Vector3 currentPos;
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        //Room camera
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPos.x, currentPos.y, transform.position.z), ref velocity, speed);
    }

    public void MoveToNewRoom(Transform newRoom)
    {
        currentPos = newRoom.position;
    }
}

