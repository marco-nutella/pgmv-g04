using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class RecursiveInstaciator : MonoBehaviour
{
    // Start is called before the first frame update
    public int recurse = 5;
    public int splitNumber = 2;
    public Vector3 pivotPosition;
    void Start()
    {
        recurse -=1;
        for(int i = 0; i < splitNumber; i++)
        {
            if (recurse > 0)
            {
                var copy = Instantiate(gameObject);
                var recursive = copy.GetComponent<RecursiveInstaciator>();
                recursive.SendMessage("Generated", new RecursiveBundle() { Index = i, Parent = this });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
