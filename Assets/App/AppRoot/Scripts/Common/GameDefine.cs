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
        NONE,   // 初期値
        ENVIRONMENT,    // 環境
        GENDER, // 性別
        VILLAIN,    // 悪人善人
        EVENT,  // 出来事
        EVENT_FIRST_RESULT, // 受け入れ方
        GOAL,   // 目的地
        HOW_TO_WARP,    // 方法
        NECESSARY,  // 必要なもの
        DIFICCULT,  // 難易度
        EVENT_SECOND_WARP,  // 達成度
        CHANGE, //  変化
        EVALUATION  //評価
    }

    // DeepLのAPIKEY
    public const string DEEPL_API_KEY = "6e4382ec-3c39-0c93-36ee-3fcfd158c6a6:fx";
    // DeepLのAPI Endpoint
    public const string DEEPL_ENDPOINT = "https://api-free.deepl.com/v2/translate?auth_key=";
    // ChatGPTのAPI Endpoint
    public const string CHAT_GPT_API_END_POINT = "https://api.openai.com/v1/completions";

}
