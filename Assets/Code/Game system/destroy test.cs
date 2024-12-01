using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroytest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
