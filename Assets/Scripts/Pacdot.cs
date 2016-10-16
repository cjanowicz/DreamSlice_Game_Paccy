using UnityEngine;
using System.Collections;

public class Pacdot : MonoBehaviour {
    
	void OnTriggerEnter2D(Collider2D co) {
        if(co.name == "pacman") {
            //Increase High Score.
            //TODO: Invisible not destroy
            GameManager.instance.SendMessage("EatDot");
            Debug.Log("Child Count =" + this.transform.parent.childCount);
            this.gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    //TODO: Reactivate Function;
}
