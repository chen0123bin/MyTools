using LWFramework.Core;
using LWFramework.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
namespace LWFrameworkStep
{
    public class StepManager : MonoBehaviour
    {
        private static StepManager _instance;

        public static StepManager GetInstance()
        {
            return _instance;
        }
        /// <summary>
        /// 是否为下一步
        /// </summary>
        public bool isNext { get; set; } = true;
        private void Awake()
        {
            _instance = this;
        }
        //xml配置文件
        private XElement _root;
        private List<XElement> _steps;
        private int _currentStepIndex = 0;
        private int _maxStep = 0;
        //当前章节的所有步骤是否已经完成
        private bool allStepIsFinish = true;
        private bool currentStepIsFinish = false;
        private StepEvent_Base _stepEvent;
        private GameObject _eventGameObject;


        private float waitTime = 0f;
        private float waitTimeMax = 0.3f;
        private bool isPlay = false;
        // Use this for initialization
        void Start()
        {
            MainManager.Instance.GetManager<GlobalMessageManager>().AddListener(CommonMessageType.Common_PlayPauseBtn, SetPlayState);
        }

        private void SetPlayState(Message msg)
        {
            isPlay = !isPlay;
            //throw new NotImplementedException();
            if (isPlay)
            {
                MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher(CommonMessageType.Common_NextBtn);
            }
            else if (!isPlay)
            {
                StopAllCoroutines();
            }
        }

        public GameObject GetEventGameobject()
        {
            return _eventGameObject;
        }
        //当前章节的步骤是否结束
        public bool AllStepIsFinish()
        {
            return allStepIsFinish;
        }
        /// <summary>
        /// 初始化当前章节的所有步骤
        /// </summary>
        /// <param name="xElement"></param>
        public void SetStepsXml(XElement xElement)
        {
            _root = xElement;
            _steps = _root.Elements("Step").ToList();
            allStepIsFinish = false;
            currentStepIsFinish = true;
            _currentStepIndex = 0;
            _maxStep = _steps.Count;
        }
        /// <summary>
        /// 初始化上一章节的所有步骤
        /// </summary>
        /// <param name="xElement"></param>
        public void SetLastStepsXml(XElement xElement)
        {
            _root = xElement;
            _steps = _root.Elements("Step").ToList();
            allStepIsFinish = false;
            currentStepIsFinish = true;

            _maxStep = _steps.Count;
            _currentStepIndex = _maxStep - 1;
        }
        public void Finish()
        {
            Debug.Log("完成当前 第" + _currentStepIndex + "步骤" + _maxStep);
            _currentStepIndex++;
            currentStepIsFinish = true;
            GameObject.Destroy(_eventGameObject.GetComponent<EventObjectPrompt>());
            _eventGameObject = null;
            //重新启动新步骤需要增加0.2秒的延迟
            waitTime = waitTimeMax;
            //判断是否完成所有的章节
            if (_currentStepIndex == _maxStep)
            {
                allStepIsFinish = true;
                isNext = true;
            }
        }
        public void Back()
        {

            _currentStepIndex--;
            Debug.Log("返回上一步步骤" + _currentStepIndex);
            currentStepIsFinish = true;
            GameObject.Destroy(_eventGameObject.GetComponent<EventObjectPrompt>());
            _eventGameObject = null;
            //重新启动新步骤需要增加0.2秒的延迟
            waitTime = waitTimeMax;
            //判断是否完成所有的章节
            if (_currentStepIndex == -1)
            {
                allStepIsFinish = true;
                isNext = false;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void LateUpdate()
        {
            if (_eventGameObject == null && currentStepIsFinish == true && allStepIsFinish == false)
            {
                waitTime -= Time.deltaTime;
                if (waitTime <= 0)
                {
                    Debug.Log("开启新步骤" + _currentStepIndex);
                    currentStepIsFinish = false;
                    SetStep();
                }
            }
        }
        //获取当前步骤下的节点
        private XElement GetStep()
        {

            return _steps[_currentStepIndex];
        }
        //根据参数配置模型
        private void SetStep()
        {
            var mList = GetStep();
            Debug.Log(mList);

            //设置当前步骤下的模型脚本
            List<XElement> list = mList.Elements().ToList();
            //StepModel.ModelOperateCount = list.Count;
            //记录涉及到的模型数量
            List<string> modelNameList = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                XElement m = list[i];

                switch (m.Name.ToString())
                {
                    //配置模型节点
                    case "Model":
                        new StepModel().Exec(m);
                        modelNameList.Add(m.Attribute("Name").Value);
                        break;
                    //配置摄像头镜头
                    case "CameraTransform":
                        if (m != null)
                        {
                            Vector3 position = VectorUtil.XmlToVector3(m.Element("Position"));
                            Vector3 rotation = VectorUtil.XmlToVector3(m.Element("Rotation"));
                            //RigidBodyFPSControl.Instance.SetTransform(position, rotation);
                            Message message = MessagePool.GetMessage(CommonMessageType.Common_CameraMove);
                            message["Position"] = position;
                            message["Rotation"] = rotation;
                            MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher(message);
                        }
                        break;
                    //配置摄像头镜头
                    case "VRTransform":
                        if (m != null)
                        {
                            // Vector3 position = VectorUtil.XmlToVector3(m.Element("Position"));
                            //  Vector3 rotation = VectorUtil.XmlToVector3(m.Element("Rotation"));
                            // RigidBodyFPSControl.Instance.SetTransform(position, rotation);
                            // VRMoveControl.Instance.Teleport(transform, position, rotation);
                        }
                        break;
                    //配置模型节点
                    case "Questions":
                        new StepQuestion().Exec(m);
                        modelNameList.Add(m.Attribute("Name").Value);
                        break;

                }
            }

            //设置当前步骤下的事件脚本
            Debug.Log("EventObject ————名称：" + mList.Attribute("EventObject").Value);
            _eventGameObject = ObjectManager.Instance.GetGameObject(mList.Attribute("EventObject").Value);
            ComponentUtil.AddComponent(mList.Attribute("EventScript").Value, _eventGameObject);
            _stepEvent = _eventGameObject.GetComponent<StepEvent_Base>();
            _stepEvent.SetStepControlGameObject(modelNameList);
            if (mList.Attribute("TriggerValue") == null)
            {
                ComponentUtil.AddComponent(mList.Attribute("TriggerScript").Value, _eventGameObject);
            }
            else
            {
                ComponentUtil.AddComponent(mList.Attribute("TriggerScript").Value, mList.Attribute("TriggerValue").Value, _eventGameObject);
            }

            //帮助文字
            string helpStr = mList.Attribute("Help").Value;
            string promptType = "";
            //帮助提示
            if (mList.Attribute("PromptType") != null)
            {
                promptType = mList.Attribute("PromptType").Value;
            }
            EventObjectPrompt prompt = _eventGameObject.AddComponent<EventObjectPrompt>();
            prompt.SetPromptType(promptType, helpStr);
            if (isPlay)
            {
                StartCoroutine(AutoPlay());
                // ContentMessageManager.GetInstance()._messageManager.Dispatcher(CommonMessageType.Common_NextBtn);
            }
        }
        IEnumerator AutoPlay()
        {

            yield return new WaitForSeconds(4);
            MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher(CommonMessageType.Common_NextBtn);
        }
    }

}
