using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testee : MonoBehaviour
{
    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager.Play("Gaga", gameObject);

        //audioManager.Play("Gaga", gameObject);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
