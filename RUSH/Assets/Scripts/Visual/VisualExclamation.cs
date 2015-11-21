using UnityEngine;
using System.Collections;

public class VisualExclamation : MonoBehaviour {

    private Vector3 startPosition;
    private Vector3 targetPosition;

    [SerializeField]
    private float duration = .3f;

    [SerializeField]
    private float heightBetweenStartAndTarget = 3;

    public void InitializePosition(Vector3 endPosition)
    {
        transform.position = endPosition + Vector3.up * heightBetweenStartAndTarget;
        startPosition = transform.position;
        targetPosition = endPosition;
        StartCoroutine(ApearCoroutine());
    }

    IEnumerator ApearCoroutine ()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }


    }
}
