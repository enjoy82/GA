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
  //こいつをTestからコピーさせて、0-1で左右どちらかに重心をかけるようにしたい
  public bool[] gene = new bool[180];
  public float points = 0;
  public int movement_indicator = 0;
  public bool isTraining = true;

  void Start () {

  }
  void Update () {
    //動かす
    if(isTraining){
      //板と足の角度の調整
      //Debug.Log(cube.transform.localEulerAngles.z);
      legs[2].offset((float)(cube.transform.localEulerAngles.z),0,1,0);
      legs[5].offset((float)(cube.transform.localEulerAngles.z),0,1,0);
      //角度を出す
      //Debug.Log(humanoid.transform.localEulerAngles.y);
      int angle = (int)(humanoid.transform.localEulerAngles.y) + 89;
      if(humanoid.transform.localEulerAngles.y > 180){
        angle = (int)(humanoid.transform.localEulerAngles.y) -360 + 89;
      }
      Debug.Log(angle);
      if(angle <0){
        angle = 0;
      }else if(angle >179){
        angle = 179;
      }
      //位置が右だったら(geneで制御)
      if(gene[angle]){
        //インジケータの増加
        if(movement_indicator<=30){
          movement_indicator++;
        }
      }else{
         //インジケータの減少
        if(movement_indicator<=-30){
          movement_indicator--;
        }
      }
      //左に動かす
      if(movement_indicator > 0){
        legs[0].offset((float)movement_indicator,0,0,1);
        legs[1].offset((float)(-2*movement_indicator),0,0,1);
        legs[2].offset((float)(-movement_indicator),0,0,1);
      }else if(movement_indicator == 0){
        //戻す
        legs[0].offset((float)movement_indicator,0,0,1);
        legs[1].offset((float)(-2*movement_indicator),0,0,1);
        legs[2].offset((float)(-movement_indicator),0,0,1);
        legs[3].offset((float)movement_indicator,0,0,1);
        legs[4].offset((float)(-2*movement_indicator),0,0,1);
        legs[5].offset((float)(-movement_indicator),0,0,1);
      }else{
        //右に動かす
        legs[3].offset((float)movement_indicator,0,0,1);
        legs[4].offset((float)(-2*movement_indicator),0,0,1);
        legs[5].offset((float)(-movement_indicator),0,0,1);
      }
      //評価(time bonus + angle point)
      points += 20 + (90 - Mathf.Abs(humanoid.transform.localEulerAngles.y));
    }

  }
  void set_plane(GameObject input_plane){
    plane = input_plane;
  }
  void OnCollisionEnter(Collision collision){
    if (collision.gameObject == plane){
      Debug.Log("Hit");
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





