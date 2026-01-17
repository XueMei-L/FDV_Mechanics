using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 300f;

    private Rigidbody2D rb2d;
    private bool isJumping = false;
    private Transform currentPlatform;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButton("Jump") && !isJumping)
        {
            rb2d.AddForce(Vector2.up * jumpForce);
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0f);
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            isJumping = false;
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0f);

            currentPlatform = collision.transform;
            transform.SetParent(currentPlatform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // plataforma invisible
        if(collision.gameObject.layer == LayerMask.NameToLayer("PlatInv"))
        {
            Debug.Log("Jugador salió de la plataforma invisible");
            Invoke(nameof(DetachFromPlatform), 0.01f);
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }

        if(collision.gameObject.layer!=LayerMask.NameToLayer("NoCollis")){
        //Lógica para los elementos que si colisionan.
            if (collision.gameObject.CompareTag("Platform"))
            {
                // No hacemos SetParent aquí directamente，porque da error, hacemos un frame después
                Invoke(nameof(DetachFromPlatform), 0.01f);
            }
        }
    }

    private void DetachFromPlatform()
    {
        transform.SetParent(null);
        currentPlatform = null;
    }
}
