using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleCollide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("particle1"))
        {
            print("yeah");
        }
    }
}
