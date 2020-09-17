using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeLineManager: MonoBehaviour {
    //Timeline基础控制对象
    private  PlayableDirector playableDirector;
    //Timeline资源
    private TimelineAsset timelineAsset;
    // Use this for initialization
    void Awake ()
    {
        playableDirector = GetComponent<PlayableDirector>();
       
    }
    /// <summary>
    /// 给当前的PlayableDirector设置timeline资源
    /// </summary>
    /// <param name="_timelineAsset">资源</param>
    public void SetTimelineAsset(TimelineAsset _timelineAsset) {
        playableDirector.playableAsset = _timelineAsset;
        this.timelineAsset = _timelineAsset;
    }
 
    /// <summary>
    /// 绑定轨道对象
    /// </summary>
    /// <param name="trackName">轨道名称</param>
    /// <param name="go">绑定的对象</param>
    public TrackAsset SetBinding(string trackName,GameObject go) {
        if (timelineAsset == null) {
            Debug.LogError("请先设置对应的TimelineAsset");
        }
        TrackAsset track = timelineAsset.GetOutputTracks().First(c => c.name == trackName);
        playableDirector.SetGenericBinding(track, go);
        return track;
    }
    /// <summary>
    /// 绑定动画轨道对象
    /// </summary>
    /// <param name="trackName">轨道名称</param>
    /// <param name="go">绑定的对象</param>
    /// <param name="posi">动画播放的位置</param>
    /// <param name="isWorld">是否为世界坐标</param>
    public void SetBindingAnimation(string trackName, GameObject go, Vector3 posi,Vector3 euler, bool isWorld)
    {
        AnimationTrack animationTrack = SetBinding(trackName,go) as AnimationTrack;
     //   animationTrack.applyOffsets = true;
        if (isWorld)
        {
            animationTrack.position = posi;
           
        }
        else {
            
            animationTrack.position = transform.position + posi;
        }
        animationTrack.rotation = Quaternion.Euler(euler);
    }
    public void PlayTimeLine() {
        //播放TimeLine
        playableDirector.Play();
    }
    /// <summary>
    /// 测试代码
    /// </summary>
    void Test() {
        //// 获取timeLine 资源文件，并且转换
        //var timelineAsset = playableDirector.playableAsset as TimelineAsset;
        //if (timelineAsset == null)
        //    return;

        ////获取轨道资源，并转换成动画轨道
        //AnimationTrack animationTrack =  timelineAsset.GetOutputTrack(0) as AnimationTrack;
        //playableDirector.SetGenericBinding(animationTrack, g1);
        ////绑定轨道对象
        //foreach (var item in timelineAsset.outputs)
        //{
        //    playableDirector.SetGenericBinding(item.sourceObject, g1);
        //}
        ////播放TimeLine
        //playableDirector.Play();

    }
}
