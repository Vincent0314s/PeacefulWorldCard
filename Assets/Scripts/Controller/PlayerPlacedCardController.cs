using UnityEngine;
using System;

public class PlayerPlacedCardController : MonoBehaviour, IInitialization
{
    private Camera _mainCam;
    public LayerMask CannonMask;

    private Card_Cannon _currentCannonCard;

    private bool _isHoldingCannon;

    public void IAwake()
    {
        _mainCam = Camera.main;

    }

    public void IStart()
    {
    }

    public void CardSelectingLogic(Action OnSelectingFinshed)
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
                    hit.transform.TryGetComponent(out IDestroyable destroyableObject);
                    destroyableObject.DestroyObject();
                    _currentCannonCard.EnableTrajectoryLine(false);
                    _isHoldingCannon = false;
                    OnSelectingFinshed?.Invoke();
                }
            }
        }
    }
}
