// ScoreItem.cs
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public int pointValue = 1; // Số điểm mà item này sẽ cộng thêm
    public bool destroyOnCollect = true; // Có xóa item sau khi nhặt hay không
    public GameObject collectEffect; // Hiệu ứng khi nhặt item (nếu có)

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem đối tượng va chạm có phải là người chơi không
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        
        if (player != null)
        {
            // Lấy ScoreManager từ người chơi
            ScoreCount scoreManager = player.GetComponent<ScoreCount>();
            
            if (scoreManager != null)
            {
                // Tăng điểm
                scoreManager.AddScore(pointValue);
                
                // Tạo hiệu ứng nếu có
                if (collectEffect != null)
                {
                    Instantiate(collectEffect, transform.position, Quaternion.identity);
                }
                
                // Xóa item nếu cấu hình như vậy
                if (destroyOnCollect)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}