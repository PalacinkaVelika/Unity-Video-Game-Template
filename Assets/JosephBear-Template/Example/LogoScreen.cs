using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScreen : MonoBehaviour {

    public Animator UIanim;

    public void PlayLogoAnimation() {
        UIanim.SetTrigger("show");
    }

    public float GetAnimationLenght() {
        AnimatorStateInfo stateInfo = UIanim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length;
    }

}
