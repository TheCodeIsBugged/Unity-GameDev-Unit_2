using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerControllerX : MonoBehaviour
{
    Rigidbody rb;
    float speed = 10f;
    float movementX;
    float movementY;

    [SerializeField]
    Light spotLight;
    [SerializeField]
    ParticleSystem attackedVFX, pickupVFX, keyVFX, fogVFX;
    [SerializeField]
    TextMeshProUGUI candleText, keyText, gameoverText, winText;
    [SerializeField]
    GameObject gameoverUI;
    [SerializeField]
    Animator gateAnimator, gameoverAnimator, hudAnimator;
    Animator enemyAnimator;

    int health = 3;
    int candleQuantitiy = 3;
    int candleCollected = 0;
    int keyQuantity = 1;
    int keyCollected = 0;
    bool hasKey = false;
    bool isGameover = false;
    public bool IsGameover 
    {
        get { return isGameover; } 
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();        

        // Play the fog visualFX
        fogVFX.Play();

        // Set the health bar to the max amount of the healt player
        HealthBar.Instance.SetMaxHealth(health);

        // Set the initial candle text and key text
        candleText.text = $"{candleCollected}/{candleQuantitiy}";
        keyText.text = $"{keyCollected}/{keyQuantity}";
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameover && Input.GetKeyDown(KeyCode.Space))
        {
            GameManagerX.Instance.RestartGame();
        }
    }

    private void FixedUpdate()
    {
        // Direction based on the input of the player from PlayerInput component
        Vector3 movementDirection = new Vector3(movementX, 0f, movementY);
        rb.AddForce(movementDirection * speed);
    }

    // This function is called when an input is detected by the InputAction
    public void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            // Increase the amount of candles currently collected
            candleCollected++;

            // Set and update the text UI for the candle
            candleText.text = $"{candleCollected}/{candleQuantitiy}";

            // Insntantiate a spark visualFX
            Instantiate(pickupVFX, other.transform.position, pickupVFX.transform.rotation);

            // Increase the range and intensity of the spot light illuminating the player
            spotLight.intensity *= 2f;
            spotLight.spotAngle += 30f;

            // Play soundFX from the AudioManager Instance
            AudioManager.Instance.PlaySFX("PickupSFX");

            // Destroy the pick up after being collected  
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Key"))
        {
            // Event flag if the key has been collected
            hasKey = true;
            keyCollected++;

            // // Set and update the text UI for the key
            keyText.text = $"{keyCollected}/{keyQuantity}";

            // Play soundFX from the AudioManager Instance
            AudioManager.Instance.PlaySFX("KeySFX");

            // Instantiate a visualFX, must be a child gameobject of the player
            Instantiate(keyVFX, transform.position, keyVFX.transform.rotation, this.transform);

            // Destroy the key after being collected.
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Play SoundFX from the AudioManager Instance
            AudioManager.Instance.PlaySFX("BiteSFX");

            // Play attack animation
            enemyAnimator = collision.gameObject.GetComponentInChildren<Animator>();
            enemyAnimator.SetTrigger("isAttacking");

            // Instantiate a visualFX, must be a child gameobject of the player
            Instantiate(attackedVFX, transform.position, attackedVFX.transform.rotation, this.transform);

            // Reduce the health of the player every contact with the enemy
            health--;

            // Disable the hud animator
            hudAnimator.enabled = false;

            // Update the healt bar based on the current healt of the player
            HealthBar.Instance.SetHealth(health);

            // Decrease the player's speed based on the health amount left
            switch (health)
            {
                case 1: speed = 4f; 
                    break;
                case 2: speed = 7f;
                    break;
                case 3: speed = 10f;
                    break;
                default: speed = 0f; 
                    break;

            }

            // Display game over when health is less than or equal to zero
            if (health <= 0)
            {
                isGameover = true;

                // Play the lose sfx from the audio manager
                AudioManager.Instance.PlaySFX("LoseSFX");

                // Display the gameover text and hide the win text
                gameoverUI.SetActive(true);
                winText.enabled = false;
                gameoverText.enabled = true;
                gameoverAnimator.SetTrigger("hasWon");

                // Destroy the player
                gameObject.SetActive(false);
            }
        }
        
        // The win condition of the game
        if (collision.gameObject.CompareTag("Gate") && hasKey)
        {
            isGameover = true;

            // Play the gate opening animation
            // gateAnimator = collision.gameObject.GetComponent<Animator>();
            gateAnimator.SetTrigger("hasKey");

            // Play the win SFX from the audio manager
            AudioManager.Instance.PlaySFX("WinSFX");

            // Display the win text and hide the gameover text
            gameoverUI.SetActive(true);
            winText.enabled = true;
            gameoverText.enabled = false;
            gameoverAnimator.SetTrigger("hasWon");
        }
    }
}
