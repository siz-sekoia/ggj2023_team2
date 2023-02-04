using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[DefaultExecutionOrder (-100)]
public class AudioManager : App.SingletonMonoBehaviour<AudioManager> {
    private Dictionary<string, AudioSource> BGMSource = new Dictionary<string, AudioSource> ();
    private AudioSource SESource = null;
    private Dictionary<string, AudioClip> SEClip = new Dictionary<string, AudioClip> ();

    protected override void Awake () {
        base.Awake ();

        RegisterAudioData ();
    }

    private void RegisterAudioData () {
        object[] bgmData = Resources.LoadAll ("Audio/BGM");
        object[] seData = Resources.LoadAll ("Audio/SE");

        foreach (AudioClip bgm in bgmData) {
            var audioSource = gameObject.AddComponent<AudioSource> ();
            BGMSource[bgm.name] = audioSource;
            BGMSource[bgm.name].clip = bgm;
        }

        foreach (AudioClip se in seData) {
            SEClip[se.name] = se;
        }

        SESource = gameObject.AddComponent<AudioSource> ();
    }

    /// <summary>
    /// seをならす
    /// </summary>
    public void PlaySE (string name, float volume = 1) {
        if (!SEClip.ContainsKey (name)) return;

        SESource.PlayOneShot (SEClip[name] as AudioClip, volume);
    }

    /// <summary>
    /// bgmをならす
    /// </summary>
    public void PlayBGM (string name, bool isLoop = true, float volume = 1) {
        if (!BGMSource.ContainsKey (name)) return;
        if (BGMSource[name].isPlaying) return;

        BGMSource[name].loop = isLoop;
        BGMSource[name].volume = volume;
        BGMSource[name].Play ();
    }

    /// <summary>
    /// bgmを止める
    /// </summary>
    public void StopBGM (string name) {
        if (!BGMSource.ContainsKey (name)) return;
        if (!BGMSource[name].isPlaying) return;

        BGMSource[name].Stop ();
    }

    public float[] GetBGMSpectrumData (string name, int numSumples) {
        if (!BGMSource.ContainsKey (name)) return null;

        float[] spectrum = new float[numSumples];
        BGMSource[name].GetSpectrumData (spectrum, 0, FFTWindow.BlackmanHarris);
        return spectrum;
    }

    /// <summary>
    /// 指定した拍からどれくらいずれているかを取得(秒)
    /// </summary>
    public float GetGapFromBeat (string name, float targetBeat) {
        if (!BGMSource.ContainsKey (name)) return -1;

        float bpm = ConvertNameToBPM (name);
        if (bpm == -1) return -1;

        return Mathf.Abs (BGMSource[name].time - 1 / (bpm / 60) * targetBeat);
    }

    /// <summary>
    /// 指定した曲の1拍の長さを取得(秒)
    /// </summary>
    public float GetBeatLength (string name) {
        if (!BGMSource.ContainsKey (name)) return -1;

        float bpm = ConvertNameToBPM (name);
        if (bpm == -1) return -1;

        return (bpm / 60);
    }

    /// <summary>
    /// 指定した曲の現在の再生時間を取得
    /// </summary>
    public float GetTime (string name) {
        if (!BGMSource.ContainsKey (name)) return -1;
        return BGMSource[name].time;
    }

    private float ConvertNameToBPM (string name) {
        string[] nameSplit = name.Split ('_');
        string bpm = nameSplit[1];
        if (bpm is null) return -1;
        return int.Parse (bpm);
    }

    /// <summary>
    /// bgmをフェードインする
    /// </summary>
    public async void FadeInBGM (string name, bool isLoop = true, float volume = 1) {
        if (!BGMSource.ContainsKey (name)) return;
        if (BGMSource[name].isPlaying) return;

        BGMSource[name].loop = isLoop;
        BGMSource[name].volume = 0;
        BGMSource[name].Play ();

        while (true) {
            await Task.Delay (10);
            BGMSource[name].volume += 0.01f;
            if (BGMSource[name].volume >= volume) {
                BGMSource[name].volume = volume;
                break;
            }
        }
    }

    /// <summary>
    /// bgmをフェードアウトする
    /// </summary>
    public async void FadeOutBGM (string name, float volume = 0) {
        if (!BGMSource.ContainsKey (name)) return;
        if (!BGMSource[name].isPlaying) return;

        while (true) {
            await Task.Delay (10);
            BGMSource[name].volume -= 0.01f;
            if (BGMSource[name].volume <= volume) {
                BGMSource[name].volume = volume;
                BGMSource[name].Stop ();
                break;
            }
        }
    }

    /// <summary>
    /// bgmをフェードアウトする(コルーチン版)
    /// TODO:仮置き
    /// </summary>
    public void FadeOutBGMCol (string name, float targetVolume = 0, float updateSpeed = 0.01f) {
        if (!BGMSource.ContainsKey (name)) return;

        var volume = BGMSource[name].volume;
        volume -= updateSpeed;
        BGMSource[name].volume = volume;
        if (volume <= targetVolume) {
            BGMSource[name].volume = targetVolume;
            StopBGM (name);
        } else {
            StartCoroutine (WaitOneFrame (() => FadeOutBGMCol (name, targetVolume, updateSpeed)));
        }
    }

    /// <summary>
    /// bgmをフェードインする(コルーチン版)
    /// TODO:仮置き
    /// </summary>
    public void FadeInBGMCol (string name, bool isLoop = true, float targetVolume = 1, float updateSpeed = 0.01f) {
        if (!BGMSource.ContainsKey (name)) return;
        if (!BGMSource[name].isPlaying) {
            PlayBGM (name, isLoop, 0);
        }

        BGMSource[name].volume += updateSpeed;
        if (BGMSource[name].volume >= targetVolume) {
            BGMSource[name].volume = targetVolume;
        } else {
            StartCoroutine (WaitOneFrame (() => FadeInBGMCol (name, isLoop, targetVolume, updateSpeed)));
        }
    }

    private IEnumerator WaitOneFrame (System.Action onComplete) {
        yield return null;
        onComplete ();
    }
}