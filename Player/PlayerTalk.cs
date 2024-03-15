using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class PlayerTalk : MonoBehaviour
{
    //インスタンス化
    public static PlayerTalk instance;
    //会話中の状態
    public enum TalkState
    {
        Talk,
        Dictionary,
        Translation,
        Hint
    }
    private TalkState _talkState;
    //　会話可能な相手（初手説明文から入るため、相手として少女を設定）
    [SerializeField]
    private GameObject _conversationPartner;
    // TalkUIゲームオブジェクト
    [SerializeField]
    private GameObject _talkUI = null;
    //　メッセージUI
    [SerializeField]
    private TextMeshProUGUI _messageText = null;
    //　背景UI
    [SerializeField]
    private Image _bgUI;
    //　立ち絵UI
    [SerializeField]
    private Image _charPictureUI;
    //　話しているキャラの名前
    [SerializeField]
    private TextMeshProUGUI _talkCharName = null;
    //　1選択した時に最初に表示するメッセージ
    private List<string> _stringFirstSelectedList = new List<string>();
    //　2を選択した時に最初に表示するメッセージ
    private List<string> _stringSecondSelectedList = new List<string>();
    //　3を選択した時に最初に表示するメッセージ
    private List<string> _stringThirdSelectedList = new List<string>();
    //　表示するメッセージ
    private string _allMessage = null;
    //　使用する分割文字列
    [SerializeField]
    private string _splitString = "<>";
    //　改行文字
    [SerializeField]
    private string _lineString = "/";
    //　分割したメッセージ
    private string[] _splitMessage;
    //　分割したメッセージの何番目か
    private int _messageNum;
    //　テキストスピード
    [SerializeField]
    private float _textSpeed = 0.05f;
    //　経過時間
    private float _elapsedTime = 0f;
    //　今見ている文字番号
    private int _nowTextNum = 0;
    //　マウスクリックを促すアイコン
    [SerializeField]
    private Image _clickIcon = null;
    //　クリックアイコンの点滅秒数
    [SerializeField]
    private float _clickFlashTime = 0.2f;
    //  テキストの非表示時間
    //[SerializeField]
    //private float _flashTextTime = 0.5f;
    //　1回分のメッセージを表示したかどうか
    private bool _isOneMessage = false;
    //　メッセージをすべて表示したかどうか
    private bool _isEndMessage = false;
    //　選択肢ウインドウ
    [SerializeField]
    private GameObject _choiceWindow;
    //　現在の文字列番号
    private int _numOfSelections;
    //　選択UIのテキスト
    [SerializeField]
    private TextMeshProUGUI _firstButtonText;
    [SerializeField]
    private TextMeshProUGUI _secondButtonText;
    [SerializeField]
    private TextMeshProUGUI _thirdButtonText;
    //　選択時にUIに表示するテキスト
    private List<string> _firstTitleTextList = new List<string>();
    private List<string> _secondTitleTextList = new List<string>();
    private List<string> _thirdTitleTextList = new List<string>();
    private bool _isChoosing;
    //　会話を終わらせるフラグ
    private bool _isFinishTalk;
    //　会話の段階を示す
    private int _convFhase;

    //　GameManager
    private GameManager _gameManager;

    //　キーを無効化する時間
    [SerializeField]
    private float _disableInterval = 5.0f;
    //　最後にキーが押された時間
    private float lastKeyPressTime;

    //　選択肢によって反映させたいフラグ
    private string _isActiveOneFlag = "";
    private string _isActiveTwoFlag = "";
    private string _isActiveThreeFlag = "";

    //　選択肢によって反映させたい好感度
    private int _favRate1 = 0;
    private int _favRate2 = 0;
    private int _favRate3 = 0;

    [SerializeField]
    private DictionaryMode _dictionaryMode;
    [SerializeField]
    private OptionCommands _optionCommands;

    //会話を再表示させる際に使用する文字番号
    private int _reMessageNum = 0;
    private int _reNowTextNum = 0;

    //今行われている会話のChapter
    private string _nowChapter = "";

    //どの選択肢を選んだか
    private int _choiseFlag;
    private int _choiseNum = 0;

    //選択肢のCanvasGroup
    [SerializeField]
    private CanvasGroup _choiseCanvasGroup;

    // TipsController
    [SerializeField]
    private TipsController _tipsController;

    // 背景・立ち絵の状態
    public enum BG
    {
        Normal,
        Exploration,
        House
    }
    private BG _bgState;

    public enum CharPicture
    {
        None,
        Normal,
        Joy,
        Anger,
        Surprise,
        Sadness,
        Fear,
        Disgust,
    }
    private CharPicture _charPictureState;

    private bool _isCenterChar = false;
    [SerializeField]
    private List<Sprite> _bgList = new List<Sprite>();
    [SerializeField]
    private List<Sprite> _charPictureList = new List<Sprite>();

    //会話ログ
    [SerializeField]
    private TalkLog _talkLog;

    public TextMeshProUGUI MessageText { get => _messageText; set => _messageText = value; }
    public TextMeshProUGUI TalkCharName { get => _talkCharName; set => _talkCharName = value; }
    public float DisableInterval { get => _disableInterval; set => _disableInterval = value; }
    public string IsActiveOneFlag { get => _isActiveOneFlag; set => _isActiveOneFlag = value; }
    public string IsActiveTwoFlag { get => _isActiveTwoFlag; set => _isActiveTwoFlag = value; }
    public string IsActiveThreeFlag { get => _isActiveThreeFlag; set => _isActiveThreeFlag = value; }
    public int FavRate1 { get => _favRate1; set => _favRate1 = value; }
    public int FavRate2 { get => _favRate2; set => _favRate2 = value; }
    public int FavRate3 { get => _favRate3; set => _favRate3 = value; }
    public BG BgState { get => _bgState; set => _bgState = value; }
    public CharPicture CharPictureState { get => _charPictureState; set => _charPictureState = value; }
    public bool IsCenterChar { get => _isCenterChar; set => _isCenterChar = value; }
    public string NowChapter { get => _nowChapter; set => _nowChapter = value; }

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _isFinishTalk = false;
        _clickIcon.enabled = false;
        _convFhase = 0;
        _gameManager = GameManager.gameManager;
        _tipsController = TipsController.tipsController;
        //Initialize();
        if (_gameManager.FirstConv == false)
        {
            _conversationPartner = null;
        }

        if (_optionCommands == null)
        {
            _optionCommands = OptionCommands.optionCommands;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_optionCommands == null)
        {
            _optionCommands = OptionCommands.optionCommands;
        }

        if (_dictionaryMode.CurrentDictionaryState != DictionaryMode.DictionaryState.None)
        {
            //コマンドを開いたら選択できなくする
            if (_choiseCanvasGroup.interactable == true)
            {
                _choiseCanvasGroup.interactable = false;
            }
        }
        else
        {
            if (_choiseCanvasGroup.interactable == false)
            {
                _choiseCanvasGroup.interactable = true;
            }
        }

        //　メッセージが終わっているか、メッセージがない場合はこれ以降何もしない
        if (_isEndMessage == true || _allMessage == null || _isChoosing == true)
        {
            return;
        }

        // Tipsが出ている間は何も出来なくする
        //if (_tipsController.IsActiveTips == true)
        //{
        //    return;
        //}

        //　コマンドを開いている時はreturn
        if (_dictionaryMode.CurrentDictionaryState != DictionaryMode.DictionaryState.None
            || _optionCommands.CurrentOption != OptionCommands.OptionMode.None)
        {
            return;
        }
        else
        {
            //　1回に表示するメッセージを表示していない	
            if (!_isOneMessage)
            {
                //　テキスト表示時間を経過したらメッセージを追加
                if (_elapsedTime >= _textSpeed)
                {
                    MessageText.enabled = false;
                    TalkChar(MessageText.text);
                    MessageText.text += ConvFoolCoolFont(_splitMessage[_messageNum][_nowTextNum].ToString());
                    NewLine(MessageText.text);
                    _reMessageNum = _messageNum;

                    _nowTextNum++;
                    _elapsedTime = 0f;

                    //　メッセージを全部表示、または行数が最大数表示された
                    if (_nowTextNum >= _splitMessage[_messageNum].Length)
                    {
                        _isOneMessage = true;
                        MessageText.enabled = true;
                    }
                }
                _elapsedTime += Time.deltaTime;

                //　メッセージ表示中にマウスの左ボタンを押したら一括表示
                //if (Input.GetButtonDown("Jump"))
                //{
                //    float currentTime2 = Time.time;

                //    //　前回の入力から一定時間経過していなければ無効
                //    if (currentTime2 - lastKeyPressTime < disableInterval)
                //    {
                //        return;
                //    }

                //    //　ここまでに表示しているテキストに残りのメッセージを足す
                //    messageText.text += ConvFoolCoolFont(_splitMessage[_messageNum].Substring(_nowTextNum));
                //    _isOneMessage = true;
                //}
                //　1回に表示するメッセージを表示した
            }
            else
            {
                _elapsedTime += Time.deltaTime;

                //　クリックアイコンを点滅する時間を超えた時、反転させる
                if (_elapsedTime >= _clickFlashTime)
                {
                    _clickIcon.enabled = !_clickIcon.enabled;
                    _elapsedTime = 0f;
                }

                //　現在のメッセージの次が最後のメッセージであり、かつ最後のメッセージが空文字の場合
                if (_messageNum + 1 >= _splitMessage.Length - 1 && _splitMessage[_splitMessage.Length - 1] == "")
                {
                    DisplayTheChoiceWindow();
                }
                //　現在のメッセージの次が最後のメッセージであり、かつ最後のメッセージが”＠”の場合
                if (_messageNum + 1 >= _splitMessage.Length - 1 && _splitMessage[_splitMessage.Length - 1] == "@")
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        _isFinishTalk = false;
                        StartTalking(messNum: _convFhase + 1);
                        _convFhase++;
                    }
                }

                //　マウスクリックされたら次の文字表示処理
                if (Input.GetKeyDown(KeyCode.F))
                {
                    float currentTime = Time.time;

                    //　前回の入力から一定時間経過していなければ無効
                    if (currentTime - lastKeyPressTime < DisableInterval)
                    {
                        return;
                    }

                    //　会話ログに記録
                    if (_talkLog == null)
                    {
                        _talkLog = _optionCommands.gameObject.transform.Find("TalkLogPanel").GetComponent<TalkLog>();
                    }
                    _talkLog.AddMessage(TalkCharName.text, MessageText.text);

                    _nowTextNum = 0;
                    _messageNum++;
                    MessageText.text = "";
                    _clickIcon.enabled = false;
                    _elapsedTime = 0f;
                    _isOneMessage = false;

                    //　メッセージが全部表示されていたらゲームオブジェクト自体の削除
                    if (_messageNum >= _splitMessage.Length)
                    {
                        Initialize();
                        EndTalking();
                    }
                    else
                    {
                        MessageText.text = "";
                    }
                }
            }
        }

        //　選択解除された時（マウスでUI外をクリックした）は現在のモードによって無理やり選択させる
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (_choiceWindow.activeSelf == true)
            {
                EventSystem.current.SetSelectedGameObject(_choiceWindow.transform.GetChild(0).gameObject);
            }
        }
    }

    //　会話相手を設定
    public void SetConversationPartner(GameObject partnerObj)
    {
        _conversationPartner = partnerObj;
    }

    //　会話相手をリセット
    public void ResetConversationPartner(GameObject parterObj)
    {
        //　会話相手がいない場合は何もしない
        if (_conversationPartner == null)
        {
            return;
        }
        //　会話相手と引数で受け取った相手が同じインスタンスIDを持つなら会話相手をなくす
        if (_conversationPartner.GetInstanceID() == parterObj.GetInstanceID())
        {
            _conversationPartner = null;
        }
    }

    //　会話相手を返す
    public GameObject GetConversationPartner()
    {
        return _conversationPartner;
    }

    //　会話を開始する
    public void StartTalking(string message = null, int messNum = 0)
    {
        if (message != null)
        {
            this.MessageText.text = message;
        }
        
        IConversation conversation = _conversationPartner.GetComponent<IConversation>();
        
        if (_isFinishTalk == false)
        {
            _isFinishTalk = true;
            if (messNum == 0)
            {
                //if (conversation.GetConversation() == null)
                //{
                //    return;
                //}
                this._allMessage = conversation.GetConversation().GetConversationMessage();
            }
            else if (messNum == 1)
            {
                this._allMessage = conversation.GetConversation2().GetConversationMessage();
            }
            else if (messNum == 2)
            {
                this._allMessage = conversation.GetConversation3().GetConversationMessage();
            }
            //　分割文字列で一回に表示するメッセージを分割する
            _splitMessage = Regex.Split(this._allMessage, @"\s*" + _splitString + @"\s*");
        }
        else
        {
            //　分割文字列で一回に表示するメッセージを分割する
            _splitMessage = Regex.Split(this.MessageText.text, @"\s*" + _splitString + @"\s*");
        }
        //　初期化処理
        _nowTextNum = 0;
        _messageNum = 0;
        MessageText.text = "";
        _talkUI.SetActive(true);
        _isOneMessage = false;
        _isEndMessage = false;
        //　会話開始時の入力は一旦リセット
        Input.ResetInputAxes();

        //　背景・立ち絵を決定
        ChangeBg(_bgState);
        ChangeCharPicture(_charPictureState);
    }

    //　選択ボタンを押した時に実行
    public void OnPushButton(int onPush)
    {
        //　選択肢ウインドウを非表示にする
        _isChoosing = false;
        _choiceWindow.SetActive(false);
        IConversation conversation = _conversationPartner.GetComponent<IConversation>();
        //　押されたボタンに応じて次に表示するメッセージを変える
        if (onPush == 1)
        {
            //　反映させたいフラグが渡されていたら
            if (_gameManager.AllGameFlags.ContainsKey(_isActiveOneFlag))
            {
                _gameManager.AllGameFlags[_isActiveOneFlag] = true;
            }

            //　反映させたい好感度の値が渡されていたら
            if (FavRate1 != 0)
            {
                //_gameManager.Favorability += FavRate1;
            }
            
            _choiseFlag = onPush;

            if (_convFhase == 0)
            {
                StartTalking(conversation.GetConversation().ReturnOne());
                _choiseNum = 1;
            }
            else if (_convFhase == 1)
            {
                StartTalking(conversation.GetConversation2().ReturnOne());
                _choiseNum = 2;
            }
            else if (_convFhase == 2)
            {
                StartTalking(conversation.GetConversation3().ReturnOne());
                _choiseNum = 3;
            }
        }
        else if (onPush == 2)
        {
            if (_gameManager.AllGameFlags.ContainsKey(_isActiveTwoFlag))
            {
                _gameManager.AllGameFlags[_isActiveTwoFlag] = true;
            }

            if (FavRate2 != 0)
            {
                //_gameManager.Favorability += FavRate2;
            }

            _choiseFlag = onPush;

            if (_convFhase == 0)
            {
                StartTalking(conversation.GetConversation().ReturnTwo());
                _choiseNum = 1;
            }
            else if (_convFhase == 1)
            {
                StartTalking(conversation.GetConversation2().ReturnTwo());
                _choiseNum = 2;
            }
            else if (_convFhase == 2)
            {
                StartTalking(conversation.GetConversation3().ReturnTwo());
                _choiseNum = 3;
            }
        }
        else if (onPush == 3)
        {
            if (_gameManager.AllGameFlags.ContainsKey(_isActiveThreeFlag))
            {
                _gameManager.AllGameFlags[_isActiveThreeFlag] = true;
            }

            if (FavRate3 != 0)
            {
                //_gameManager.Favorability += FavRate3;
            }

            _choiseFlag = onPush;

            if (_convFhase == 0)
            {
                StartTalking(conversation.GetConversation().ReturnTwo());
                _choiseNum = 1;
            }
            else if (_convFhase == 1)
            {
                StartTalking(conversation.GetConversation2().ReturnTwo());
                _choiseNum = 2;
            }
            else if (_convFhase == 2)
            {
                StartTalking(conversation.GetConversation3().ReturnTwo());
                _choiseNum = 3;
            }
        }
        _numOfSelections++;
    }

    //　会話を終了する
    public void EndTalking()
    {
        _isFinishTalk = false;
        _isEndMessage = true;
        _talkUI.SetActive(false);
        _convFhase = 0;
        //　プレイヤーの状態を変更する
        GetComponent<PlayerController>().SetPlayerState(PlayerController.PlayerState.Normal);
        Input.ResetInputAxes();
        // 会話相手に会話が終了したことを返す
        IConversation conversation = _conversationPartner.GetComponent<IConversation>();
        conversation.FinishTalking();

        if (_gameManager.FirstConv == true)
        {
            _gameManager.FirstConv = false;
            _conversationPartner = null;
        }
    }

    //　選択肢に表示するテキストの設定
    public void SetTheTitleTextOfTheChoice(List<string> stringFirstSelectedList, List<string> stringSecondSelected, List<string> stringThirdSelected, List<string> firstTitleList, List<string> secondTitleList, List<string> thirdTitleList)
    {
        Initialize();
        this._stringFirstSelectedList = stringFirstSelectedList;
        this._stringSecondSelectedList = stringSecondSelected;
        this._stringThirdSelectedList = stringThirdSelected;
        this._firstTitleTextList = firstTitleList;
        this._secondTitleTextList = secondTitleList;
        this._thirdTitleTextList = thirdTitleList;
    }

    //　選択肢ウインドウの表示
    public void DisplayTheChoiceWindow()
    {
        _firstButtonText.text = ConvFoolCoolFont(_firstTitleTextList[_numOfSelections]);
        _secondButtonText.text = ConvFoolCoolFont(_secondTitleTextList[_numOfSelections]);
        _thirdButtonText.text = ConvFoolCoolFont(_thirdTitleTextList[_numOfSelections]);
        _isChoosing = true;
        _choiceWindow.SetActive(true);
        _choiseNum = 0;
        EventSystem.current.SetSelectedGameObject(_choiceWindow.transform.GetChild(0).gameObject);
    }

    //　設定の初期化
    public void Initialize()
    {
        _clickIcon.enabled = false;
        _stringFirstSelectedList.Clear();
        _stringSecondSelectedList.Clear();
        _stringThirdSelectedList.Clear();
        //_yesTitleTextList.Clear();
        //_noTitleTextList.Clear();
        _numOfSelections = 0;
        _isEndMessage = true;
        _isChoosing = false;
        _talkUI.SetActive(false);
        _choiceWindow.SetActive(false);
    }

    //会話文章を改行する
    public void NewLine(string message)
    {
        int index = message.IndexOf(_lineString);
        if (index != -1)
        {
            string modifiedText = message.Replace(_lineString, "\n");
            MessageText.text = modifiedText;
        }
        else
        {
            MessageText.text = message;
        }
    }

    //会話を再表示させる
    public void ReStartTalking()
    {
        if (_choiseNum > 0)
        {
            OnPushButton(_choiseFlag);
            _choiseNum = 0;
        }
        else
        {
            _isFinishTalk = false;
            if (NowChapter != "")
            {
                _gameManager.AllGameFlags[NowChapter] = false;
            }
            StartTalking(messNum: _convFhase);
        }
        _messageNum = _reMessageNum;
    }

    //話しているキャラクターの名前を表示する
    public void TalkChar(string str)
    {
        if (str.Contains("&"))
        {
            MessageText.text = str.Replace("&", "");
            TalkCharName.text = "";
        }
        else if (str.Contains("#"))
        {
            MessageText.text = str.Replace("#", "");
            TalkCharName.text = "プレイヤー";
        }
        else if (str.Contains("$"))
        {
            MessageText.text = str.Replace("$", "");
            TalkCharName.text = "少女";
        }
        else if (str.Contains("%"))
        {
            MessageText.text = str.Replace("%", "");
            TalkCharName.text = "村長";
        }
        else if (str.Contains("*"))
        {
            MessageText.text = str.Replace("*", "");
            TalkCharName.text = "村人";
        }
    }

    //異世界言語の表示変換
    public string ConvFoolCoolFont(string str)
    {
        string rtnStr = "";
        bool isSelect = false;
        if (str == null)
        {
            return rtnStr;
        }
        //文字列を1文字ずつ変換
        for (int i = 0; i < str.Length; i++)
        {
            //文字列変換
            string convStr;
            switch (str[i])
            {
                case 'A':
                    convStr = "0";
                    break;
                case 'B':
                    convStr = "1";
                    break;
                case 'C':
                    convStr = "2";
                    break;
                case 'D':
                    convStr = "3";
                    break;
                case 'E':
                    convStr = "4";
                    break;
                case 'F':
                    convStr = "5";
                    break;
                case 'G':
                    convStr = "6";
                    break;
                case 'H':
                    convStr = "7";
                    break;
                case 'I':
                    convStr = "8";
                    break;
                case 'J':
                    convStr = "9";
                    break;
                case 'K':
                    convStr = "10";
                    break;
                case 'L':
                    convStr = "11";
                    break;
                case 'M':
                    convStr = "12";
                    break;
                case 'N':
                    convStr = "13";
                    break;
                case 'O':
                    convStr = "14";
                    break;
                case 'P':
                    convStr = "15";
                    break;
                case 'Q':
                    convStr = "16";
                    break;
                case 'R':
                    convStr = "17";
                    break;
                case 'S':
                    convStr = "18";
                    break;
                case 'T':
                    convStr = "19";
                    break;
                case 'U':
                    convStr = "20";
                    break;
                case 'V':
                    convStr = "21";
                    break;
                case 'W':
                    convStr = "22";
                    break;
                case 'X':
                    convStr = "23";
                    break;
                case 'Y':
                    convStr = "24";
                    break;
                case 'Z':
                    convStr = "25";
                    break;
                //それ以外
                default:
                    isSelect = true;
                    convStr = str[i].ToString();
                    break;
            }
            //変換
            if (isSelect == false)
            {
                rtnStr += "<sprite=" + convStr + ">";
            }
            else
            {
                rtnStr += convStr;
            }
        }
        return rtnStr;
    }

    //背景の変更
    private void ChangeBg(BG bg)
    {
        if (bg == BG.Normal)
        {
            _bgUI.sprite = null;
            _bgUI.color = new Color32(255, 255, 255, 0);
        }
        else if (bg == BG.Exploration)
        {
            _bgUI.sprite = _bgList[0];
            _bgUI.color = new Color32(255, 255, 255, 255);
        }
        else if (bg == BG.House)
        {
            _bgUI.sprite = _bgList[1];
            _bgUI.color = new Color32(255, 255, 255, 255);
        }
    }

    private void ChangeCharPicture(CharPicture charPicture)
    {
        if (_isCenterChar == true)
        {
            _charPictureUI.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        else
        {
            _charPictureUI.transform.localPosition = new Vector3(0f, 120f, 0f);
        }

        if (charPicture == CharPicture.None)
        {
            _charPictureUI.sprite = null;
            _charPictureUI.color = new Color32(255, 255, 255, 0);
        }
        else if (charPicture == CharPicture.Normal)
        {
            _charPictureUI.sprite = _charPictureList[0];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
        else if (charPicture == CharPicture.Joy)
        {
            _charPictureUI.sprite = _charPictureList[1];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
        else if (charPicture == CharPicture.Anger)
        {
            _charPictureUI.sprite = _charPictureList[2];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
        else if (charPicture == CharPicture.Surprise)
        {
            _charPictureUI.sprite = _charPictureList[3];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
        else if (charPicture == CharPicture.Sadness)
        {
            _charPictureUI.sprite = _charPictureList[4];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
        else if (charPicture == CharPicture.Fear)
        {
            _charPictureUI.sprite = _charPictureList[5];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
        else if (charPicture == CharPicture.Disgust)
        {
            _charPictureUI.sprite = _charPictureList[6];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
    }
}
