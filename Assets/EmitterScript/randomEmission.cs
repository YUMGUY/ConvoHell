using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomEmission : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Type of Random Emission")]
    public float minAngle; // for the child
    public float maxAngle;
    public float offsetAngle; // for the parent
    public int numberIterations;


    [Header("Particle System Variables")]
    public ParticleSystem systemRandom;
    public Material randMat;

    public Sprite textureR;
    public AnimationCurve speedCurveR;

    [Header("Flags for Types of Shooting")]
    // flag for shooting
    public bool isShootingR;
    public bool isRandomSpread;
    public bool isRandomShooting;
    public bool isExpanding;
    public bool isShrinking;
    public bool hasGravity;

    [Header("Flag for Type of Rotation")]
    public bool isTrackingPlayer;
    private Quaternion targetRotation;
    public float TrackingSpeed;
    public Transform player;

    // affect the particles , these are radial particles/projectilese
    [Header("Particle Variables")]
    public Color colorR;
    public float speedR;
    public int numColR;
    public float particleAngleR;
    public float part_SizeR;

    public float fireRateR;
    public float partLifetimeR;

    [Header("Affect down/up/left/right motion")]
    public float gravityModMax;
    public float gravityModMin;

    public bool changeWorldXvel;
    public float xVelEndMax;
    public float xVelEndMin;

    private float rotationTimerR;
    private float invertVR = 1;
    private float timeR;
    //private float durationRotate;

    float minthreshold;
    float maxThreshold;
    private void Awake()
    {
        
        CreateParticleSystem();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            invertVR *= -1;
            print("you pressed it");
        }

        if (Input.GetKey(KeyCode.D))
        {
            isShootingR = false;
        }

        if (Input.GetKey(KeyCode.F))
        {
            isShootingR = true;
        }
        timeR += Time.deltaTime;



        if (isTrackingPlayer)
        {
            float offset = 0f;
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            targetRotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TrackingSpeed * Time.deltaTime);
        }
        //durationRotate = 5f;
        //this.transform.eulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 0, 80), timeR/durationRotate); 

    }

    private void FixedUpdate()
    {
        //rotationTimerR += Time.fixedDeltaTime;


        //transform.rotation = Quaternion.Euler(0, 0, invertVR * 40 * rotationTimerR);


    }

    void CreateParticleSystem()
    {
        particleAngleR = (maxAngle - minAngle) / numColR; // e.g 30 - 0 = 30/5 = 6 degrees for each
        if(offsetAngle < 0)
        {
            offsetAngle = 360 + offsetAngle;
        }

        if(isShrinking == true)
        {
            minthreshold = offsetAngle;
            maxThreshold = (particleAngleR * (numColR - 1)) + offsetAngle;
        }

        else if (isExpanding == true)
        {
            minthreshold = offsetAngle + (particleAngleR * 2);
            maxThreshold = offsetAngle + (particleAngleR * 2);
        }
        
        // this.transform.eulerAngles = new Vector3(0, 0,this.transform.eulerAngles.z + 90);
        for (int i = 0; i < numColR; ++i)
        {


            Material particleMat = randMat;
            // Create a child Particle System
            var go = new GameObject("Particle System");
           
           // go.transform.Rotate((particleAngleR * i), 90, 0); // Rotate so the system emits upwards.
          //  print(particleAngleR * i + " is the angle");
            go.transform.parent = this.transform;

            // THIS WORKS, CHANGE IT SO THAT THE PARENT IS BEFORE THE ROTATION  // IS CHANGED
            if (isRandomShooting == false)
            go.transform.eulerAngles = new Vector3(particleAngleR * i + offsetAngle, 90, 0);

            else
            {
              //  print("angles are randomized");
                float angleRandom = Random.Range(offsetAngle, maxAngle + offsetAngle);
              //  print("the random angle is " + angleRandom);
                go.transform.eulerAngles = new Vector3(angleRandom, 90, 0);
            }
            
            
                                                                                            
            go.transform.position = this.transform.position;



            systemRandom = go.AddComponent<ParticleSystem>();
            go.GetComponent<ParticleSystemRenderer>().material = particleMat;


            var mainModule = systemRandom.main;
            //mainModule.startColor = Color.green;
            //mainModule.startSize = 0.5f;
            mainModule.startSpeed = speedR;
            mainModule.maxParticles = 100000;
            mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
           

            var emissionM = systemRandom.emission;
            emissionM.enabled = false;

            var particleForm = systemRandom.shape;
            particleForm.enabled = true;
            particleForm.shapeType = ParticleSystemShapeType.Sprite;
            particleForm.sprite = null;

            // since it is sprite, then we use Sprite Default Material as the material of the particle system
            var texture_ = systemRandom.textureSheetAnimation;
            texture_.mode = ParticleSystemAnimationMode.Sprites;
            texture_.enabled = true;
            texture_.AddSprite(textureR);


            // Change Velocity over Lifetime ////////////////////////////////////

            var vel = systemRandom.velocityOverLifetime;

            if(changeWorldXvel == true)
            vel.space = ParticleSystemSimulationSpace.World;
            else { vel.space = ParticleSystemSimulationSpace.Local; }

            vel.enabled = true;
        

            // RIGHT NOW ONLY CHANGING X VELOCITY OVER LIFETIME, RANDOM VALUES

            if(changeWorldXvel == true)
            {
                vel.x = new ParticleSystem.MinMaxCurve(xVelEndMin, xVelEndMax);
                vel.y = new ParticleSystem.MinMaxCurve(0,0);
                vel.z = new ParticleSystem.MinMaxCurve(0,0);
            }
           

            else { vel.x = new ParticleSystem.MinMaxCurve(0); vel.y = new ParticleSystem.MinMaxCurve(0); vel.z = new ParticleSystem.MinMaxCurve(0); }

            // for making roses
           vel.speedModifier = new ParticleSystem.MinMaxCurve(1.0f, speedCurveR); // pos -> neg speed for rose while rotating


            // give collision
            var collisionMod = systemRandom.collision;
            collisionMod.enabled = true; // very important
            collisionMod.type = ParticleSystemCollisionType.World;
            collisionMod.mode = ParticleSystemCollisionMode.Collision2D;// IMPORTANT
            collisionMod.sendCollisionMessages = true;
            collisionMod.collidesWith = LayerMask.GetMask("collide");

            // OPTIONAL, could also do lifetimeLoss in collision

            // get rid of bounce
            collisionMod.bounce = 0;
            collisionMod.lifetimeLoss = 3;



            //GRAVITY MODIFIER
            if(hasGravity == true)
            {
                // random select between 2 values
                mainModule.gravityModifier = new ParticleSystem.MinMaxCurve(gravityModMin,gravityModMax);
            }
            

            else
            {
                mainModule.gravityModifier = 0;
            }


        }


        // Every .3 secs we will emit.
        
        InvokeRepeating("DoEmit", fireRateR, fireRateR);
    }

    void DoEmit()
    {
        if (isShootingR == true && isRandomSpread)
        {


            
            foreach (Transform particleSchild in transform)
            {
                print(particleSchild.localEulerAngles.x + " is the child angle");
                if (particleSchild.localRotation.eulerAngles.x <= maxThreshold + .1f && particleSchild.localRotation.eulerAngles.x >= minthreshold - .1f)
                {

                    
                   // print(particleSchild.rotation.eulerAngles.x);
                    // give tag to each child
                    particleSchild.tag = "particle1";

                    systemRandom = particleSchild.GetComponent<ParticleSystem>();

                    // Any parameters we assign in emitParams will override the current system's when we call Emit.
                    // Here we will override the start color and size.
                    var emitParams = new ParticleSystem.EmitParams();
                    emitParams.startColor = colorR;
                    emitParams.startSize = part_SizeR;
                    emitParams.startLifetime = partLifetimeR;
                    systemRandom.Emit(emitParams, 10); 
                
                }
          

            }

            print(maxThreshold + " is the max");
            print(minthreshold + " is the min");
            if (isShrinking)
            {
                minthreshold += particleAngleR;
                maxThreshold -= particleAngleR;
            }


            else if (isExpanding)
            {
                minthreshold -= particleAngleR;
                maxThreshold += particleAngleR;
            }
            
            // reset for shrinking
            if(minthreshold >= (particleAngleR * 3) + offsetAngle)
            {
                print("reset");
                minthreshold = offsetAngle;
                maxThreshold = ((numColR - 1) * particleAngleR) + offsetAngle;
            }

            // reset for expanding
            if(maxThreshold > (particleAngleR * 4) + offsetAngle && isExpanding == true)
            {
                print("reset");
                minthreshold = offsetAngle + (particleAngleR * 2);
                maxThreshold = offsetAngle + (particleAngleR * 2); // temporary, must find the median after wards, cast to an int or round down, rn works for 5
            }
        }


        else if(isShootingR == true && isRandomSpread == false || isRandomShooting == true)
        {
            foreach (Transform particleSchild in transform)
            {

                // give tag to each child
                particleSchild.tag = "particle1";

                systemRandom = particleSchild.GetComponent<ParticleSystem>();

                // Any parameters we assign in emitParams will override the current system's when we call Emit.
                // Here we will override the start color and size.

                var emitParams = new ParticleSystem.EmitParams();
                emitParams.startColor = colorR;
                emitParams.startSize = part_SizeR;
                emitParams.startLifetime = partLifetimeR;


                // IT WORKS, randomized angles for each shot of particles 
                if(isRandomShooting == true)
                {
                    float angleRandom = Random.Range(offsetAngle, maxAngle + offsetAngle);
                    print("the random angle is " + angleRandom);
                    particleSchild.eulerAngles = new Vector3(angleRandom, 90, 0);
                }
                // THIS AFFECTS GRAVITY for some reason, if 10, then multiplied by 10??? , just keep this as 1 for now
                systemRandom.Emit(emitParams, 1);

            }
        }



        else
        {
            //print("not shooting");
        }
    }
}
