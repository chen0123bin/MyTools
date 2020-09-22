using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStepGraph : MonoBehaviour
{
    public StepGraph m_StepGraph;
    // Start is called before the first frame update
    void Start()
    {
        m_StepGraph.JumpNode(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) {
            m_StepGraph.Continue();
        }
    }
}
