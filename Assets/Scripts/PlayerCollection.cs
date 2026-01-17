using UnityEngine;
using UnityEngine.UI; // For handling UI

public class PlayerCollectJump : MonoBehaviour
{
    public int score = 0;               // Current score
    public float jumpIncrease = 5f;     // Amount to increase jump force per collectible
    public Text scoreText;              // UI Text for score display (top-left corner)
    public float jumpForce = 300f;      // Initial jump force

    private Rigidbody2D rb2d;
    private bool isJumping = false;     // Flag to check if player is in the air

    void Start()
    {
        // Get the Rigidbody2D component
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D is missing on the player!");
        }

        // Initialize score display safely
        UpdateScoreUI();
    }

    void Update()
    {
        // Jump input
        if (Input.GetButton("Jump") && !isJumping)
        {
            if (rb2d != null)
            {
                rb2d.AddForce(Vector2.up * jumpForce);
                isJumping = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check collision with objects that are not in the NoCollis layer
        if (collision.gameObject.layer != LayerMask.NameToLayer("NoCollis"))
        {
            isJumping = false; // Player can jump again

            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0f); // Reset vertical velocity
        }
        // If we collide with a platform, parent the player to it
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(collision.transform);
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        // When leaving a platform, unparent the player
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Use Invoke to avoid Unity error when deactivating/parenting in the same frame
            Invoke(nameof(DetachFromPlatform), 0.01f);
        }
    }

    private void DetachFromPlatform()
    {
        transform.SetParent(null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is a collectible
        if (other != null && other.CompareTag("Collectible"))
        {
            score += 10;                        // Increase score by 10
            jumpForce += jumpIncrease;          // Increase jump force
            UpdateScoreUI();                    // Update the UI

            Destroy(other.gameObject);          // Remove the collected item
            Debug.Log("Collected a star! Current jump force: " + jumpForce);
        }

    }

    // Update the score text on UI safely
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

}
