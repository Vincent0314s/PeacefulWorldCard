using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class VButton : MonoBehaviour,IPointerEnterHandler,IPointerDownHandler,IPointerExitHandler
{

    public Sprite mainSprite;
    //Foldable
    public bool ableToChangeScene = false;
    public SceneInTheBuild sceneToChange;

    //Foldable
    public bool ableToAssignSFX = false;
    public UI_SoundEffects hover;
    public UI_SoundEffects click;
    public UI_SoundEffects exit;

    public UnityEvent OnEnter;
    public UnityEvent OnClick;
    public UnityEvent OnExit;

    public enum transition
    {
        Color,
        Sprite,
    }

    public transition Transition;


    public Image image;
    public Color normalColor = Color.white;
    public Color highLightedColor = Color.gray;
    public Color clickedColor = Color.red;

    public enum MouseState {
        Hover,
        Click,
        Exit,
    }

    public MouseState mouseState;

    public Sprite normalSprite;
    public Sprite highLightedSprite;
    public Sprite clickedSprite;

    public Animator anim;

    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (Transition) {
            case transition.Color:
                image.color = highLightedColor;
                break;
            case transition.Sprite:
                image.sprite = highLightedSprite;
                break;
        }

        //mouseState = MouseState.Hover;
        OnEnter?.Invoke();
        if (ableToAssignSFX) {
            SoundManager.PlaySound(hover);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (Transition)
        {
            case transition.Color:
                image.color = clickedColor;
                break;
            case transition.Sprite:
                image.sprite = clickedSprite;
                break;
        }
        //mouseState = MouseState.Click;
        OnClick?.Invoke();
        if (ableToChangeScene) {
            SceneController.LoadScene(sceneToChange);
        }
        if (ableToAssignSFX)
        {
            SoundManager.PlaySound(click);
        }
    }
  

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (Transition)
        {
            case transition.Color:
                image.color = normalColor;
                break;
            case transition.Sprite:
                image.sprite = normalSprite;
                break;
        }
        //mouseState = MouseState.Exit;
        OnExit?.Invoke();
        if (ableToAssignSFX)
        {
            SoundManager.PlaySound(exit);
        }
    }
}
