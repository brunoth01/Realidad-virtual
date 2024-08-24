using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointerManager : MonoBehaviour
{
    public static CameraPointerManager Instance;
    [SerializeField] private GameObject pointer;
    [SerializeField] private float masDistancePointer = 4.5f;
    [Range(0, 1)]
    [SerializeField] private float disPointerObject = 0.95f;

    private const float _maxDistance = 10;
    private GameObject _gazedAtObject = null;

    private readonly string interactableTag = "Interactable";
    private float scaleSize = 0.025f;

    [HideInInspector] //esto sirve para ocultar la variable del Inspector
    public Vector3 hitPoint;

    private void Awake()
    {
        if (Instance != null && Instance != this) //Con esta funcion nos aseguramos que solo exista una instancia de CameraPOinterManager.
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        GazeManager.Instance.OnGazeSelection += GazeSelection; //Este evento es llamado cada vez que se cumple el tiempo de seleccion
    }

    private void GazeSelection()
    {
        _gazedAtObject?.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);
        //PARA LOS MOVIMIENTOS
        if ((bool)(_gazedAtObject?.transform.CompareTag("caja_move")))
        {
            _gazedAtObject?.SendMessage("OnPointerClickMove", null, SendMessageOptions.DontRequireReceiver);
        }

    }


    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    public void Update()
    {
        // Casts ray towards camera's forward direction, to detect if a GameObject is being gazed
        // at.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            hitPoint = hit.point; //Asignar el Point a la variable publica hitPoint

            // GameObject detected in front of the camera.
            if (_gazedAtObject != hit.transform.gameObject)
            {
                // New GameObject.
                _gazedAtObject?.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnPointerEnterXR", null, SendMessageOptions.DontRequireReceiver);
                GazeManager.Instance.StartGazeSelection(); //Se habilita la Barra de carga
            }
            if (hit.transform.CompareTag(interactableTag) || hit.transform.CompareTag("caja_move")) //comparamos si el objeto tiene el tag que hemos definido
            {
                PointerOnGaze(hit.point); //Funcion para mostrar el pointer
            }
            else
            {
                PointerOutGaze(); //Funcion para retirar el puntero
            }

        }
        else
        {
            // No GameObject detected in front of the camera.
            _gazedAtObject?.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);
            _gazedAtObject = null;
        }

        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            if (_gazedAtObject != null)
                _gazedAtObject?.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);

        }
    }
    private void PointerOutGaze()
    {
        pointer.transform.localScale = Vector3.one * 0.1f;
        pointer.transform.parent.transform.localPosition = new Vector3(0, 0, masDistancePointer);
        pointer.transform.parent.parent.transform.rotation = transform.rotation;
        GazeManager.Instance.CancelGazeSelection(); //Como no estamos interactuando con un objeto de cancela EL gAZEmANAGER

    }

    private void PointerOnGaze(Vector3 hitPoint)
    {
        float scaleFactor = scaleSize * Vector3.Distance(transform.position, hitPoint); //Aqui calculamo la distancia del objeto al puntero
        pointer.transform.localScale = Vector3.one * scaleFactor; //Asegurar que el pointer tenga el mismo tama�o
        //Aqui colocamos pointer debido a la estructura del pointer por lo tanto se debe asignar al padre del pointer por eso se usa parent.
        pointer.transform.parent.position = CalculatePointerPosition(transform.position, hitPoint, disPointerObject); //Calculamos la posicion donde debe mostrarse el puntero entre la camara y el objeto
    }

    private Vector3 CalculatePointerPosition(Vector3 p0, Vector3 p1, float t) //Para recalcular la posicion del puntero, se usa matematica.
    {
        float x = p0.x + t * (p1.x - p0.x);
        float y = p0.y + t * (p1.y - p0.y);
        float z = p0.z + t * (p1.z - p0.z);

        return new Vector3(x, y, z);
    }

}
