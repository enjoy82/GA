using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_kansetu : MonoBehaviour
{
    //test用スクリプト、関節動作の確認
    // Start is called before the first frame update
    void Start()
    {
        GameObject Objct = (GameObject)Resources.Load ("unit1");
        Instantiate (Objct, new Vector3(0,2.5f,4.0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
