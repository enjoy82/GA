﻿using System.Collections;
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
  public int points = 0;
  public int movement_indicator = 0;
  public bool isTraining = true;
  int i;

  void Start () {

  }
  void Update () {
    //板と足の角度の調整
    Debug.Log(cube.transform.localEulerAngles.z);
    legs[2].offset((float)(cube.transform.localEulerAngles.z),0,1,0);
    legs[5].offset((float)(cube.transform.localEulerAngles.z),0,1,0);
    if(isTraining){
      //動かす
      //位置が右だったら(geneで制御)
        //インジケータの増加
        if(movement_indicator<=30){
          movement_indicator++;
        }
        //インジケータの減少
        if(movement_indicator<=-30){
          //movement_indicator--;
        }
      //左に動かす
      if(movement_indicator > 0){
        legs[0].offset((float)movement_indicator,0,0,1);
        legs[1].offset((float)(-2*movement_indicator),0,0,1);
        legs[2].offset((float)(-movement_indicator),0,0,1);
      }else if(movement_indicator == 0){
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
      //評価
    }

  }
  void set_plane(GameObject input_plane){
    plane = input_plane;
  }
  /*
  void OnCollisionEnter(Collision collision){
    if (collision.gameObject == plane){
      Debug.Log("Hit");
      //判定終了
      Rigidbody rb = cube.GetComponent<Rigidbody>();
      rb.isKinematic = true;
      rb = humanoid.GetComponent<Rigidbody>();
      //rb.isKinematic = true;
      rb = capsule.GetComponent<Rigidbody>();
      //rb.isKinematic = true;
      //isTraining = false;
    }
  }
  */
}


