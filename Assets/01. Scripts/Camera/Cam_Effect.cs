using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Effect : MonoBehaviour
{
    public  static  Cam_Effect  Instance;

    private float   Shake_timer;

    private Vector3 Default_Pos;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Default_Pos = transform.position;
    }

    public IEnumerator Cam_Shake(float ShakeAmount, float ShakeTime)
    {
        float timer = 0;
        while (timer <= ShakeTime)
        {
            Camera.main.transform.position =
                Default_Pos + (Vector3)Random.insideUnitCircle * ShakeAmount;
            timer += Time.deltaTime;
            yield return null;
        }

        float y = 0.0f;

        while (y <= 1.0f)
        {
            y += Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, Default_Pos, y);

            yield return null;
        }
    }
}
