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

