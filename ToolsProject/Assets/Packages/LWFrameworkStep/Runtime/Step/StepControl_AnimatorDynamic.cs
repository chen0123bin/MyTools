using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Xml.Linq;
namespace LWFrameworkStep {
    public class StepControl_AnimatorDynamic : StepControl_Base
    {
        private Animator animator;
        private string oldAnimName;
        private float oldTime;
        private string newAnimName;
        private float newTime;
        // Use this for initialization

        public override void StepStart()
        {
            base.StepStart();
            animator = GetComponent<Animator>();
            animator.Play(oldAnimName, 0, oldTime);
        }
        public override void StepEnd()
        {
            if (newAnimName == "Dynamic")
                animator.Play(StepData.AnimationName, 0, newTime);
            base.StepEnd();
        }




        public override void InitEnd(object xmlData)
        {

            base.InitEnd(xmlData);
            XElement value = (XElement)xmlData;
            newAnimName = value.Element("AnimtorControl").Attribute("AnimationName").Value;
            newTime = float.Parse(value.Element("AnimtorControl").Attribute("InitValue").Value);
        }
        public void InitStart(object xmlData)
        {
            XElement value = (XElement)xmlData;
            oldAnimName = value.Element("AnimtorControl").Attribute("AnimationName").Value;
            oldTime = float.Parse(value.Element("AnimtorControl").Attribute("InitValue").Value);


        }

    }

}
