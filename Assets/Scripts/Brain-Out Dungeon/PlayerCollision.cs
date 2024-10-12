using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerCollision : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField]
    Light spotLight;
    [SerializeField]
    ParticleSystem attackedVFX, pickupVFX, keyVFX;
    [SerializeField]
    TextMeshProUGUI candleText, keyText, gameoverText, winText;
    [SerializeField]
    Animator gateAnimator;

    int health = 3;
    int candleQuantity = 3;
    int candleCollected = 0;
    int keyQuantity = 1;
    int keyCollected = 0;
    bool hasMaxHealthSet = false;
    bool hasKey = false;
    bool isGameover = false;

    // Properties
    public bool IsGameover
    {
        get { return isGameover; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the PlayerMovement script
        playerMovement = GetComponent<PlayerMovement>();

        // Set the health bar to the max amount of the healt player
        HealthBar.Instance.SetMaxHealth(health);

        // Set the initial candle text and key text
        candleText.text = $"{candleCollected}/{candleQuantity}";
        keyText.text = $"{keyCollected}/{keyQuantity}";
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            // Increase the amount of candles currently collected
            candleCollected++;

            // Set and update the text UI for the candle
            candleText.text = $"{candleCollected}/{candleQuantity}";

            // Insntantiate a spark visualFX
            Instantiate(pickupVFX, other.transform.position, pickupVFX.transform.rotation);

            // Increase the range and intensity of the spot light illuminating the player
            StartCoroutine(StrongerLight(spotLight));

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

            // Instantiate a visualFX, must be a child gameobject of the player
            Instantiate(attackedVFX, transform.position, attackedVFX.transform.rotation, this.transform);

            // Reduce the health of the player every contact with the enemy
            health--;

            // Update the healt bar based on the current healt of the player
            HealthBar.Instance.SetHealth(health);

            // Decrease the player's speed based on the health amount left
            ReduceSpeedBasedOnHealth(health);

            // Display game over when health is less than or equal to zero
            PlayerDeath(health);
        }

        // The win condition of the game
        if (collision.gameObject.CompareTag("Gate") && hasKey)
        {
            isGameover = true;

            // Play the gate opening animation
            gateAnimator.SetTrigger("hasKey");

            // Play the win SFX from the audio manager
            AudioManager.Instance.PlaySFX("WinSFX");

            // Display the win text and hide the gameover text
            winText.enabled = true;
            gameoverText.enabled = false;
            UIManager.Instance.GameoverStartCoroutine();
        }
    }

    private void ReduceSpeedBasedOnHealth(int h)
    {
        switch (h)
        {
            case 1:
                playerMovement.Speed = 4f;
                break;
            case 2:
                playerMovement.Speed = 7f;
                break;
            case 3:
                playerMovement.Speed = 10f;
                break;
            default:
                playerMovement.Speed = 0f;
                break;
        }
    }

    private void PlayerDeath(int h)
    {
        if (h <= 0)
        {
            isGameover = true;

            // Play the lose sfx from the audio manager
            AudioManager.Instance.PlaySFX("LoseSFX");

            // Display the gameover text and hide the win text
            winText.enabled = false;
            gameoverText.enabled = true;
            UIManager.Instance.GameoverStartCoroutine();

            // Destroy the player
            gameObject.SetActive(false);
        }
    }

    private IEnumerator StrongerLight(Light light)
    {
        float doubleLight = light.intensity * 2;

        while (light.intensity <= doubleLight)
        {
            light.intensity = Mathf.Lerp(light.intensity, light.intensity * 2f, Time.deltaTime);
            light.spotAngle = Mathf.Lerp(light.spotAngle, light.spotAngle + 30f, Time.deltaTime);
        }
        yield return null;
    }
}
