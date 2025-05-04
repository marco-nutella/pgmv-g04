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

    public IEnumerator TweenGameObject(GameObject tweenObject, Vector3 endPosition, float tweenDuration, AnimationCurve? animationCurve, Action<bool>? debounceCallback)  // Adaptado de https://www.youtube.com/watch?v=MyVY-y_jK1I
    {
        float elapsedTweenTime = 0f;
        Vector3 startPosition = new Vector3(tweenObject.transform.position.x, tweenObject.transform.position.y, tweenObject.transform.position.z); 

        while (elapsedTweenTime < tweenDuration) {
            elapsedTweenTime += Time.deltaTime;
            float progress = elapsedTweenTime/tweenDuration;

            if (animationCurve != null) {
                tweenObject.transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            } else {
                tweenObject.transform.position = Vector3.Lerp(startPosition, endPosition, animationCurve.Evaluate(progress));
            }
            yield return null;
        }

        if (debounceCallback != null) { // Adaptado de https://discussions.unity.com/t/is-there-a-way-to-find-out-when-a-coroutine-is-done-executing/209396/3
            debounceCallback(false);
        }
    }
}
