using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class BattleResult : MonoBehaviour
{
    //�@���ʂ�\�����Ă��烏�[���h�}�b�v�ɖ߂��悤�ɂȂ�܂ł̎���
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
    //�@�퓬���ʕ\�������Ă��邩�ǂ���
    private bool _isDisplayResult;
    //�@���ʂ�\�����퓬���甲���o���邩�ǂ���
    private bool _isFinishResult;
    //�@�퓬�ɏ����������ǂ���
    private bool _won;
    //�@���������ǂ���
    private bool _ranAway;
    //�@�퓬���ʃe�L�X�g�̃X�N���[���l
    [SerializeField]
    private float _scrollValue = 50f;
    //�@MusicManager
    //[SerializeField]
    //private MusicManager _musicManager;
    // �{�^���A�ł�h��
    private bool _lockInput = false;

    void Update()
    {
        //�@���ʕ\���O�͉������Ȃ�
        if (!_isDisplayResult)
        {
            return;
        }

        //�@���ʕ\����͌��ʕ\���e�L�X�g���X�N���[�����Č����悤�ɂ���
        if (Input.GetAxis("Vertical") != 0f)
        {
            _resultText.transform.localPosition += new Vector3(0f, -Input.GetAxis("Vertical") * _scrollValue, 0f);
        }
        //�@�퓬�𔲂��o���܂ł̑ҋ@���Ԃ��z���Ă��Ȃ�
        if (!_isFinishResult)
        {
            return;
        }
        //�@Submit��Action��Fire1�{�^�����������烏�[���h�}�b�v�ɖ߂�
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

    //�@�������̏�������
    public void InitialProcessingOfVictoryResult(List<GameObject> allCharacterList, GameObject playerInBattle)
    {
        StartCoroutine(DisplayVictoryResult(allCharacterList, playerInBattle));
    }

    //�@�������̌���
    public IEnumerator DisplayVictoryResult(List<GameObject> allCharacterList, GameObject playerInBattle)
    {
        yield return new WaitForSeconds(_timeToDisplay);
        _won = true;
        _resultPanel.SetActive(true);
        //�@�퓬�Ŋl�������o���l
        int earnedExperience = 0;
        //�@�퓬�Ŋl����������
        int earnedMoney = 0;
        //�@�퓬�Ŋl�������A�C�e���Ƃ��̌�
        Dictionary<Item, int> getItemDictionary = new Dictionary<Item, int>();
        //�@Float�̃����_���l
        float randomFloat;
        //�@�A�C�e���擾�m��
        float probability;
        //�@�L�����N�^�[�X�e�[�^�X
        CharacterStatus characterStatus;
        //�@�G�̃A�C�e���f�B�N�V���i���[
        ItemDictionary enemyItemDictionary;

        foreach (var character in allCharacterList)
        {
            characterStatus = character.GetComponent<CharacterBattleScript>().CharacterStatus;
            if (characterStatus as EnemyStatus != null)
            {
                earnedExperience += ((EnemyStatus)characterStatus).GetExperience;
                earnedMoney += ((EnemyStatus)characterStatus).GetMoney;
                enemyItemDictionary = ((EnemyStatus)characterStatus).DropItemDictionary;
                //�@�G�������Ă���A�C�e���̎�ނ̐������J��Ԃ�
                foreach (var item in enemyItemDictionary.Keys)
                {
                    //�@0�`100�̊Ԃ̃����_���l���擾
                    randomFloat = Random.Range(0f, 100f);
                    //�@�A�C�e���̎擾�m�����擾
                    probability = enemyItemDictionary[item];
                    //�@�����_���l���A�C�e���擾�m���ȉ��̒l�ł���΃A�C�e���擾
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
        _resultText.text = earnedExperience + "�̌o���l���l�������B\n";
        _resultText.text += earnedMoney + "�̂������l�������B\n";

        //�@�p�[�e�B�[�X�e�[�^�X�ɂ����𔽉f����
        _playerStatus.Money += earnedMoney;

        //�@�擾�����A�C�e���𖡕��p�[�e�B�[�ɕ��z����
        foreach (var item in getItemDictionary.Keys)
        {
            //�@�L�����N�^�[�����ɃA�C�e���������Ă��鎞
            if (_playerStatus.ItemDictionary.ContainsKey(item))
            {
                _playerStatus.ItemDictionary[item] += getItemDictionary[item];
            }
            else
            {
                _playerStatus.ItemDictionary.Add(item, getItemDictionary[item]);
            }
            _resultText.text += item.ItemName + "��" + getItemDictionary[item] + "��ɓ��ꂽ�B\n";
            _resultText.text += "\n";
        }

        StartCoroutine(LevelUpCharacter(playerInBattle, earnedExperience));
    }

    //�@���x���A�b�v����
    private IEnumerator LevelUpCharacter(GameObject playerInBattle, int earnedExperience)
    {
        //�@�オ�������x��
        var levelUpCount = 0;
        //�@�オ����HP
        var raisedHp = 0;
        //�@�オ����MP
        var raisedMp = 0;
        //�@�オ������
        var raisedPower = 0;
        //�@�オ�����ł��ꋭ��
        var raisedDefense = 0;
        //�@LevelUpData
        LevelUpData levelUpData;

        //�@���x���A�b�v���̌v�Z
        var character = (PlayerStatus)playerInBattle.GetComponent<CharacterBattleScript>().CharacterStatus;
        //�@�ϐ���������
        levelUpCount = 0;
        raisedHp = 0;
        raisedMp = 0;
        raisedPower = 0;
        raisedDefense = 0;
        levelUpData = character.LevelUpData;

        //�@�L�����N�^�[�Ɍo���l�𔽉f
        character.EarnedExperience += earnedExperience;

        //�@���̃L�����N�^�[�̌o���l�ŉ����x���A�b�v�������ǂ���
        for (int i = 1; i < levelUpData.RequiredExperience.Count; i++)
        {
            //�@���x���A�b�v�ɕK�v�Ȍo���l�𖞂����Ă�����
            if (character.EarnedExperience >= levelUpData.GetRequiredExperience(character.Level + i))
            {
                levelUpCount++;
            }
            else
            {
                break;
            }
        }
        //�@���x���𔽉f
        character.Level += levelUpCount;

        //�@���x���A�b�v���̃X�e�[�^�X�A�b�v���v�Z�����f����
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
            _resultText.text += levelUpCount + "���x���オ����Lv" + character.Level + "�ɂȂ����B\n";
            if (raisedHp > 0)
            {
                _resultText.text += "�ő�HP��" + raisedHp + "�オ�����B\n";
                character.MaxHp += raisedHp;
            }
            if (raisedMp > 0)
            {
                _resultText.text += "�ő�MP��" + raisedMp + "�オ�����B\n";
                character.MaxMp += raisedMp;
            }
            if (raisedPower > 0)
            {
                _resultText.text += "�U���͂�" + raisedPower + "�オ�����B\n";
                character.Power += raisedPower;
            }
            if (raisedDefense > 0)
            {
                _resultText.text += "�h��͂�" + raisedDefense + "�オ�����B\n";
                character.Defense += raisedDefense;
            }
            _resultText.text += "\n";
        }

        //�@���ʂ��v�Z���I�����
        _isDisplayResult = true;

        //�@�퓬�I����BGM�ɕύX����
        //_musicManager.ChangeBGM();

        //�@���ʌ�ɐ��b�ҋ@
        yield return new WaitForSeconds(_timeToDisplay);
        //�@�퓬���甲���o��
        _finishText.gameObject.SetActive(true);
        _isFinishResult = true;
    }

    //�@�s�펞�̏�������
    public void InitialProcessingOfDefeatResult()
    {
        StartCoroutine(DisplayDefeatResult());
    }

    //�@�s�펞�̕\��
    public IEnumerator DisplayDefeatResult()
    {
        yield return new WaitForSeconds(_timeToDisplay);
        _resultPanel.SetActive(true);
        _resultText.text = "�v���C���[�͖��𗎂Ƃ����B";
        _isDisplayResult = true;
        yield return new WaitForSeconds(_timeToDisplay);
        var finishText = _finishText;
        finishText.GetComponent<TextMeshProUGUI>().text = "�ŏ��̊X��";
        finishText.gameObject.SetActive(true);

        //�@�������S�ł����̂Ń��j�e�B������HP���������񕜂��Ă���
        if (_playerStatus != null)
        {
            _playerStatus.Hp = 1;
        }

        _isFinishResult = true;
    }

    //�@���������̏�������
    public void InitialProcessingOfRanAwayResult()
    {
        StartCoroutine(DisplayRanAwayResult());
    }

    //�@���������̕\��
    public IEnumerator DisplayRanAwayResult()
    {
        yield return new WaitForSeconds(_timeToDisplay);
        _ranAway = true;
        _resultPanel.SetActive(true);
        _resultText.text = "���j�e�B�����B�͓����o�����B";
        _isDisplayResult = true;
        yield return new WaitForSeconds(_timeToDisplay);
        var finishText = _finishText;
        finishText.GetComponent<TextMeshProUGUI>().text = "���[���h�}�b�v��";
        finishText.gameObject.SetActive(true);
        _isFinishResult = true;
    }
}
