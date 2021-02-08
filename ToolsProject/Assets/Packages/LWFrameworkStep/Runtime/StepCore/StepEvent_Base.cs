using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LWFrameworkStep
{
    public class StepEvent_Base : MonoBehaviour
    {

        //所有涉及步骤控制的对象
        private List<GameObject> stepControlGameObjects = new List<GameObject>();
        // Use this for initialization
        void Start()
        {
            Trigger_Base.nextEvent += TriggerEvent;
            Trigger_Base.backEvent += BackEvent;
        }
        private void OnDisable()
        {
            Trigger_Base.nextEvent -= TriggerEvent;
            Trigger_Base.backEvent -= BackEvent;
        }
        private void TriggerEvent(GameObject value)
        {
            for (int i = 0; i < stepControlGameObjects.Count; i++)
            {

                StepControl_Base[] allStepControl = stepControlGameObjects[i].GetComponents<StepControl_Base>();
                for (int j = 0; j < allStepControl.Length; j++)
                {
                    allStepControl[j].StepEnd();
                }
            }
            Trigger_Base.nextEvent -= TriggerEvent;
            Destroy(this);
        }
        private void BackEvent(GameObject value)
        {
            for (int i = 0; i < stepControlGameObjects.Count; i++)
            {

                StepControl_Base[] allStepControl = stepControlGameObjects[i].GetComponents<StepControl_Base>();
                for (int j = 0; j < allStepControl.Length; j++)
                {
                    allStepControl[j].StepBack();
                }
            }
            Trigger_Base.backEvent -= TriggerEvent;
            Destroy(this);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                for (int i = 0; i < stepControlGameObjects.Count; i++)
                {
                    if (stepControlGameObjects[i] != null)
                    {
                        Debug.Log(stepControlGameObjects[i].name);
                    }
                    else
                    {
                        Debug.LogError("没有触发对象");
                    }
                }

            }
        }
        public void SetStepControlGameObject(List<string> gameObjectNameList)
        {
            List<string> nameList = new List<string>();
            for (int i = 0; i < gameObjectNameList.Count; i++)
            {

                if (!nameList.Contains(gameObjectNameList[i]))
                {
                    GameObject go = ObjectManager.Instance.GetGameObject(gameObjectNameList[i]);
                    StepControl_Base stepControl = go.GetComponent<StepControl_Base>();

                    if (stepControl)
                    {
                        nameList.Add(gameObjectNameList[i]);
                        stepControl.StepStart();
                        stepControlGameObjects.Add(go);
                    }
                }


            }

        }
    }
}