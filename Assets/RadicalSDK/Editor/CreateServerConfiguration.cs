using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "Radical/Configuration", order = 1)]
public class RadicalServerConfiguration : ScriptableObject
{
    public string objectName = "Radical Server Settings";
    public string ServerURL = "https://test.radicalmotion.com/wp-json/radapi/v4/user/liveauth?room=4ad7b61c-ffe3-42a5-bcfd-9cb4406e96fa&client-key=39763a2f-eb38-4fae-93fc-dde99f2e34bf&client=unity";
    public string Room;
    public string UserToken;

}

