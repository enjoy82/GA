using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UnitControl : MonoBehaviour {
  public GameObject unit;
  public GameObject plane;
  public GameObject humanoid;
  public Vector3 bodyRotation = new Vector3(0,0,0);
  public int points;
  RigBone rightLowerLeg;
  void set(GameObject input_plane){
    plane = input_plane;
  }
  void Start () {
    rightLowerLeg = new RigBone(humanoid, HumanBodyBones.RightLowerLeg);
    if(rightLowerLeg == null){
      Debug.Log("this is null");
    }
  }
  void Update () {
    double t = Math.Sin(Time.time * Math.PI); // [-1, 1]
    double s = (t+1)/2;                       // [0, 1]
    double u = 1-s/2;                         // [0.5, 1]
    rightLowerLeg.offset((float)(90*s),0,0,1);
    humanoid.transform.rotation
      = Quaternion.AngleAxis(bodyRotation.z,new Vector3(0,0,1))
      * Quaternion.AngleAxis(bodyRotation.x,new Vector3(1,0,0))
      * Quaternion.AngleAxis(bodyRotation.y,new Vector3(0,1,0));
    //評価
  }
  
  void OnCollisionEnter(Collision collision){
    if (collision.gameObject.name == "Plane"){
      Debug.Log("Hit");
      //判定終了
    }
  }
}


