using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager loadSceneManager;
    //�@�V�[���ړ��Ɋւ���f�[�^�t�@�C��
    [SerializeField]
    private SceneMove sceneMove = null;
    //�@�t�F�[�h�v���n�u
    [SerializeField]
    private GameObject fadePrefab = null;
    //�@�t�F�[�h�C���X�^���X
    private GameObject fadeInstance;
    //�@�t�F�[�h�̉摜
    private Image fadeImage;
    [SerializeField]
    private float fadeSpeed = 5f;
    //�@�J�ڒ����ǂ���
    [SerializeField]
    private bool _isTransition = false;
    //�@1�O�̃V�[��
    private string _previousScene;

    public bool IsTransition { get => _isTransition; set => _isTransition = value; }
    public string PreviousScene { get => _previousScene; set => _previousScene = value; }

    private void Awake()
    {
        // LoadSceneManger�͏�Ɉ�����ɂ���
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

    //�@���̃V�[�����Ăяo��
    public void GoToNextScene(SceneMove.SceneType scene)
    {
        sceneMove.SetSceneType(scene);
        StartCoroutine(FadeAndLoadScene(scene));
        IsTransition = true;
    }

    //�@�t�F�[�h��������ɃV�[���ǂݍ���
    IEnumerator FadeAndLoadScene(SceneMove.SceneType scene)
    {
        //�@�t�F�[�hUI�̃C���X�^���X��
        fadeInstance = Instantiate<GameObject>(fadePrefab);
        fadeImage = fadeInstance.GetComponentInChildren<Image>();
        //�@�t�F�[�h�A�E�g����
        yield return StartCoroutine(Fade(1f));

        //�@�V�[���̓ǂݍ���
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

        //�@�t�F�[�hUI�̃C���X�^���X��
        fadeInstance = Instantiate<GameObject>(fadePrefab);
        fadeImage = fadeInstance.GetComponentInChildren<Image>();
        fadeImage.color = new Color(0f, 0f, 0f, 1f);

        //�@�t�F�[�h�C������
        yield return StartCoroutine(Fade(0f));
        
        Destroy(fadeInstance);
        IsTransition = false;
    }

    //�@�t�F�[�h����
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

    //�@���ۂɃV�[����ǂݍ��ޏ���
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
