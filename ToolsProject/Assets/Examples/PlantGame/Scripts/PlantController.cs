using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using LWFramework.Message;

public class PlantController : MonoBehaviour {
    private SkeletonAnimation skeletonAnimation;
    public int plantId;
    //对外使用的时间大小
    private  float timeScale = 1;
    private bool isPlay = false;
    //动画时长
    private float animationDuration;
    //对当前动画使用的时间大小
    public float defaultTimeScale = 0.5f;
    //当前进度
    public float currProgress;
    //当前健康值
    public float currHealth = 1;
    public float healthScore = 20;
    //虫害 杂草
    private float currDebuff = 0;
    //当前水分值
    public float currWater = 0;
    private float bestWater;
    //是否在浇水
    private bool isWater = false;
    public float waterScore = 20;
    //当前肥料值
    public float currFL = 0;
    private float bestFL;
    public float flScore = 20;
    //是否在施肥
    private bool isFl;
    //当前天气
    private Weather weather = Weather.Sun;
    //最终得分
    public float resultScore;

    //虫子
    public GameObject[] maochongArray;
    //杂草
    public GameObject[] zacaoArray;

    private float showTipsTimeMax = 15;
    private float showTipsTime;
    // Use this for initialization
    void Start () {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.Event += AnimationState_Event;
        skeletonAnimation.AnimationState.Complete += AnimationState_Complete;
        animationDuration = skeletonAnimation.skeleton.data.Animations.ToArray()[0].Duration;

        skeletonAnimation.AnimationState.SetAnimation(0, "animation", false);

        showTipsTime = showTipsTimeMax;
        //Debug.Log(skeletonAnimation.skeleton.data.Animations.ToArray()[0].Duration);
        //Debug.Log(skeletonAnimation.skeleton.data.animations[0].duration.ToString());//动画持续时间？
    }
    /// <summary>
    /// 初始化数据  开始播放动画
    /// </summary>
    /// <param name="bestWater"></param>
    /// <param name="bestFL"></param>
    public void StartPlay(float bestWater,float bestFL) {
        this.bestWater = bestWater;
        this.bestFL = bestFL;
        currWater = bestWater;
        currFL = bestFL;
        isPlay = true;
    }
    public void SetTimeScale(float timeScale) {
        this.timeScale = timeScale;
    }
    public float GetProgress() {
        return this.currProgress/ animationDuration;
    }
    public float GetCurrHealth() {
        return this.currHealth;
    }
    public float GetCurrWater()
    {
        return this.currWater;
    }
    public float GetCurrFL()
    {
        return this.currFL;
    }
    public void SetWeather(Weather weather) {
        this.weather = weather;
    }
    /// <summary>
    /// 设置浇水状态
    /// </summary>
    /// <param name="value">是否在浇水</param>
    public void SetIsWater(bool value) {
        isWater = value;
    }
    /// <summary>
    /// 设置施肥状态
    /// </summary>
    /// <param name="value">是否在施肥</param>
    public void SetIsFl(bool value) {
        isFl = value;
    }
    /// <summary>
    /// 添加虫子或杂草
    /// </summary>
    /// <param name="type">1-虫子  2-杂草</param>
    public void AddDebuff(int type) {
        if (type == 1 && GetProgress()> 0.5f) {
            for (int i = 0; i < maochongArray.Length; i++)
            {
                if (!maochongArray[i].activeInHierarchy) {
                    maochongArray[i].SetActive(true);
                    currDebuff++;
                }
            }
        }
        if (type == 2)
        {
            for (int i = 0; i < zacaoArray.Length; i++)
            {
                if (!zacaoArray[i].activeInHierarchy)
                {
                    zacaoArray[i].SetActive(true);
                    currDebuff++;
                }
            }
        }
    }
    public void DeleteDebuff() {
        currDebuff--;
    }
    private void AnimationState_Complete(Spine.TrackEntry trackEntry)
    {
       // PlayEnd();

    }
    void PlayEnd() {
        resultScore = 40 + healthScore + waterScore + flScore;
        Debug.Log("动画结束 种植得分：" + resultScore);
        Message message = MessagePool.GetMessage(ZWMessageType.PlantAnimationEnd);
        message["plantId"] = plantId;
        message["resultScore"] = resultScore;
       // ContentMessageManager.GetInstance().MessageManager.Dispatcher(message);
        isPlay = false;
    } 

    private void AnimationState_Event(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.ToString() == "1")
        {
            Debug.Log("开始发芽");
        }else if (e.ToString() == "2")
        {
            Debug.Log("生长");
        }
        else if(e.ToString() == "3")
        {
            Debug.Log("结果");
        }
        else if (e.ToString() == "4")
        {
            Debug.Log("结束");
        }

    }

    // Update is called once per frame
    void Update () {
        
        if (isPlay) {
            skeletonAnimation.AnimationState.TimeScale = timeScale * defaultTimeScale;
            currProgress += (timeScale * defaultTimeScale * Time.deltaTime);
            bool checkShowTipsTime = false;

            if (GetProgress() >= 1) {
                PlayEnd();
            }

            //设置健康度分数
            if (currHealth < 0.8f && healthScore>0)
            {
                healthScore -= 0.5f * Time.deltaTime;
                checkShowTipsTime = true;
            }
            //设置水分分数
            if (Mathf.Abs(currWater - bestWater) > 0.25f && waterScore > 0)
            {
                waterScore -= 0.5f * Time.deltaTime;
                checkShowTipsTime = true;
            }
            //设置肥料分数
            if (Mathf.Abs(currFL - bestFL) > 0.25f && flScore > 0)
            {
                flScore -= 0.5f * Time.deltaTime;
                checkShowTipsTime = true;
            }

            //设置健康度流失 及回复
            if (currDebuff > 0)
            {
                currHealth -= 0.005f * currDebuff * Time.deltaTime;
            }
            else {
                currHealth += 0.005f * currDebuff * Time.deltaTime;
            }
            //设置天气水分的影响
            if (weather == Weather.Sun && currWater>0)
            {
                currWater -= 0.005f * Time.deltaTime;
            }
            else if (weather == Weather.Rain && currWater <= 1) {
                currWater += 0.006f * Time.deltaTime;
            }
            //浇水状态
            if (isWater && currWater <= 1) {
                currWater += 0.04f * Time.deltaTime;
            }
            //设置肥料流失
            currFL -= 0.005f * Time.deltaTime;
            //施肥状态
            if (isFl && currFL <= 1) {
                currFL += 0.04f * Time.deltaTime;
            }
            //检测提示信息
            if (checkShowTipsTime) {
                showTipsTime -= Time.deltaTime;
                if (showTipsTime < 0) {
                    showTipsTime = showTipsTimeMax;
                    ZWMessageType.ShowTipsByMessage("植物当前状态不对哦，请注意！！", 3);
                }
            }
        }
        


        //Debug.Log(currProgress / animationDuration);

    }
}
