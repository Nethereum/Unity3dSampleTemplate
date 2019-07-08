using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextInWebGL : MonoBehaviour
{    
    // Start is called before the first frame update
    void Start()
    {
        Text text = GetComponent<Text>();
#if UNITY_WEBGL
        if (text != null)
        {
            text.text += "\nNot supported in WebGL";
        }
#endif        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
