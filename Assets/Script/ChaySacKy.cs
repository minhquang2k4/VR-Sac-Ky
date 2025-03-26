using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaySacKy : MonoBehaviour
{

    // [SerializeField] private Canvas _canvas;
    //
    // void OnTriggerEnter(Collider other) {
    //     if( other.gameObject.CompareTag("BinhSacKy"))
    //     {
    //         StartCoroutine(SacKy());
    //     }
    // }
    // IEnumerator SacKy()
    // {
    //     yield return new WaitForSeconds(20);
    //     _canvas.gameObject.SetActive(true);
    // }
    [SerializeField] private RectTransform _canvasMask; // Mask hoặc UI Panel che phần trên
    private float _startHeight = 0f;  // Chiều cao ban đầu (ẩn)
    private float _targetHeight = 0.01f;  // Chiều cao tối đa (hiển thị đầy đủ)

    void Start()
    {
        _canvasMask.sizeDelta = new Vector2(_canvasMask.sizeDelta.x, _startHeight); // Ẩn ban đầu
        _canvasMask.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BinhSacKy"))
        {
            StartCoroutine(SacKy());
        }
    }

    IEnumerator SacKy()
    {
        yield return new WaitForSeconds(2); // Đợi 2 giây trước khi hiện

        _canvasMask.gameObject.SetActive(true);
        float duration = 10f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newHeight = Mathf.Lerp(_startHeight, _targetHeight, elapsedTime / duration);
            _canvasMask.sizeDelta = new Vector2(_canvasMask.sizeDelta.x, newHeight);
            yield return null;
        }

        _canvasMask.sizeDelta = new Vector2(_canvasMask.sizeDelta.x, _targetHeight);
    }
}
