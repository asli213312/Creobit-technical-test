﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class Helper
{
    public static IEnumerator DoLocalMove(this RectTransform rectTransform, Vector2 position)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        float t = Time.deltaTime;
        while (t < 0.1f)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, position, t / 0.1f);
            yield return null;
            t += Time.deltaTime;
        }

        rectTransform.anchoredPosition = position;
    }

    public static IEnumerator DoLerp(Vector2 start, Vector2 target, Action<Vector2> onUpdate)
    {
        float t = Time.deltaTime;
        while (t < 0.1f)
        {
            onUpdate?.Invoke(Vector2.Lerp(start, target, t / 0.1f));
            yield return null;
            t += Time.deltaTime;
        }

        onUpdate?.Invoke(target);
    }

    public static IEnumerator DoLocalScale(this Transform transform, Vector3 scale, UnityAction onComplete = null)
    {
        Vector3 startScale = transform.localScale;
        float t = Time.deltaTime;
        while (t < 0.1f)
        {
            transform.localScale = Vector3.Lerp(startScale, scale, t / 0.1f);
            yield return null;
            t += Time.deltaTime;
        }

        transform.localScale = scale;
        onComplete?.Invoke();
    }

    public static IEnumerator DoLocalScaleAndUnscale(this Transform transform, MonoBehaviour behaviour,
        Vector3 scale, bool needDeactivate = false, float deactivateAfter = 0, UnityAction onComplete = null)
    {
        behaviour.Activate(transform);
        
        Vector3 initScale = transform.localScale;
        Vector3 initPosition = transform.position;

        behaviour.StartCoroutine(LocalScale(transform, scale));

        onComplete?.Invoke();
        
        if (needDeactivate)
        {
            transform.position = Vector3.one * 100;
            yield return new WaitForSeconds(deactivateAfter);
            ReturnScale();
        }
        else
        {
            yield return new WaitUntil(() => transform.gameObject.activeSelf == false);
            transform.position = Vector3.one * 100;
            behaviour.Activate(transform);
            ReturnScale();
        }

        void ReturnScale()
        {
            behaviour.StartCoroutine(LocalScale(transform, initScale,
                () =>
                {
                    behaviour.Deactivate(transform);
                    transform.position = initPosition;
                }));
        }

        IEnumerator LocalScale(Transform currentTransform, Vector3 endScale, UnityAction onCompleteScale = null)
        {
            Vector3 startScale = currentTransform.localScale;
            float t = Time.deltaTime;
            while (t < 0.1f)
            {
                currentTransform.localScale = Vector3.Lerp(startScale, endScale, t / 0.1f);
                yield return null;
                t += Time.deltaTime;
            }

            currentTransform.localScale = endScale;
            onCompleteScale?.Invoke();
        }
    }

    public static IEnumerator DoLocalScaleAndUnscale(this RectTransform rectTransform, MonoBehaviour behaviour,
        Vector3 scale, bool needDeactivate = false, float deactivateAfter = 0, UnityAction onComplete = null)
    {
        behaviour.Activate(rectTransform);
        
        Vector3 initScale = rectTransform.localScale;
        Vector3 initPosition = rectTransform.anchoredPosition;

        behaviour.StartCoroutine(LocalScale(rectTransform, scale));

        onComplete?.Invoke();
        
        if (needDeactivate)
        {
            rectTransform.anchoredPosition = Vector3.one * 100;
            yield return new WaitForSeconds(deactivateAfter);
            ReturnScale();
        }
        else
        {
            yield return new WaitUntil(() => rectTransform.gameObject.activeSelf == false);
            rectTransform.anchoredPosition = Vector3.one * 100;
            behaviour.Activate(rectTransform);
            ReturnScale();
        }

        void ReturnScale()
        {
            behaviour.StartCoroutine(LocalScale(rectTransform, initScale,
                () =>
                {
                    behaviour.Deactivate(rectTransform);
                    rectTransform.anchoredPosition = initPosition;
                }));
        }

        IEnumerator LocalScale(Transform currentTransform, Vector3 endScale, UnityAction onCompleteScale = null)
        {
            Vector3 startScale = currentTransform.localScale;
            float t = Time.deltaTime;
            while (t < 0.1f)
            {
                currentTransform.localScale = Vector3.Lerp(startScale, endScale, t / 0.1f);
                yield return null;
                t += Time.deltaTime;
            }

            currentTransform.localScale = endScale;
            onCompleteScale?.Invoke();
        }
    }

    public static IEnumerator DoRotation(this Transform transform, Quaternion targetRotation, Action onComplete)
    {
        Quaternion startRotation = transform.localRotation;
        float t = Time.deltaTime;
        while (t < 0.15f)
        {
            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t / 0.15f);
            yield return null;
            t += Time.deltaTime;
        }

        transform.localRotation = targetRotation;
        onComplete?.Invoke();
    }

    public static void DelayedCall(this MonoBehaviour behaviour, float delay, Action onComplete)
    {
        behaviour.StartCoroutine(DoDelayedCall(delay, onComplete));
    }
    
    public static void DelayedBoolCall(this MonoBehaviour behaviour, float delay, Action<bool> onComplete, bool boolParameter)
    {
        behaviour.StartCoroutine(DoDelayedBoolCall(delay, onComplete, boolParameter));
    }

    private static IEnumerator DoDelayedBoolCall(float delay, Action<bool> onComplete, bool boolParameter)
    {
        yield return new WaitForSeconds(delay);
        onComplete?.Invoke(boolParameter);
    }

    private static IEnumerator DoDelayedCall(float delay, Action onComplete)
    {
        yield return new WaitForSeconds(delay);
        onComplete?.Invoke();
    }
}
