using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Xml.Linq;
using LWFramework.Message;
using LWFramework.Core;
namespace LWFrameworkStep
{
    public class StepControl_ContentMessage : StepControl_Base
    {
        private string oldMessageType;
        private string oldInitValue;
        private string newMessageType;
        private string newInitValue;
        // Use this for initialization

        public override void StepStart()
        {
            base.StepStart();
            if (oldMessageType != "None")
            {
                Message msg = MessagePool.GetMessage(oldMessageType);

                SetMsg(oldInitValue, msg);
                MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher(msg);
            }

        }
        public override void StepEnd()
        {
            //Debug.Log(newMessageType);
            if (newMessageType != "None")
            {
                Message msg = MessagePool.GetMessage(newMessageType);
                SetMsg(newInitValue, msg);
                MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher(msg);
            }

            base.StepEnd();
        }


        void SetMsg(string xmlStr, Message msg)
        {
            if (xmlStr == "") return;
            string[] xmlValues = xmlStr.Split(';');
            for (int i = 0; i < xmlValues.Length; i++)
            {

                string[] filed = xmlValues[i].Split('=');
                msg[filed[0]] = filed[1];
            }
        }

        public override void InitEnd(object xmlData)
        {

            base.InitEnd(xmlData);
            XElement value = (XElement)xmlData;
            newMessageType = value.Element("ContentMessage").Attribute("MessageType").Value;
            newInitValue = value.Element("ContentMessage").Attribute("InitValue").Value;
        }
        public void InitStart(object xmlData)
        {
            XElement value = (XElement)xmlData;
            oldMessageType = value.Element("ContentMessage").Attribute("MessageType").Value;
            oldInitValue = value.Element("ContentMessage").Attribute("InitValue").Value;


        }

    }
}