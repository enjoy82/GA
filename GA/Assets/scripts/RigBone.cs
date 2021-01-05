using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigBone : MonoBehaviour{
  public GameObject bone;
  public Quaternion savedValue;
  void Start(){
    savedValue = bone.transform.localRotation;
  }
  public void set(float a, float x, float y, float z) {
    set(Quaternion.AngleAxis(a, new Vector3(x,y,z)));
  }
  public void set(Quaternion q) {
    bone.transform.localRotation = q;
    savedValue = q;
  }
  public void first_do() {
    savedValue = bone.transform.localRotation;
  }
  public void mul(float a, float x, float y, float z) {
    mul(Quaternion.AngleAxis(a, new Vector3(x,y,z)));
  }
  public void mul(Quaternion q) {
    bone.transform.localRotation = q * bone.transform.localRotation;
  }
  public void offset(float a, float x, float y, float z) {
    offset(Quaternion.AngleAxis(a, new Vector3(x,y,z)));
  }
  public void offset(Quaternion q) {
    bone.transform.localRotation = q * savedValue;
  }
}
