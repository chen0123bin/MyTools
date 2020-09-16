using System.Collections.Generic;
using UnityEngine;
using LWFramework.UI;
using UnityEngine.UI;
using LWFramework.Core;
using LWFramework.Asset;
using LWFramework;

[UIViewData("Assets/@Resources/Prefabs/TestHotfixView.prefab", FindType.Name, "LWFramework/Canvas/Normal")]
public class TestHotfixView : LWFramework.UI.BaseUIView 
{
    [UIElement("Button1")]
    public Button button1;
    [UIElement("Button2", "Assets/Res/Runtime/Sprites/log2.png")]
    public Button button2;
    [UIElement("Text1")]
    public Text text1;
    [UIElement("Parent")]
    public Transform parent;
    [UIElement("Parent2")]
    public Transform parent2;
    /// <summary>
    /// 节点处理  
    /// </summary>
    [UIElement("Parent/TestChildTemplate")]
    public Transform testChildTemplate;
    private GameObjectPool<TestChildNode> _pool;
    private List<TestChildNode> _nodes;

    public async override void CreateView(GameObject gameObject)
    {
        base.CreateView(gameObject);
        //节点处理
        _pool = new GameObjectPool<TestChildNode>(5,testChildTemplate.gameObject);
        _nodes = new List<TestChildNode>();
        button1.onClick.AddListener(() =>
        {
            CreateNode();
        });

        button2.onClick.AddListener(() =>
        {
            _nodes.ForEach(item => _pool.Unspawn(item));
            _nodes.Clear();
        });


        //可用于拆分逻辑代码
        TestChildItem2 uIViewBase2 = (TestChildItem2)MainManager.Instance.GetManager<UIManager>().CreateView<TestChildItem2>(parent2);
        uIViewBase2._HeadImg.sprite = UIUtility.Instance.GetSprite("Assets/@Resources/Sprites/log3.png");
       // var asset2 = await MainManager.Instance.GetManager<AssetsManager>().LoadAsyncTask<Texture2D>("http://192.168.2.109:8089/Windows/%E9%A6%96%E9%A1%B5.png");
       // Texture2D texture = (Texture2D)asset2.asset;
        //button1.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    void CreateNode() {
        for (int i = 0; i < 3; i++)
        {
            TestChildNode testChildNode = _pool.Spawn();
            testChildNode.NameText = "aaaa" + Random.Range(0, 10);
            testChildNode.HeadImgName = "Assets/@Resources/Sprites/log3.png";
            _nodes.Add(testChildNode);
        }
    }
}
