using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private FloatVariable moveOffset;
    [SerializeField] private LayerMask cellLayer;
    [SerializeField] private PlayerTrail playerTrail;

    private Animator anim;
    private FallDetection fallDetection;
    private PlayerSounds playerSounds;
    private PlayerController playerController;

    [SerializeField] AnimationCurve jumpEase;

    //events
    [SerializeField] private GameEvent onGoalReached;
    [SerializeField] private IntEvent onPlayerSetOrientation;
    [SerializeField] private GameEvent onHideArrows;
    [SerializeField] private GameEvent onLose;
    [SerializeField] private GameEvent onVibrate;
    [SerializeField] private LocalEvent onSpawnConfetti;
    [SerializeField] private LocalEvent onWaterSplash;
    [SerializeField] private LocalEvent onJumpBubbles;
    [SerializeField] private LocalEvent onTeleport;

    //interactions
    private bool isSliding = false;
    private bool alive;
    private bool fall;
    private bool isJumping;
    private int slideCounter;
    private bool waterSplashed;

    //movement AND rotation
    public Orientation orientation;
    private Vector3 nextPoint;
    private Vector3 direction;
    private bool reachedNextPoint;

    private Cell cellUnderPlayer;
    
    private void Start()
    {
        reachedNextPoint = false;
        waterSplashed = false;
        playerSounds = GetComponent<PlayerSounds>();
        fallDetection = GetComponent<FallDetection>();
        alive = true;
        orientation = Orientation.Vertical;
        anim = GetComponent<Animator>();
        nextPoint = transform.position;
    }

    private void Update()
    {
        if (alive)
        {
            float actualSpeed = isSliding ? moveSpeed * 1.5f : moveSpeed;
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, actualSpeed * Time.deltaTime);
        }

        if (fall && transform.position.y >= -10f)
        {
            transform.Translate(Vector3.down * moveSpeed * 3 * Time.deltaTime);
            if (Physics.Raycast(transform.position + (Vector3.up * 2), Vector3.down, out RaycastHit hit, 1f))
            {
                if (!waterSplashed)
                {
                    waterSplashed = true;
                    onWaterSplash.Raise(hit.point);
                }
                onLose.Raise();
            }
        }
    }

    public void MoveToNextCell(Orientation or, float inputDir)
    {
        if (!reachedNextPoint) return;
        anim.SetFloat("Speed", 1);
        onHideArrows.Raise();
        reachedNextPoint = false;
        if (or == Orientation.Vertical)
        {
            direction = new Vector3(inputDir, 0, 0);
            transform.rotation = Quaternion.Euler(0, 90 * inputDir, 0);
        } else
        {
            direction = new Vector3(0, 0, inputDir);
            transform.rotation = inputDir > 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        }
        nextPoint = transform.position + (direction * moveOffset.Value);
        orientation = 1 - orientation;
        StartCoroutine(WaitForNextPointReached());
    }

    private IEnumerator WaitForNextPointReached()
    {
        yield return new WaitUntil(() => transform.position == nextPoint || !alive);
        InteractWithCell();
    }

    private Cell GetCellUnderPlayer(Vector3 rotation = default)
    {
        if (cellUnderPlayer != null)
        {
            cellUnderPlayer.SwitchProperty(rotation.normalized);
        }
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 5f, cellLayer))
        {
            return hit.collider.GetComponent<Cell>();
        }
        else
        {
            return null;
        }
    }

    private void InteractWithCell()
    {
        if (!alive) return;
        cellUnderPlayer = GetCellUnderPlayer(direction);
        if (cellUnderPlayer.property == CellProperty.Slide)
        {
            StartSliding();
            onVibrate.Raise();
        }
        else if (cellUnderPlayer.property == CellProperty.Bounce)
        {
            if (isSliding) StopSliding();
            cellUnderPlayer.Bounce();
            Jump();
            onVibrate.Raise();
        }
        else if (cellUnderPlayer.property == CellProperty.Free || cellUnderPlayer.property == CellProperty.Static)
        {
            if (isSliding) StopSliding();
            anim.SetFloat("Speed", 0);
            reachedNextPoint = true;
            onPlayerSetOrientation.Raise((int)orientation);
        }
        else if (cellUnderPlayer.property == CellProperty.Goal)
        {
            Win();
        }
        else if (cellUnderPlayer.property == CellProperty.Teleport)
        {
            if (isSliding) StopSliding();
            onVibrate.Raise();
            StartCoroutine(TeleportToTwinCell(cellUnderPlayer.twinCell));
        }
    }

    private void Jump()
    {
        onJumpBubbles.Raise(transform.position);
        anim.Rebind();
        anim.ResetTrigger("Jump");
        isJumping = true;
        anim.SetTrigger("Jump");
        nextPoint = transform.position + (direction * moveOffset.Value) * 2;
        transform.DOJump(nextPoint, 5, 1, 1f)
            .SetEase(jumpEase)
            .OnComplete(LandJump);
    }

    private void LandJump()
    {
        fallDetection.StartDetecting(true);
        isJumping = false;
        if (GetCellUnderPlayer() == null)
        {
            alive = false;
            fall = true;
            isJumping = true;
        }
        else
        {
            anim.SetTrigger("JumpDown");
            InteractWithCell();
        }
    }

    private void StartSliding()
    {
        if (!isSliding)
        {
            playerSounds.PlaySlidingSound(true);
            playerTrail.PlaySlideTrail(true);
            anim.Rebind();
            anim.ResetTrigger("JumpDown");
            anim.SetTrigger("Slide");
        } 
        isSliding = true;
        slideCounter++;
        nextPoint = transform.position + (direction * moveOffset.Value);
        StartCoroutine(WaitForNextPointReached());
    }

    private void StopSliding()
    {
        playerSounds.PlaySlidingSound(false);
        playerTrail.PlaySlideTrail(false);
        isSliding = false;
        slideCounter = 0;
        anim.Rebind();
        anim.ResetTrigger("Slide");
    }

    private IEnumerator TeleportToTwinCell(Cell cell)
    {
        float yPos = transform.position.y;
        onTeleport.Raise(transform.position + Vector3.up);
        fallDetection.StartDetecting(false);
        transform.position = new Vector3(0, 30, 0);
        yield return new WaitForSeconds(0.5f);
        transform.position = new Vector3(cell.transform.position.x, yPos, cell.transform.position.z);
        fallDetection.StartDetecting(true);
        onTeleport.Raise(transform.position + Vector3.up);
        nextPoint = transform.position;
        reachedNextPoint = true;
        anim.SetFloat("Speed", 0);
        onPlayerSetOrientation.Raise((int)orientation);
    }

    private void Win()
    {
        if (isSliding) StopSliding();
        onSpawnConfetti.Raise(transform.position);
        anim.SetTrigger("Dance");
        fallDetection.StartDetecting(false);
        onGoalReached.Raise();
        onHideArrows.Raise();
    }

    public void ResetPlayer(Vector3 position)
    {
        waterSplashed = false;
        alive = true;
        isSliding = false;
        isJumping = false;
        direction = Vector3.zero;
        anim.Rebind();
        anim.ResetTrigger("Fall");
        anim.ResetTrigger("Dance");
        orientation = Orientation.Vertical;
        StartCoroutine(PlayerSpawn(position));
    }

    public void PlayerDespawn()
    {
        reachedNextPoint = false;
        transform.position = new Vector3(0, 25, 0);
        fall = false;
        nextPoint = transform.position;
    }

    private IEnumerator PlayerSpawn(Vector3 position)
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = position;
        nextPoint = transform.position;
        isJumping = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        cellUnderPlayer = GetCellUnderPlayer(Vector3.zero);
        onPlayerSetOrientation.Raise((int)orientation);
        fallDetection.StartDetecting(true);
        onTeleport.Raise(transform.position + Vector3.up);
        reachedNextPoint = true;
    }

    public IEnumerator Lose()
    {
        if (isJumping) yield break;
        if (isSliding) StopSliding();
        alive = false;
        anim.SetTrigger("Fall");
        yield return new WaitForSeconds(0.7f);
        fall = true;
    }   
}

public enum Orientation
{
    Vertical, Horizontal
}
