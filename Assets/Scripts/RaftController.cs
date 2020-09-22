using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RaftController : MonoBehaviour
{
    public GameObject XRRig;
    public GameObject playerBody;
    public GameObject raft;
    private HeadSteeringProvider headSteeringProvider;
    private bool playerIsOnRaft;

    // Start is called before the first frame update
    void Start()
    {
        headSteeringProvider = XRRig.GetComponent<HeadSteeringProvider>();
        Debug.Log(headSteeringProvider.isActiveAndEnabled);
        playerIsOnRaft = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsOnRaft)
        {
            Vector3 newRaftPos = new Vector3(playerBody.gameObject.transform.position.x, raft.gameObject.transform.position.y, playerBody.gameObject.transform.position.z);
            raft.gameObject.transform.position = newRaftPos;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag.Equals("Player"))
        {
            Debug.Log("user ENTER raft");
            headSteeringProvider.gameObject.SetActive(true);
            headSteeringProvider.enabled = true;
            playerIsOnRaft = true;
            Debug.Log("head steering is ACTIVE: " + headSteeringProvider.isActiveAndEnabled);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag.Equals("Player"))
        {
            Debug.Log("user EXIT raft");
            headSteeringProvider.gameObject.SetActive(false);
            headSteeringProvider.enabled = false;
            playerIsOnRaft = false;
            Debug.Log("head steering is ACTIVE: " + headSteeringProvider.isActiveAndEnabled);
        }
    }
}
