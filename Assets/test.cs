using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0.5f);
    }

}
