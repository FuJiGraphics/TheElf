using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SwordAura : BulletSC
{
    float currentAngle = 0f;
    [SerializeField] int rotationCount = 1;
    [SerializeField] float orbitRadius = 5f;
    [SerializeField] float offsetAngle = 0f;
    [SerializeField] float maxAngle = 360f;

    private bool isFirstCalled = true;
    private int currentRotateCount = 0;
    private Vector3 targetPos = Vector3.zero;

    protected override void DisableTrigger()
    {
        rb.velocity = Vector3.zero;
        isFirstCalled = true;
        currentAngle = 0;
        currentRotateCount = 0;
    }

    protected override void MoveTrigger()
    {
        if (currentRotateCount >= rotationCount)
        {
            base.Destroy();
            return;
        }
        if (isFirstCalled)
        {
            isFirstCalled = false;
            setPosition = owner.transform.position + new Vector3(orbitRadius, 0, 0); // 처음 위치 설정
            transform.position = setPosition;
            rb.velocity = Vector3.zero;
            currentAngle = 0f;
            Quaternion rotOffset = Quaternion.Euler(0f, 0f, offsetAngle);
            Vector3 dir = rotOffset * setDirection.normalized;
            targetPos = (dir * orbitRadius);
        }
        currentAngle += Mathf.Clamp(Time.deltaTime * projectileSpeed * 10f, 0f, maxAngle);
        if (currentAngle >= maxAngle)
        {
            currentAngle = 0f;
            currentRotateCount++;
        }

        transform.rotation = Quaternion.identity;
        transform.position = owner.transform.position + targetPos;
        transform.RotateAround(owner.transform.position, Vector3.forward, currentAngle);
    }
} // class SwordAura
