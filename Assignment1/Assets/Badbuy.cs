using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Badbuy : MonoBehaviour


{

    public int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
        

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            score += 1;
        }
    }
}
