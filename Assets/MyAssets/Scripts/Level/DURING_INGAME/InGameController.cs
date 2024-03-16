using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace MoleSurvivor
{
    public class InGameController : MonoBehaviour
    {
        public static InGameController Instance { get; private set; }

        private void Awake()
        {
            // Set Parent to Root
            transform.parent = null;

            //If there is more than one instance, destroy the extra else Set the static instance to this instance
            if (Instance != null && Instance != this) { Destroy(this.gameObject); } else { Instance = this; }
        }

        private bool setActiveUpdate;

        #region SANDWITCH LEVEL SETTING INSPECTOR
        [BoxGroup("BOX LEVEL SANDWITCH", false)]
        [Title("BOX LEVEL SANDWITCH/LEVEL SANDWITCH")]
        public Transform levelGridParent;
        
        [Space]

        [BoxGroup("BOX LEVEL SANDWITCH")]
        public Transform delayText;
        [BoxGroup("BOX LEVEL SANDWITCH")]
        public int countdownStartValue = 3;
        [BoxGroup("BOX LEVEL SANDWITCH")]
        public float delayBetweenCountdown = 1f;

        [Space]

        [BoxGroup("BOX LEVEL SANDWITCH")]
        public int eachLayerSize = 30;
        [BoxGroup("BOX LEVEL SANDWITCH")]
        public float levelDuration;

        [Space]

        [BoxGroup("BOX LEVEL SANDWITCH")]
        public Transform[] levelSandwitch;

        Transform[] currentlevelSandwitch;
        int endGoal;
        #endregion

        #region CAMERA & GRID SETTING INSPECTOR
        [BoxGroup("BOX CAMERA HOLDER", false)]
        [TitleGroup("BOX CAMERA HOLDER/CAMERA & GRID")]
        public Transform inGameCamera;
        [BoxGroup("BOX CAMERA HOLDER")]
        public Grid grid; // Reference to the isometric grid
        [BoxGroup("BOX CAMERA HOLDER")]
        public Transform finishLine;
        [BoxGroup("BOX CAMERA HOLDER")]
        public Vector2 _screenSpace;
        [BoxGroup("BOX CAMERA HOLDER")]
        public float vertical;
        [BoxGroup("BOX CAMERA HOLDER")]
        public float horizontal;

        Transform _instaFinishLine;
        #endregion

        #region PLAYER SETTING INSPECTOR
        [BoxGroup("BOX PLAYER", false)]
        [TitleGroup("BOX PLAYER/CHECK PLAYER")]
        public PlayerController 
        player1,
        player2,
        player3,
        player4;

        [BoxGroup("BOX PLAYER")]
        [TitleGroup("BOX PLAYER/PLAYER HUD")]
        public PlayerHud 
        player1Health,
        player2Health,
        player3Health,
        player4Health;

        [BoxGroup("BOX PLAYER")]
        [TitleGroup("BOX PLAYER/PLAYER COLOR")]
        public Color 
        player1Color,
        player2Color,
        player3Color,
        player4Color;

        //---------------------------------------------------------------------------------------------------------------------------------------

        [TitleGroup("BOX PLAYER/PLAYER START POSITION", "Set all the players start position depending on how many players")]
        [FoldoutGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder")]
        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Set start height all Player")]
        [HideLabel]
        [SerializeField]
        public int
        setAllPlayerHeight;

        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Only have 1 Player")] 
        [HideLabel]
        [SerializeField]
        public int 
        only1Player;

        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Only have 2 Player")]
        [HideLabel]
        [SerializeField]
        public int
        only2Player1,
        only2Player2;

        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Only have 3 Player")]
        [HideLabel]
        [SerializeField]
        public int
        only3Player1,
        only3Player2,
        only3Player3;

        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Only have 4 Player")]
        [HideLabel]
        [SerializeField]
        public int
        only4Player1,
        only4Player2,
        only4Player3,
        only4Player4;

        //---------------------------------------------------------------------------------------------------------------------------------------

        //[TitleGroup("BOX PLAYER/PLAYER START POSITION", "Set all the players start position depending on how many players")]
        //[FoldoutGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder")]
        //[BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Set start height all Player")]

        //[BoxGroup("BOX LEVEL SANDWITCH", false)]
        //[Title("BOX LEVEL SANDWITCH/LEVEL SANDWITCH")]

        [FoldoutGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder")]
        [TitleGroup("BOX PLAYER/PLAYER RESPAWN POSITION", "Set all the players respawn position")]
        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/Player Respawn Timer")]
        [HideLabel]
        [SerializeField]
        public float
        playerRespawnTimer;

        float
        p1SpawnTimer,
        p2SpawnTimer,
        p3SpawnTimer,
        p4SpawnTimer;

        bool
        p1Death,
        p2Death,
        p3Death,
        p4Death;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/Player Height Respawn Location")]
        [HideLabel]
        [SerializeField]
        public float
        playerRespawnHeightLocation;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/1 Player Respawn Location")]
        [HideLabel]
        [SerializeField]
        public float
        only1player1RespawnLocation;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/1 Player Respawn Location")]
        [LabelText("Debug")]
        [SerializeField]
        public bool
        only1playersRespawnLocationDebug;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/2 Player Respawn Location")]
        [HideLabel]
        [SerializeField]
        public float
        only2player1RespawnLocation,
        only2player2RespawnLocation;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/2 Player Respawn Location")]
        [LabelText("Debug")]
        [SerializeField]
        public bool
        only2playersRespawnLocationDebug;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/3 Player Respawn Location")]
        [HideLabel]
        [SerializeField]
        public float
        only3player1RespawnLocation,
        only3player2RespawnLocation,
        only3player3RespawnLocation;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/3 Player Respawn Location")]
        [LabelText("Debug")]
        [SerializeField]
        public bool
        only3playersRespawnLocationDebug;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/4 Player Respawn Location")]
        [HideLabel]
        [SerializeField]
        public float
        only4player1RespawnLocation,
        only4player2RespawnLocation,
        only4player3RespawnLocation,
        only4player4RespawnLocation;

        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/4 Player Respawn Location")]
        [LabelText("Debug")]
        [SerializeField]
        public bool
        only4playersRespawnLocationDebug;

        [BoxGroup("BOX PLAYER")]
        [TitleGroup("BOX PLAYER/PLAYER DEATH LINE")]
        public float playerDeathHeightLocation;

        [BoxGroup("BOX PLAYER")]
        [TitleGroup("BOX PLAYER/PLAYER BOUND LINE")]
        public float playerBoundLocation;

        [BoxGroup("BOX PLAYER")]
        [TitleGroup("BOX PLAYER/PLAYER FINISH LINE")]
        public float playerFinishHeightLocation;

        [TitleGroup("BOX PLAYER/START SPEED & ROTATION", "Set all the players the Speed and Rotation on the start")]
        [BoxGroup("BOX PLAYER", false)]
        public float 
        playerMoveSpeed,
        playerRotateSpeed,
        playerRotateDelay;

        [TitleGroup("BOX PLAYER/START HEALTH", "Set all the players health")]
        [BoxGroup("BOX PLAYER", false)]
        public float
        playerHealth;

        //[TitleGroup("BOX PLAYER/CHECK PLAYER ACTIVE", "Set how many players in the game")]
        //[BoxGroup("BOX PLAYER")]
        [HideInInspector]
        public bool 
        p1Active,
        p2Active,
        p3Active,
        p4Active;

        [TitleGroup("BOX PLAYER/CHECK PLAYER ACTIVE", "Set how many players in the game")]
        [BoxGroup("BOX PLAYER")]
        public int playersActive;

        int P1Pos, P2Pos, P3Pos, P4Pos;

        [TitleGroup("BOX PLAYER/SET MODE", "Singleplayer / Multiplayer")]
        [BoxGroup("BOX PLAYER")]
        public bool soloMode;
        #endregion

        public List<Transform> whatPlace;

        private void Start() 
        {
            // First, ensure currentlevelSandwitch is properly initialized.
            currentlevelSandwitch = new Transform[levelSandwitch.Length];

            endGoal = -((levelSandwitch.Length * eachLayerSize) - eachLayerSize);

            // Then, instantiate the objects.
            for (int i = 0; i < levelSandwitch.Length; i++)
            {
                currentlevelSandwitch[i] = Instantiate(levelSandwitch[i]);
                currentlevelSandwitch[i].parent = levelGridParent;
                currentlevelSandwitch[i].transform.localPosition = new Vector3(0, -(i * eachLayerSize), 0);
                currentlevelSandwitch[i].gameObject.SetActive(false);
            }

            currentlevelSandwitch[0].gameObject.SetActive(true);

            finishPosition = new Vector3(0, endGoal + playerFinishHeightLocation, inGameCamera.position.z);
            _instaFinishLine = Instantiate(finishLine);
            _instaFinishLine.parent = levelGridParent;
            _instaFinishLine.position = SnapToGrid(new Vector3(0, finishPosition.y, 0));


            if (player1 != null && p1Active) { player1.gameObject.SetActive(true); }
            if (player2 != null && p2Active) { player2.gameObject.SetActive(true); }
            if (player3 != null && p3Active) { player3.gameObject.SetActive(true); }
            if (player4 != null && p4Active) { player4.gameObject.SetActive(true); }

            StartCoroutine(CountdownRoutine(countdownStartValue, delayBetweenCountdown)); 
        }

        void SetStartAfterCountdown()
        {
            SetPlayerPositionActivation(playersActive);

            if (player1 != null && p1Active) { SetStartPlayer(player1, P1Pos, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player1Health, player1Color); }
            if (player2 != null && p2Active) { SetStartPlayer(player2, P2Pos, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player2Health, player2Color); }
            if (player3 != null && p3Active) { SetStartPlayer(player3, P3Pos, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player3Health, player3Color); }
            if (player4 != null && p4Active) { SetStartPlayer(player4, P4Pos, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player4Health, player4Color); }

            p1SpawnTimer = playerRespawnTimer;
            p2SpawnTimer = playerRespawnTimer;
            p3SpawnTimer = playerRespawnTimer;
            p4SpawnTimer = playerRespawnTimer;

            // Start the coroutine to move the camera
            StartCoroutine(MoveCameraCoroutine());
        }

        public void SetPlayerPositionActivation(int numberOfPlayers)
        {
            // First, deactivate all players
            p1Active = p2Active = p3Active = p4Active = false;

            // Then, activate players based on the number of players
            switch (numberOfPlayers)
            {
                case 1:
                    // Set Player Position
                    P1Pos = only1Player;
                    // Optionally reset other player values
                    P2Pos = P3Pos = P4Pos = 0; // Or any default value

                    // Set Player Active
                    p1Active = true;
                    player1.AssignGamepad(0);
                    break;
                case 2:
                    // Set Player Position
                    P1Pos = only2Player1;
                    P2Pos = only2Player2;
                    // Optionally reset other player values
                    P3Pos = P4Pos = 0; // Or any default value

                    // Set Player Active
                    p1Active = p2Active = true;
                    player1.AssignGamepad(0);
                    player2.AssignGamepad(1);
                    break;
                case 3:
                    // Set Player Position
                    P1Pos = only3Player1;
                    P2Pos = only3Player2;
                    P3Pos = only3Player3;
                    // Optionally reset other player value
                    P4Pos = 0; // Or any default value

                    // Set Player Active
                    p1Active = p2Active = p3Active = true;
                    player1.AssignGamepad(0);
                    player2.AssignGamepad(1);
                    player3.AssignGamepad(2);
                    break;
                case 4:
                    // Set Player Position
                    P1Pos = only4Player1;
                    P2Pos = only4Player2;
                    P3Pos = only4Player3;
                    P4Pos = only4Player4;

                    // Set Player Active
                    p1Active = p2Active = p3Active = p4Active = true;
                    player1.AssignGamepad(0);
                    player2.AssignGamepad(1);
                    player3.AssignGamepad(2);
                    player4.AssignGamepad(3);
                    break;
                default:
                    Debug.LogError("Unsupported number of players: " + numberOfPlayers);

                    // Optionally reset all player Position
                    P1Pos = P2Pos = P3Pos = P4Pos = 0; // Or any default value
                    break;
            }

            // Update PlayerControllers' active status based on the active flags
            player1.gameObject.SetActive(p1Active);
            player2.gameObject.SetActive(p2Active);
            player3.gameObject.SetActive(p3Active);
            player4.gameObject.SetActive(p4Active);
        }

        private void Update() 
        {
            SetUpdate(setActiveUpdate); 
        }

        void SetUpdate(bool active)
        {
            if (active != true) { return; }

            if (active)
            {
                if (player1 != null && p1Active) { player1.SetUpdate(); SetPlayerDeath(player1); }
                if (player2 != null && p2Active) { player2.SetUpdate(); SetPlayerDeath(player2); }
                if (player3 != null && p3Active) { player3.SetUpdate(); SetPlayerDeath(player3); }
                if (player4 != null && p4Active) { player4.SetUpdate(); SetPlayerDeath(player4); }
                SetRespawnTimerCountDown();

                // Set the Level to false
                for (int i = 0; i < currentlevelSandwitch.Length; i++)
                {
                    OutsideBound(currentlevelSandwitch[i], eachLayerSize / 2);
                }
            }
        }

        IEnumerator MoveCameraCoroutine()
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = inGameCamera.transform.position;

            while (elapsedTime < levelDuration)
            {
                elapsedTime += Time.fixedDeltaTime;
                inGameCamera.transform.position = Vector3.Lerp(startingPosition, new Vector3(0, endGoal, startingPosition.z), elapsedTime / levelDuration);
                yield return new WaitForFixedUpdate();
            }

            // Ensure the camera reaches exactly the target position
            inGameCamera.transform.position = new Vector3(0, endGoal, startingPosition.z);
        }

        void SetStartPlayer
        (PlayerController player, int pPosition, float pMoveSpeed, float pRotateSpeed, float pRotateDelay, bool solo, float pHealth, PlayerHud pHud, Color pColor)
        {
            player.transform.position = new Vector3(pPosition, setAllPlayerHeight, player.transform.position.z);

            player.currentHealth = pHealth;
            player.moveSpeed = pMoveSpeed;
            player.rotateDuration = pRotateSpeed;
            player.rotateDelay = pRotateDelay;

            player.singleMovement = solo;

            player.playerColor = pColor;
            player.playerHud = pHud;

            player.SetStart();
        }

        void SetPlayerDeath(PlayerController player)
        {
            // Check if the player is now inactive
            if (!player.transform.gameObject.activeSelf) { return; }

            if (player.transform.position.y > InGameController.Instance.deathPosition.y)
            {
                player.transform.gameObject.SetActive(false);

                switch (player.playerId)
                {
                    case 0: p1Active = false; p1Death = true; break;
                    case 1: p2Active = false; p2Death = true; break;
                    case 2: p3Active = false; p3Death = true; break;
                    case 3: p4Active = false; p4Death = true; break;
                    // Default case if needed
                    default: break;
                }
            }
        }

        void SetRespawnTimerCountDown()
        {
            // Update each player's spawn timer if it's above 0
            if (p1Death == true) { p1SpawnTimer -= Time.deltaTime; SetSpawnPosition(); }
            if (p2Death == true) { p2SpawnTimer -= Time.deltaTime; SetSpawnPosition(); }
            if (p3Death == true) { p3SpawnTimer -= Time.deltaTime; SetSpawnPosition(); }
            if (p4Death == true) { p4SpawnTimer -= Time.deltaTime; SetSpawnPosition(); }
        }

        float p1SpawnPos, p2SpawnPos, p3SpawnPos, p4SpawnPos;

        void SetSpawnPosition()
        {
            // Then, activate players based on the number of players
            switch (playersActive)
            {
                case 1:
                    // Set Player Position
                    p1SpawnPos = only1player1RespawnLocation;
                    break;
                case 2:
                    // Set Player Position
                    p1SpawnPos = only2player1RespawnLocation;
                    p2SpawnPos = only2player2RespawnLocation;
                    break;
                case 3:
                    // Set Player Position
                    p1SpawnPos = only3player1RespawnLocation;
                    p2SpawnPos = only3player2RespawnLocation;
                    p3SpawnPos = only3player3RespawnLocation;
                    break;
                case 4:
                    // Set Player Position
                    p1SpawnPos = only4player1RespawnLocation;
                    p2SpawnPos = only4player2RespawnLocation;
                    p3SpawnPos = only4player3RespawnLocation;
                    p4SpawnPos = only4player4RespawnLocation;
                    break;
                default:
                    break;
            }

            if (p1SpawnTimer <= 0) {
                player1.transform.position = SnapToGrid(new Vector3(p1SpawnPos, respawnPosition.y, player1.transform.position.z));
                player1.transform.gameObject.SetActive(true);
                player1.IsCheckTile(SnapToGrid(player1.transform.position));
                p1Active = true;
                p1SpawnTimer = playerRespawnTimer;
                p1Death = false; }
            if (p2SpawnTimer <= 0) {
                player2.transform.position = SnapToGrid(new Vector3(p2SpawnPos, respawnPosition.y, player2.transform.position.z));
                player2.transform.gameObject.SetActive(true);
                player2.IsCheckTile(SnapToGrid(player2.transform.position));
                p2Active = true;
                p2SpawnTimer = playerRespawnTimer;
                p2Death = false; }
            if (p3SpawnTimer <= 0) {
                player3.transform.position = SnapToGrid(new Vector3(p3SpawnPos, respawnPosition.y, player3.transform.position.z));
                player3.transform.gameObject.SetActive(true);
                player3.IsCheckTile(SnapToGrid(player3.transform.position));
                p3Active = true;
                p3SpawnTimer = playerRespawnTimer;
                p3Death = false; }
            if (p4SpawnTimer <= 0) {
                player4.transform.position = SnapToGrid(new Vector3(p4SpawnPos, respawnPosition.y, player4.transform.position.z));
                player4.transform.gameObject.SetActive(true);
                player4.IsCheckTile(SnapToGrid(player4.transform.position));
                p4Active = true;
                p4SpawnTimer = playerRespawnTimer;
                p4Death = false; }
        }

        IEnumerator CountdownRoutine(int startValue, float delay)
        {
            TextMeshProUGUI countdownText = delayText.GetComponent<TextMeshProUGUI>();

            for (int i = startValue; i > 0; i--)
            {
                // Set text to current countdown value
                countdownText.text = i.ToString();

                // Resize using DoTween
                countdownText.rectTransform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    // Restore original size
                    countdownText.rectTransform.DOScale(1f, 0.2f);
                });

                // Delay between countdown numbers
                yield return new WaitForSeconds(delay);
            }

            // Display "Go!" or any other message
            countdownText.text = "Go!";
            countdownText.rectTransform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                // Restore original size
                countdownText.rectTransform.DOScale(1f, 0.2f);
                countdownText.gameObject.SetActive(false);
                SetStartAfterCountdown();
                setActiveUpdate = true;
            });
        }

        public void SetDestroy()
        {
            Destroy(this.gameObject);
        }

        public void OutsideBound(Transform objectBound, float objectBoundPositionOffset)
        {
            // Assuming inGameCamera, horizontal, vertical, and _screenSpace are already defined

            // Center position based on the inGameCamera
            Vector3 centerPosition = inGameCamera.position;

            // Inner boundaries
            float innerLeft = centerPosition.x - horizontal;
            float innerRight = centerPosition.x + horizontal;
            float innerBottom = centerPosition.y - vertical;
            float innerTop = centerPosition.y + vertical;

            // Outer boundaries (expanded by _screenSpace)
            float outerLeft = innerLeft - _screenSpace.x;
            float outerRight = innerRight + _screenSpace.x;
            float outerBottom = innerBottom - _screenSpace.y;
            float outerTop = innerTop + _screenSpace.y;

            Vector3 pointToCheckTop = objectBound.position + new Vector3(0, -objectBoundPositionOffset, 0); // Example point
            Vector3 pointToCheckDown = objectBound.position + new Vector3(0, objectBoundPositionOffset, 0); // Example point

            if (pointToCheckTop.y > outerTop)
            {
                // The point the outer boundaries
                objectBound.gameObject.SetActive(false);
            }
            if (pointToCheckDown.y > outerBottom && pointToCheckTop.y < outerTop)
            {
                // The point the outer boundaries
                objectBound.gameObject.SetActive(true);
            }
        }

        Vector3 SnapToGrid(Vector3 player)
        {
            // Convert world position to cell position and back to snap to the grid
            player = grid.CellToWorld(grid.WorldToCell(player));
            return player;
        }

        [HideInInspector] public Vector3 respawnPosition;
        [HideInInspector] public Vector3 deathPosition;
        [HideInInspector] public Vector3 finishPosition;

        private void OnDrawGizmos()
        {
            if (inGameCamera == null)
            {
                Debug.LogWarning("inGameCamera Transform is not set.");
                return;
            }

            Vector3 centerPosition = inGameCamera.position;

            // Adjusted positions to center around inGameCamera
            Vector3 bottomLeft = centerPosition + new Vector3(-horizontal, -vertical);
            Vector3 bottomRight = centerPosition + new Vector3(horizontal, -vertical);
            Vector3 topLeft = centerPosition + new Vector3(-horizontal, vertical);
            Vector3 topRight = centerPosition + new Vector3(horizontal, vertical);

            // Drawing inner borders
            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(bottomRight, topRight);

            Gizmos.color = Color.yellow;

            // Adjusted positions for outer borders using _screenSpace
            Vector3 outerBottomLeft = bottomLeft + new Vector3(-_screenSpace.x, -_screenSpace.y);
            Vector3 outerBottomRight = bottomRight + new Vector3(_screenSpace.x, -_screenSpace.y);
            Vector3 outerTopLeft = topLeft + new Vector3(-_screenSpace.x, _screenSpace.y);
            Vector3 outerTopRight = topRight + new Vector3(_screenSpace.x, _screenSpace.y);

            // Drawing outer borders
            Gizmos.DrawLine(outerBottomLeft, outerBottomRight);
            Gizmos.DrawLine(outerTopLeft, outerTopRight);
            Gizmos.DrawLine(outerBottomLeft, outerTopLeft);
            Gizmos.DrawLine(outerBottomRight, outerTopRight);

            //---------------------------------------------------------------------------------------
            
            Gizmos.color = Color.red;

            respawnPosition = new Vector3(0, inGameCamera.position.y + playerRespawnHeightLocation, inGameCamera.position.z);
            Gizmos.DrawLine(respawnPosition + new Vector3(-horizontal, 0), respawnPosition + new Vector3(horizontal, 0));

            if (only1playersRespawnLocationDebug)
            {
                Gizmos.DrawWireSphere(respawnPosition + new Vector3(only1player1RespawnLocation, 0, 0), 0.5f);
            }

            if (only2playersRespawnLocationDebug)
            {
                Gizmos.DrawWireSphere(respawnPosition + new Vector3(only2player1RespawnLocation, 0, 0), 0.5f);
                Gizmos.DrawWireSphere(respawnPosition + new Vector3(only2player2RespawnLocation, 0, 0), 0.5f);
            }

            if (only3playersRespawnLocationDebug)
            {
                Gizmos.DrawWireSphere(respawnPosition + new Vector3(only3player1RespawnLocation, 0, 0), 0.5f);
                Gizmos.DrawWireSphere(respawnPosition + new Vector3(only3player2RespawnLocation, 0, 0), 0.5f);
                Gizmos.DrawWireSphere(respawnPosition + new Vector3(only3player3RespawnLocation, 0, 0), 0.5f);
            }

            if (only4playersRespawnLocationDebug)
            {
                Gizmos.DrawWireSphere(respawnPosition + new Vector3(only4player1RespawnLocation, 0, 0), 0.5f);
                Gizmos.DrawWireSphere(respawnPosition + new Vector3(only4player2RespawnLocation, 0, 0), 0.5f);
                Gizmos.DrawWireSphere(respawnPosition + new Vector3(only4player3RespawnLocation, 0, 0), 0.5f);
                Gizmos.DrawWireSphere(respawnPosition + new Vector3(only4player4RespawnLocation, 0, 0), 0.5f);
            }

            //---------------------------------------------------------------------------------------

            Gizmos.color = Color.blue;

            deathPosition = new Vector3(0, inGameCamera.position.y + playerDeathHeightLocation, inGameCamera.position.z);
            Gizmos.DrawLine(deathPosition + new Vector3(-horizontal, 0), deathPosition + new Vector3(horizontal, 0));

            //---------------------------------------------------------------------------------------

            Gizmos.color = Color.white;

            Vector3 fPosition = new Vector3(0, inGameCamera.position.y + playerFinishHeightLocation, inGameCamera.position.z);
            Gizmos.DrawLine(fPosition + new Vector3(-horizontal, 0), fPosition + new Vector3(horizontal, 0));

            //---------------------------------------------------------------------------------------

            Gizmos.color = Color.blue;

            Vector3 boundSidePositionLeft = new Vector3(-playerBoundLocation, inGameCamera.position.y , inGameCamera.position.z);
            Gizmos.DrawLine(boundSidePositionLeft + new Vector3(0, -vertical), boundSidePositionLeft + new Vector3(0, vertical));

            Vector3 boundSidePositionRight = new Vector3(playerBoundLocation, inGameCamera.position.y, inGameCamera.position.z);
            Gizmos.DrawLine(boundSidePositionRight + new Vector3(0, -vertical), boundSidePositionRight + new Vector3(0, vertical));
        }

    }
}
