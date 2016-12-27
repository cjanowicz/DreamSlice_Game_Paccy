using UnityEngine;
using System.Collections;

public class Pacdot : MonoBehaviour {

    public LayerMask m_EnemiesLayer;
    
	void OnTriggerEnter(Collider co) {
        /// NOTE: Need to set layermask as defined
        if(this.gameObject.layer != m_EnemiesLayer) { 
            if(co.tag == "Player") {
                //Increase High Score.
                //TODO: Invisible not destroy
                this.gameObject.SetActive(false);
                GameManager.instance.SendMessage("EatDot");
                //Destroy(gameObject);
            }
        }
    }

    //TODO: Reactivate Function;
}
