using LWFramework.Core;
using LWFramework.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObjectPrompt : MonoBehaviour {
    private string promptType;
    private string helpStr;
    private bool isShow = false;
    private float waitTime = 0;
    public void SetPromptType(string promptType,string helpStr) {
        this.promptType = promptType;
        this.helpStr = helpStr;
       
    }

    private void Update()
    {
        if (!isShow) {
            waitTime += Time.deltaTime;
           
            switch (StaticValue.CurrMode)
            {
                case Mode.watchMode:
                    if (waitTime > 0.1)
                    {
                        ShowPrompt();
                        isShow = true;
                    }
                    break;
                case Mode.virtualMode:
                    if (waitTime > 0.1) {
                        ShowPrompt();
                        isShow = true;
                    }
                    break;
                case Mode.independentMode:
                    if (waitTime > 10)
                    {
                        ShowPrompt();
                        isShow = true;
                    }
                    break;
                case Mode.testMode:
                    if (waitTime > 7)
                    {
                       // ShowPrompt();
                        isShow = true;
                        //ResultManager.Instance.AddErrorCount();
                    }
                    break;
                
                default:
                    break;
            }
        }
    }
    void ShowPrompt() {

        switch (promptType)
        {
            case "HighLight":
                MainManager.Instance.GetManager<IHighlightingManager>().AddHighlighting(gameObject, Color.red);
                //gameObject.AddComponent<HighlightableObject>().FlashingOn(Color.green,Color.red,1);
                break;
            case "MeshRenderer":
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                break;
            case "UIPrompt":
                
                gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
                break;
            default:
                break;
        }
        //VRUIManager.Instance.ShowTips(helpStr);
        //LogicUIManager.GetInstance().SetHelpString(helpStr);
        Message msg = MessagePool.GetMessage(CommonMessageType.GL_SetHelpText);
        msg["HelpText"] = helpStr;
        MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher(msg);
        //帮助语音
       // VoiceUtil.GetInstance().Play(helpStr);
    }
    private void OnDisable()
    {
        switch (promptType)
        {
            case "HighLight":
                MainManager.Instance.GetManager<IHighlightingManager>().RemoveHighlighting(gameObject);
                // Destroy(gameObject.GetComponent<HighlightableObject>());
                break;
            case "MeshRenderer":
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                break;
            case "UIPrompt":
                gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
                break;
            default:
                break;
        }
    }


}
