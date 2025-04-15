using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class CardAnimator : MonoBehaviour
{
    [SerializeField] private float _cardSpeed = 100f;

    public async Task Animate(Vector3 startPos, Vector3 targetPos, Transform originalTransform) {
        var tcs = new TaskCompletionSource<bool>();
        StartCoroutine(DrawingAnimation(startPos, targetPos, originalTransform, tcs));
        await tcs.Task;
    }

    IEnumerator DrawingAnimation(Vector3 startPos, Vector3 targetPos, Transform originalTransform, TaskCompletionSource<bool> tcs) {
        // Ensure that the card is at the start position first.
        originalTransform.position = startPos;

        float distance = Vector3.Distance(startPos, targetPos);
        float timeToReachTarget = distance / _cardSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < timeToReachTarget) {
            float t = elapsedTime / timeToReachTarget;
            originalTransform.position = Vector3.Lerp(startPos, targetPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the card reaches the exact target position at the end.
        originalTransform.position = targetPos;

        tcs.SetResult(true);
    }
}
