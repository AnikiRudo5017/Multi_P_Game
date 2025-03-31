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

    public CinemachineCamera Cam1;
    public CinemachineCamera Cam2;
    public CinemachineCamera Cam3;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;
        HandleMovement();
        HandleJump();
        velocity.y -= gravity * Runner.DeltaTime;
        HandleCameraSwitch();
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

    private void HandleCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchCamera(Cam1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchCamera(Cam2);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchCamera(Cam3);
    }

    private void SwitchCamera(CinemachineCamera activeCam)
    {
        Cam1.enabled = Cam1 == activeCam;
        Cam2.enabled = Cam2 == activeCam;
        Cam3.enabled = Cam3 == activeCam;
    }
    private void changeAnim(string AnimName)
    {
        if (currentAnim != AnimName)
        {
            _animator.ResetTrigger(AnimName);
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