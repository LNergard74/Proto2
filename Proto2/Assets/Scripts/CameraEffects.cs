using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    public float minSpeed = 0f;
    public float maxSpeed = 40f;
    public float player_speed;
    float m_FieldOfView;
    public ParticleSystem Speedlines;

    void Start()
    {
        m_FieldOfView = 60.0f;

        //Speedlines = gameObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        player_speed = GameObject.Find("Player").GetComponent<Rigidbody>().velocity.magnitude;

        float parameter = Mathf.InverseLerp(minSpeed, maxSpeed, player_speed);

        m_FieldOfView = Mathf.Lerp(60, 100, parameter);
        Camera.main.fieldOfView = m_FieldOfView;

        if (player_speed >= 15)
        {
            Speedlines.Play();
        }
        else
        {
            Speedlines.Stop();
        }
    }
}
