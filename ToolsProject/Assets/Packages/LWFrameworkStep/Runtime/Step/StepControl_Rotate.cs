using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Xml.Linq;
namespace LWFrameworkStep {
    public class StepControl_Rotate : StepControl_Base
    {
        public Vector3 _rotation;
        public float _time;
        public float _speed;
        public bool _canRotate = false;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_canRotate)
            {
                if (_time > 0)
                {
                    transform.Rotate(_rotation, _speed * Time.deltaTime);
                    _time -= Time.deltaTime;
                }
                else
                {
                    base.StepEnd();
                }
            }


        }
        public override void StepStart()
        {
            base.StepStart();
        }
        public override void StepEnd()
        {
            //  base.StepEnd();
            _canRotate = true;
        }

        private void Complete()
        {
            base.StepEnd();
        }

        private void WayPointChange(int value)
        {

        }

        public override void InitEnd(object xmlData)
        {

            base.InitEnd(xmlData);
            XElement value = (XElement)xmlData;
            _rotation = VectorUtil.XmlToVector3(value.Element("Rotation"));


            _time = float.Parse(value.Attribute("Time").Value);
            _speed = float.Parse(value.Attribute("Speed").Value);
        }
    }
}

