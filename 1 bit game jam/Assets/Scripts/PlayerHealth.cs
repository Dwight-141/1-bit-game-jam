using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public TextMeshProUGUI lives;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lives.text = health.ToString();
    }
}
