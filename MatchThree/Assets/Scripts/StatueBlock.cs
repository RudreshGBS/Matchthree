using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueBlock : MonoBehaviour
{
    public int id;
    GameObject particalSystem;

    private void Awake()
    {
        id = int.Parse(gameObject.name);
    }
    public void EnableBlock(bool forFirstTime)
    {
       particalSystem?.SetActive(forFirstTime);
       gameObject.SetActive(true); 
    }
}
