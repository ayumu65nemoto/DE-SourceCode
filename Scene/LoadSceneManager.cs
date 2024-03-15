using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager loadSceneManager;
    //　シーン移動に関するデータファイル
    [SerializeField]
    private SceneMove sceneMove = null;
    //　フェードプレハブ
    [SerializeField]
    private GameObject fadePrefab = null;
    //　フェードインスタンス
    private GameObject fadeInstance;
    //　フェードの画像
    private Image fadeImage;
    [SerializeField]
    private float fadeSpeed = 5f;
    //　遷移中かどうか
    [SerializeField]
    private bool _isTransition = false;
    //　1つ前のシーン
    private string _previousScene;

    public bool IsTransition { get => _isTransition; set => _isTransition = value; }
    public string PreviousScene { get => _previousScene; set => _previousScene = value; }

    private void Awake()
    {
        // LoadSceneMangerは常に一つだけにする
        if (loadSceneManager == null)
        {
            loadSceneManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //　次のシーンを呼び出す
    public void GoToNextScene(SceneMove.SceneType scene)
    {
        sceneMove.SetSceneType(scene);
        StartCoroutine(FadeAndLoadScene(scene));
        IsTransition = true;
    }

    //　フェードをした後にシーン読み込み
    IEnumerator FadeAndLoadScene(SceneMove.SceneType scene)
    {
        //　フェードUIのインスタンス化
        fadeInstance = Instantiate<GameObject>(fadePrefab);
        fadeImage = fadeInstance.GetComponentInChildren<Image>();
        //　フェードアウト処理
        yield return StartCoroutine(Fade(1f));

        //　シーンの読み込み
        if (scene == SceneMove.SceneType.FirstVillage)
        {
            yield return StartCoroutine(LoadScene("Map1"));
        }
        else if (scene == SceneMove.SceneType.FirstVillageToRoom)
        {
            yield return StartCoroutine(LoadScene("Room"));
        }
        else if (scene == SceneMove.SceneType.RoomToFirstVillage)
        {
            yield return StartCoroutine(LoadScene("Map1"));
        }
        //else if (scene == SceneMove.SceneType.FirstVillageToWorldMap)
        //{
        //    yield return StartCoroutine(LoadScene("WorldMap"));
        //}
        //else if (scene == SceneMove.SceneType.WorldMapToBattle)
        //{
        //    yield return StartCoroutine(LoadScene("Battle"));
        //}
        //else if (scene == SceneMove.SceneType.BattleToWorldMap)
        //{
        //    yield return StartCoroutine(LoadScene("WorldMap"));
        //}
        //else if (scene == SceneMove.SceneType.VillageToBattle)
        //{
        //    yield return StartCoroutine(LoadScene("Battle"));
        //}
        //else if (scene == SceneMove.SceneType.BattleToVillage)
        //{
        //    yield return StartCoroutine(LoadScene("Map1"));
        //}
        else if (scene == SceneMove.SceneType.StartGame)
        {
            yield return StartCoroutine(LoadScene("Map1"));
        }

        //　フェードUIのインスタンス化
        fadeInstance = Instantiate<GameObject>(fadePrefab);
        fadeImage = fadeInstance.GetComponentInChildren<Image>();
        fadeImage.color = new Color(0f, 0f, 0f, 1f);

        //　フェードイン処理
        yield return StartCoroutine(Fade(0f));
        
        Destroy(fadeInstance);
        IsTransition = false;
    }

    //　フェード処理
    IEnumerator Fade(float alpha)
    {
        var fadeImageAlpha = fadeImage.color.a;

        while (Mathf.Abs(fadeImageAlpha - alpha) > 0.01f)
        {
            if (fadeImage == null)
            {
                break;
            }
            fadeImageAlpha = Mathf.Lerp(fadeImageAlpha, alpha, fadeSpeed * Time.deltaTime);
            fadeImage.color = new Color(0f, 0f, 0f, fadeImageAlpha);
            yield return null;
        }
    }

    //　実際にシーンを読み込む処理
    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        _previousScene = SceneManager.GetActiveScene().name;

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
