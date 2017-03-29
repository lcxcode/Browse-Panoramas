using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldPointer : DestinationMarker
{

    public enum pointerVisiblityStates
    {
        On_When_Active,
        Always_On,
        Always_Off
    }

    public ControllerEvent controller = null;

    public Material pointerMaterial;

    public Color pointerHieColor = new Color(0f, 0.5f, 0f, 1f);

    public Color pointerMissColor = new Color(0.8f, 0f, 0f, 1f);

    public bool showPlayAreaCursor = false;

    public Vector2 playAreaCursorDimensions = Vector2.zero;

    public bool handlePlayAreaCursorCollisions = false;

    public string ignoreTargetWithTagOrClass;

    public TagOrScriptPolicyList targetTagOrScriptListPolicy;

    public pointerVisiblityStates pointerVisibility = pointerVisiblityStates.On_When_Active;

    public bool holdButtonToActivate = true;

    public float activateDelay = 0f;

    protected Vector3 destinationPosition;

    protected float pointerContractDistance = 0f;

    protected Transform pointerContactTarget = null;

    protected uint controllerIndex;

    protected bool playAreaCursorCollided = false;

    private Transform playArea;

    private GameObject playAreaCursor;

    private GameObject[] playAreaCursorBoundaries;

    private BoxCollider playAreaCursorCollider;

    private Transform headset;

    private bool isActive;

    private bool destinationSetActive;

    private float activateDelayTimer = 0f;

    private int beamEnabledState = 0;

    private InteractableObject interactableObject = null;


    public virtual void setPlayAreaCursorCollision(bool state)
    {
        if(handlePlayAreaCursorCollisions)
        {
            playAreaCursorCollided = state;
        }
    }

    public virtual bool IsActive()
    {
        return isActive;
    }

    public virtual bool CanActivate()
    {
        return (Time.time >= activateDelayTimer);
    }


    public virtual void ToggleBeam(bool state)
    {
        var index = DeviceFinder.GetControllerIndex(gameObject);
        if(state)
        {

        }
        else
        {

        }
    }



    protected virtual void Awake()
    {
        if(controller == null)
        {
            controller = GetComponent<ControllerEvent>();
        }

        if(controller == null)
        {
            Debug.Log("");
            return;
        }

        //Utilities

        headset = DeviceFinder.HeadsetTransform();
        playArea = DeviceFinder.PlayAreaTransform();
        playAreaCursorBoundaries = new GameObject[4];
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        
    }

}//End Class
