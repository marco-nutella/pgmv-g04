using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class RecursiveInstaciator : MonoBehaviour
{
    // Start is called before the first frame update
    public int recursive = 5;
    public int splitNumber = 2;
    public Vector3 pivotPosition;
    void Start()
    {
        recursive -=1;
        for(int i = 0; i < splitNumber; i++)
        {
            if (recursive > 0)
            {
                var copy = Instantiate(gameObject);
                var recursive = copy.GetComponent<RecursiveInstaciator>();
                recursive.SendMessage("Generatee");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
