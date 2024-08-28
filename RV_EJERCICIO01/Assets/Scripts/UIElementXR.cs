using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementXR : MonoBehaviour
{
    
    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        
    }
    
    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnterXR()
    {
       
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExitXR()
    {
        
    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClickXR()
    {
        Debug.Log("Estoy ejecutando el metodo que llega desde el mensaje");
    }
   
}
