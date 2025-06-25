using UnityEngine;

public class DestructibleBox : MonoBehaviour, IAttackable
{
    public void Damage(int damageValue)
    {
        print($"{name} wurde mit {damageValue} schaden getroffen.");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
