using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDefine
{
    /// <summary>
    /// 設定言語
    /// </summary>
    public enum Language
    {
        JA,
        EN
    }

    public enum ItemParamType
    {
        NONE = -1,   // 初期値
        ENVIRONMENT,    // 環境
        GENDER, // 性別
        VILLAIN,    // 悪人善人
        EVENT,  // 出来事
        GOAL,   // 目的地
        HOW_TO_WARP,    // 方法
        DIFICCULT,  // 難易度
        EVALUATION  //評価
    }

    // DeepLのAPIKEY
    public const string DEEPL_API_KEY = "6e4382ec-3c39-0c93-36ee-3fcfd158c6a6:fx";
    // DeepLのAPI Endpoint
    public const string DEEPL_ENDPOINT = "https://api-free.deepl.com/v2/translate?auth_key=";
    // ChatGPTのAPI Endpoint
    public const string CHAT_GPT_API_END_POINT = "https://api.openai.com/v1/completions";

    public static string chatGptDefaultText
    = "数字なしで[#0]の気候を持つ惑星の風景を描写する。" +
    "数字なしでその惑星に住む[#1]の自分の性的正体生を持つ主人公の外見を描写する。" +
    "数字なしで[#2]の価値観を持っている主人公の日常を描写する。" +
    "数字なしでこの惑星で主人公に起きた[#3]出来事と旅を決心する。" +
    "数字なしで主人公が[#4]の距離がある目的地に行きたい理由を説明する。" +
    "数字なしで主人公が目的地まで旅する[#5]な方法を説明する。" +
    "数字なしで主人公が目的地まで[#6]の難易度で旅する過程を描写する。" +
    "数字なしでこの世界のことを見守っている世界樹が主人公に[#7]の評価をする理由を説明する。" +
    "200文字以下ですべての答えに数字を使わないで教えてください。";

}
