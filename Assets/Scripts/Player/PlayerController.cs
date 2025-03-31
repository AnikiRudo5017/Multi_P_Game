using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    private float horizontal;
    private float vertical;
    private string currentAnim;
    private bool isGround;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 40f;
    [SerializeField] private float attackCooldown = 0.7f;
    private float lastAttackTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Object.HasStateAuthority) return;
        Atttack();
        Movement();
        // RotateCharacter();
        Jump();
        Debug.Log(isGround);
    }

    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            changeAnim("Running");
            Vector3 move = new Vector3(horizontal, 0, vertical).normalized;
            move.y = 0;
            rb.MovePosition(transform.position + move * moveSpeed * Runner.DeltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Runner.DeltaTime);
        }
        else if (isGround)
        {
            rb.linearVelocity = Vector3.zero;
            changeAnim("Idle");
        }
    }

    private void changeAnim(string AnimName)
    {
        if (currentAnim != AnimName)
        {
            anim.ResetTrigger(AnimName);
            currentAnim = AnimName;
            anim.SetTrigger(currentAnim);

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }

    private void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {

            changeAnim("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Atttack()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= lastAttackTime + attackCooldown)
        {
            changeAnim("Attack");
            lastAttackTime = Time.time;
            changeAnim("Idle");
        }
    }
}
