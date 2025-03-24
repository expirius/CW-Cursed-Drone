using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.XR;


public class CursedDroneBehaviour : MonoBehaviour
{
    public Player target; // Цель (игрок)
    public float speed = 5f; // Базовая скорость
    public float followDistance = 3f; // Дистанция следования
    public float heightOffset = 1f; // Высота относительно игрока
    private Rigidbody rb;

    public void Initialize(Player __instance)
    {
        target = __instance;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        Debug.Log($"[Cursed Drone] Cursed Drone is Initialize() target pV.ViewID: {__instance.photonView.ViewID} , pV.Name: {__instance.photonView.name}");
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // Рассчитываем целевую позицию
        Vector3 targetPos = target.HeadPosition()
                    - target.data.lookDirection * followDistance
                    + Vector3.up * 1.2f;

        // Направление движения
        Vector3 direction = (targetPos - transform.position).normalized;

        // Плавное вращение в сторону цели
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.fixedDeltaTime * 5f
            );
        }

        // Применяем силу с учетом расстояния
        float distance = Vector3.Distance(transform.position, targetPos);
        float adaptiveSpeed = speed * Mathf.Clamp(distance / followDistance, 0.5f, 2f);

        rb.AddForce(direction * adaptiveSpeed, ForceMode.Acceleration);

        // Ограничение максимальной скорости
        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
        Debug.DrawLine(transform.position, targetPos, Color.green);
    }

}
