using System.Collections;
using System.Collections.Generic;
using UFramework.Config;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var bc = UConfig.Read<BaseConfig>();
        UConfig.Write<BaseConfig>(bc);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
