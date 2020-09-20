/// <summary>
/// 节点，控制Step的流程
/// </summary>
public interface IStepNode
{
   // string Remark { get; set; }
   /// <summary>
   /// 下一步
   /// </summary>
    void MoveNext();
    /// <summary>
    /// 进入节点
    /// </summary>
    void OnEnter();
    /// <summary>
    /// 退出节点
    /// </summary>
    void OnExit();
    /// <summary>
    /// 设置自身未当前运行的节点
    /// </summary>
    void SetCurrent();
}