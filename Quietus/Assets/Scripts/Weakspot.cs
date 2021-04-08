using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakspot : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float moveDistance = 5f;
    [SerializeField] float wait = 1f;
    private Vector3 newPos;

    private void OnEnable()
    {
        StartCoroutine(SetNewPosition());
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, Time.deltaTime * speed);
    }

    private IEnumerator SetNewPosition()
    {
        while (true)
        {
            float randomX = Random.Range(transform.localPosition.x - moveDistance, transform.localPosition.x + moveDistance);
            float randomY = Random.Range(transform.localPosition.y - moveDistance, transform.localPosition.y + moveDistance);
            newPos = new Vector3(randomX, randomY, transform.localPosition.z);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
