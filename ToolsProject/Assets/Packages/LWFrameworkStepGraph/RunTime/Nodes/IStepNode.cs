/// <summary>
/// 节点，控制Step的流程
/// </summary>
public interface IStepNode
{
    StepNodeState CurrState { get; set; }
   // string Remark { get; set; }
   /// <summary>
   /// 下一步
   /// </summary>
    void MoveNext();
    /// <summary>
    /// 上一步
    /// </summary>
    void MovePrev();
    /// <summary>
    /// 进入节点,启动控制器
    /// </summary>
    void StartController();
    /// <summary>
    /// 退出节点，停止控制器
    /// </summary>
    void StopController();
    /// <summary>
    /// 进入节点，启动触发器
    /// </summary>
    void StartTrigger();
    /// <summary>
    /// 退出节点，停止触发器
    /// </summary>
    void StopTrigger();
    /// <summary>
    /// 设置自身未当前运行的节点
    /// </summary>
    void SetCurrent();
}