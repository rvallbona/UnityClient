using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Bullet : MonoBehaviourPun, IPunObservable
{
    [Header("Stats")]
    [SerializeField]
    private float velocityBullet = 10f;
    Vector3 direction;
    Vector3 bulletPos = Vector3.zero;
    
    private PhotonView pv;
    public void Initialize(bool isFacingRight)
    {
        if (isFacingRight)
            direction = Vector3.right;
        else 
            direction = Vector3.left;
    }
    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 20;
    }
    private void Update()
    {
        if (pv.IsMine) 
        { 
            transform.position += direction * Time.deltaTime * velocityBullet; 
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, bulletPos, Time.deltaTime * 20);
        }
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) 
            stream.SendNext(transform.position); 
        else if (stream.IsReading)
            bulletPos = (Vector3)stream.ReceiveNext();
    }
    private void OnBecameInvisible()
    {
        pv.RPC("NetworkDestroy", RpcTarget.All);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();

        if (character == null) { return; }

        Debug.Log("Has colisionado con: " + collision);

        character.Damage();

        pv.RPC("NetworkDestroy", RpcTarget.All);
    }
    [PunRPC]
    public void NetworkDestroy()
    {
        Destroy(this.gameObject);
    }
}