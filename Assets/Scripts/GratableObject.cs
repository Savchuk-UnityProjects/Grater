using UnityEngine;
using System;
using System.Collections.Generic;

public class GratableObject : MonoBehaviour
{
    [Header("Motion")]
    [SerializeField] private float SpeedOfVerticalMovement;
    [SerializeField] private Vector3 TheLowestPoint;
    [SerializeField] private Vector3 TheHighestPoint;
    [SerializeField] private Vector3 DirectionOfDecreasingObject;
    [SerializeField] private float MaximalDistanceInDirectionOfDecreasing;

    [Header("Shaking the camera")]
    [SerializeField] private ShakableObject ShakingCamera;
    [SerializeField] private float DurationOfShaking;
    [SerializeField] private float MagnitudeOfShaking;
    [SerializeField] private float NoizeOfShaking;

    [Header("Pieces")]
    [SerializeField] private GameObject PrefabOfPieceOfThisObject;
    [SerializeField] private Vector3 OffsetOfNewPieces;
    [SerializeField] private int FrequencyOfAppearingPieces;
    [SerializeField] private ChangingMaterialOfAllRenderesInChildren ChangingMaterialOfAllRenderesInChildren;
    [SerializeField] private Material MaterialOfPiecesOfThisObject;

    [Header("Assessment")]
    [SerializeField] private List<string> Assessments;
    [SerializeField] private List<float> BordersForAssessment;
    [SerializeField] private int PrecisionOfSavingPercentOfGratedPart;
    [SerializeField] private string MessageWhenFingerWasNotReleasedWhenGrating;

    [Header("Particles")]
    [SerializeField] private bool IsParticleSystemUsed;
    [SerializeField] private ParticleSystem ParticleSystem;
    [SerializeField] private Vector3 OffsetOfParticleSystem;

    private Vector3 DisplacementWithOneIteration;
    private int QuantityOfIterations;
    private int OrderOfCurrentIteration = 0;
    private int OrderOfCurrentIterationFromStarting = 0;
    private bool IsGratedNow = false;
    private Vector3 OvercomeDistanceInDirectionOfDecreasing = Vector3.zero;
    private bool IsRisedNow = true;

    public event Action OnFinishingGrating;
    public string Result { get; private set; }

    public void CancelAllActionsOnFinishingGrating()
    {
        OnFinishingGrating = null;
    }

    private int BoolToSign(bool BoolValue)
    {
        return BoolValue ? 1 : -1;
    }

    private void Start()
    {
        if(Assessments.Count != BordersForAssessment.Count + 1)
        {
            Debug.LogWarning($"{gameObject.name}: There should be one fewer borders than assessments");
        }
        InitializeValuesOnStarting();
    }

    private void InitializeValuesOnStarting()
    {
        QuantityOfIterations = (int)(System.Math.Floor((TheHighestPoint - TheLowestPoint).magnitude / SpeedOfVerticalMovement));
        if(QuantityOfIterations == 0)
        {
            QuantityOfIterations = 1;
        }
        DisplacementWithOneIteration = (TheHighestPoint - TheLowestPoint) / QuantityOfIterations;
    }

    public void PrepareGrating()
    {
        ChangingMaterialOfAllRenderesInChildren.ChangeMaterialOfAllRenderesInChildren(MaterialOfPiecesOfThisObject);
        OrderOfCurrentIteration = 0;
        OrderOfCurrentIterationFromStarting = 0;
        OvercomeDistanceInDirectionOfDecreasing = Vector3.zero;
        transform.position = TheLowestPoint;
        IsRisedNow = true;
        gameObject.SetActive(true);
    }

    public void StartGrating()
    {
        PrepareGrating();
        if (IsParticleSystemUsed)
        {
            ParticleSystem.Play();
        }
        IsGratedNow = true;
    }

    public void CompleteOneIterationOfGrating()
    {
        OrderOfCurrentIterationFromStarting++;
        OrderOfCurrentIteration++;
        transform.position += BoolToSign(IsRisedNow) * DisplacementWithOneIteration;
        transform.position += DirectionOfDecreasingObject;
        OvercomeDistanceInDirectionOfDecreasing += DirectionOfDecreasingObject;
        if (OrderOfCurrentIterationFromStarting % FrequencyOfAppearingPieces == 0)
        {
            GameObject NewPiece = Instantiate(PrefabOfPieceOfThisObject);
            NewPiece.transform.position = transform.position - OvercomeDistanceInDirectionOfDecreasing + OffsetOfNewPieces;
        }
        if (IsParticleSystemUsed)
        {
            ParticleSystem.gameObject.transform.position = transform.position - OvercomeDistanceInDirectionOfDecreasing + OffsetOfParticleSystem;
        }
        if (OvercomeDistanceInDirectionOfDecreasing.magnitude >= MaximalDistanceInDirectionOfDecreasing)
        {
            ShakingCamera.Shake(DurationOfShaking, MagnitudeOfShaking, NoizeOfShaking);
            StopGrating();
        }
        if (OrderOfCurrentIteration == QuantityOfIterations)
        {
            OrderOfCurrentIteration = 0;
            transform.position = IsRisedNow ? TheHighestPoint : TheLowestPoint;
            transform.position += OvercomeDistanceInDirectionOfDecreasing;
            IsRisedNow = !IsRisedNow;
        }
    }

    private void FixedUpdate()
    {
        if (IsGratedNow)
        {
            CompleteOneIterationOfGrating();
        }
    }

    public void StopGrating()
    {
        if (IsParticleSystemUsed)
        {
            ParticleSystem.Stop();
        }
        float PercentOfGratedPartWithoutRounding = OvercomeDistanceInDirectionOfDecreasing.magnitude / MaximalDistanceInDirectionOfDecreasing * 100f;
        float PercentOfGratedPart = (float)Math.Round(PercentOfGratedPartWithoutRounding, PrecisionOfSavingPercentOfGratedPart);
        string Assessment = Assess(PercentOfGratedPart);
        Result = $"Result:\n{PercentOfGratedPart}%";
        if (PercentOfGratedPart <= 100)
        {
            Result = $"{Assessment}\n{Result}";
        }
        else
        {
            Result = MessageWhenFingerWasNotReleasedWhenGrating;
        }
        IsGratedNow = false;
        gameObject.SetActive(false);
        OnFinishingGrating?.Invoke();
    }

    private string Assess(float ValueToBeAssessed)
    {
        if(ValueToBeAssessed <= BordersForAssessment[0])
        {
            return Assessments[0];
        }
        if(ValueToBeAssessed >= BordersForAssessment[^1])
        {
            return Assessments[^1];
        }
        for (int i = 1; i < Assessments.Count - 1; i++)
        {
            if(ValueToBeAssessed <= BordersForAssessment[i] && ValueToBeAssessed >= BordersForAssessment[i-1])
            {
                return Assessments[i];
            }
        }
        return "";
    }

    private void OnDestroy()
    {
        OnFinishingGrating = null;
    }
}