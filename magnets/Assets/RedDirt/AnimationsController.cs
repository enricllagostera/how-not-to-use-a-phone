using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedDirt
{


    public class AnimationsController : MonoBehaviour
    {
        public Animator animator;
        private bool eyeOpen;

        public void OpenEye()
        {
            StopAllCoroutines();
            eyeOpen = true;
            StartCoroutine(OpenAnimation());
        }

        public void CloseEye()
        {
            StopAllCoroutines();
            eyeOpen = false;
            animator.SetTrigger("ClosedEye");
        }

        public void Blink()
        {
            StartCoroutine(BlinkAnimation());
        }

        private IEnumerator OpenAnimation()
        {
            animator.SetTrigger("OpenedEye");
            while (eyeOpen)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f, 2.5f));
                StartCoroutine(BlinkAnimation());
            }
        }

        private IEnumerator BlinkAnimation()
        {
            animator.SetTrigger("ClosedEye");
            yield return new WaitForSeconds(0.1f);
            animator.SetTrigger("OpenedEye");
        }
    }
}