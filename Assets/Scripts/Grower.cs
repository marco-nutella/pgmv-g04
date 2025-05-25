using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grower : MonoBehaviour
{
    // Start is called before the first frame update
    public void Generated(RecursiveBundle bundle)
    {
        this.transform.position += this.transform.up * this.transform.localScale.y;
    }
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }
}
