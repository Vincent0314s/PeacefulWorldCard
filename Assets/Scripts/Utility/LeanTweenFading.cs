using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeanTweenFading : MonoBehaviour
{
    public enum FadingComponent
    {
        SpriteRenderer,
        Image,
    }
    public FadingComponent ComponentToFade;

    [Range(0, 1)]
    [SerializeField] private float _fadeOutAlpha;
    [SerializeField] private float _fadeOutDuration = 1f;
    [Range(0, 1)]
    [SerializeField] private float _fadeInAlpha = 1f;
    [SerializeField] private float _fadeInDuration = 1f;

    private SpriteRenderer _spriteRenderer;
    private Image _image;

    private float _currentAlpha;

    void Awake()
    {
        switch (ComponentToFade)
        {
            case FadingComponent.SpriteRenderer:
                TryGetComponent(out _spriteRenderer);
                break;
            case FadingComponent.Image:
                TryGetComponent(out _image);
                break;
        }
    }

    public void FadeOut()
    {
        switch (ComponentToFade)
        {
            case FadingComponent.SpriteRenderer:
                _currentAlpha = _spriteRenderer.color.a;
                break;
            case FadingComponent.Image:
                _currentAlpha = _image.color.a;
                break;
        }
        LeanTween.value(gameObject, UpdateAlpha, _currentAlpha, _fadeOutAlpha, _fadeOutDuration);
    }

    public void FadeIn()
    {
        switch (ComponentToFade)
        {
            case FadingComponent.SpriteRenderer:
                _currentAlpha = _spriteRenderer.color.a;
                break;
            case FadingComponent.Image:
                _currentAlpha = _image.color.a;
                break;
        }
        LeanTween.value(gameObject, UpdateAlpha, _currentAlpha, _fadeInAlpha, _fadeInDuration);
    }

    public void ResetAlpha()
    {
        UpdateAlpha(1);
    }

    private void UpdateAlpha(float newAlpha)
    {
        switch (ComponentToFade)
        {
            case FadingComponent.SpriteRenderer:
                _currentAlpha = newAlpha;
                _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _currentAlpha);
                break;
            case FadingComponent.Image:
                _currentAlpha = newAlpha;
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _currentAlpha);
                break;
        }
    }
}
