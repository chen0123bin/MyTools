using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStepManager
{ 
    Action StepAllCompleted { get; set; }
    IStep GetNextStepByIndex(int index);
    void StartStep();
    void MoveNext();
    void MovePrev();
}
