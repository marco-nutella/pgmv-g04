using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    // Start is called before the first frame update
    public float scalar = 0.5f;
    // RecursiveBundle bundle

    public void Generated(int index)
    {
        this.transform.localScale *= scalar;
    }
}
