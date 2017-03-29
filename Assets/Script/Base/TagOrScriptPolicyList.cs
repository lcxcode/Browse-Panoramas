using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagOrScriptPolicyList : MonoBehaviour
{

    public enum OperationTypes
    {
        Ignore,
        Include
    }

    public enum CheckTypes
    {
        Tag,
        Script,
        Tag_Or_Script
    }

    public OperationTypes operation = OperationTypes.Ignore;

    public CheckTypes checkType = CheckTypes.Tag;


    public List<string> identifiers = new List<string>() { "" };


    public bool Find(GameObject obj)
    {
        if(operation == OperationTypes.Ignore)
        {
            return TypeCheck(obj, true);
        }
        else
        {
            return TypeCheck(obj, false);
        }
    }

    private bool ScriptCheck(GameObject obj, bool returnState)
    {
        foreach (var identifier in identifiers)
        {
            if(obj.GetComponent(identifier))
            {
                return returnState;
            }
        }
        return !returnState;
    }

    private bool TagCheck(GameObject obj, bool returnState)
    {
        if(returnState)
        {
            return identifiers.Contains(obj.tag);
        }
        else
        {
            return !identifiers.Contains(obj.tag);
        }
    }

    private bool TypeCheck(GameObject obj, bool returnState)
    {
        switch (checkType)
        {
            case CheckTypes.Tag:
                return TagCheck(obj, returnState);
            case CheckTypes.Script:
                return ScriptCheck(obj, returnState);
            case CheckTypes.Tag_Or_Script:
                if((returnState && ScriptCheck(obj, returnState)) || (!returnState && !ScriptCheck(obj, returnState)))
                {
                    return returnState;
                }
                if((returnState && TagCheck(obj, returnState)) || (!returnState && !TagCheck(obj, returnState)))
                {
                    return returnState;
                }
                break;
        }
        return !returnState;
    }

}//End Class
