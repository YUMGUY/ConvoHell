using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emitStar : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem system2;
    public Material material2;
    public Sprite texture2;
    public AnimationCurve speedCurve2;


    // flag for shooting
    public bool isShooting2;

    // affect the particles , these are radial particles/projectilese
    [Header("Particle Variables")]
    public Color color1_2;
    public float speed2;
    public int numCol2;
    public float particleAngle2;
    public float part_Size2;

    public float fireRate2;
    public int numberOfPoints;
    public float partLifetime2;

    private float rotationTimer2;
    private float invertV2 = 1;
    private void Awake()
    {
        CreateParticleSystem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            invertV2 *= -1;
            print("you pressed it");
        }

        if (Input.GetKey(KeyCode.D))
        {
            isShooting2 = false;
        }

        if (Input.GetKey(KeyCode.F))
        {
            isShooting2 = true;
        }


    }

    private void FixedUpdate()
    {
        //rotationTimer += Time.fixedDeltaTime;


        //transform.rotation = Quaternion.Euler(0, 0, invertV * 20 * rotationTimer);


    }

    void CreateParticleSystem()
    {
        particleAngle2 = 360 / numCol2;

        for (int i = 0; i < numCol2; ++i)
        {


            Material particleMat = material2;
            // Create a green Particle System.
            var go = new GameObject("Particle System");
            go.transform.Rotate(particleAngle2 * i, 90, 0); // Rotate so the system emits upwards.
            print(particleAngle2 * i + " is the angle");
            go.transform.parent = this.transform;
            go.transform.position = this.transform.position;



            system2 = go.AddComponent<ParticleSystem>();
            go.GetComponent<ParticleSystemRenderer>().material = particleMat;


            var mainModule = system2.main;
            //mainModule.startColor = Color.green;
            //mainModule.startSize = 0.5f;
            mainModule.startSpeed = speed2;


            // adding additional speed?
            var main2child = system2.main;
            main2child.startSpeed = new ParticleSystem.MinMaxCurve(speed2 + Mathf.Abs(Mathf.Sin(numberOfPoints * particleAngle2 * i *  Mathf.PI / 360)));

            mainModule.maxParticles = 100000;
            mainModule.simulationSpace = ParticleSystemSimulationSpace.World;


            var emissionM = system2.emission;
            emissionM.enabled = false;

            var particleForm = system2.shape;
            particleForm.enabled = true;
            particleForm.shapeType = ParticleSystemShapeType.Sprite;
            particleForm.sprite = null;

            // since it is sprite, then we use Sprite Default Material as the material of the particle system
            var texture_ = system2.textureSheetAnimation;
            texture_.mode = ParticleSystemAnimationMode.Sprites;
            texture_.enabled = true;
            texture_.AddSprite(texture2);


            // change velocity over lifetime

            var vel = system2.velocityOverLifetime;
            vel.enabled = true;
            AnimationCurve curve = new AnimationCurve();

            curve.AddKey(0.0f, 1.0f);
            curve.AddKey(1.0f, -10.0f);
            //vel.x = new ParticleSystem.MinMaxCurve(10f, curve);
            //vel.y = new ParticleSystem.MinMaxCurve(0, curve);
            //vel.z = new ParticleSystem.MinMaxCurve(0, curve);

           // vel.speedModifier = new ParticleSystem.MinMaxCurve(1.0f, speedCurve2);
        }


        // Every .3 secs we will emit.

        InvokeRepeating("DoEmit", fireRate2, fireRate2);
    }

    void DoEmit()
    {
        if (isShooting2 == true)
        {


            foreach (Transform particleSchild in transform)
            {

                system2 = particleSchild.GetComponent<ParticleSystem>();

                // Any parameters we assign in emitParams will override the current system's when we call Emit.
                // Here we will override the start color and size.
                var emitParams = new ParticleSystem.EmitParams();
                emitParams.startColor = color1_2;
                emitParams.startSize = part_Size2;
                emitParams.startLifetime = partLifetime2;
               
                system2.Emit(emitParams, 10);

               

            }
        }

        else
        {
            print("not shooting");
        }
    }
}
