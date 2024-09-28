using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private float speed = 10f;

    private int count;
    public TextMeshProUGUI countText;
    public GameObject winText;

    private AudioSource audioSource;
    public AudioClip pickupAudio;
    public AudioClip winAudio;

    public ParticleSystem pickupFX;
    public ParticleSystem gameoverFX;
    public ParticleSystem movementFX;
    public ParticleSystem winFX;

    [SerializeField]
    private bool isMoving = false;
    private Vector3 targetPos;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        count = 0;
        SetCountText();

        winText.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    targetPos = hit.point;
                    isMoving = true;
                }
            }
        }
        else
        {
            isMoving = false;
        }
    }

    void FixedUpdate()
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(movementX, 0f, movementY);
        rb.AddForce(movement * speed);

        if (isMoving)
        {
            Vector3 direction = targetPos - rb.position;
            direction.Normalize();
            rb.AddForce(direction * speed);
        }

        if (Vector3.Distance(rb.position, targetPos) < 0.5f)
        {
            isMoving = false;
        }

        if (rb.velocity.magnitude > 5f)
        {
            Instantiate(movementFX, transform.position, Quaternion.identity);
        }
    }

    // This function is called when a move input is detected.
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
            count++;            
            audioSource.clip = pickupAudio;
            audioSource.Play();
            SetCountText();

            Instantiate(pickupFX, other.transform.position, Quaternion.identity);

            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponentInChildren<Animator>().SetFloat("speed_f", 0f);

            collision.gameObject.GetComponent<AudioSource>().Play();
            winText.GetComponent<TextMeshProUGUI>().text = "YOU LOSE!";
            winText.SetActive(true);

            Instantiate(gameoverFX, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            collision.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    void SetCountText()
    {
        countText.text = $"Count: {count}";

        if (count >= 16)
        {
            audioSource.clip = winAudio;
            audioSource.Play();
            winText.SetActive(true);
            winFX.gameObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }
}
