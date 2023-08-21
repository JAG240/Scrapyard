using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SplashBehavior : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float lifeTime;
    [SerializeField] private float curveMax;
    [SerializeField] private TMP_Text damageNumber;

    private Color numberColor;
    private float life = 0f;

    public void Init(float num)
    {
        damageNumber.text = num.ToString();
        numberColor = Color.white;
    }

    private void Update()
    {
        life += Time.deltaTime;

        UpdateColor();
        UpdatePos();

        if(life > lifeTime)
            Destroy(gameObject);
    }

    private void UpdateColor()
    {
        Color newColor = numberColor;
        newColor.a = curve.Evaluate(life / lifeTime);
        damageNumber.color = newColor;
    }

    private void UpdatePos()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        Vector3 newPos = transform.position;
        newPos.y = curve.Evaluate(life / lifeTime) * curveMax;
        transform.position = newPos;
    }
}
