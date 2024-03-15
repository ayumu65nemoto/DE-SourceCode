using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConversation
{
    Conversation GetConversation();
    Conversation GetConversation2();
    Conversation GetConversation3();

    void FinishTalking();
}
