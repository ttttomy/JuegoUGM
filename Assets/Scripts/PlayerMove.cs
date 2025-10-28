using UnityEngine.Serialization;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("playerSpeed")]
    private float moveSpeed = 10f;
    [SerializeField, FormerlySerializedAs("momentumDamping")]
    private float inputDamping = 5f;

    [SerializeField, FormerlySerializedAs("myCaracterController")]
    private CharacterController controller;
    [SerializeField, FormerlySerializedAs("camAnim")]
    private Animator animator;
    private bool isWalking;

    private Vector3 inputVector;
    private Vector3 movementVector;
    [SerializeField, FormerlySerializedAs("myGravity")]
    private float gravity = -9.81f; // Consistencia con physics

    void Start()
    {
        myCaracterController = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        GetInput();
        MovePlayer();

        camAnim.SetBool("isWalking", isWalking);
    }

    void GetInput()
    {
        // Si pulsamos wasd, obtenemos el vector de movimiento
        if(Input.GetKey(KeyCode.W) || 
           Input.GetKey(KeyCode.A) ||
           Input.GetKey(KeyCode.S) || 
           Input.GetKey(KeyCode.D))
        {
            inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            inputVector.Normalize();
            inputVector = transform.TransformDirection(inputVector);

            isWalking = true;
        }
        else
        {
            inputVector = Vector3.Lerp(inputVector, Vector3.zero, momentumDamping * Time.deltaTime);

            isWalking = false;
        }

        movementVector = (inputVector * playerSpeed) + (Vector3.up * myGravity);
    }
    void MovePlayer()
    {
        myCaracterController.Move(movementVector * Time.deltaTime);
    }
}
