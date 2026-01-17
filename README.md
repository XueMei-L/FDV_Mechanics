# FDV_2D_Mechanics

```
>> PRACTICA:   Unity Project - FDV_2D_Mechanics
>> COMPONENTE: XueMei Lin
>> GITHUB:     https://github.com/XueMei-L/FDV_2D_Mechanics.git
>> Versión:    1.0.0
```

# Objetivo
## En este proyecto, reutilizo el proyecto Sprite2D para practicar mecánicas que implican el uso del motor de físicas

## Salto
Esta mecánica se consigue con objetos físicos simplemente activando una fuerza en el eje Y positivo que lo empuje hacia arriba cuando se pulse una tecla a la que le damos esa función. Es necesario controlar que el jugador sólo salte si está en el suelo, puesto que si no, saldrá volando.

### 1. Recupera el Rigidbody2D del objeto y aplica una fuerza vertical al objeto cuando se pulse la tecla que hayas configurado para el salto en el Input.Manager:
1. Añadir al jugador **Rigibody2D** y **Collider2D**
2. configurar el salto en el Input Manager, por defecto tiene la siguiente configuración
![alt text](image.png)

3. Crear el script de salto
```
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 300f;

    private Rigidbody2D rb2d;
    private bool isJumping = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButton("Jump") && !isJumping)
        {
            // Saltar
            rb2d.AddForce(Vector2.up * jumpForce);

            // Desactivar la bandera
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Si el jugador colisiona con un objeto con la etiqueta suelo
        if (other.gameObject.CompareTag("Ground"))
        {
            // Activar la bandera para que vuelva a saltar
            isJumping = false;

            // Anular la velocidad remanente que tuviera
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocityX, 0f);
        }
    }
}
```
4. Configurar que el "suelo" hay una etiqueta **suelo**
![alt text](image-1.png)

Resultado:
![alt text](Unity_RzKnov9Hug.gif)


## Mécánicas relacionadas con plataformas
1. Crear una plataforma movida con la etiqueta **platform**
2. Crear un script para que el player cuando está encima de la plataforma activa como hijo de la plataforma, y que se mueve con la plataforma. Cuando no está encima, desactiva.
```
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

    // aqui porque hubo un problema con setparent
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // No hacemos SetParent aquí directamente， porque da error, hacemos un frame después
            Invoke(nameof(DetachFromPlatform), 0.01f);
        }
    }

    private void DetachFromPlatform()
    {
        transform.SetParent(null);
        currentPlatform = null;
    }
}
```

3. Crear un script para que la plataforma se mueve horizontal y vertical automaticamente
   
```
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // mover plataforma automaticamente
        float movement = Mathf.PingPong(Time.time * speed, distance);
        transform.position = startPos + Vector3.right * movement;
    }
}
```

Resultado:
![alt text](Unity_3eqEE4b69s.gif)


## Manejar colisiones con elementos de una capa determinada
Podemos utilizar las capas para que el efecto de las colisiones sólo se tenga en cuenta cuando se pertenece a una determinada capa, o se descarten.

1. Podemos modificar el codigo para que comprobar primero que si está en la capa NoCollis, que no hace nada y luego verificar que si el objeto es una plataforma(con etiqueta **platform**), finalmente, ejecuta la lógica para separarse de la plataforma.
```
private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.layer!=LayerMask.NameToLayer("NoCollis")){
        //Lógica para los elementos que si colisionan.
            if (collision.gameObject.CompareTag("Platform"))
            {
                // No hacemos SetParent aquí directamente，porque da error, hacemos un frame después
                Invoke(nameof(DetachFromPlatform), 0.01f);
            }
        }
    }
```

## Plataformas invisibles que se vuelven visibles
Podemos hacer una gestión similar al caso anterior, sólo que en este caso nos interesa manejar los casos en el que se pertenece a la capa de plataformas invisibles. Una vez detectado el caso, se ejecutará la lógica que queramos llevar a cabo, incluido, si así lo consideramos volver la plataforma visible.

1. Configurar una plataforma invisible
![alt text](image-2.png)

2. Añadir el siguiente código en la funcion **OnCollisionExit2D**
```
// plataforma invisible
if(collision.gameObject.layer == LayerMask.NameToLayer("PlatInv"))
{
    Debug.Log("Jugador salió de la plataforma invisible");
    Invoke(nameof(DetachFromPlatform), 0.01f);
    collision.gameObject.GetComponent<SpriteRenderer>().enabled = true;
}
```

Resultado:
![alt text](Unity_e28cZwToaR.gif)

## Mecánica de recolección
La recolección de objetos se realiza implementando la lógica de la recolección en alguno de los eventos de colisión con ese objeto. En ocasiones se actualizará la UI, en otras se destruirá el objeto, en otras se mejora alguna característica del personaje. En esta tarea se debe implementar una mecánica en la que el jugador recolecte objetos que le supongan alguna recompensa. En concreto, aumentará su puntuación, al llega a un valor de incremento de puntuación, adquirirá más potencia de salto. La puntuación se mostrará continuamente en la pantalla. El objeto recolectado ya no se usará más.
1. Añadir las estrellas con etiqueta **Collectible**
2. Añadir **Collider2D** a cada estrella, activando la opción **Is Trigger**
3. Crear un UI de texto para acumular los puntos
4. Crear el script para el usuario, cuando el usuario choca con la estrella, suma la puntuación y fuerza su salto
(Incluyendo algunos apartados anteriores)

*PlayerCollection.cs*
```
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
```

Resultado:
![alt text](Unity_z8MTxeaC3u.gif)
