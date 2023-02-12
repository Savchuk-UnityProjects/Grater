using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerWithGravity : MonoBehaviour
{
    [SerializeField] private float Gravity;
    
    private CharacterController ThisCharacterController;

    private void Awake()
    {
        ThisCharacterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ThisCharacterController.Move(Gravity * Vector3.down * Time.deltaTime);
    }
}