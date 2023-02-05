using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDefine
{
    /// <summary>
    /// �ݒ茾��
    /// </summary>
    public enum Language
    {
        JA,
        EN
    }

    public enum ItemParamType
    {
        NONE = -1,   // �����l
        ENVIRONMENT,    // ��
        GENDER, // ����
        VILLAIN,    // ���l�P�l
        EVENT,  // �o����
        GOAL,   // �ړI�n
        HOW_TO_WARP,    // ���@
        DIFICCULT,  // ��Փx
        EVALUATION  //�]��
    }

    // DeepL��APIKEY
    public const string DEEPL_API_KEY = "6e4382ec-3c39-0c93-36ee-3fcfd158c6a6:fx";
    // DeepL��API Endpoint
    public const string DEEPL_ENDPOINT = "https://api-free.deepl.com/v2/translate?auth_key=";
    // ChatGPT��API Endpoint
    public const string CHAT_GPT_API_END_POINT = "https://api.openai.com/v1/completions";

    public static string chatGptDefaultText
    = "�����Ȃ���[#0]�̋C������f���̕��i��`�ʂ���B" +
    "�����Ȃ��ł��̘f���ɏZ��[#1]�̎����̐��I���̐�������l���̊O����`�ʂ���B" +
    "�����Ȃ���[#2]�̉��l�ς������Ă����l���̓����`�ʂ���B" +
    "�����Ȃ��ł��̘f���Ŏ�l���ɋN����[#3]�o�����Ɨ������S����B" +
    "�����Ȃ��Ŏ�l����[#4]�̋���������ړI�n�ɍs���������R���������B" +
    "�����Ȃ��Ŏ�l�����ړI�n�܂ŗ�����[#5]�ȕ��@���������B" +
    "�����Ȃ��Ŏ�l�����ړI�n�܂�[#6]�̓�Փx�ŗ�����ߒ���`�ʂ���B" +
    "�����Ȃ��ł��̐��E�̂��Ƃ�������Ă��鐢�E������l����[#7]�̕]�������闝�R���������B" +
    "200�����ȉ��ł��ׂĂ̓����ɐ������g��Ȃ��ŋ����Ă��������B";

    public static string default_story
    = "�f���̕��i�́A�M�т̕��͋C���Y���Ă��܂��B�z�C�ȐF������ۓI�ȐA���������΂�A�M�������������܂��B\n" +
    "��l���́A���I�ȓ������������������ł��B�ނ�́A�؂₩�ȐF�ʂƑ@�ׂȓ����������Ă��܂��B\n" +
    "��l���̓���́A�Ƒ���F�l�Ƃ̏[�������֌W���؂ɂ��邱�Ƃ������I�ł��B�ނ�́A�l�Ɛl�Ƃ̐M���֌W���؂ɂ��Ă��܂��B\n" +
    "��l���ɂ́A���ʂȏo�������N����܂����B���̏o�����ɂ��A�ނ�͉����ꏊ�ւ̗������ӂ��܂����B\n" +
    "��l���́A�ړI�n�ւ̗��s��ʂ��ĐV���Ȍo���𓾂����Ǝv���Ă��܂��B�ނ�́A�����n�ւ̒T���S�������������ł��B\n" +
    "��l���́A���������ċ���Ԃ��Ƃɂ���ĖړI�n�ւ̗��s�������߂܂��B�ނ�́A����Ԃ��Ƃ���D���ł��B\n" +
    "���̉ߒ��́A�����댯�������܂����A��l���͗E���ɐi�݂܂��B�ނ�́A��������z���ĖړI�n�֓��B���邽�߂ɓw�͂��܂��B\n" +
    "���E���́A��l���̗���������Ă��܂��B�ނ�́A��l�����V���Ȓm����o���𓾂邱�Ƃ��ł��邱�Ƃ�]��ł��܂��B\n";
}
