using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    bool wasCollected;

    public ParticleSystem sparkles;
    public ParticleSystemForceField collectForce;
    ParticleSystem.MinMaxCurve collectForceStartForce;
    private void Start()
    {
        collectForceStartForce = collectForce.gravity;
        collectForce.gravity = new ParticleSystem.MinMaxCurve(0.0f);
    }
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
            StartCoroutine(DestroySequence());
           
        }
    }

    System.Collections.IEnumerator DestroySequence()
    {
        sparkles.Stop();
        collectForce.enabled = true;
        collectForce.gravity = collectForceStartForce;
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
    
}
