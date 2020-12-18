using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;
using LWFramework.Core;
using LWFramework.FMS;

[UIViewData("Assets/@Resources/PlantGame/Prefabs/UI/StartView.prefab", FindType.Name, "LWFramework/Canvas/Normal")]
public class StartView : BaseUIView 
{

	[UIElement("BtnStart")]
	private Button m_BtnStart;
	public override  void CreateView(GameObject go)
	{
        base.CreateView(go);
		m_BtnStart.onClick.AddListener(() => 		{
            MainManager.Instance.GetManager<IFSMManager>().GetFSMProcedure().GetState<PlantStartProcedure>().EnterGameScene();
		});
	}
}
