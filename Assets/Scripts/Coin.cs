using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool wasCollected;
    
    private void OnTriggerEnter(Collider other)
    {
        // 1. Wenn wasCollected nicht true ist (Stichwort if())
        if(wasCollected == false)
        {
            // 1.1. Setze wasCollected auf true (Stichwort zuweisung / = )
            wasCollected = true;
            print("Coin collected.");
            // Zähle CoinsCollected um 1 hinauf.

            MyCharacterController.CoinsCollected += 1;

        }


    }
}
