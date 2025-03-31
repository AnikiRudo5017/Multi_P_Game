using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 2f; // Tốc độ xoay
    public float minVerticalAngle = -60f, maxVerticalAngle = 60f; // Giới hạn góc quay dọc
    public float returnSpeed = 5f; // Tốc độ trở về vị trí ban đầu

    private Vector2 turn; // Lưu trữ góc quay
    private Quaternion originalRotation; // Lưu trữ góc quay ban đầu

    private void Start()
    {
        originalRotation = transform.localRotation; // Lưu góc quay ban đầu
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // Khi giữ chuột trái
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            turn.x += mouseX;
            turn.y -= mouseY; // Đảo ngược trục Y để xoay đúng hướng
            turn.y = Mathf.Clamp(turn.y, minVerticalAngle, maxVerticalAngle); // Giới hạn góc dọc

            transform.localRotation = Quaternion.Euler(turn.y, turn.x, 0);
        }
        else // Khi nhả chuột, quay về vị trí cũ
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, Time.deltaTime * returnSpeed);
        }
    }
}