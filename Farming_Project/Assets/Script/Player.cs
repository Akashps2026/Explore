using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;

    private float horizontalInput;
    private float verticalInput;

    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
       
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * speed;
        rb.MovePosition(transform.position + movement * Time.fixedDeltaTime);
    }
}
