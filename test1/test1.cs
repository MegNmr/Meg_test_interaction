using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{

    // アイテムのデータを保持する辞書
    Dictionary<int, string> itemInfo;

    // 敵がドロップするアイテムの辞書
    Dictionary<int, float> itemDropDict;

    void Start()
    {
        GetDropItem();
    }

    void GetDropItem()
    {
        // 各種辞書の初期化
        InitializeDicts();

        // ドロップアイテムの抽選
        int itemId = Choose();

        // アイテムIDに応じたメッセージ出力
        if (itemId != 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " を入手した!");
        }
        else
        {
            Debug.Log("アイテムは入手できませんでした。");
        }
    }

    void InitializeDicts()
    {
        itemInfo = new Dictionary<int, string>();
        itemInfo.Add(0, "なし");
        itemInfo.Add(1, "竜のひげ");
        itemInfo.Add(2, "竜の爪");
        itemInfo.Add(3, "竜のうろこ");
        itemInfo.Add(4, "竜の翼");
        itemInfo.Add(5, "竜の逆鱗");
        itemInfo.Add(6, "竜の紅玉");

        itemDropDict = new Dictionary<int, float>();
        itemDropDict.Add(0, 60.0f);
        itemDropDict.Add(2, 25.0f);
        itemDropDict.Add(3, 12.0f);
        itemDropDict.Add(5, 3.0f);
    }

    int Choose()
    {
        // 確率の合計値を格納
        float total = 0;

        // 敵ドロップ用の辞書からドロップ率を合計する
        foreach (KeyValuePair<int, float> elem in itemDropDict)
        {
            total += elem.Value;
        }

        // Random.valueでは0から1までのfloat値を返すので
        // そこにドロップ率の合計を掛ける
        float randomPoint = Random.value * total;

        // randomPointの位置に該当するキーを返す
        foreach (KeyValuePair<int, float> elem in itemDropDict)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint -= elem.Value;
            }
        }
        return 0;
    }
}

/*
//probsは確率表記をいれた配列[50,30,10,5]

float Choose(float[] probs)
{

    float total = 0;

    foreach (float elem in probs)
    {
        total += elem;  //合計値に足し込み
    }

    float randomPoint = Random.value * total;//ガチャの値の合計値内相対的位置を算出

    for (int i = 0; i < probs.Length; i++)
    {
        if (randomPoint < probs[i]) //左から当たったガチャ内容を探索
        {
            return i;
        }
        else
        {
            randomPoint -= probs[i];
        }
    }
    return probs.Length - 1;
}

*/
