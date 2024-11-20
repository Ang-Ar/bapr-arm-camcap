using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Radical.LiveConnector))]
public class RadicalRoomPicker : MonoBehaviour
{
    public RadicalRoom room;

    private void OnValidate()
    {
        this.gameObject.GetComponent<Radical.LiveConnector>().radicalRoomID = room.roomID;
    }
}
