using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";

    [SerializeField] Transform door;
    [SerializeField] Vector3 openDistance = new Vector3(0, 3, 0);

    [SerializeField] bool opening = false;

    [SerializeField] float doorSpeed = 3f;

    public UnityEvent OnEntered;
    public UnityEvent OnExited;

    Vector3 closedPosition;
    Vector3 openPosition;

    void Awake()
    {
        if (!door) door = transform;
        closedPosition = door.position;
        openPosition = closedPosition + openDistance;

        Collider col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    void Update()
    {
        OpenDoor();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        opening = true;
        OnEntered?.Invoke();
    }
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        opening = false;
        OnExited?.Invoke();
    }

    void OpenDoor()
    {
        if (opening)
        {
            door.position = Vector3.MoveTowards(door.position, openPosition, doorSpeed * Time.deltaTime);
        }
        else
        {
            door.position = Vector3.MoveTowards(door.position, closedPosition, doorSpeed * Time.deltaTime);
        }    
    }
}