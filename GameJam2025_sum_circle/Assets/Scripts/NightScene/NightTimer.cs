using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NightTimer : MonoBehaviour
{
    [SerializeField] float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if () {
            Timer();
        //}
    }
    void Timer()
    {
        timer -= Time.deltaTime;
        if (timer < 0 )
        {

        }
    }
}
