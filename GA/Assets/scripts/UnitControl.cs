using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UnitControl : MonoBehaviour{
  public GameObject cube;
  public GameObject plane;
  public GameObject capsule;
  public GameObject humanoid;
  public RigBone[] legs = new RigBone[6];
  public float points = 0;
  public bool isTraining = true;
  const float frame_ms = 0.04f;

  void Start () {
  }
//選択と強さ
  public void movement(int select, int movement_indicator) {
    //動かす
    if(isTraining){
      if(select % 3 == 1){
        movement_indicator *= 2;
      }
      legs[select].offset((float)(movement_indicator),0,0,1);

      points += 1;
      
    }
  }


  void set_plane(GameObject input_plane){
    plane = input_plane;
  }
  void OnCollisionEnter(Collision collision){
    if (isTraining && collision.gameObject == plane){
      //Debug.Log("Hit");
      //判定終了
      Rigidbody rb = cube.GetComponent<Rigidbody>();
      rb.isKinematic = true;
      rb = humanoid.GetComponent<Rigidbody>();
      rb.isKinematic = true;
      rb = capsule.GetComponent<Rigidbody>();
      rb.isKinematic = true;
      isTraining = false;
    }
  }
}





