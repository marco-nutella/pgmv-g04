using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float angle = 30;
    //RecursiveBundle bundle

    public void Generated(int index)
    {
        this.transform.rotation *= Quaternion.Euler(angle * ((index * 2) - 1), 0, 0);
    }
}
