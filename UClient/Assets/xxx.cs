using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xxx : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private float t = 0;

     void Update()
    {
        t += Time.deltaTime;
        if (t > 1f)
        {
            Debug.Log(Random.Range(1, 100).ToString());
            t = 0;
        }
    }
}
