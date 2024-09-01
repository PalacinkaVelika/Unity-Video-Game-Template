using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinLogic : MonoBehaviour {
    public GameObject particle;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Instantiate(particle, this.transform.position, Quaternion.identity);
            FindAnyObjectByType<CoinSpawner>().CoinCollected();
            Destroy(this.gameObject);
        }
    }
}
