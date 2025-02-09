using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatytest : MonoBehaviour
{
    public Vector3 Randoml = new Vector3(1, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(Random.Range(-Randoml.x, Randoml.x), Random.Range(-Randoml.y, Randoml.y), Random.Range(-Randoml.z, Randoml.z));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
