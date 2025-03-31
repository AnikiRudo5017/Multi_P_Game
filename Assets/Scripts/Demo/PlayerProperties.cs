using System;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProperties : NetworkBehaviour
{
    //[SerializeField]
    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    private int Health { get; set; }
    public Slider healthSlider;
    private void OnHealthChanged()
    {
        // Chỉ client sở hữu quyền InputAuthority mới cập nhật slider
        if (Object.HasInputAuthority && healthSlider != null)
        {   
            healthSlider.value = Health;
        }
        Debug.Log($"Health changed to {Health} on client {Runner.LocalPlayer.PlayerId}");
    }
    private void Awake()
    {
        // Tìm slider trong prefab
        healthSlider = GetComponentInChildren<Slider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trip"))
        {
            TakeDamage(10);
        }
    }

    public override void Spawned()
    {
        // Đảm bảo slider đã được liên kết
        healthSlider = GetComponentInChildren<Slider>();

        // Khởi tạo giá trị Health sau khi object được spawn
        if (HasStateAuthority)
        {
            Health = 100;
        }
    }

    // void Update()
    // {
    //     //if (HasStateAuthority)
    //     //{
    //     //    if (Input.GetKeyDown(KeyCode.Space))
    //     //    {
    //     //        Health -= 10;
    //     //    }
    //     //}
    //     if (HasStateAuthority && Object.HasInputAuthority && Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Health -= 10;
    //     }
    // }

    public void TakeDamage(int damage)
    {
        if (HasStateAuthority)
        {
            Health = Mathf.Max(0, Health - damage);
        }
    }
}

