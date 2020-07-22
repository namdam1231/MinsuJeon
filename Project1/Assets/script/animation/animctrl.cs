using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animctrl : MonoBehaviour
{
    Animator m_animator;
    
    public float GetAnimationClipLength(string animClipName) //animation의 클립의 재생시간을 return
    {
        var ac = m_animator.runtimeAnimatorController;
        var clips = ac.animationClips;
        for(int i=0; i<clips.Length; i++)
        {
            if (clips[i].name.Equals(animClipName))
                return clips[i].length;
        }
        return 0f;
    }


    public void Play(string animTriggerName, bool isFade)   //isFade -> blending 해줄 것 인지
    {
        if (isFade)
        {
            m_animator.SetTrigger(animTriggerName);   //모션블랜딩
        }
        else
        {
            m_animator.Play(animTriggerName, 0, 0f);   //블랜딩 하지 않고 즉시 모션을 실행  //0으로 돌려야 처음부터 다시 재생됨
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
