using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]    //실행중이 아님에도 값을 변화시킬 수 있음 -> Game Scene애서 보임

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    [Range(0f,20f)]    //slidebar 생김
    float m_distance;
    [SerializeField]
    [Range(0f, 20f)]
    float m_height;
    [SerializeField]
    [Range(0f, 180f)]
    float m_angle;
    [SerializeField]
    [Range(0.2f,2f)]
    float mSpeed;
    [SerializeField]
    Transform m_target;

    Transform m_prevTransform;   //이전좌표

    void UpdatePosition()
    {
        transform.position = new Vector3(Mathf.Lerp(m_prevTransform.position.x, m_target.position.x, mSpeed * Time.deltaTime),
           Mathf.Lerp(m_prevTransform.position.y, m_target.position.y + m_height, mSpeed * Time.deltaTime),
           Mathf.Lerp(m_prevTransform.position.z, m_target.position.z - m_distance, mSpeed * Time.deltaTime)
           );

        transform.eulerAngles = new Vector3(Mathf.Lerp(m_prevTransform.eulerAngles.x, m_angle, mSpeed * Time.deltaTime), 0f, 0f);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        m_prevTransform = m_target;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    private void LateUpdate()
    {
        m_prevTransform = transform;
    }
}
