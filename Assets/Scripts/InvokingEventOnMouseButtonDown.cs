using UnityEngine;

public class InvokingEventOnMouseButtonDown : InvokingEventWithCondition
{
    private new void Update()
    {
        Condition = Input.GetMouseButtonDown(0);
        base.Update();
    }
}