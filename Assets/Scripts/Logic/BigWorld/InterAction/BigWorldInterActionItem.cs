using System.Collections.Generic;
using UnityEngine;

public class BigWorldInterActionItem : MonoBehaviour
{
    public GameObject BattleTransform;
    private float _interActionDistance = 150f;
    private bool _isStartInterAction = false;

    void Update()
    {
        if (!(Player.Instance != null && Player.Instance.IsEnterWorld())) return;

        Vector3 distanceVec = transform.InverseTransformPoint(BigWorldManager.Instance.World.transform.position);

        if (Vector3.Distance(distanceVec, Vector3.zero) < _interActionDistance)
        {
            if(_isStartInterAction == false)
            {
                EnterInterAction();
            }
            _isStartInterAction = true;
        }
        else
        {
            if(_isStartInterAction == true)
            {
                ExitInterAction();
            }
            _isStartInterAction = false;
        }

    }

    protected virtual void EnterInterAction()
    {
        Debug.Log("EnterInterAction");
        if(BattleTransform != null)
        {
            UIManager.Instance.Open(BattleTransform.gameObject, UIViewName.UIBigWorldInterAction);
        }
    }

    protected virtual void ExitInterAction()
    {
        Debug.Log("ExitInterAction");
        UIManager.Instance.Close(UIViewName.UIBigWorldInterAction);
    }
}
