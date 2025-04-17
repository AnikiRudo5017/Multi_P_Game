using System;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public CharacterController controller;
    public float movementSpeed = 7f;
    public float jumpForce = 10f;
    public float gravity = 9.81f;
    public Vector3 velocity;
    public float rotationSpeed = 5f;
    private Animator _animator;
    private string currentAnim;
    private bool _isGrounded;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("❌ Animator not found on " + gameObject.name);
        }
        if (RoomManager.Instance.isStarted == false) return;
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;
        HandleMovement();
        HandleJump();
        velocity.y -= gravity * Runner.DeltaTime;
        
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontal, 0, vertical).normalized;

        if (move.magnitude >= 0.1f) // Chỉ di chuyển khi có input
        {
            changeAnim("Running");
            controller.Move(move * movementSpeed * Runner.DeltaTime);
            
            // Xoay theo hướng di chuyển
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Runner.DeltaTime);
        }
        else
        {
            changeAnim("Idle");
        }
    }
    private void HandleJump()
    {
        if (controller.isGrounded)
        {
            velocity.y = -2f; // Reset trọng lực khi chạm đất

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpForce;
            }
        }
    }
    private void changeAnim(string AnimName)
    {
        if (currentAnim != AnimName)
        {
            _animator.ResetTrigger(currentAnim);
            currentAnim = AnimName;
            _animator.SetTrigger(currentAnim);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }
}