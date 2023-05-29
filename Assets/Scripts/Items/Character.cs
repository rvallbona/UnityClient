using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
public class Character : MonoBehaviour, IPunObservable
{
    [SerializeField] private Race race;
    [Header("Stats")]
    private float maxHealth;
    private float damage;
    private float velocity;
    private float jumpForce;
    private float cadence;

    private float jump = 5;

    private static float maxLives = 3;
    private float actualLives;
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask layerMask;
    private bool canDoubleJump;
    [SerializeField] private Transform spawnBullet;
    private bool isRight;

    private Rigidbody2D rb;
    private float desiredMovementAxis = 0f;

    private PhotonView pv;
    private Vector3 enemyPosition = Vector3.zero;

    private PlayerView playerView;
    private Animator animator;
    private bool isRunning = false;
    private bool wasGrounded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();

        animator = GetComponentInChildren<Animator>();
        wasGrounded = true;

        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 20;

        playerView = new PlayerView(gameObject);
    }
    private void Start()
    {
        if (pv.IsMine)
        {
            playerView.SetName(Session_Manager._SESSION_MANAGER.CurrentUser.name);
            race = Session_Manager._SESSION_MANAGER.Races[Session_Manager._SESSION_MANAGER.CurrentUser.id_race_user];

            maxHealth = race.MaxHealth;
            damage = race.Damage;
            velocity = race.Velocity;
            jumpForce = race.JumpForce;
            cadence = race.Cadence;
        }
        else
        {
            pv.RPC("SetNameEnemyPlayer", RpcTarget.All, Session_Manager._SESSION_MANAGER.CurrentUser.name);
            pv.RPC("SetRaceStatsPlayer", RpcTarget.All, Session_Manager._SESSION_MANAGER.CurrentUser.id_race_user);
        }


        isRight = true;

        actualLives = maxLives;
    }
    private void Update()
    {
        cadence -= Time.deltaTime;

        if (pv.IsMine)
        {
            CheckInputs();
        }
        else
        {
            SmoothReplicate();
        }
        if (IsGrounded() != wasGrounded)
        {
            if (IsGrounded())
            {
                // Si está en el suelo, establece "isJumping" a false
                animator.SetBool("isJumping", false);
            }
            else
            {
                // Si está en el aire, establece "isJumping" a true
                animator.SetBool("isJumping", true);
            }
        }

        // Actualiza wasGrounded para el próximo frame
        wasGrounded = IsGrounded();

    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(desiredMovementAxis * Time.fixedDeltaTime * velocity, rb.velocity.y);
    }
    private void CheckInputs()
    {
        desiredMovementAxis = Input.GetAxisRaw("Horizontal");

        desiredMovementAxis *= velocity * Time.deltaTime;

        if (desiredMovementAxis != 0 && IsGrounded())
        {
            isRunning = true;
            animator.SetBool("isRunning", true);
        }
        else
        {
            isRunning = false;
            animator.SetBool("isRunning", false);
        }

        // Mueve el personaje con transform.Translate
        transform.Translate(new Vector3(desiredMovementAxis, 0f, 0f));
        FlipPlayer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                rb.velocity = Vector3.up * (jumpForce + jump);
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                rb.velocity = Vector3.up * (jumpForce + jump);
                canDoubleJump = false;
            }
        }

        //Codido disparo
        if (Input.GetKeyDown(KeyCode.E) && cadence <= 0) { Disparo(); cadence = cadence; }
    }
    private void SmoothReplicate()
    {
        desiredMovementAxis = enemyPosition.x - transform.position.x;
        transform.position = Vector3.Lerp(transform.position, enemyPosition, Time.deltaTime * 20);
        FlipPlayer();
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(transform.position);
        else if (stream.IsReading)
            enemyPosition = (Vector3)stream.ReceiveNext();
    }
    private void Disparo() 
    {
        PhotonNetwork.Instantiate("Bullet", spawnBullet.position, transform.rotation).GetComponent<Bullet>().Initialize(isRight);
    }
    public void Damage()
    {
        maxHealth -= damage;

        if (maxHealth <= 0 && actualLives != 0)
        {
            pv.RPC("NetworkDamage", RpcTarget.All);
        }

        if (maxHealth <= 0 && actualLives == 0)
        {
            pv.RPC("NetworkDeath", RpcTarget.All);
        }
    }
    [PunRPC]
    public void NetworkDamage()
    {
        //HACERSE DAÑO
        if (pv.IsMine)
            transform.position = Game_Manager._GAME_MANAGER.GetPositionSpawnPlayer1(); 
        else
            transform.position = Game_Manager._GAME_MANAGER.GetPositionSpawnPlayer2();

        maxHealth = maxHealth;
        actualLives--;
    }
    [PunRPC]
    private void NetworkDeath()
    {
        Destroy(this.gameObject);
        Scene_Manager.Instance.LoadScene(1);
    }
    [PunRPC]
    private void SetNameEnemyPlayer(string value)
    {
        playerView.SetName(value);
    }
    [PunRPC]
    private void SetRaceStatsPlayer(int id_race)
    {
        race = Session_Manager._SESSION_MANAGER.Races[id_race];
        maxHealth = race.MaxHealth;
        damage = race.Damage;
        velocity = race.Velocity;
        jumpForce = race.JumpForce;
        cadence = race.Cadence;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }
    private bool IsGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask))
            return true;
        else
            return false;
    }

    void FlipPlayer()
    {
        if (!isRight && desiredMovementAxis > 0 || isRight && desiredMovementAxis < 0)
        {
            isRight = !isRight;

            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }
}
