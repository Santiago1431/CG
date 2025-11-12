using UnityEngine;
using System.Collections;
public class PlayerLife : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public int maxLife;
    public int currentLife;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLife = maxLife;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentLife -= damage;
        StartCoroutine(FlashRed());
        if (currentLife <= 0)
        {
            Debug.Log("Player Dead");
        }
    }
    
    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}
