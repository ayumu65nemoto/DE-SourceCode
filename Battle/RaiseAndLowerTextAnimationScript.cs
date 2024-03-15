using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaiseAndLowerTextAnimationScript : MonoBehaviour
{
    //　目的のYの値
    [SerializeField]
    private float _destinationYValue = 0.5f;
    //　文字の移動スピード
    [SerializeField]
    private float _charSpeed = 5f;

    private TMP_Text _m_TextComponent;
    private bool _hasTextChanged;

    private VertexAnim[] _vertexAnims;
    private TMP_TextInfo _textInfo;
    private CharacterAnim[] _characterAnims;
    private TMP_MeshInfo[] _cachedMeshInfo;
    private TMP_CharacterInfo _charInfo;

    //　頂点用構造体
    private struct VertexAnim
    {
        private float _yValue;

        public float YValue { get => _yValue; set => _yValue = value; }
    }
    //　文字毎の構造体
    private struct CharacterAnim
    {
        private bool _isAnimationStart;
        private bool _isAnimationEnd;
        private bool _isFlap;

        public bool IsAnimationStart { get => _isAnimationStart; set => _isAnimationStart = value; }
        public bool IsAnimationEnd { get => _isAnimationEnd; set => _isAnimationEnd = value; }
        public bool IsFlap { get => _isFlap; set => _isFlap = value; }
    }

    void Awake()
    {
        _m_TextComponent = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        // Subscribe to event fired when text object has been regenerated.
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
    }

    void Start()
    {
        //　ないとエラーになる
        _m_TextComponent.ForceMeshUpdate();

        _textInfo = _m_TextComponent.textInfo;

        _hasTextChanged = true;

        //　文字に使っている頂点用データの作成
        _vertexAnims = new VertexAnim[_textInfo.characterCount * 4];
        //　文字毎のデータ作成
        _characterAnims = new CharacterAnim[_textInfo.characterCount];

        //　最初の文字のアニメーションをスタートさせる
        _characterAnims[0].IsAnimationStart = true;

        // Cache the vertex data of the text object as the Jitter FX is applied to the original position of the characters.
        _cachedMeshInfo = _textInfo.CopyMeshInfoVertexData();

        // Get new copy of vertex data if the text has changed.
        if (_hasTextChanged)
        {
            // Update the copy of the vertex data for the text object.
            _cachedMeshInfo = _textInfo.CopyMeshInfoVertexData();

            _hasTextChanged = false;
        }

    }

    void ON_TEXT_CHANGED(Object obj)
    {
        if (obj == _m_TextComponent)
            _hasTextChanged = true;
    }

    private void Update()
    {

        _m_TextComponent.ForceMeshUpdate();

        Matrix4x4 matrix;

        int characterCount = _textInfo.characterCount;

        // If No Characters then just yield and wait for some text to be added
        if (characterCount == 0)
        {
            return;
        }

        //　使用する変数宣言
        Vector3[] charOffset = new Vector3[characterCount];
        int materialIndex;
        int vertexIndex;
        Vector3[] sourceVertices;
        Vector2 charMidBasline;
        Vector3 offset;
        Vector3[] destinationVertices;


        for (int i = 0; i < characterCount; i++)
        {
            //　その文字のアニメーションがスタートしてなければ次の文字
            if (_characterAnims[i].IsAnimationStart == false)
            {
                continue;
            }
            //　その文字のアニメーションが終わっていれば次の文字
            if (_characterAnims[i].IsAnimationEnd == true)
            {
                continue;
            }

            _charInfo = _textInfo.characterInfo[i];

            // 文字が見えなければ（空白文字）次の文字をアニメーションさせる
            if (_charInfo.isVisible == false)
            {
                _characterAnims[i].IsAnimationStart = false;
                _characterAnims[i].IsAnimationEnd = true;
                //　最後の文字まで進んでいなければ次の文字をスタートさせる
                if (i < characterCount - 1)
                {
                    _characterAnims[i + 1].IsAnimationStart = true;
                }
                else
                {
                    //　最後の文字が終わっていればゲームオブジェクトを1秒後に削除
                    Destroy(this.gameObject, 1f);
                }

                continue;
            }

            // Get the index of the material used by the current character.
            materialIndex = _textInfo.characterInfo[i].materialReferenceIndex;

            // Get the index of the first vertex used by this text element.
            vertexIndex = _textInfo.characterInfo[i].vertexIndex;

            // Get the cached vertices of the mesh used by this text element (character or sprite).
            sourceVertices = _cachedMeshInfo[materialIndex].vertices;

            // Determine the center point of each character at the baseline.
            //charMidBasline = new Vector2((sourceVertices[vertexIndex + 0].x + sourceVertices[vertexIndex + 2].x) / 2, charInfo.baseLine);
            // Determine the center point of each character.
            charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

            // Need to translate all 4 vertices of each quad to aligned with middle of character / baseline.
            // This is needed so the matrix TRS is applied at the origin for each character.
            offset = charMidBasline;

            destinationVertices = _textInfo.meshInfo[materialIndex].vertices;

            destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
            destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
            destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
            destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

            //　文字が上まで行っていない時は上に移動
            if (_characterAnims[i].IsFlap == false)
            {
                charOffset[i] = new Vector3(0f, Mathf.MoveTowards(_vertexAnims[i].YValue, _destinationYValue, _charSpeed * Time.deltaTime), 0f);
                _vertexAnims[i].YValue = charOffset[i].y;
                Debug.Log(_vertexAnims[i].YValue);
                //　目的地を越えたら反転
                if (charOffset[i].y >= _destinationYValue)
                {
                    _characterAnims[i].IsFlap = true;
                }
            }
            else
            {
                charOffset[i] = new Vector3(0f, Mathf.MoveTowards(_vertexAnims[i].YValue, 0f, _charSpeed * Time.deltaTime), 0f);
                _vertexAnims[i].YValue = charOffset[i].y;
                //　文字が元の位置に戻ったら次の文字をスタート
                if (_vertexAnims[i].YValue <= 0f)
                {
                    _characterAnims[i].IsAnimationStart = false;
                    _characterAnims[i].IsAnimationEnd = true;
                    //　最後の文字でなければ次の文字をスタート
                    if (i < characterCount - 1)
                    {
                        _characterAnims[i + 1].IsAnimationStart = true;
                    }
                    else
                    {
                        //　最後の文字であればゲームオブジェクトを1秒後に削除
                        Destroy(this.gameObject, 1f);
                    }
                }
            }
            //　位置、回転、スケールから行列を作成
            matrix = Matrix4x4.TRS(charOffset[i], Quaternion.identity, Vector3.one);
            //　引数で与えた頂点をmatrix変換行列に従って変換
            destinationVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
            destinationVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
            destinationVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
            destinationVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);

            destinationVertices[vertexIndex + 0] += offset;
            destinationVertices[vertexIndex + 1] += offset;
            destinationVertices[vertexIndex + 2] += offset;
            destinationVertices[vertexIndex + 3] += offset;

        }

        // Push changes into meshes
        for (int i = 0; i < _textInfo.meshInfo.Length; i++)
        {
            _textInfo.meshInfo[i].mesh.vertices = _textInfo.meshInfo[i].vertices;
            _m_TextComponent.UpdateGeometry(_textInfo.meshInfo[i].mesh, i);
        }
    }
}
