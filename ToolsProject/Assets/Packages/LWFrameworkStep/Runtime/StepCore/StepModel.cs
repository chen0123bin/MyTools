using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using System;
namespace LWFrameworkStep {
    public class StepModel
    {
        private GameObject gameObject;
        private XElement Element;
        //完成状态信号量
        private static int _finishSemaphore;


        //当前步骤处理完成
        public static void FinishScript()
        {
            //Debug.Log(_finishSemaphore);
            _finishSemaphore--;

            //点击代码执行完成
            if (_finishSemaphore <= 0)
            {
                StepManager.GetInstance().Finish();
                _finishSemaphore = 0;
            }
        }
        //当前步骤处理完成
        public static void BackScript()
        {
            _finishSemaphore--;

            //点击代码执行完成
            if (_finishSemaphore <= 0)
            {
                StepManager.GetInstance().Back();
                _finishSemaphore = 0;
            }
        }
        void SetManagerFinish()
        {
        }
        //设置此步骤有多少步操作模型
        public static int StepControlCount
        {
            set
            {
                _finishSemaphore = value;
            }
            get
            {
                return _finishSemaphore;
            }
        }

        public StepModel()
        {

        }

        public void Exec(XElement x)
        {
            Element = x;
            gameObject = ObjectManager.Instance.GetGameObject(Element.Attribute("Name").Value);
            if (Element.Attribute("IsStepControl") != null && Element.Attribute("IsStepControl").Value == "true")
            {
                if (Element.Attribute("ControlCount") != null)
                {
                    StepControlCount += int.Parse(Element.Attribute("ControlCount").Value);
                }
                else
                {
                    StepControlCount++;
                }

            }

            List<XElement> list = Element.Elements().ToList();
            for (int i = 0; i < list.Count; i++)
            {
                XElement e = list[i];
                Factory(e.Name.ToString());
            }

        }

        private void Factory(string tag)
        {
            switch (tag)
            {
                case "Transform":
                    SetTransform(Element.Element("Transform"), gameObject);
                    break;
                case "Component":
                    // Debug.Log("AddComponent");
                    ComponentUtil.AddComponents(Element.Element("Component"), gameObject);
                    break;
                case "Delete":
                    SetDelete();
                    break;
                case "Active":
                    SetActive(Element.Element("Active"));
                    break;
                case "RenderingMode":
                    SetRenderingMode(Element.Element("RenderingMode"), gameObject);
                    break;

            }
        }
        private void SetRenderingMode(XElement t, GameObject gameObject)
        {
            string mode = t.Attribute("Mode").Value;
            float alphaValue = float.Parse(t.Attribute("AlphaValue").Value);
            bool isAll = t.Attribute("IsAll").Value == "true" ? true : false;

            if (isAll)
            {
                MeshRenderer[] rendererArray = gameObject.GetComponentsInChildren<MeshRenderer>();
                for (int i = 0; i < rendererArray.Length; i++)
                {
                    SetGameObjectTransparent(mode, rendererArray[i], alphaValue);
                }
            }
            else
            {
                SetGameObjectTransparent(mode, gameObject.GetComponent<MeshRenderer>(), alphaValue);

            }
        }
        void SetGameObjectTransparent(string mode, MeshRenderer renderer, float alphaValue)
        {
            Material[] matArray = renderer.materials;
            for (int i = 0; i < matArray.Length; i++)
            {
                matArray[i].color = new Color(matArray[i].color.r, matArray[i].color.g, matArray[i].color.b, alphaValue);
                RenderingModeUtil.SetMaterialRenderingModeShader(matArray[i], RenderingModeUtil.StringConvertToEnum(mode));
            }

        }
        private void SetDelete()
        {
            GameObject.Destroy(gameObject);
        }

        //设置模型是否可见
        private void SetActive(XElement t)
        {
            if (t.Attribute("Type").Value == "true")
            {
                gameObject.SetActive(true);

            }
            else if (t.Attribute("Type").Value == "false")
            {
                gameObject.SetActive(false);
            }
        }


        private void SetTransform(XElement t, GameObject go)
        {
            //判断是否有父对象
            if (t.Attribute("Parent") != null && t.Attribute("Parent").Value != "")
            {
                go.transform.parent = ObjectManager.Instance.GetGameObject(t.Attribute("Parent").Value).transform;
            }
            if (t.Element("Position") != null)
            {
                Vector3 position = VectorUtil.XmlToVector3(t.Element("Position"));
                go.transform.localPosition = position;
            }
            if (t.Element("Rotation") != null)
            {
                Vector3 rotation = VectorUtil.XmlToVector3(t.Element("Rotation"));
                go.transform.localEulerAngles = rotation;
            }
            if (t.Element("Scale") != null)
            {
                Vector3 scale = VectorUtil.XmlToVector3(t.Element("Scale"));
                go.transform.localScale = scale;
            }



        }



    }

}
