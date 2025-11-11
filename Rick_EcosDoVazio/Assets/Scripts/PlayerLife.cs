using UnityEngine;

public class PlayerLife : MonoBehaviour
{

    public int maxLife;
    public int currentLife;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void TakeDamage(int damage)
    {
        currentLife -= damage;
        if (currentLife <= 0)
        {
            Debug.Log("Player Dead");
        }
    }
}
