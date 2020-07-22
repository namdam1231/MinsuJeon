using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerctrl : MonoBehaviour
{
    Vector3 m_dir;
    [SerializeField]
    private float mSpeed;
    private Rigidbody mRB;

    [SerializeField]
    GameObject m_attackAreaObj;
    AttackArea[] m_attackArea;

    CharacterController m_charCtrl;
    private Animator mAnim;
    PlayerAnimCtrl m_animCtr;
    GameObject m_fxHitEffectPrefab;   //피격시 이펙트

    List<PlayerAnimCtrl.eAnimState> m_comboList = new List<PlayerAnimCtrl.eAnimState>() {
        PlayerAnimCtrl.eAnimState.ATTACK1,  PlayerAnimCtrl.eAnimState.ATTACK2, PlayerAnimCtrl.eAnimState.ATTACK3 };

    int m_comboIndex;
    bool m_isPressAttack;
    bool m_isCombo;
    Queue<KeyCode> m_keyBuffer = new Queue<KeyCode>();

    #region Animation Event Methods
    void AnimEvent_Attack()
    {
        var unitList = m_attackArea[0].UnitList;
        for (int i = 0; i < unitList.Count; i++)
        {
            var effect = Instantiate(m_fxHitEffectPrefab) as GameObject;
            var dummy = Util.FindChildObject(unitList[i], "Dummy_Hit");
            effect.transform.position = dummy.transform.position;
            var pos = transform.position + Vector3.up * 1f;
            effect.transform.LookAt(pos);   //주인공을 바라보게 -> 안더해주면 이펙트가 주인공의 발을 봄
        }
    }

    void AnimEvent_AttackFinished()
    {
        m_isCombo = false;
        if (m_isPressAttack || m_keyBuffer.Count >0)
        {
            if (m_keyBuffer.Count > 1)
            {
                ResetKeyBuffer();
            }
            else
            {
                if(m_keyBuffer.Count>0)
                    m_keyBuffer.Dequeue();   //key하나 소모

                m_comboIndex++;
                if (m_comboIndex > m_comboList.Count)
                    m_comboIndex = 0;
                m_animCtr.Play(m_comboList[m_comboIndex]);
                m_isCombo = true;
            }
        }

        if (!m_isCombo)
        {
            m_comboIndex = 0;
            if (m_dir != Vector3.zero)
            {
                    m_animCtr.Play(PlayerAnimCtrl.eAnimState.RUN);
                    transform.forward = m_dir;
            }
            else
            { 
                    m_animCtr.Play(PlayerAnimCtrl.eAnimState.IDLE);
            }
        }
    }
    #endregion

   

    bool IsAttack()
    {
        if (m_animCtr.GetAnimState() == PlayerAnimCtrl.eAnimState.ATTACK1 ||
            m_animCtr.GetAnimState() == PlayerAnimCtrl.eAnimState.ATTACK2 ||
            m_animCtr.GetAnimState() == PlayerAnimCtrl.eAnimState.ATTACK3 )
            return true;
        return false;
    }
    void SetLocomotion()
    {
        if (m_dir != Vector3.zero && !IsAttack())
        {
            if (m_animCtr.GetAnimState() == PlayerAnimCtrl.eAnimState.IDLE)
                m_animCtr.Play(PlayerAnimCtrl.eAnimState.RUN);
            if (!IsAttack())
                transform.forward = m_dir;

        }
        else
        {
            if (m_animCtr.GetAnimState() == PlayerAnimCtrl.eAnimState.RUN)
                m_animCtr.Play(PlayerAnimCtrl.eAnimState.IDLE);
        }
    }

    private void ResetKeyBuffer()
    {
        m_keyBuffer.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        mRB = GetComponent<Rigidbody>();
        m_charCtrl = GetComponent<CharacterController>();
        m_animCtr = GetComponent<PlayerAnimCtrl>();
        mAnim = GetComponent<Animator>();
        m_attackArea = m_attackAreaObj.GetComponentsInChildren<AttackArea>();

        m_fxHitEffectPrefab = Resources.Load("Prefab/Effect/FX_Attack01_01") as GameObject;
    }

    private void FixedUpdate()
    {
      //  m_dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //mRB.AddForce(m_dir * mSpeed, ForceMode.Force);
      //  mRB.MovePosition(transform.position + m_dir * mSpeed * Time.fixedDeltaTime); //등가속도 운동하는 것을 없애기 위해 -> 미끄러짐
        //mRB.velocity = m_dir.normalized * mSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        m_dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        SetLocomotion();

         if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsInvoking("ResetKeyBuffer"))
                CancelInvoke("ResetKeyBuffer");
         
            m_isPressAttack = true;
            if(m_animCtr.GetAnimState() == PlayerAnimCtrl.eAnimState.IDLE || m_animCtr.GetAnimState() == PlayerAnimCtrl.eAnimState.IDLE)
            {
                m_animCtr.Play(PlayerAnimCtrl.eAnimState.ATTACK1);
            }
            else if (IsAttack())
            {                
                m_keyBuffer.Enqueue(KeyCode.Space);
                Invoke("ResetKeyBuffer", m_animCtr.GetAnimationClipLength(m_animCtr.GetAnimState().ToString()) / 2f);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_isPressAttack = false;
        }

        if (!IsAttack())
            m_charCtrl.SimpleMove(m_dir * mSpeed);  //SimpleMove -> 중력의 영향 받음     Move -> 중력의 영향 안받음
    }
}


/*
[SerializeField]
private float mSpeed;

private Rigidbody mRB;

void Awake()
{
    mRB = GetComponent<Rigidbody>();
}

void Update()
{
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vettical");
    Vector3 dir = new Vector3(horizontal, 0, vertical);
    mRB.velocity = dir.normalize*mSpeed;
}
 */

//충돌처리를 해주기 위해서 plane(땅)에다가 physical material을 넣어주어야 한다
// -> static friction 조정해주기

//character ctrl를 부착해준다 -> 이동할 수 있는 slop 각도 정해줄 수 있다
// -> rigidbody, capsule collider 사용 X

//modeling 수정 -> ex 바이오하자드 모드

//mixamo -> animation

//animation 길이에 따라 key buffer 시간을 정해줌 -> 보통 animation의 절반 -> combo 하려면