using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public TextMeshProUGUI score;
    public int count;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            count++;
            score.text = count.ToString();
        }
    }
}
