using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateRotate : MonoBehaviour
{

    public AnimationCurve curve;
    public float timeSq;
    private float durationSq = 5;

    [Header("Rotate Flags for Anim")]
    [SerializeField]
    private bool rotateCCWFlag;



    // Start is called before the first frame update
    void Start()
    {
        rotateCCWFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
       // timeSq += Time.deltaTime;
        //print(curve.Evaluate(timeSq)/durationSq);

    }
}
