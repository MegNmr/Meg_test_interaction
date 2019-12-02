﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : SingletonDontDestroy<Fader>
{
    float fadeLevel = 0.0f;
    bool is_end = false;
    UnityEngine.UI.Image fadeImage = null;

    #region フェードレベルと色に合わせて更新

    protected override void Awake() {
        // 基底クラスのAwakeを呼び出す
        base.Awake();
        // Canvasに追加したImageのコンポーネントを取得
        if (fadeImage == null){
            fadeImage = GetComponent<UnityEngine.UI.Image>();
        }
        // Instance チェック
        if (Instance != this){
            Debug.Log("Fader : Other Object.");
        }
    }

    // フェードレベルを更新・反映
    void UpdateFade(float level)
    {
        enabled = true;
        // level を0.0～1.0に丸め込む
        fadeLevel = Mathf.Clamp(level, 0.0f, 1.0f);

        // level に合わせてalpha値を設定する
        Color col = fadeImage.color;
        col.a = level;
        fadeImage.color = col;

        // levelが0.0 のときは機能を停止する
        if (level <= 0.0f){
            enabled = false;
        }
    }

    // フェードレベルを設定
    public void SetFadeLevel(float level) {
        UpdateFade(level);
    }

    // フェードカラーを設定（ホワイト／ブラックなど）
    public void SetFadeColor(Color color) {
        fadeImage.color = color;
        UpdateFade(fadeLevel);
    }

    #endregion

    #region 時間とともにフェードイン・アウトを行う

    // コルーチン用：フェードインの処理
    public IEnumerator YieldFadeIn(float fadeTime = 1.0f)
    {
        yield return YieldUpdateFade(1.0f, 0.0f,fadeTime);
    }

    // コルーチン用：フェードアウトの処理
    public IEnumerator YieldFadeOut(float fadeTime = 1.0f)
    {
        yield return YieldUpdateFade(0.0f, 1.0f, fadeTime);
    }

    // コルーチン用：stVal ー＞ edVal まで fadeTime 時間かけてフェードさせる
    public IEnumerator YieldUpdateFade(float stVal, float edVal, float fadeTime)
    {
        is_end = false;
        if (fadeTime > 0.0f)
        {
            // 段階的に反映
            float time = 0.0f;
            while (time < fadeTime)
            {
                float val = Mathf.Lerp(stVal, edVal, time / fadeTime);
                UpdateFade(val);
                time += Time.deltaTime;
                yield return null;
            }
        }
        UpdateFade(edVal);
        is_end = true;
        yield return null;
    }

    /// <summary>
    /// Static関数：フェードイン・アウトが終了したか？
    /// </summary>
    static public bool IsEnd
    {
        get { return Instance.is_end; }
    }

    /// <summary>
    /// Static関数：フェードイン
    /// </summary>
    static public Coroutine FadeIn(float fadeTime = 1.0f)
    {
        return Instance.StartCoroutine(Instance.YieldFadeIn(fadeTime));
    }

    /// <summary>
    /// Static関数：フェードアウト
    /// </summary>
    static public Coroutine FadeOut(float fadeTime = 1.0f)
    {
        return Instance.StartCoroutine(Instance.YieldFadeOut(fadeTime));
    }

    #endregion

    #region シーン切り替え

    // コルーチン用：フェードイン・アウトを挟んで次のシーンへ
    public IEnumerator YieldSwitchScene(string sceneName, float fadeTime = 1.0f)
    {
        yield return YieldFadeOut(fadeTime);

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        yield return YieldFadeIn(fadeTime);
    }

    /// <summary>
    /// Static関数：フェードアウト・インを挟んでシーン切り替え
    /// </summary>
    static public Coroutine SwitchScene(string sceneName, float fadeTime = 1.0f)
    {
        return Instance.StartCoroutine(
            Instance.YieldSwitchScene(sceneName,fadeTime));
    }

    #endregion
}
