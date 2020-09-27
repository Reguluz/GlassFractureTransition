using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMove : MonoBehaviour
{
    public float Amplitude;

    public float Frequence;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var oldPos = transform.position;
        oldPos.y = Mathf.Sin(Frequence * Time.time) * Amplitude;
        transform.position = oldPos;
    }
}
