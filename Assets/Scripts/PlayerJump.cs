// using UnityEngine;

// public class PlayerJump : MonoBehaviour
// {
//     public float jumpForce = 300f;

//     private Rigidbody2D rb2d;
//     private bool isJumping = false;

//     void Start()
//     {
//         rb2d = GetComponent<Rigidbody2D>();
//     }

//     void Update()
//     {
//         if (Input.GetButton("Jump") && !isJumping)
//         {
//             // Saltar
//             rb2d.AddForce(Vector2.up * jumpForce);

//             // Desactivar la bandera
//             isJumping = true;
//         }
//     }

//     private void OnCollisionEnter2D(Collision2D other)
//     {
//         // Si el jugador colisiona con un objeto con la etiqueta suelo
//         if (other.gameObject.CompareTag("Ground"))
//         {
//             // Activar la bandera para que vuelva a saltar
//             isJumping = false;

//             // Anular la velocidad remanente que tuviera
//             rb2d.linearVelocity = new Vector2(rb2d.linearVelocityX, 0f);
//         }
//     }
// }
