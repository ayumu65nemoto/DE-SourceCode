using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class BattleResult : MonoBehaviour
{
    //　結果を表示してからワールドマップに戻れるようになるまでの時間
    [SerializeField]
    private float _timeToDisplay = 3f;
    [SerializeField]
    private GameObject _resultPanel;
    [SerializeField]
    private TextMeshProUGUI _resultText;
    [SerializeField]
    private TextMeshProUGUI _finishText;
    [SerializeField]
    private PlayerStatus _playerStatus;
    //　戦闘結果表示をしているかどうか
    private bool _isDisplayResult;
    //　結果を表示し戦闘から抜け出せるかどうか
    private bool _isFinishResult;
    //　戦闘に勝利したかどうか
    private bool _won;
    //　逃げたかどうか
    private bool _ranAway;
    //　戦闘結果テキストのスクロール値
    [SerializeField]
    private float _scrollValue = 50f;
    //　MusicManager
    //[SerializeField]
    //private MusicManager _musicManager;
    // ボタン連打を防ぐ
    private bool _lockInput = false;

    void Update()
    {
        //　結果表示前は何もしない
        if (!_isDisplayResult)
        {
            return;
        }

        //　結果表示後は結果表示テキストをスクロールして見れるようにする
        if (Input.GetAxis("Vertical") != 0f)
        {
            _resultText.transform.localPosition += new Vector3(0f, -Input.GetAxis("Vertical") * _scrollValue, 0f);
        }
        //　戦闘を抜け出すまでの待機時間を越えていない
        if (!_isFinishResult)
        {
            return;
        }
        //　SubmitやActionやFire1ボタンを押したらワールドマップに戻る
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_lockInput == true)
            {
                return;
            }

            if (_won || _ranAway)
            {
                //if (LoadSceneManager.loadSceneManager.PreviousScene == "Map1")
                //{
                //    _lockInput = true;
                //    LoadSceneManager.loadSceneManager.GoToNextScene(SceneMove.SceneType.BattleToVillage);
                //}
                //else
                //{
                //    _lockInput = true;
                //    LoadSceneManager.loadSceneManager.GoToNextScene(SceneMove.SceneType.BattleToWorldMap);
                //}
            }
            else
            {
                _lockInput = true;
                LoadSceneManager.loadSceneManager.GoToNextScene(SceneMove.SceneType.FirstVillage);
            }
        }
    }

    //　勝利時の初期処理
    public void InitialProcessingOfVictoryResult(List<GameObject> allCharacterList, GameObject playerInBattle)
    {
        StartCoroutine(DisplayVictoryResult(allCharacterList, playerInBattle));
    }

    //　勝利時の結果
    public IEnumerator DisplayVictoryResult(List<GameObject> allCharacterList, GameObject playerInBattle)
    {
        yield return new WaitForSeconds(_timeToDisplay);
        _won = true;
        _resultPanel.SetActive(true);
        //　戦闘で獲得した経験値
        int earnedExperience = 0;
        //　戦闘で獲得したお金
        int earnedMoney = 0;
        //　戦闘で獲得したアイテムとその個数
        Dictionary<Item, int> getItemDictionary = new Dictionary<Item, int>();
        //　Floatのランダム値
        float randomFloat;
        //　アイテム取得確率
        float probability;
        //　キャラクターステータス
        CharacterStatus characterStatus;
        //　敵のアイテムディクショナリー
        ItemDictionary enemyItemDictionary;

        foreach (var character in allCharacterList)
        {
            characterStatus = character.GetComponent<CharacterBattleScript>().CharacterStatus;
            if (characterStatus as EnemyStatus != null)
            {
                earnedExperience += ((EnemyStatus)characterStatus).GetExperience;
                earnedMoney += ((EnemyStatus)characterStatus).GetMoney;
                enemyItemDictionary = ((EnemyStatus)characterStatus).DropItemDictionary;
                //　敵が持っているアイテムの種類の数だけ繰り返し
                foreach (var item in enemyItemDictionary.Keys)
                {
                    //　0～100の間のランダム値を取得
                    randomFloat = Random.Range(0f, 100f);
                    //　アイテムの取得確率を取得
                    probability = enemyItemDictionary[item];
                    //　ランダム値がアイテム取得確率以下の値であればアイテム取得
                    if (randomFloat <= probability)
                    {
                        if (getItemDictionary.ContainsKey(item))
                        {
                            getItemDictionary[item]++;
                        }
                        else
                        {
                            getItemDictionary.Add(item, 1);
                        }
                    }
                }
            }
        }
        _resultText.text = earnedExperience + "の経験値を獲得した。\n";
        _resultText.text += earnedMoney + "のお金を獲得した。\n";

        //　パーティーステータスにお金を反映する
        _playerStatus.Money += earnedMoney;

        //　取得したアイテムを味方パーティーに分配する
        foreach (var item in getItemDictionary.Keys)
        {
            //　キャラクターが既にアイテムを持っている時
            if (_playerStatus.ItemDictionary.ContainsKey(item))
            {
                _playerStatus.ItemDictionary[item] += getItemDictionary[item];
            }
            else
            {
                _playerStatus.ItemDictionary.Add(item, getItemDictionary[item]);
            }
            _resultText.text += item.ItemName + "を" + getItemDictionary[item] + "個手に入れた。\n";
            _resultText.text += "\n";
        }

        StartCoroutine(LevelUpCharacter(playerInBattle, earnedExperience));
    }

    //　レベルアップ処理
    private IEnumerator LevelUpCharacter(GameObject playerInBattle, int earnedExperience)
    {
        //　上がったレベル
        var levelUpCount = 0;
        //　上がったHP
        var raisedHp = 0;
        //　上がったMP
        var raisedMp = 0;
        //　上がった力
        var raisedPower = 0;
        //　上がった打たれ強さ
        var raisedDefense = 0;
        //　LevelUpData
        LevelUpData levelUpData;

        //　レベルアップ等の計算
        var character = (PlayerStatus)playerInBattle.GetComponent<CharacterBattleScript>().CharacterStatus;
        //　変数を初期化
        levelUpCount = 0;
        raisedHp = 0;
        raisedMp = 0;
        raisedPower = 0;
        raisedDefense = 0;
        levelUpData = character.LevelUpData;

        //　キャラクターに経験値を反映
        character.EarnedExperience += earnedExperience;

        //　そのキャラクターの経験値で何レベルアップしたかどうか
        for (int i = 1; i < levelUpData.RequiredExperience.Count; i++)
        {
            //　レベルアップに必要な経験値を満たしていたら
            if (character.EarnedExperience >= levelUpData.GetRequiredExperience(character.Level + i))
            {
                levelUpCount++;
            }
            else
            {
                break;
            }
        }
        //　レベルを反映
        character.Level += levelUpCount;

        //　レベルアップ分のステータスアップを計算し反映する
        for (int i = 0; i < levelUpCount; i++)
        {
            if (Random.Range(0f, 100f) <= levelUpData.ProbabilityToIncreaseMaxHP)
            {
                raisedHp += Random.Range(levelUpData.MinHPRisingLimit, levelUpData.MaxHPRisingLimit);
            }
            if (Random.Range(0f, 100f) <= levelUpData.ProbabliityToIncreaseMaxMP)
            {
                raisedMp += Random.Range(levelUpData.MinMPRisingLimit, levelUpData.MaxMPRisingLimit);
            }
            if (Random.Range(0f, 100f) <= levelUpData.ProbabilityToIncreasePower)
            {
                raisedPower += Random.Range(levelUpData.MinPowerRisingLimit, levelUpData.MaxPowerRisingLimit);
            }
            if (Random.Range(0f, 100f) <= levelUpData.ProbabilityToIncreaseDefense)
            {
                raisedDefense += Random.Range(levelUpData.MinDefenseRisingLimit, levelUpData.MaxDefenseRisingLimit);
            }
        }
        if (levelUpCount > 0)
        {
            _resultText.text += levelUpCount + "レベル上がってLv" + character.Level + "になった。\n";
            if (raisedHp > 0)
            {
                _resultText.text += "最大HPが" + raisedHp + "上がった。\n";
                character.MaxHp += raisedHp;
            }
            if (raisedMp > 0)
            {
                _resultText.text += "最大MPが" + raisedMp + "上がった。\n";
                character.MaxMp += raisedMp;
            }
            if (raisedPower > 0)
            {
                _resultText.text += "攻撃力が" + raisedPower + "上がった。\n";
                character.Power += raisedPower;
            }
            if (raisedDefense > 0)
            {
                _resultText.text += "防御力が" + raisedDefense + "上がった。\n";
                character.Defense += raisedDefense;
            }
            _resultText.text += "\n";
        }

        //　結果を計算し終わった
        _isDisplayResult = true;

        //　戦闘終了のBGMに変更する
        //_musicManager.ChangeBGM();

        //　結果後に数秒待機
        yield return new WaitForSeconds(_timeToDisplay);
        //　戦闘から抜け出す
        _finishText.gameObject.SetActive(true);
        _isFinishResult = true;
    }

    //　敗戦時の初期処理
    public void InitialProcessingOfDefeatResult()
    {
        StartCoroutine(DisplayDefeatResult());
    }

    //　敗戦時の表示
    public IEnumerator DisplayDefeatResult()
    {
        yield return new WaitForSeconds(_timeToDisplay);
        _resultPanel.SetActive(true);
        _resultText.text = "プレイヤーは命を落とした。";
        _isDisplayResult = true;
        yield return new WaitForSeconds(_timeToDisplay);
        var finishText = _finishText;
        finishText.GetComponent<TextMeshProUGUI>().text = "最初の街へ";
        finishText.gameObject.SetActive(true);

        //　味方が全滅したのでユニティちゃんのHPだけ少し回復しておく
        if (_playerStatus != null)
        {
            _playerStatus.Hp = 1;
        }

        _isFinishResult = true;
    }

    //　逃げた時の初期処理
    public void InitialProcessingOfRanAwayResult()
    {
        StartCoroutine(DisplayRanAwayResult());
    }

    //　逃げた時の表示
    public IEnumerator DisplayRanAwayResult()
    {
        yield return new WaitForSeconds(_timeToDisplay);
        _ranAway = true;
        _resultPanel.SetActive(true);
        _resultText.text = "ユニティちゃん達は逃げ出した。";
        _isDisplayResult = true;
        yield return new WaitForSeconds(_timeToDisplay);
        var finishText = _finishText;
        finishText.GetComponent<TextMeshProUGUI>().text = "ワールドマップへ";
        finishText.gameObject.SetActive(true);
        _isFinishResult = true;
    }
}
