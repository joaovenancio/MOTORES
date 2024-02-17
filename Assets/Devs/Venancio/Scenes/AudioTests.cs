using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTests : MonoBehaviour
{
    public bool T1;
    public bool T2;
    public GameObject A1;
    public GameObject A2;

    // Start is called before the first frame update
    void Start()
    {
        if (T1)
        {
            AudioManager.Instance.Play("1", A1, "G1");
            AudioManager.Instance.Play("2", A2, "G1");
        } 
        
        if (T2)
        {
            AudioManager.Instance.Play("3", gameObject, "G2");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
