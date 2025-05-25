using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveBundle : MonoBehaviour
{
    // Start is called before the first frame update
    public int Index { get; set; }
    public RecursiveInstantiator Parent { get; set; }
}
