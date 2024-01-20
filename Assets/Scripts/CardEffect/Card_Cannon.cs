using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Cannon : CardBase, IDestroyable
{
    [SerializeField] private TrajectoryLine _trajectoryLine;

    public void SetAttackStartPoint(Vector3 startPosition)
    {
        _trajectoryLine.SetStartPoint(startPosition);
    }

    public void SetAttackPoint(Vector3 targetPosition)
    {
        _trajectoryLine.SetTargetPoint(targetPosition);
    }

    public void EnableTrajectoryLine(bool enableTrajectoryline)
    {
        if (enableTrajectoryline)
        {
            _trajectoryLine.CreateLine();
        }
        else
        {
            _trajectoryLine.ResetLine();
        }
    }

    public void DestroyObject()
    {
        CardPlacementController.DestroyCard(EnumDefs.Card.Cannon);
        Destroy(transform.gameObject);
    }
}
