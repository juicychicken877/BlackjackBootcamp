using System.Collections;
using UnityEngine;

public class CardAnimator : MonoBehaviour
{
    [SerializeField] private float _drawingAnimationDuration = 0.5f;

    public void StartDrawingAnimation(Vector3 startPos, Vector3 targetPos, Transform originalTransform) {
        StartCoroutine(DrawingAnimation(startPos, targetPos, originalTransform));
    }

    IEnumerator DrawingAnimation(Vector3 startPos, Vector3 targetPos, Transform originalTransform) {
        // Ensure that the card is at the start position first.
        originalTransform.position = startPos;

        float elapsedTime = 0f;

        while (elapsedTime < _drawingAnimationDuration) {
            float t = elapsedTime / _drawingAnimationDuration;

            originalTransform.position = Vector3.Lerp(startPos, targetPos, t);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the card reaches the exact target position at the end
        originalTransform.position = targetPos;
    }
}
