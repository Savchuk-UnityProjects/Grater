using UnityEngine;
using TMPro;

public class GratingAllObjects : MonoBehaviour
{
    [SerializeField] private GratableObject[] AllGratableObjects;
    [SerializeField] private Behaviour[] BehavioursToBeDisabledWhenGrating;
    [SerializeField] private Behaviour[] BehavioursToBeEnabledOnlyWhenGrating;
    [SerializeField] private InvokingEventOnMouseButtonDown StartingGratingOnMouseButtonDown;
    [SerializeField] private InvokingEventOnMouseButtonUp StoppingGratingOnMouseButtonUp;
    [SerializeField] private TextMeshProUGUI TextWithResult;

    private int IndexOfObjectWhichIsGratedNow = 0;

    private int GetIndexOfNextGratableObject()
    {
        return (IndexOfObjectWhichIsGratedNow + 1 < AllGratableObjects.Length) ? IndexOfObjectWhichIsGratedNow + 1 : 0;
    }

    private void SetEnabledAllBehavioursInArray(Behaviour[] ArrayOfBehaviours, bool EnableOrDisable)
    {
        for(int i = 0; i < ArrayOfBehaviours.Length; i++)
        {
            ArrayOfBehaviours[i].enabled = EnableOrDisable;
        }
    }

    private void Awake()
    {
        StoppingGratingOnMouseButtonUp.Event += () => AllGratableObjects[IndexOfObjectWhichIsGratedNow].StopGrating();
        
        StartingGratingOnMouseButtonDown.Event += () => SetEnabledAllBehavioursInArray(BehavioursToBeDisabledWhenGrating, false);
        StartingGratingOnMouseButtonDown.Event += () => SetEnabledAllBehavioursInArray(BehavioursToBeEnabledOnlyWhenGrating, true);
        StartingGratingOnMouseButtonDown.Event += () => AllGratableObjects[IndexOfObjectWhichIsGratedNow].StartGrating();
        StartingGratingOnMouseButtonDown.Event += () => StartingGratingOnMouseButtonDown.enabled = false;

        GrateOneObject();
    }

    private void GrateOneObject()
    {
        AllGratableObjects[IndexOfObjectWhichIsGratedNow].PrepareGrating();
        StartingGratingOnMouseButtonDown.enabled = true;
        AllGratableObjects[IndexOfObjectWhichIsGratedNow].CancelAllActionsOnFinishingGrating();
        AllGratableObjects[IndexOfObjectWhichIsGratedNow].OnFinishingGrating += () => SetEnabledAllBehavioursInArray(BehavioursToBeDisabledWhenGrating, true);
        AllGratableObjects[IndexOfObjectWhichIsGratedNow].OnFinishingGrating += () => SetEnabledAllBehavioursInArray(BehavioursToBeEnabledOnlyWhenGrating, false);
        AllGratableObjects[IndexOfObjectWhichIsGratedNow].OnFinishingGrating += () => TextWithResult.text = AllGratableObjects[IndexOfObjectWhichIsGratedNow].Result;
        AllGratableObjects[IndexOfObjectWhichIsGratedNow].OnFinishingGrating += () => IndexOfObjectWhichIsGratedNow = GetIndexOfNextGratableObject();
        AllGratableObjects[IndexOfObjectWhichIsGratedNow].OnFinishingGrating += GrateOneObject;
    }

    public void StopGratingCurrentObject()
    {
        AllGratableObjects[IndexOfObjectWhichIsGratedNow].StopGrating();
    }
}