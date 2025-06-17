using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardVisual : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private bool initalize = false;

    [Header("Card")]
    public EnumDefs.Card parentCard;
    private Transform cardTransform;
    private Vector3 rotationDelta;
    [SerializeField] private int savedIndex;
    Vector3 movementDelta;
    private Canvas canvas;

    [Header("References")]
    public Transform visualShadow;
    private float shadowOffset = 20;
    private Vector2 shadowDistance;
    private Canvas shadowCanvas;
    [SerializeField] private Transform shakeParent;
    [SerializeField] private Transform tiltParent;
    [SerializeField] private Image cardImage;

    [Header("Follow Parameters")]
    [SerializeField] private float followSpeed = 30;

    [Header("Rotation Parameters")]
    [SerializeField] private float rotationAmount = 20;
    [SerializeField] private float rotationSpeed = 20;
    [SerializeField] private float autoTiltAmount = 30;
    [SerializeField] private float manualTiltAmount = 20;
    [SerializeField] private float tiltSpeed = 20;

    [Header("Scale Parameters")]
    [SerializeField] private bool scaleAnimations = true;
    [SerializeField] private float scaleOnHover = 1.15f;
    [SerializeField] private float scaleOnSelect = 1.25f;
    [SerializeField] private float scaleTransition = .15f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    [Header("Select Parameters")]
    [SerializeField] private float selectPunchAmount = 20;

    [Header("Hober Parameters")]
    [SerializeField] private float hoverPunchAngle = 5;
    [SerializeField] private float hoverTransition = .15f;

    [Header("Swap Parameters")]
    [SerializeField] private bool swapAnimations = true;
    [SerializeField] private float swapRotationAngle = 30;
    [SerializeField] private float swapTransition = .15f;
    [SerializeField] private int swapVibrato = 5;

    [Header("Curve")]
    [SerializeField] private CurveParametersSO curve;

    private float curveYOffset;
    private float curveRotationOffset;

    void Start()
    {
    }

    void Update()
    {
        HandPositioning();
        SmoothFollow();
        //FollowRotation();
        CardTilt();
    }

    private void HandPositioning()
    {
        curveYOffset = (curve.positioning.Evaluate(NormalizedPosition()) * curve.positioningInfluence) * SiblingAmount();
        curveYOffset = SiblingAmount() < 5 ? 0 : curveYOffset;
        curveRotationOffset = curve.rotation.Evaluate(NormalizedPosition());
    }



    private void SmoothFollow()
    {
        Vector3 verticalOffset = (Vector3.up * curveYOffset);
        tiltParent.position = Vector3.Lerp(tiltParent.position, transform.position + verticalOffset, followSpeed * Time.deltaTime);
    }

    private void FollowRotation()
    {
        Vector3 movement = (transform.position - tiltParent.position);
        movementDelta = Vector3.Lerp(movementDelta, movement, 25 * Time.deltaTime);
        Vector3 movementRotation = movement * rotationAmount;
        rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
        tiltParent.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(rotationDelta.x, -60, 60));
    }

    private void CardTilt()
    {
        float sine = Mathf.Sin(Time.time + savedIndex);
        float cosine = Mathf.Cos(Time.time + savedIndex);

        Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float tiltX = 0;
        float tiltY = 0;
        float tiltZ = (curveRotationOffset * (curve.rotationInfluence * SiblingAmount()));

        float lerpX = Mathf.LerpAngle(tiltParent.eulerAngles.x, tiltX + (sine * autoTiltAmount), tiltSpeed * Time.deltaTime);
        float lerpY = Mathf.LerpAngle(tiltParent.eulerAngles.y, tiltY + (cosine * autoTiltAmount), tiltSpeed * Time.deltaTime);
        float lerpZ = Mathf.LerpAngle(tiltParent.eulerAngles.z, tiltZ, tiltSpeed / 2 * Time.deltaTime);

        tiltParent.eulerAngles = new Vector3(lerpX, lerpY, lerpZ);
    }

    public int SiblingAmount()
    {
        return transform.parent.childCount - 1;
    }

    public int ParentIndex()
    {
        return transform.GetSiblingIndex();
    }

    public float NormalizedPosition()
    {
        return ExtensionMethods.Remap((float)ParentIndex(), 0, (float)(transform.parent.childCount - 1), 0, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tiltParent.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tiltParent.DOScale(1, scaleTransition).SetEase(scaleEase);
    }


    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
