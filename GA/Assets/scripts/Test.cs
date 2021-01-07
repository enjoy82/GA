using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Text;


public class Test : MonoBehaviour
{
    const int elite = 8;
    //オブジェクトの個数(1世代当たりのこの数)、進化させる世代数、遺伝で使う時間(per frame), 突然変異の確率
    const int object_num = elite + (elite/2 * (elite-1)), generations = 1000, time = 250, mutation = 10;
    const float frame_ms = 0.04f;
    //結果格納部分(point,index)
    int[,] result = new int[object_num,2];
    //現在の世代数
    public int count;
    //経過したフレーム数
    public int frame;
    //遺伝子の長さ
    const int gene_length = time;
    //子を格納する配列
    protected GameObject[] boxiy;
    protected UnitControl[] unitControl = new UnitControl[object_num];
    //オブジェクト読み込み用変数
    private UnityEngine.GameObject Objct;
    //GA配列部分
    int[][,] GA_list;
    //コピー用配列
    int[][,] next_GA_list;
    const int max_power = 8;
    // Start is called before the first frame update
    void Start () {
        // unit1をGameObject型で取得
        Objct = Resources.Load ("unit1") as GameObject;
        count = 1;
        frame = 0;
        //初期gene生成
        GA_list = new int[object_num][,];
        next_GA_list = new int[object_num][,];
        for(int i = 0; i < object_num; i++){
            int[,] list = new int[gene_length,2];
            for(int j = 0;j<gene_length;j++){
                list[j,0] = Random.Range(0, 6);
                list[j,1] = Random.Range((-max_power), max_power);
            }
            GA_list[i] = list;
        }
        initiarize();
    }

    // Update is called once per frame
    void execute(){
        //Debug.Log(frame+ ":フレーム数");
        if(frame == time){
            frame = 0;
            evaluate();
            return;
        }
        for(int i = 0; i < object_num; i++){
            //動作させる(方向と角度変異)
            //Debug.Log(GA_list[i].Length);
            unitControl[i].movement(GA_list[i][frame, 0], GA_list[i][frame, 1]);
        }
        frame++;
        Invoke("execute", frame_ms);
    }

    //初期設定
    void initiarize(){
        //結果の初期化
        for(int i = 0; i < object_num; i++){
            result[i,0] = 0;
            result[i,1] = i;
        }
        createobjects();
        execute();
    }

    //オブジェクト生成
    void createobjects(){
        boxiy = new GameObject[object_num];
        unitControl = new UnitControl[object_num];
        for(int i = 0; i < object_num; i++){
            if(i % 2 == 0){
                float pos = (float)20.0 * (i / 2);
                boxiy[i] = Instantiate (Objct, new Vector3(pos,2.5f,0.0f), Quaternion.identity) as GameObject;
                //Debug.Log(boxiy[i].transform.GetChild(0).gameObject.GetComponent<UnitControl>());
                unitControl[i] = boxiy[i].transform.GetChild(0).gameObject.GetComponent<UnitControl>();
                //Debug.Log(GameObject.Find("Plane") as GameObject);
                unitControl[i].plane = GameObject.Find("Plane");
                //Debug.Log(boxiy[i].transform.GetChild(2).gameObject);
                unitControl[i].cube = boxiy[i].transform.GetChild(2).gameObject;

            }else{
                float pos = (float)20.0 * ((i+1) / 2) * -1;
                boxiy[i] = Instantiate (Objct, new Vector3(pos,2.5f,0.0f), Quaternion.identity);
                unitControl[i] = boxiy[i].transform.GetChild(0).gameObject.GetComponent<UnitControl>();
                unitControl[i].plane = GameObject.Find("Plane");
                unitControl[i].cube = boxiy[i].transform.GetChild(2).gameObject;
            }
        }
    }

    void evaluate(){
        make_next_generation();

        //オブジェクト削除
        for(int i = 0; i < object_num; i++){
            Destroy(boxiy[i]);
        }
        initiarize();
    }

    void make_next_generation(){
        sort_by_evaluation();
        make_next_unit();
        Debug.Log(count+ " 世代目");
        Debug.Log(result[0,0]+":最大エリート");
        textSave(result[0,0]);
        count++;
        if(count == generations+1){
            Debug.Log("フィニッシュ");
            return;
        }
    }

    void sort_by_evaluation(){
        //pointの加算とresultへのコピー
        for(int i = 0; i< object_num;i++){
            float point = unitControl[i].points;
            if(unitControl[i].isTraining){
                //生存ボーナス
                point += 1000000000;
            }
            result[i,0] = (int)point;
        }
        //resultをソート
        //配列の回数分回すバブルソート
        for (int i = 0; i < object_num; i++)
        {
            //配列の回数分回す
            for (int j = i+1; j < object_num; j++)
            {
                //比較元より大きければ入れ替え
                if (result[i,0] < result[j,0])
                {
                    int x = result[j,0];
                    int y = result[j,1];
                    result[j,0] = result[i,0];
                    result[j,1] = result[i,1];
                    result[i,0] = x;
                    result[i,1] = y;
                }
            }
        }
    }

    //交叉、突然変異、次世代の生成
    void make_next_unit(){
        //次世代生成の準備
        next_GA_list = new int[object_num][,];
        //エリートのコピー
        for(int i = 0; i < elite; i++){
            int k = result[i,1];
            next_GA_list[i] = GA_list[k];
        }
        //TODO 子孫の生成 一様交差
        int num = 0;
        for(int i = 0; i < elite-1; i++){
            for(int l = i+1; l < elite; l++){
                next_GA_list[num + elite] = new int[gene_length, 2];
                for(int j = 0; j < gene_length; j++){
                    int ran = Random.Range(0, 100);
                    if(ran < mutation){
                        next_GA_list[num+elite][j, 0] = Random.Range(0, 6);
                        next_GA_list[num+elite][j, 1] = Random.Range((-max_power), max_power);
                    }else if(ran - mutation < (ran - mutation)/2){
                        next_GA_list[num+elite][j, 0] = GA_list[i][j, 0];
                        next_GA_list[num+elite][j, 1] = GA_list[i][j, 1];
                    }else{
                        next_GA_list[num+elite][j, 0] = GA_list[i][l, 0];
                        next_GA_list[num+elite][j, 1] = GA_list[i][l, 1];
                    }
                }
                num++;
            }
        }

        GA_list = next_GA_list;
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

    public void textSave(int txt){
        StreamWriter sw = new StreamWriter("./Assets/Log/log.csv",true);
        sw.WriteLine(txt);
        sw.Flush();
        sw.Close();
    }
}