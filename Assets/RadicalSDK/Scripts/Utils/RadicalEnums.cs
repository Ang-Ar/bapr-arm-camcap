using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radical
{
    public enum MessageSeverity
    {
        None,
        Info,
        Warning,
        Error
    }

    public enum MessagePriority
    {
        None,
        Misc,
        UserNotFound,
        WrongClient,
        WrongRoomID,
        WrongAccountKey,
        BadRequest,
        WrongSubscription
    }

    public enum UIElement
    {
        ErrorReport
    }

    public enum RetargetType
    {
        None,
        ReadyPlayerMe,
        Mixamo
    }

    public enum FootState
    {
        Grounded,
        JustGrounded,
        Lifted,
        //JustAirborne
    }
}

