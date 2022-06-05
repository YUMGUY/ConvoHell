using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleCollide : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject test;
    private emit1 testScript;
    [SerializeField]
    private ParticleSystem ps;
    private int count = 0;

    ParticleSystem.Particle[] particles;
    void Start()
    {
        testScript = test.GetComponent<emit1>();
        
    }

    // Update is called once per frame
    void Update()
    {
        /* everything underneath works, but need to call this as a function with GameObject as parameter for the particle system host */
        count = 0;

        if (test.activeInHierarchy)
        {


            for (int i = 0; i < 5; ++i)
            {
                count += test.transform.GetChild(i).GetComponent<ParticleSystem>().particleCount;

            }

            particles = new ParticleSystem.Particle[count];

            int num = 0;
            for (int i = 0; i < 5; ++i)
            {
                num += test.transform.GetChild(i).GetComponent<ParticleSystem>().GetParticles(particles);
            }
            print("# " + num);
            //print(num + "is the number of active particles");
            if (Input.GetKeyDown(KeyCode.L))
            {
                testScript.isShooting = false;
                print("you pressed it");
                for (int i = 0; i < num; ++i)
                {
                    //print("gone");
                    particles[i].remainingLifetime = 0;
                }

                for (int i = 0; i < 5; ++i)
                {
                    test.transform.GetChild(i).GetComponent<ParticleSystem>().SetParticles(particles, num);
                }

                testScript.DestroyAllChildren();
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("particle1") && this.gameObject.layer == 3)
        {
            
            var col = other.GetComponent<ParticleSystem>().collision;
            col.lifetimeLoss = 0f; // testing out pachinko methods
            col.bounce = 1;
            print("it bounced off");
        }
    }
}
