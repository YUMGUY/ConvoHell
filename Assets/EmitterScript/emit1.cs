using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emit1 : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem system;
    public Material material;
    public Sprite texture;
    public AnimationCurve speedCurve;


    // flag for shooting
    public bool isShooting;
    public bool isRotating;

    // affect the particles , these are radial particles/projectilese
    [Header("Particle Variables")]
    public Color color1;
    public float speed;
    public int numCol;
    public float particleAngle;
    public float part_Size;

    public float fireRate;
    public float partLifetime;

    private float rotationTimer;
    private float invertV = 1;
    public float rotateSpeed;
    private void Awake()
    {
        CreateParticleSystem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            invertV *= -1;
            print("you pressed it");
        }

        if (Input.GetKey(KeyCode.D))
        {
            isShooting = false;
        }

        if(Input.GetKey(KeyCode.F))
        {
            isShooting = true;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            isShooting = false;
            for(int i = 0; i < this.transform.childCount; ++i)
            {
               // this.transform.GetChild(i).GetComponent<ParticleSystem>().Clear(); // this works
            }
        }

       
        if (isRotating)
        {
            rotationTimer += Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, invertV * rotateSpeed * rotationTimer);
        }
            
    }

    private void FixedUpdate()
    {
       // rotationTimer += Time.fixedDeltaTime;

      

       
    }

    void CreateParticleSystem()
    {
        particleAngle = 360 / numCol;

        for (int i = 0; i < numCol; ++i)
        {


            Material particleMat = material;
            // Create a green Particle System.
            var go = new GameObject("Particle System");
            go.transform.Rotate(particleAngle * i, 90, 0); // Rotate so the system emits upwards.
            print(particleAngle * i + " is the angle");
            go.transform.parent = this.transform;
            go.transform.position = this.transform.position;



            system = go.AddComponent<ParticleSystem>();
            go.GetComponent<ParticleSystemRenderer>().material = particleMat;


            var mainModule = system.main;
            //mainModule.startColor = Color.green;
            //mainModule.startSize = 0.5f;
            mainModule.startSpeed = speed;
            mainModule.maxParticles = 100000;
            mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
         

            var emissionM = system.emission;
            emissionM.enabled = false;

            var particleForm = system.shape;
            particleForm.enabled = true;
            particleForm.shapeType = ParticleSystemShapeType.Sprite;
            particleForm.sprite = null;

            // since it is sprite, then we use Sprite Default Material as the material of the particle system
            var texture_ = system.textureSheetAnimation;
            texture_.mode = ParticleSystemAnimationMode.Sprites;
            texture_.enabled = true;
            texture_.AddSprite(texture);


            // change velocity over lifetime

            var vel = system.velocityOverLifetime;
            vel.enabled = true;
            AnimationCurve curve = new AnimationCurve();
           
            curve.AddKey(0.0f, 1.0f);
            curve.AddKey(1.0f, 0f);
            //vel.x = new ParticleSystem.MinMaxCurve(1f, curve);
            //vel.y = new ParticleSystem.MinMaxCurve(5f, curve);
            //vel.z = new ParticleSystem.MinMaxCurve(0, curve);


            // for making roses
             vel.speedModifier = new ParticleSystem.MinMaxCurve(1.0f, speedCurve); // pos -> neg speed for rose while rotating


            // give collision
            var collisionMod = system.collision;
            collisionMod.enabled = true; // very important
            collisionMod.type = ParticleSystemCollisionType.World;
            collisionMod.mode = ParticleSystemCollisionMode.Collision2D;// IMPORTANT
            collisionMod.sendCollisionMessages = true;
            collisionMod.collidesWith = LayerMask.GetMask("collide", "Water");

            // OPTIONAL, could also do lifetimeLoss in collision

            // get rid of bounce
            collisionMod.bounce = 0;
            collisionMod.lifetimeLoss = 3;
        }


        // Every .3 secs we will emit.
       
        InvokeRepeating("DoEmit",fireRate,fireRate);
    }

    void DoEmit()
    {
        if (isShooting == true)
        {


            foreach (Transform particleSchild in transform)
            {

                // give tag to each child
                particleSchild.tag = "particle1";

                system = particleSchild.GetComponent<ParticleSystem>();

                // Any parameters we assign in emitParams will override the current system's when we call Emit.
                // Here we will override the start color and size.
                var emitParams = new ParticleSystem.EmitParams();
                emitParams.startColor = color1;
                emitParams.startSize = part_Size;
                emitParams.startLifetime = partLifetime;
                system.Emit(emitParams, 10);

            }
        }

        else
        {
            
        }
    }


    public void DestroyAllChildren()
    {
        for(int i = 0; i < this.transform.childCount; ++i)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }

        gameObject.SetActive(false);
    }

 
}
