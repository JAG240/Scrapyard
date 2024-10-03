using Scrapyard.items.weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.core.character 
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private bool inAnimation = false;

        public void PlayWeaponAnimation(WeaponUseAnimation anim, float waitTime)
        {
            if (inAnimation)
                return;

            inAnimation = true;
            string boolName = string.Empty;

            switch (anim)
            {
                case WeaponUseAnimation.LightMelee:
                    boolName = "Light_Melee";
                    break;

                default:
                    boolName = "Light_Melee";
                    break;
            }

            //TODO: Adjust clip speed to match total time

            /*AnimatorClipInfo[] clips = animator.GetNextAnimatorClipInfo(0);
            AnimationClip playClip;

            foreach(AnimatorClipInfo clip in clips)
            {
                if(clip.clip.name == boolName)
                {
                    playClip = clip.clip;
                    break;
                }
            }*/

            animator.SetBool(boolName, true);
            StartCoroutine(PlayAnim(waitTime, boolName));
        }

        IEnumerator PlayAnim(float waitTime, string boolName)
        {
            yield return new WaitForSeconds(waitTime);
            animator.SetBool(boolName, false);
            inAnimation = false;
        }
    }
}
