using UnityEngine;

// https://www.youtube.com/watch?v=QIVN-T-1QBE&list=PLhLrd8mRFD-7KfBuh-JDpjwW3OapeQTL6&index=79&ab_channel=Plai 

public class Sway : MonoBehaviour
{

    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY + 2.301f, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);

    }

}
