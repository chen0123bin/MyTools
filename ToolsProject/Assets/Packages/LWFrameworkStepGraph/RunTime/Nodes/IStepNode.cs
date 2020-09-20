public interface IStepNode
{
   // string Remark { get; set; }
    void MoveNext();
    void OnEnter();
    void OnExit();
    void SetCurrent();
}