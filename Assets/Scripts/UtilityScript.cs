using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UtilityScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool OnClickActivation(GameObject collisionObject) {
        if (Input.GetMouseButtonDown(0)) // 0 é lado esquerdo do rato
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == collisionObject)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public IEnumerator TweenGameObject(GameObject tweenObject, Vector3 endPosition, float tweenDuration, AnimationCurve? animationCurve = null, Action<bool>? callback = null)  // Adaptado de https://www.youtube.com/watch?v=MyVY-y_jK1I
    {
        float elapsedTweenTime = 0f;
        Vector3 startPosition = new Vector3(tweenObject.transform.position.x, tweenObject.transform.position.y, tweenObject.transform.position.z); 

        while (elapsedTweenTime < tweenDuration) {
            elapsedTweenTime += Time.deltaTime;
            float progress = elapsedTweenTime/tweenDuration;
            Debug.Log(progress);

            if (animationCurve == null) {
                tweenObject.transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            } else {
                tweenObject.transform.position = Vector3.Lerp(startPosition, endPosition, animationCurve.Evaluate(progress));
            }
            yield return null;
        }

        if (callback != null) { // Adaptado de https://discussions.unity.com/t/is-there-a-way-to-find-out-when-a-coroutine-is-done-executing/209396/3
            callback(false);
        }
    }

    public IEnumerator TweenGameObject(GameObject tweenObject, Vector3 unfixedStartPosition, Vector3 endPosition, float tweenDuration, AnimationCurve? animationCurve = null, Action<bool>? callback = null)  // Adaptado de https://www.youtube.com/watch?v=MyVY-y_jK1I
    {
        float elapsedTweenTime = 0f;
        Vector3 startPosition = new Vector3(unfixedStartPosition.x, unfixedStartPosition.y, unfixedStartPosition.z); 

        while (elapsedTweenTime < tweenDuration) {
            elapsedTweenTime += Time.deltaTime;
            float progress = elapsedTweenTime/tweenDuration;

            if (animationCurve == null) {
                tweenObject.transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            } else {
                tweenObject.transform.position = Vector3.Lerp(startPosition, endPosition, animationCurve.Evaluate(progress));
            }
            yield return null;
        }

        if (callback != null) { // Adaptado de https://discussions.unity.com/t/is-there-a-way-to-find-out-when-a-coroutine-is-done-executing/209396/3
            callback(false);
        }
    }

    public IEnumerator TweenGameObjectRotation(GameObject tweenObject, Vector3 vectorStartRotation, Vector3 vectorEndRotation, float tweenDuration, AnimationCurve? animationCurve = null, Action<bool>? callback = null)  // Adaptado de https://www.youtube.com/watch?v=MyVY-y_jK1I
    {
        Debug.Log(tweenObject);
        float elapsedTweenTime = 0f;
        Quaternion startRotation = Quaternion.Euler(vectorStartRotation);
        Quaternion endRotation = Quaternion.Euler(vectorEndRotation);

        while (elapsedTweenTime < tweenDuration) {
            elapsedTweenTime += Time.deltaTime;
            float progress = elapsedTweenTime/tweenDuration;

            if (animationCurve == null) {
                tweenObject.transform.rotation = Quaternion.Lerp(startRotation, endRotation, progress);
            } else {
                tweenObject.transform.rotation = Quaternion.Lerp(startRotation, endRotation, animationCurve.Evaluate(progress));
            }
            yield return null;
        }

        if (callback != null) { // Adaptado de https://discussions.unity.com/t/is-there-a-way-to-find-out-when-a-coroutine-is-done-executing/209396/3
            callback(false);
        }
    }

    public void ActivateDescendantLights(GameObject parent, bool enabled) {
        Light[] lightList = parent.GetComponentsInChildren<Light>();
        foreach (Light childLight in lightList) {
            childLight.enabled = enabled ? true : !childLight.enabled;
        }
    }
}
