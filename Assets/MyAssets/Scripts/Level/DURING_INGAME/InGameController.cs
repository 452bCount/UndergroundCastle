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
        //-----------------------------------------------------------------------------------------------------------------------------------------
        // MAKE THIS INTO INSTANCE
        #region INSTANCE THIS CONTROLLER LEVEL SCRIPT
        // Set this into an instance
        public static InGameController Instance { get; private set; }

        private void Awake()
        {
            // Set Parent to Root
            transform.parent = null;

            //If there is more than one instance, destroy the extra else Set the static instance to this instance
            if (Instance != null && Instance != this) { Destroy(this.gameObject); } else { Instance = this; }
        }
        #endregion
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //NEXT
        //-----------------------------------------------------------------------------------------------------------------------------------------
        // THIS IS TO STOP THE UPDATE LIKE WHEN PAUSING THE GAME
        #region SET PAUSE ACTIVE
        private bool setActiveUpdate;
        #endregion
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //NEXT
        //-----------------------------------------------------------------------------------------------------------------------------------------
        // THIS PART INSTANTIATE THE LEVEL STAGE
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
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //NEXT
        //-----------------------------------------------------------------------------------------------------------------------------------------
        // THIS IS TO SET UP THE CAMERA AND THE GRID
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

        Transform _instaFinishLine; // The instatiate Finish Line
        #endregion
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //NEXT
        //-----------------------------------------------------------------------------------------------------------------------------------------
        // THIS IS TO SET UP THE PLAYERS 1,2,3,4
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

        [FoldoutGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder")]
        [TitleGroup("BOX PLAYER/PLAYER RESPAWN POSITION", "Set all the players respawn position")]
        [BoxGroup("BOX PLAYER/PLAYER RESPAWN POSITION/Collapse Folder/Player Respawn Timer")]
        [HideLabel]
        [SerializeField]
        public float
        playerRespawnTimer;

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

        [TitleGroup("BOX PLAYER/CHECK PLAYER ACTIVE", "Set how many players in the game")]
        [BoxGroup("BOX PLAYER")]
        public int playersActive;

        [TitleGroup("BOX PLAYER/SET MODE", "Singleplayer / Multiplayer")]
        [BoxGroup("BOX PLAYER")]
        public bool soloMode;
        #endregion
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //NEXT
        //-----------------------------------------------------------------------------------------------------------------------------------------
        // OTHER SETTINGS
        #region OTHER SETTINGS
        [ReadOnly] public List<PlayerController> playersCollection;
        [ReadOnly] public List<PlayerHud> playersHealth;
        [ReadOnly] public List<Color> playersColor;

        [ReadOnly] public List<int> playersStartPosition;
        [ReadOnly] public List<float> playersRespawnTimer;
        [ReadOnly] public List<bool> playersDeath;
        [ReadOnly] public List<float> PlayersRespawnPosition;
        [ReadOnly] public List<bool> playersAlive;
        [ReadOnly] public List<int> playersPosition;

        [ReadOnly] public Vector3 respawnPosition;
        [ReadOnly] public Vector3 deathPosition;
        [ReadOnly] public Vector3 finishPosition;

        [ReadOnly] public List<Transform> playersFinishPlace;
        #endregion
        //-----------------------------------------------------------------------------------------------------------------------------------------

        // Start Player Function

        void AddPlayersToList(PlayerController player, PlayerHud pHealth, Color pColor)
        {
            // Add players to List
            playersCollection.Add(player);
            // Add players Health to List
            playersHealth.Add(pHealth);
            // Add players Color to List
            playersColor.Add(pColor);
        }

        void SetStartPlayer(PlayerController player, int pPosition, float pMoveSpeed, float pRotateSpeed, float pRotateDelay, bool solo, float pHealth, PlayerHud pHud, Color pColor)
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

        void SetPlayers(int playersActive)
        {
            // ADD PLAYER TO LIST

            for (int i = 0; i < playersActive; i++)
            {
                playersStartPosition.Add(0);
                playersRespawnTimer.Add(0);
                playersDeath.Add(false);
                PlayersRespawnPosition.Add(0);
                playersAlive.Add(true);
                playersPosition.Add(0);
            }

            if (playersActive == 1)
            {
                // Add players to List
                AddPlayersToList(player1, player1Health, player1Color); // Player 1

                // Set Player Position
                playersStartPosition[0] = only1Player;
                playersAlive[0] = true;

                // Set Controller
                playersCollection[0].AssignGamepad(0);

                // Update PlayerControllers' active status based on the active flags
                playersCollection[0].gameObject.SetActive(playersAlive[0]);

                // Set Player Start
                if (playersAlive[0] == true) { SetStartPlayer(playersCollection[0], playersStartPosition[0], playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player1Health, player1Color); }

                // Set Player Spawn Position
                PlayersRespawnPosition[0] = only1player1RespawnLocation;

                // Set the player Spawn Timer
                playersRespawnTimer[0] = playerRespawnTimer;
            }
            else if (playersActive == 2)
            {
                // Add players to List
                AddPlayersToList(player1, player1Health, player1Color); // Player 1
                AddPlayersToList(player2, player2Health, player2Color); // Player 2

                // Set Player Position
                playersStartPosition[0] = only2Player1;
                playersStartPosition[1] = only2Player2;
                playersAlive[0] =
                playersAlive[1] = true;

                // Set Controller
                playersCollection[0].AssignGamepad(0);
                playersCollection[1].AssignGamepad(1);

                // Update PlayerControllers' active status based on the active flags
                playersCollection[0].gameObject.SetActive(playersAlive[0]);
                playersCollection[1].gameObject.SetActive(playersAlive[1]);

                // Set Player Start
                if (playersAlive[0] == true) { SetStartPlayer(playersCollection[0], playersStartPosition[0], playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player1Health, player1Color); }
                if (playersAlive[1] == true) { SetStartPlayer(playersCollection[1], playersStartPosition[1], playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player2Health, player2Color); }

                // Set Player Spawn Position
                PlayersRespawnPosition[0] = only2player1RespawnLocation;
                PlayersRespawnPosition[1] = only2player2RespawnLocation;

                // Set the player Spawn Timer
                playersRespawnTimer[0] = playerRespawnTimer;
                playersRespawnTimer[1] = playerRespawnTimer;
            }
            else if (playersActive == 3)
            {
                // Add players to List
                AddPlayersToList(player1, player1Health, player1Color); // Player 1
                AddPlayersToList(player2, player2Health, player2Color); // Player 2
                AddPlayersToList(player3, player3Health, player3Color); // Player 3

                // Set Player Position
                playersStartPosition[0] = only3Player1;
                playersStartPosition[1] = only3Player2;
                playersStartPosition[2] = only3Player3;
                playersAlive[0] =
                playersAlive[1] =
                playersAlive[2] = true;

                // Set Controller
                playersCollection[0].AssignGamepad(0);
                playersCollection[1].AssignGamepad(1);
                playersCollection[2].AssignGamepad(2);

                // Update PlayerControllers' active status based on the active flags
                playersCollection[0].gameObject.SetActive(playersAlive[0]);
                playersCollection[1].gameObject.SetActive(playersAlive[1]);
                playersCollection[2].gameObject.SetActive(playersAlive[2]);

                // Set Player Start
                if (playersAlive[0] == true) { SetStartPlayer(playersCollection[0], playersStartPosition[0], playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player1Health, player1Color); }
                if (playersAlive[1] == true) { SetStartPlayer(playersCollection[1], playersStartPosition[1], playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player2Health, player2Color); }
                if (playersAlive[2] == true) { SetStartPlayer(playersCollection[2], playersStartPosition[2], playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player3Health, player3Color); }

                // Set Player Spawn Position
                PlayersRespawnPosition[0] = only3player1RespawnLocation;
                PlayersRespawnPosition[1] = only3player2RespawnLocation;
                PlayersRespawnPosition[2] = only3player3RespawnLocation;

                // Set the player Spawn Timer
                playersRespawnTimer[0] = playerRespawnTimer;
                playersRespawnTimer[1] = playerRespawnTimer;
                playersRespawnTimer[2] = playerRespawnTimer;
            }
            else if (playersActive == 4)
            {
                // Add players to List
                AddPlayersToList(player1, player1Health, player1Color); // Player 1
                AddPlayersToList(player2, player2Health, player2Color); // Player 2
                AddPlayersToList(player3, player3Health, player3Color); // Player 3
                AddPlayersToList(player4, player4Health, player4Color); // Player 4

                // Set Player Position
                playersStartPosition[0] = only4Player1;
                playersStartPosition[1] = only4Player2;
                playersStartPosition[2] = only4Player3;
                playersStartPosition[3] = only4Player4;
                playersAlive[0] =
                playersAlive[1] =
                playersAlive[2] =
                playersAlive[3] = true;

                // Set Controller
                playersCollection[0].AssignGamepad(0);
                playersCollection[1].AssignGamepad(1);
                playersCollection[2].AssignGamepad(2);
                playersCollection[3].AssignGamepad(3);

                // Update PlayerControllers' active status based on the active flags
                playersCollection[0].gameObject.SetActive(playersAlive[0]);
                playersCollection[1].gameObject.SetActive(playersAlive[1]);
                playersCollection[2].gameObject.SetActive(playersAlive[2]);
                playersCollection[3].gameObject.SetActive(playersAlive[3]);

                // Set Player Start
                if (playersAlive[0] == true) { SetStartPlayer(playersCollection[0], playersStartPosition[0], playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player1Health, player1Color); }
                if (playersAlive[1] == true) { SetStartPlayer(playersCollection[1], playersStartPosition[1], playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player2Health, player2Color); }
                if (playersAlive[2] == true) { SetStartPlayer(playersCollection[2], playersStartPosition[2], playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player3Health, player3Color); }
                if (playersAlive[3] == true) { SetStartPlayer(playersCollection[3], playersStartPosition[3], playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, playerHealth, player4Health, player4Color); }

                // Set Player Spawn Position
                PlayersRespawnPosition[0] = only4player1RespawnLocation;
                PlayersRespawnPosition[1] = only4player2RespawnLocation;
                PlayersRespawnPosition[2] = only4player3RespawnLocation;
                PlayersRespawnPosition[3] = only4player4RespawnLocation;

                // Set the player Spawn Timer
                playersRespawnTimer[0] = playerRespawnTimer;
                playersRespawnTimer[1] = playerRespawnTimer;
                playersRespawnTimer[2] = playerRespawnTimer;
                playersRespawnTimer[3] = playerRespawnTimer;
            }
        }

        // Update Player Function

        void PlayersRespawn(int player)
        {
            if (playersRespawnTimer[player] <= 0)
            {
                playersCollection[player].transform.position = SnapToGrid(new Vector3(PlayersRespawnPosition[player], respawnPosition.y, playersCollection[player].transform.position.z));
                playersCollection[player].transform.gameObject.SetActive(true);
                playersCollection[player].IsCheckTile(SnapToGrid(playersCollection[player].transform.position));
                playersAlive[player] = true;
                playersRespawnTimer[player] = playerRespawnTimer;
                playersDeath[player] = false;
            }
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
                    case 0: playersAlive[0] = false; playersDeath[0] = true; break;
                    case 1: playersAlive[1] = false; playersDeath[1] = true; break;
                    case 2: playersAlive[2] = false; playersDeath[2] = true; break;
                    case 3: playersAlive[3] = false; playersDeath[3] = true; break;
                    // Default case if needed
                    default: break;
                }
            }
        }

        private void Start()
        {
            //-----------------------------------------------------------------------------------------------------------------------------------------
            // INSTANTIATE LEVEL

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
            //-----------------------------------------------------------------------------------------------------------------------------------------

            finishPosition = new Vector3(0, endGoal + playerFinishHeightLocation, inGameCamera.position.z);
            _instaFinishLine = Instantiate(finishLine);
            _instaFinishLine.parent = levelGridParent;
            _instaFinishLine.position = SnapToGrid(new Vector3(0, finishPosition.y, 0));

            StartCoroutine(CountdownRoutine(countdownStartValue, delayBetweenCountdown));
        }

        void SetStartAfterCountdown()
        {
            SetPlayers(playersActive);

            // Start the coroutine to move the camera
            StartCoroutine(MoveCameraCoroutine());
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
                if (playersActive == 1)
                {
                    // Set Player Update
                    if (playersAlive[0] == true) { playersCollection[0].SetUpdate(); SetPlayerDeath(playersCollection[0]); }

                    // Player Respawn
                    PlayersRespawn(0);

                    // Update each player's spawn timer if it's above 0
                    if (playersDeath[0] == true) { playersRespawnTimer[0] -= Time.deltaTime; PlayersRespawn(0); }
                }
                else if (playersActive == 2)
                {
                    // Set Player Update
                    if (playersAlive[0] == true) { playersCollection[0].SetUpdate(); SetPlayerDeath(playersCollection[0]); }
                    if (playersAlive[1] == true) { playersCollection[1].SetUpdate(); SetPlayerDeath(playersCollection[1]); }

                    // Player Respawn
                    PlayersRespawn(0);
                    PlayersRespawn(1);

                    // Update each player's spawn timer if it's above 0
                    if (playersDeath[0] == true) { playersRespawnTimer[0] -= Time.deltaTime; PlayersRespawn(0); }
                    if (playersDeath[1] == true) { playersRespawnTimer[1] -= Time.deltaTime; PlayersRespawn(1); }
                }
                else if (playersActive == 3)
                {
                    // Set Player Update
                    if (playersAlive[0] == true) { playersCollection[0].SetUpdate(); SetPlayerDeath(playersCollection[0]); }
                    if (playersAlive[1] == true) { playersCollection[1].SetUpdate(); SetPlayerDeath(playersCollection[1]); }
                    if (playersAlive[2] == true) { playersCollection[2].SetUpdate(); SetPlayerDeath(playersCollection[2]); }

                    // Player Respawn
                    PlayersRespawn(0);
                    PlayersRespawn(1);
                    PlayersRespawn(2);

                    // Update each player's spawn timer if it's above 0
                    if (playersDeath[0] == true) { playersRespawnTimer[0] -= Time.deltaTime; PlayersRespawn(0); }
                    if (playersDeath[1] == true) { playersRespawnTimer[1] -= Time.deltaTime; PlayersRespawn(1); }
                    if (playersDeath[2] == true) { playersRespawnTimer[2] -= Time.deltaTime; PlayersRespawn(2); }
                }
                else if (playersActive == 4)
                {
                    // Set Player Update
                    if (playersAlive[0] == true) { playersCollection[0].SetUpdate(); SetPlayerDeath(playersCollection[0]); }
                    if (playersAlive[1] == true) { playersCollection[1].SetUpdate(); SetPlayerDeath(playersCollection[1]); }
                    if (playersAlive[2] == true) { playersCollection[2].SetUpdate(); SetPlayerDeath(playersCollection[2]); }
                    if (playersAlive[3] == true) { playersCollection[3].SetUpdate(); SetPlayerDeath(playersCollection[3]); }

                    // Player Respawn
                    PlayersRespawn(0);
                    PlayersRespawn(1);
                    PlayersRespawn(2);
                    PlayersRespawn(3);

                    // Update each player's spawn timer if it's above 0
                    if (playersDeath[0] == true) { playersRespawnTimer[0] -= Time.deltaTime; PlayersRespawn(0); }
                    if (playersDeath[1] == true) { playersRespawnTimer[1] -= Time.deltaTime; PlayersRespawn(1); }
                    if (playersDeath[2] == true) { playersRespawnTimer[2] -= Time.deltaTime; PlayersRespawn(2); }
                    if (playersDeath[3] == true) { playersRespawnTimer[3] -= Time.deltaTime; PlayersRespawn(3); }
                }

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

        public void SetDestroy()
        {
            Destroy(this.gameObject);
        }

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

            Vector3 boundSidePositionLeft = new Vector3(-playerBoundLocation, inGameCamera.position.y, inGameCamera.position.z);
            Gizmos.DrawLine(boundSidePositionLeft + new Vector3(0, -vertical), boundSidePositionLeft + new Vector3(0, vertical));

            Vector3 boundSidePositionRight = new Vector3(playerBoundLocation, inGameCamera.position.y, inGameCamera.position.z);
            Gizmos.DrawLine(boundSidePositionRight + new Vector3(0, -vertical), boundSidePositionRight + new Vector3(0, vertical));
        }

    }
}
