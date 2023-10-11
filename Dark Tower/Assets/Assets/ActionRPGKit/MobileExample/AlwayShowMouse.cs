using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwayShowMouse : MonoBehaviour{
	
    // Update is called once per frame
    void Update(){
		if(Cursor.lockState == CursorLockMode.Locked){
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
    }
}
