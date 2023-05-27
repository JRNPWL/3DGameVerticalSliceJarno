using UnityEngine;
using TMPro;

public class PickUpSystem : MonoBehaviour
{
    public float pickupDistance = 2f;
    public LayerMask pickupLayerMask;
    public Transform RifleTransform;
    public Transform PistolTransform;
    public Transform ShotgunTransform;
    public TextMeshProUGUI ReloadUi;
    public TextMeshProUGUI WeaponDamage;

    private GameObject heldObject;

    void Update()
    {
        // Cast Ray to detect whether object is Pickable
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * pickupDistance, Color.green);
        if (Physics.Raycast(ray, out hit, pickupDistance, pickupLayerMask))
        {
            Debug.Log($"Hit {hit.collider.gameObject}");
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUp(hit.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && heldObject != null)
        {
            Drop();
        }
    }

    void PickUp(GameObject obj)
    {
        if (heldObject != null)
        {
            Drop();
        }

        if (obj.CompareTag("Rifle"))
        {
            // Set the held object to the object that was picked up
            heldObject = obj;

            // Disable the object's rigidbody so it can be moved by the player
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            MeshCollider mc = heldObject.GetComponent<MeshCollider>();
            MonoBehaviour sway = heldObject.GetComponent<Sway>();

            if (rb != null)
            {
                rb.isKinematic = true;
                mc.enabled = false;
            }

            // Set the object's parent to the player's transform so it moves with the player
            ReloadUi.gameObject.SetActive(true);
            WeaponDamage.gameObject.SetActive(true);
            sway.enabled = true;
            heldObject.transform.SetParent(RifleTransform, false);
            heldObject.transform.position = RifleTransform.transform.position;
            heldObject.transform.rotation = RifleTransform.transform.rotation;
        }

        // Pick Up Pistol
        if (obj.CompareTag("Pistol"))
        {
            // Set the held object to the object that was picked up
            heldObject = obj;

            // Disable the object's rigidbody so it can be moved by the player
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            MeshCollider mc = heldObject.GetComponent<MeshCollider>();
            MonoBehaviour sway = heldObject.GetComponent<Sway>();

            if (rb != null)
            {
                rb.isKinematic = true;
                mc.enabled = false;
            }

            // Set the object's parent to the player's transform so it moves with the player
            ReloadUi.gameObject.SetActive(true);
            WeaponDamage.gameObject.SetActive(true);
            sway.enabled = true;
            heldObject.transform.SetParent(PistolTransform, false);
            heldObject.transform.position = PistolTransform.transform.position;
            heldObject.transform.rotation = PistolTransform.transform.rotation;
        }

        if (obj.CompareTag("Shotgun"))
        {
            // Set the held object to the object that was picked up
            heldObject = obj;

            // Disable the object's rigidbody so it can be moved by the player
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            BoxCollider bc = heldObject.GetComponent<BoxCollider>();
            MonoBehaviour sway = heldObject.GetComponent<Sway>();

            if (rb != null)
            {
                rb.isKinematic = true;
                bc.enabled = false;
            }

            // Set the object's parent to the player's transform so it moves with the player
            ReloadUi.gameObject.SetActive(true);
            WeaponDamage.gameObject.SetActive(true);
            sway.enabled = true;
            heldObject.transform.SetParent(ShotgunTransform, false);
            heldObject.transform.position = ShotgunTransform.transform.position;
            heldObject.transform.rotation = ShotgunTransform.transform.rotation;
        }
    }


    void Drop()
    {
        // Re-enable the object's rigidbody
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        MeshCollider mc = heldObject.GetComponent<MeshCollider>();
        BoxCollider bc = heldObject.GetComponent<BoxCollider>();
        MonoBehaviour sway = heldObject.GetComponent<Sway>();

        // Turns of Rigidbody Kinematic and turns on Colliders
        if (rb != null && mc != null)
        {
            rb.isKinematic = false;
            mc.enabled = true;

        }

        if (rb != null && bc != null)
        {
            rb.isKinematic = false;
            bc.enabled = true;

        }

        ReloadUi.gameObject.SetActive(false);
        WeaponDamage.gameObject.SetActive(false);
        sway.enabled = false;

        // Remove the object's parent so it stays in place
        heldObject.transform.SetParent(null);

        // Reset the held object variable
        heldObject = null;
    }
}
