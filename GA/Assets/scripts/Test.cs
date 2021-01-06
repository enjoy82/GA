using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Test : MonoBehaviour
{
    const int elite = 8;
    //オブジェクトの個数(1世代当たりのこの数)、進化させる世代数、遺伝で使う時間(per frame), 突然変異の確率
    const int object_num = elite + ((elite * (elite+1)) / 2), generations = 1000, time = 1000, mutation = 10;
    const float frame_ms = 0.02f;
    //結果格納部分(point,index)
    int[,] result = new int[object_num,2];
    //現在の世代数
    public int count;
    //経過したフレーム数
    public int frame;
    //遺伝子の長さ
    public int gene_length = 180;
    //子を格納する配列
    protected GameObject[] boxiy;
    protected UnitControl[] unitControl = new UnitControl[object_num];
    //オブジェクト読み込み用変数
    private UnityEngine.GameObject Objct;
    //GA配列部分 humanoidの角度(配列番号0->-90度、配列番号89->0度、配列番号179->90度)
    bool[][] GA_list;
    //コピー用配列
    bool[][] next_GA_list;
    // Start is called before the first frame update
    void Start () {
        // unit1をGameObject型で取得
        Objct = Resources.Load ("unit1") as GameObject;
        count = 1;
        frame = 0;
        //初期gene生成
        GA_list = new bool[object_num][];
        next_GA_list = new bool[object_num][];
        for(int i = 0; i < object_num; i++){
            bool[] list = new bool[gene_length];
            for(int j = 0;j<gene_length;j++){
                list[j] = (UnityEngine.Random.value > 0.5f);
            }
            GA_list[i] = list;
        }
        initiarize();
    }

    // Update is called once per frame(30fps, 1frame/0.033s)
    //updateが呼ばれなかったので、こうした →これでもダメ？
    void Update(){
        //Debug.Log(frame+ ":フレーム数");
        if(frame == time){
            frame = 0;
            evaluate();
            return;
            //ワンチャン遅延させなきゃダメかも
        }
        frame++;
    }

    //初期設定
    void initiarize(){
        //結果の初期化
        for(int i = 0; i < object_num; i++){
            result[i,0] = 0;
            result[i,1] = i;
        }
        createobjects();
    }

    //オブジェクト生成
    void createobjects(){
        boxiy = new GameObject[object_num];
        unitControl = new UnitControl[object_num];
        for(int i = 0; i < object_num; i++){
            if(i % 2 == 0){
                float pos = (float)8.0 * (i / 2);
                boxiy[i] = Instantiate (Objct, new Vector3(pos,2.5f,0.0f), Quaternion.identity) as GameObject;
                //Debug.Log(boxiy[i].transform.GetChild(0).gameObject.GetComponent<UnitControl>());
                unitControl[i] = boxiy[i].transform.GetChild(0).gameObject.GetComponent<UnitControl>();
                //Debug.Log(GameObject.Find("Plane") as GameObject);
                unitControl[i].plane = GameObject.Find("Plane");
                //Debug.Log(boxiy[i].transform.GetChild(2).gameObject);
                unitControl[i].cube = boxiy[i].transform.GetChild(2).gameObject;
                //遺伝子の代入
                unitControl[i].gene = GA_list[i];

            }else{
                float pos = (float)8.0 * ((i+1) / 2) * -1;
                boxiy[i] = Instantiate (Objct, new Vector3(pos,2.5f,0.0f), Quaternion.identity);
                unitControl[i] = boxiy[i].transform.GetChild(0).gameObject.GetComponent<UnitControl>();
                unitControl[i].plane = GameObject.Find("Plane");
                unitControl[i].cube = boxiy[i].transform.GetChild(2).gameObject;
                //遺伝子の代入
                unitControl[i].gene = GA_list[i];
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
        //TODO　結果出力したい
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
                point += 10000;
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

    int[] shuffle(int max,int length){
        return Enumerable.Range(0, max).OrderBy(n => Guid.NewGuid()).Take(length).ToArray();
    }

    //交叉、突然変異、次世代の生成
    void make_next_unit(){
        //次世代生成の準備
        next_GA_list = new bool[object_num][];
        //エリートのコピー
        for(int i = 0; i < elite; i++){
            int k = result[i,1];
            next_GA_list[i] = GA_list[k];
        }
        //子孫の生成
        for(int i = elite; i < object_num; i++){
            bool[] generate_gene = new bool[gene_length];
            int[] selected_parent = shuffle(elite,2);
            //Debug.Log(string.Join(",",selected_parent));
            bool[] parent1 = GA_list[selected_parent[0]];
            bool[] parent2 = GA_list[selected_parent[1]];
            int[] selected_gene = shuffle(gene_length,gene_length);
            //Debug.Log(string.Join(",",selected_gene));
            //Debug.Log(selected_gene[gene_length-1]);
            for(int j = 0;j<(int)(gene_length/2);j++){
                generate_gene[selected_gene[j]] = parent1[selected_gene[j]];
            }
            for(int j = (int)(gene_length/2);j<gene_length;j++){
                generate_gene[selected_gene[j]] = parent2[selected_gene[j]];
            }
            next_GA_list[i] = generate_gene;
        }
        //突然変異10%
        for(int i = 0; i<object_num; i++){
            int[] selected_gene = shuffle(gene_length-1,(int)(gene_length * mutation/100) );
            for(int j = 0; j < (int)(gene_length*mutation/100);j++){
                next_GA_list[i][selected_gene[j]] = !(next_GA_list[i][selected_gene[j]]);
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
}