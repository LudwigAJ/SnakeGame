using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallBack : MonoBehaviour
{

    private bool firstUpdate = true;
    
    // Update is called once per frame
    private void Update()
    {
        if (firstUpdate)
        {
            firstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
