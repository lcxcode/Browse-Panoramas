using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DestinationMarkerEventArgs
{
    public float distance;
    public Transform target;
    public Vector3 destinationPosition;
    public bool enableTeleport;
    public uint controllerIndex;
}

public delegate void DestinationMarkerEventHandler(object sender, DestinationMarkerEventArgs e);


public abstract class DestinationMarker : MonoBehaviour
{

    public bool enableTeleport = true;

    public event DestinationMarkerEventHandler DestinationMarkerEnter;

    public event DestinationMarkerEventHandler DestinationMarkerExit;

    public event DestinationMarkerEventHandler DestinationMarkerSet;


    protected string invalidTargetWithTagOrClass;
    protected TagOrScriptPolicyList invalidTagOrScriptListPilicy;
    protected float navMeshCheckDistance;
    protected bool headsetPositionCompensation;

    public virtual void OnDestinationMarkerEnter(DestinationMarkerEventArgs e)
    {
        if(DestinationMarkerEnter != null)
        {
            DestinationMarkerEnter(this, e);
        }
    }

    public virtual void OnDestinationMarkerExit(DestinationMarkerEventArgs e)
    {
        if(DestinationMarkerExit != null)
        {
            DestinationMarkerExit(this, e);
        }
    }

    public virtual void OnDestinationMarkerSet(DestinationMarkerEventArgs e)
    {
        if(DestinationMarkerSet != null)
        {
            DestinationMarkerSet(this, e);
        }
    }

    public virtual void SetInvalidTarget(string name, TagOrScriptPolicyList list)
    {
        invalidTargetWithTagOrClass = name;
        invalidTagOrScriptListPilicy = list;
    }

    public virtual void SetNavMeshCheckDistance(float distance)
    {
        navMeshCheckDistance = distance;
    }


    public virtual void SetHeadsetPositionCompensation(bool state)
    {
        headsetPositionCompensation = state;
    }


    protected virtual void OnEnable()
    {
        ObjectCache.registeredDestinationMarkers.Add(this);
    }

    protected virtual void OnDisable()
    {
        ObjectCache.registeredDestinationMarkers.Remove(this);
    }


    protected DestinationMarkerEventArgs SetDestinationMarkerEvent(float distance, Transform target, Vector3 position, uint controllerIndex)
    {
        DestinationMarkerEventArgs e;
        e.controllerIndex = controllerIndex;
        e.distance = distance;
        e.target = target;
        e.destinationPosition = position;
        e.enableTeleport = enableTeleport;
        return e;
    }


}//End
