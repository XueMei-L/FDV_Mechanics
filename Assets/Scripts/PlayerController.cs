using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        // get horizontal input
        float horizontalInput  = Input.GetAxisRaw("Horizontal");

        // Create a movement vector
        Vector2 movement = new Vector2(horizontalInput, 0f);

        // Move the object frame rate independently
        transform.Translate(movement * speed * Time.deltaTime);
    }
}