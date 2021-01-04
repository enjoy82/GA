using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    const int elite = 8;
    //オブジェクトの個数(1世代当たりのこの数)、進化させる世代数、遺伝で使う時間(per frame), 突然変異の確率
    const int object_num = elite + ((elite * (elite+1)) / 2), generations = 1000, time = 1000, mutation = 1;
    const float frame_ms = 0.02f;
    //関節可動域 [left&right upleg_front-back, upleg_in-out, upleg_twist_in-out, low_leg_stretch]
    int[] Range_list = new int[8]{60, 60, 60, 60, 60, 60, 60, 60};
    //結果格納部分
    int[, ] result = new int[object_num, 2];
    //現在の世代数
    public int count;
    //経過したフレーム数
    public int frame;
    //子を格納する配列
    UnityEngine.GameObject[] boxiy;
    UnitControl[] humanoids;
    //オブジェクト読み込み用変数
    private UnityEngine.GameObject Objct;
    //GA配列部分 第一変数->オブジェクトの番号 第二変数->時間 第三変数->対応する関節角度
    int[][ , ] GA_list;
    //コピー用配列
    int[][ , ] next_GA_list;
    // Start is called before the first frame update
    void Start () {
        // unit1をGameObject型で取得
        Objct = Resources.Load ("unit1") as GameObject;
        count = 1;
        frame = 0;
        initiarize();
    }

    // Update is called once per frame(30fps, 1frame/0.033s)
    //TODO フレームごとに関節角度の動作
    //TODO フレームごとに落下判定
    //updateが呼ばれなかったので、こうした
    void execute(){
        //Debug.Log(frame+ ":フレーム数");
        bool f = true;
        if(frame == time){
            frame = 0;
            f = false;
            evaluate();
            return;
            //ワンチャン遅延させなきゃダメかも
        }
        for(int i = 0; i < object_num; i++){
            //落下判定、引数は自由に定義していい
            if(check()){
                result[i,0] = frame;
            }
        }
        for(int i = 0; i < object_num; i++){
            //関節動作、引数は自由に定義していい
            movement();
        }
        frame++;
        if(f){
            Invoke("execute", frame_ms);
        }
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
        return false;
    }

    //TODOどう評価するか
    void evaluate(){
        delete_objct();
        make_next_generation();
        next_gen();
        execute();
    }

    void delete_objct(){
        for(int i = 0; i < object_num; i++){
            Destroy(boxiy[i]);
        }
    }

    void make_next_generation(){
        sort_by_evaluation();
        make_next_unit();
        Debug.Log(count+ " 世代目");
        Debug.Log(result[0,0]+":最大エリート");
        //TODO　結果出力したい
        count++;
        if(count == generations+1){
            Debug.Log("フィニッシュ");
            return;
        }
    }

    //交叉、突然変異、次世代の生成
    void make_next_unit(){
        //次世代生成の準備
        next_GA_list = new int[object_num][,];
        for(int i = 0; i < elite; i++){
            int k = result[i,1];
            next_GA_list[i] = GA_list[k];
        }
        for(int i = elite; i < object_num; i++){
            next_GA_list[i] = new int[time, Range_list.Length];
        }

        int g = 0;
        //二点交叉(多分意味ない)ので確率で交叉、突然変異
        for(int i = 0; i < elite-1; i++){
            for(int l = i+1; l < elite; l++){
                for(int k = 0; k < time; k++){
                    int c = Random.Range(0, 10);
                    if(c < mutation){
                        for(int m = 0; m < Range_list.Length; m++){
                            //TODO 関節の挙動
                            next_GA_list[elite+g][k,m] = Random.Range(-1 * Range_list[m], Range_list[m]);
                        }
                    }else if(c >= mutation && (c-mutation) <= (c-mutation)/2){
                        for(int m = 0; m < Range_list.Length; m++){
                            next_GA_list[elite+g][k,m] = GA_list[i][k, m];
                        }
                    }else{
                        for(int m = 0; m < Range_list.Length; m++){
                            next_GA_list[elite+g][k,m] = GA_list[l][k, m];
                        }
                    }
                }
                g++;
            }
        }
        GA_list = next_GA_list;
    }

    void next_gen(){
        for(int i = 0; i < object_num; i++){
            result[i,0] = i * 1000;
            result[i,1] = i;
        }
        createobjects();
    }

    //初期設定
    void initiarize(){
        next_gen();

        //配列初期生成
        GA_list = new int[object_num][,];
        for(int i = 0; i < object_num; i++){
            GA_list[i] = new int[time, Range_list.Length];
        }
        for(int i = 0; i < object_num; i++){
            for(int l = 0; l < time; l++){
                for(int k = 0; k < Range_list.Length; k++){
                    //TODO 関節の挙動
                    GA_list[i][l,k] = Random.Range(-1 * Range_list[k], Range_list[k]);
                }
            }
        }

        execute();
    }
    //オブジェクト生成
    void createobjects(){
        boxiy = new UnityEngine.GameObject[object_num];
        humanoids = new UnitControl[object_num];
        for(int i = 0; i < object_num; i++){
            if(i % 2 == 0){
                float pos = (float)8.0 * (i / 2);
                boxiy[i] = Instantiate (Objct, new Vector3(pos,2.5f,0.0f), Quaternion.identity);
                humanoids[i] = new UnitControl(boxiy[i].transform.Find ("Robot Kyle").GetComponent<Animator>());
            }else{
                float pos = (float)8.0 * ((i+1) / 2) * -1;
                boxiy[i] = Instantiate (Objct, new Vector3(pos,2.5f,0.0f), Quaternion.identity);
            }
        }
    }

    void sort_by_evaluation(){
        //配列の回数分回すバブルソート
        for (int i = 0; i < object_num; i++)
        {
            //配列の回数分回す
            for (int j = i+1; j < object_num; j++)
            {
                //比較元より大きければ入れ替え
                if (result[i,0] < result[j,0])
                {
                    int x = result[j,0],y = result[j,1];
                    result[j,0] = result[i,0];
                    result[j,1] = result[i,1];
                    result[i,0] = x;
                    result[i,1] = y;
                }
            }
        }
    }
    void OnGUI() {
		string str = "";
		str += string.Format("Generation: {0}\n", this.count);
		str += string.Format("Frame: {0}\n", this.frame);
		str += string.Format("\n");
		str += string.Format("Best score\n");
		/*
        for(int i=0; i<10; ++i ) {
			str += string.Format("  {0:D2}: {1:F2}\n", bestScoreIds[i], bestScores[i]);
		}
        */

		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.black;
		GUI.Label(new Rect(10, 10, 100, 40), str, style);
	}
}