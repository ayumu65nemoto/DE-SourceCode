using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationScope : MonoBehaviour
{
    void OnTriggerStay(Collider col) {
        if (col.tag == "Player"
            && col.GetComponent<PlayerController>().GetState() != PlayerController.PlayerState.Talk
            ) {
            //�@���j�e�B����񂪋߂Â������b����Ƃ��Ď����̃Q�[���I�u�W�F�N�g��n��
            col.GetComponent<PlayerTalk>().SetConversationPartner(transform.parent.gameObject);
        }
    }
 
    void OnTriggerExit(Collider col) {
        if (col.tag == "Player"
            && col.GetComponent<PlayerController>().GetState() != PlayerController.PlayerState.Talk
            ) {
            //�@���j�e�B����񂪉������������b���肩��O��
            col.GetComponent<PlayerTalk>().ResetConversationPartner(transform.parent.gameObject);
        }
    }
}
