using UnityEngine;

public class ExplorePlayerController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public Transform characterBody;

    float hAxis, vAxis;
    bool crouch;

    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (GameSystem.Instance.State != GameState.EXPLORE) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            crouch = !crouch;
            //anim.SetBool("isCrouch", crouch);
        }

        Move();
    }

    void Move()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        float currentSpeed = moveSpeed * (crouch ? 0.5f : 1.0f);
        Vector3 movement = new Vector3(hAxis * currentSpeed, 0, vAxis * currentSpeed);

        rb.velocity = new Vector3(movement.x * Time.deltaTime, 0, movement.z * Time.deltaTime);

        if (rb.velocity.x < 0) characterBody.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
        else characterBody.rotation = Quaternion.Euler(new Vector3(-45, 180, 0));

        anim.SetBool("isWalk", movement != Vector3.zero);
    }

}
