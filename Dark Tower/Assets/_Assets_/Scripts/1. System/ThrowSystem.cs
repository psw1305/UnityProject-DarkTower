using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 던지는 시스템 참고 자료
/// https://www.youtube.com/watch?v=Qxs3GrhcZI8


public class ThrowSystem : MonoBehaviour 
{
    [SerializeField] float _angle;
    [SerializeField] float _step;
    [SerializeField] LineRenderer _line;
    [SerializeField] Transform _firePoint;
    private Camera _cam;
    private int layermask;

    private void Start()
    {
        _cam = Camera.main;
        layermask = 1 << LayerMask.NameToLayer("ExploreObject");
    }

    private void Update() 
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            Vector3 dir = hit.point - _firePoint.position;
            Vector3 groundDir = new Vector3(dir.x, 0, dir.z);

            Vector3 targetPos = new Vector3(groundDir.magnitude, dir.y, 0);
            float height = targetPos.y + targetPos.magnitude / 2f;
            height = Mathf.Max(0.01f, height);
            float angle;
            float v0;
            float time;
            
            CalculatePathWithHeight(targetPos, height, out v0, out angle, out time);
            DrawPath(groundDir.normalized, v0, angle, time, _step);
            
            if (Input.GetMouseButtonDown(0))
            {
                Debug.DrawRay(ray.origin, ray.direction * 200, Color.red, 5f);
                StopAllCoroutines();
                StartCoroutine(Coroutine_Movement(groundDir.normalized, v0, angle, time));
            }
        }
    }

    private void DrawPath(Vector3 dir, float v0, float angle, float time, float step) 
    {
        step = Mathf.Max(0.01f, step);
        _line.positionCount = (int)(time / step) + 2;

        int count = 0;
        for (float i = 0; i < time; i+= step) 
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);
            _line.SetPosition(count, _firePoint.position + (dir * x) + (Vector3.up * y));
            count++;
        }

        float endX = v0 * time * Mathf.Cos(angle);
        float endY = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        _line.SetPosition(count, _firePoint.position + (dir * endX) + (Vector3.up * endY));
    }

    private float QuadraticEquation(float a, float b, float c, float sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }

    private void CalculatePathWithHeight(Vector3 targetPos, float h, out float v0, out float angle, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float b = Mathf.Sqrt(2 * g * h);
        float a = (-0.5f * g);
        float c = -yt;

        float tplus = QuadraticEquation(a, b, c, 1);
        float tmin = QuadraticEquation(a, b, c, -1);
        time = tplus > tmin ? tplus : tmin;

        angle = Mathf.Atan(b * time / xt);

        v0 = b / Mathf.Sin(angle);
    }

    private void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1 / (v2 - v3));

        time = xt / (v0 * Mathf.Cos(angle));
    }

    IEnumerator Coroutine_Movement(Vector3 dir, float v0, float angle, float time) 
    {
        float t = 0;
        while (t < time) 
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = _firePoint.position + (dir * x) + (Vector3.up * y);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
