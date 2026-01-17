using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public float speed = 10;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer component
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        // float moveVertical = Input.GetAxisRaw("Vertical");
        
        Vector2 movement = new Vector2(horizontalInput, 0f);
        
        // Vector3 movement = new Vector3(horizontalInput, moveVertical, 0.0f).normalized;
        transform.Translate(movement * speed * Time.deltaTime);

        // Flip the sprite based on horizontal direction
        if (horizontalInput < 0) // Moving left
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput  > 0) // Moving right
        {
            spriteRenderer.flipX = true;
        }
    }
}