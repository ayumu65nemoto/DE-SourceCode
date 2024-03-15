using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationScope : MonoBehaviour
{
    void OnTriggerStay(Collider col) {
        if (col.tag == "Player"
            && col.GetComponent<PlayerController>().GetState() != PlayerController.PlayerState.Talk
            ) {
            //　ユニティちゃんが近づいたら会話相手として自分のゲームオブジェクトを渡す
            col.GetComponent<PlayerTalk>().SetConversationPartner(transform.parent.gameObject);
        }
    }
 
    void OnTriggerExit(Collider col) {
        if (col.tag == "Player"
            && col.GetComponent<PlayerController>().GetState() != PlayerController.PlayerState.Talk
            ) {
            //　ユニティちゃんが遠ざかったら会話相手から外す
            col.GetComponent<PlayerTalk>().ResetConversationPartner(transform.parent.gameObject);
        }
    }
}
