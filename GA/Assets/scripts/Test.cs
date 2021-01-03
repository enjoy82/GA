using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    const int elite = 8;
    //オブジェクトの個数(1世代当たりのこの数)、進化させる世代数、遺伝で使う時間(per frame)
    const int object_num = elite + ((elite * (elite+1)) / 2), generations = 1000, time = 1000;
    //関節可動域 [left&right upleg_front-back, upleg_in-out, upleg_twist_in-out, low_leg_stretch]
    int[] Range_list = new int[8]{60, 60, 60, 60, 60, 60, 60, 60};
    //結果格納部分
    int[] result = new int[object_num];
    //現在の世代数
    public int count;
    //経過したフレーム数
    public int frame;
    //子を格納する配列
    UnityEngine.GameObject[] boxiy;
    //オブジェクト読み込み用変数
    private UnityEngine.GameObject Objct;
    //GA配列部分 第一変数->オブジェクトの番号 第二変数->時間 第三変数->対応する関節角度
    int[ , , ] GA_list;
    // Start is called before the first frame update
    void Start () {
        // unit1をGameObject型で取得
        Objct = (GameObject)Resources.Load ("unit1");
        count = 1;
        frame = 0;
        initiarize();
    }

    // Update is called once per frame(30fps, 1frame/0.033s)
    //TODO フレームごとに関節角度の動作
    //TODO フレームごとに落下判定
    void Update()
    {
        if(frame == time){
            evaluate();
            //ワンチャン遅延させなきゃダメかも
        }
        for(int i = 0; i < object_num; i++){
            //落下判定、引数は自由に定義していい
            if(check()){
                result[i] = frame;
            }
        }
        for(int i = 0; i < object_num; i++){
            //関節動作、引数は自由に定義していい
            movement();
        }
        
        frame++;
    }
    //TODO関節角度の動作
    void movement(){
        for(int i = 0; i < object_num; i++){
            for(int l = 0; l < Range_list.Length; l++){
                
            }
        }
    }

    bool check(){
        //if 落下
        return true;
    }

    //TODOどう評価するか
    void evaluate(){

        make_next_generation();
    }
    //TODO 評価順にソート、選択、交叉、突然変異、次世代の生成を行う。(やって欲しい)
    void make_next_generation(){
        sort_by_evaluation();
        //select();
        make_next_unit();
        Debug.Log(count+ " 世代目");
        //TODO　結果出力したい
        count++;
        frame = 0;
        if(count == generations+1){
            return;
        }
    }
    //TODO 評価順にソート
    void sort_by_evaluation(){

    }
    //選択(特別なアルゴリズム入れない限りいらない)
    void select(){

    }
    //TODO 交叉、突然変異、次世代の生成
    void make_next_unit(){

    }

    void next_gen(){
        for(int i = 0; i < object_num; i++){
            result[i] = -1;
        }
        boxiy = new UnityEngine.GameObject[object_num];
        createobjects();
    }

    //初期設定
    void initiarize(){
        next_gen();

        //配列初期生成
        GA_list = new int[object_num, time, Range_list.Length];
        for(int i = 0; i < object_num; i++){
            for(int l = 0; l < time; l++){
                for(int k = 0; k < Range_list.Length; k++){
                    GA_list[i,l,k] = Random.Range(-1 * Range_list[k], Range_list[k]);
                }
            }
        }
    }
    //オブジェクト生成
    void createobjects(){
        for(int i = 0; i < object_num; i++){
            if(i % 2 == 0){
                float pos = (float)8.0 * (i / 2);
                boxiy[i] = Instantiate (Objct, new Vector3(pos,2.5f,0.0f), Quaternion.identity);
            }else{
                float pos = (float)8.0 * ((i+1) / 2) * -1;
                boxiy[i] = Instantiate (Objct, new Vector3(pos,2.5f,0.0f), Quaternion.identity);
            }
        }
    }
}
