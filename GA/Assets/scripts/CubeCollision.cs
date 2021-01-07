using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    public UnitControl unit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision){
        if (unit.isTraining && collision.gameObject == unit.plane){
            Debug.Log("Hit");
            //判定終了
            Rigidbody rb = unit.cube.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb = unit.humanoid.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb = unit.capsule.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            unit.isTraining = false;
        }
    }
}
