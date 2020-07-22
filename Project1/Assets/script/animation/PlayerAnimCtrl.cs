using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCtrl : animctrl
{
    public enum eAnimState
    {
        IDLE,
        RUN,
        ATTACK1,
        ATTACK2,
        ATTACK3,
        MAX
    }


    public void Play(eAnimState animState, bool isFade = true)
    {
        m_state = animState;
        base.Play(animState.ToString(), isFade);  //부모 -> animctrl의 play 함수를 사용
    }

    eAnimState m_state;

    public eAnimState GetAnimState()
    {
        return m_state;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

