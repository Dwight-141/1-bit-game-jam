using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public TextMeshProUGUI lives;
    public MainMenuManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lives.text = health.ToString();
        if (health <= 0 )
        {
            gameManager.BackToMain();
        }
    }
}
