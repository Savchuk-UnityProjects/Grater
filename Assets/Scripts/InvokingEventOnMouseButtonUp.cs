using UnityEngine;

public class InvokingEventOnMouseButtonUp : InvokingEventWithCondition
{
    private new void Update()
    {
        Condition = Input.GetMouseButtonUp(0);
        base.Update();
    }
}