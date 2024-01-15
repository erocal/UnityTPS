using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("物件上下移動頻率")]
    [SerializeField] float verticalBobFrequency = 1f;
    [Header("物件上下移動的距離")]
    [SerializeField] float bobbingAmount = 1f;
    [Header("每秒旋轉的角度")]
    [SerializeField] float rotatingSpeed = 360f;

    public event Action<GameObject> onPick;


    Rigidbody rigidbody;
    Collider collider;

    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        rigidbody.isKinematic = true;
        collider.isTrigger = true;

        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 上下移動的公式
        float bobbingAnimationPhase = ((Mathf.Sin(Time.time * verticalBobFrequency) * 0.5f) + 0.5f) * bobbingAmount;
        transform.position = startPosition + Vector3.up * bobbingAnimationPhase;

        transform.Rotate(Vector3.up, rotatingSpeed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onPick?.Invoke(other.gameObject);
        }
    }
}
