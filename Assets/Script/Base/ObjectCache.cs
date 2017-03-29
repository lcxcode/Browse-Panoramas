using UnityEngine;
using System.Collections.Generic;

public class ObjectCache : MonoBehaviour
{


    public static List<DestinationMarker> registeredDestinationMarkers = new List<DestinationMarker>();
    //public static HeadsetCollision registeredHeadsetCollider = null;
    public static Dictionary<uint, GameObject> trackedControllers = new Dictionary<uint, GameObject>();

}
