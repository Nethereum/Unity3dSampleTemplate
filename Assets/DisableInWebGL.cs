using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInWebGL : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
#if UNITY_WEBGL
        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
        }
#endif        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
