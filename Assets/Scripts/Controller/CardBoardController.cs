using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBoardController : MonoBehaviour
{
    private Camera _mainCam;
    public LayerMask CannonMask;
    public LayerMask EnemyMask;

    private Card_Cannon _currentCannonCard;

    private bool _isHoldingCannon;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = _mainCam.ScreenPointToRay(mouse);
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, CannonMask))
            {
                _isHoldingCannon = true;
                Vector3 startPosition = new Vector3(0, 0.2f, 0);
                _currentCannonCard = hit.transform.GetComponent<Card_Cannon>();
                _currentCannonCard.EnableTrajectoryLine(true);
                _currentCannonCard.SetAttackStartPoint(startPosition);
            }
        }
        if (_isHoldingCannon)
        {
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                Vector3 destination = new Vector3(hit.point.x - _currentCannonCard.transform.position.x, 0.2f, hit.point.z - _currentCannonCard.transform.position.z);
                _currentCannonCard.SetAttackPoint(destination);
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    Debug.Log(hit.transform.name);
                    _currentCannonCard.EnableTrajectoryLine(false);
                    _isHoldingCannon = false;
                }
            }
        }
    }
}
