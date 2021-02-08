using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Xml.Linq;
namespace LWFrameworkStep
{
    public class StepControl_TweenMove : StepControl_Base
    {
        public Vector3 _postion;
        public Vector3 _rotation;
        public Vector3 _scale;
        private bool changeRotation;
        private bool changeScale;
        private float _time;
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
            // transform.DOMoveX(10, 4);
            //Vector3 v1 = new Vector3(0, 3, 3);
            //Vector3 v2 = new Vector3(2, 4, 5);
            //Vector3 v3 = new Vector3(2, 6, 5);
            //Vector3[] aa = new Vector3[] { v1, v2, v3 };
            //transform.DOPath(aa,4).OnWaypointChange(WayPointChange).OnComplete(Complete);

            transform.DOLocalMove(_postion, _time + 0.04f).OnComplete(Complete);
            if (changeRotation)
                transform.DOLocalRotate(_rotation, _time);
            if (changeScale)
                transform.DOScale(_scale, _time);
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
            _postion = VectorUtil.XmlToVector3(value.Element("Position"));
            if (value.Element("Rotation") != null)
            {
                _rotation = VectorUtil.XmlToVector3(value.Element("Rotation"));
                changeRotation = true;
            }
            else
            {
                changeRotation = false;
            }
            if (value.Element("Scale") != null)
            {
                _scale = VectorUtil.XmlToVector3(value.Element("Scale"));
                changeScale = true;
            }
            else
            {
                changeScale = false;
            }

            _time = float.Parse(value.Attribute("Time").Value);
        }
        public void InitStart(object xmlData)
        {
            XElement value = (XElement)xmlData;
            transform.localPosition = VectorUtil.XmlToVector3(value.Element("Position"));

            if (value.Element("Rotation") != null)
            {
                transform.localRotation = Quaternion.Euler(VectorUtil.XmlToVector3(value.Element("Rotation")));
            }
            if (value.Element("Scale") != null)
            {
                transform.localScale = VectorUtil.XmlToVector3(value.Element("Scale"));
            }
        }
    }
}