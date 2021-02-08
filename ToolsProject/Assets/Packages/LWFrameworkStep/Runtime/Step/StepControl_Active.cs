using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Xml.Linq;
namespace LWFrameworkStep
{
    public class StepControl_Active : StepControl_Base
    {
        public string isActive;
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
        public override void StepStart()
        {
            base.StepStart();

        }
        public override void StepEnd()
        {
            Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            if (isActive == "true")
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
            base.StepEnd();
        }


    }
}