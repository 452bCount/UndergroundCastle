using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace MoleSurvivor
{
    [System.Serializable]
    public class PlayerCustom
    {
        public PlayerController playerController;
        public PlayerHud playerHealth;
        public Color playerColor;

        public float playerRespawnTimer;
        public bool playerDeath;
        public bool playerAlive;
    }

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
        public Transform inGameMapCam;
        [BoxGroup("BOX CAMERA HOLDER")]
        public InGameCanvas inGameCanvas;
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
        [BoxGroup("BOX CAMERA HOLDER")]
        public float MapCamClamp;

        Transform _instaFinishLine; // The instatiate Finish Line
        #endregion
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //NEXT
        //-----------------------------------------------------------------------------------------------------------------------------------------
        // THIS IS TO SET UP THE PLAYERS 1,2,3,4
        #region PLAYER SETTING INSPECTOR
        [BoxGroup("BOX PLAYER", false)] [TitleGroup("BOX PLAYER/CHECK PLAYER")]
        public PlayerController player1, player2, player3, player4;
        [HideInInspector] public List<PlayerController> pCollection;

        [BoxGroup("BOX PLAYER")] [TitleGroup("BOX PLAYER/PLAYER HUD")]
        public PlayerHud player1Health, player2Health, player3Health, player4Health;
        [HideInInspector] public List<PlayerHud> pHudCollection;

        [BoxGroup("BOX PLAYER")] [TitleGroup("BOX PLAYER/PLAYER COLOR")]
        public Color player1Color, player2Color, player3Color, player4Color;
        [HideInInspector] public List<Color> pColorCollection;

        //---------------------------------------------------------------------------------------------------------------------------------------

        [BoxGroup("BOX PLAYER")] [TitleGroup("BOX PLAYER/PLAYER START LINE")]  [LabelText("Start Line")]
        [SerializeField] public float playersStartPosition;

        //---------------------------------------------------------------------------------------------------------------------------------------

        [TitleGroup("BOX PLAYER/PLAYER RESPAWN POSITION", "Set all the players respawn position")]
        [BoxGroup("BOX PLAYER")] [LabelText("Respawn Timer")]
        [SerializeField] public float playerRespawnTimer;

        [BoxGroup("BOX PLAYER")] [LabelText("Respawn Line")]
        [SerializeField] public float playersRespawnPosition;

        [BoxGroup("BOX PLAYER")] [LabelText("Death Line")]
        [TitleGroup("BOX PLAYER/PLAYER DEATH LINE", "Set all the players death position")]
        public float playersDeathPosition;

        [BoxGroup("BOX PLAYER")] [LabelText("Bound Side Line")]
        [TitleGroup("BOX PLAYER/PLAYER BOUND LINE")]
        public float playersSideBoundPosition;

        [BoxGroup("BOX PLAYER")] [LabelText("Bound Bottom Line")]
        public float playersBottomBoundPosition;

        [BoxGroup("BOX PLAYER")] [LabelText("Finish Line")]
        [TitleGroup("BOX PLAYER/PLAYER FINISH LINE", "Set finish position")]
        public float playersFinishPosition;

        [BoxGroup("BOX PLAYER")] [LabelText("Health Height")]
        [TitleGroup("BOX PLAYER/PLAYER HEALTH LINE", "Set all the players Health UI position to the Canvas")]
        public float playerHealthHeightLocation;

        [TitleGroup("BOX PLAYER/START SPEED & ROTATION", "Set all the players the Speed and Rotation on the start")]
        [BoxGroup("BOX PLAYER", false)]
        public float playerMoveSpeed, playerRotateSpeed, playerRotateDelay;

        [TitleGroup("BOX PLAYER/START HEALTH", "Set all the players health")]
        [BoxGroup("BOX PLAYER", false)]
        public float playerHealth;

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
        [ReadOnly] public List<PlayerCustom> playerEach;
        [HideInInspector] public Vector3 startPosition;
        [HideInInspector] public Vector3 respawnPosition;
        [HideInInspector] public Vector3 deathPosition;
        [HideInInspector] public Vector3 finishPosition;
        [HideInInspector] public Vector3 boundBottomPosition;
        [ReadOnly] public List<Transform> playersFinishPlace;
        [ReadOnly] public bool allPlayerFinishLevel;
        [ReadOnly] public bool seeFinishLine;
        #endregion
        //-----------------------------------------------------------------------------------------------------------------------------------------
        //NEXT
        //-----------------------------------------------------------------------------------------------------------------------------------------
        // START PLAYER FUNCTION
        #region START PLAYER FUNCTION
        void SetStartPlayerCustom(PlayerCustom playerCustom, PlayerController player, PlayerHud pHealth, Color pColor, float gridPos, int pGamepad)
        {
            playerCustom.playerController = player;
            playerCustom.playerHealth = pHealth;
            playerCustom.playerColor = pColor;

            // Set Player Position
            float pStartPosition = gridPos;
            // Set Player is ALive
            playerCustom.playerAlive = true;
            // Set Controller
            playerCustom.playerController.AssignGamepad(pGamepad);
            // Set Player GameObject Active
            playerCustom.playerController.gameObject.SetActive(playerCustom.playerAlive);
            // Set Player Start
            if (playerCustom.playerAlive == true) 
            {
                playerCustom.playerController.transform.position = SnapToGrid(new Vector3(pStartPosition, startPosition.y, playerCustom.playerController.transform.position.z));
                playerCustom.playerController.currentHealth = playerHealth;
                playerCustom.playerController.moveSpeed = playerMoveSpeed;
                playerCustom.playerController.rotateDuration = playerRotateSpeed;
                playerCustom.playerController.rotateDelay = playerRotateDelay;

                playerCustom.playerController.singleMovement = soloMode;

                playerCustom.playerController.boundarySide = playersSideBoundPosition;

                playerCustom.playerController.playerColor = pColor;
                playerCustom.playerController.playerHud = playerCustom.playerHealth;

                playerCustom.playerController.SetStart();
            }
            // Set Player is Dead
            playerCustom.playerDeath = false;
            // Set the player Spawn Timer
            playerCustom.playerRespawnTimer = playerRespawnTimer;
        }

        void SetPlayers(int playersActive)
        {
            // ADD PLAYER TO LIST
            pCollection.Add(player1);
            pCollection.Add(player2);
            pCollection.Add(player3);
            pCollection.Add(player4);

            pHudCollection.Add(player1Health);
            pHudCollection.Add(player2Health);
            pHudCollection.Add(player3Health);
            pHudCollection.Add(player4Health);

            pColorCollection.Add(player1Color);
            pColorCollection.Add(player2Color);
            pColorCollection.Add(player3Color);
            pColorCollection.Add(player4Color);

            for (int i = 0; i < playersActive; i++)
            {
                playerEach.Add(new PlayerCustom());
                SetStartPlayerCustom(playerEach[i], pCollection[i], pHudCollection[i], pColorCollection[i], GridColumns[i].x, i);
            }
        }
        #endregion
        // UPDATE PLATER FUNCTION
        #region UPDATE PLATER FUNCTION

        void PUpdates(int pNumber)
        {
            // Set Player Update
            if (playerEach[pNumber].playerAlive == true) { playerEach[pNumber].playerController.SetUpdate(); SetPlayerDeath(playerEach[pNumber].playerController); }
            // Player Respawn
            PlayersRespawn(pNumber);
            // Update each player's spawn timer if it's above 0
            if (playerEach[pNumber].playerDeath == true) { playerEach[pNumber].playerRespawnTimer -= Time.deltaTime; PlayersRespawn(pNumber); }
            // Update when bottom bound
            playerEach[pNumber].playerController.boundaryBottom = boundBottomPosition.y;
        }

        void PlayersRespawn(int player)
        {
            if (playerEach[player].playerRespawnTimer <= 0)
            {
                playerEach[player].playerController.transform.position = SnapToGrid(new Vector3(GridColumns[player].x, respawnPosition.y));
                playerEach[player].playerController.transform.gameObject.SetActive(true);
                playerEach[player].playerController.IsCheckTile(playerEach[player].playerController.transform.position);
                playerEach[player].playerAlive = true;
                playerEach[player].playerRespawnTimer = playerRespawnTimer;
                playerEach[player].playerDeath = false;
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
                    case 0: playerEach[0].playerAlive = false; playerEach[0].playerDeath = true; break;
                    case 1: playerEach[1].playerAlive = false; playerEach[1].playerDeath = true; break;
                    case 2: playerEach[2].playerAlive = false; playerEach[2].playerDeath = true; break;
                    case 3: playerEach[3].playerAlive = false; playerEach[3].playerDeath = true; break;
                    // Default case if needed
                    default: break;
                }
            }
        }
        #endregion
        //-----------------------------------------------------------------------------------------------------------------------------------------

        // How many Grid Columns based on the LevelStage.cs
        #region Grid Columns
        [Min(0)] public float range;
        
        private int columns; // Renamed to columns for clarity
        [ReadOnly] public List<Vector3> GridColumns; // Now tracking columns
        [HideInInspector] public int cColumns; // Renamed to cColumns for clarity
        #endregion

        private void Start()
        {
            // MAKE GRID COLUMNS
            #region GRID COLUMNS
            columns = playersActive;
            cColumns = columns; // Create how many Grid Columns

            foreach (var r in GetGridColumns(inGameCamera.transform, range, columns)) // Now getting columns
            { GridColumns.Add(r.origin); }
            #endregion
            //-----------------------------------------------------------------------------------------------------------------------------------------
            // INSTANTIATE LEVELS
            #region INSTANTIATE LEVELS
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
            currentlevelSandwitch[1].gameObject.SetActive(true);
            #endregion
            // INSTANTIATE FINISH LINE
            #region INSTANTIATE FINISH LEVEL
            _instaFinishLine = Instantiate(finishLine);
            _instaFinishLine.parent = levelGridParent;
            Vector3 fPos = new Vector3(0.5f, endGoal + playersFinishPosition, inGameCamera.position.z);
            finishPosition = SnapToGrid(fPos);
            _instaFinishLine.position = new Vector3(0, finishPosition.y, 0);
            #endregion
            // SET PLAYERHUB POSITION
            #region I WILL UPDATE THIS LATER
            RectTransform canvasRectTransform = inGameCanvas.GetComponent<RectTransform>();
            float canvasWidth = canvasRectTransform.rect.width;
            float columnWidth = canvasWidth / (playersActive + 1);
            #endregion
            // START COUNTDOWN
            #region START COUNTDOWN
            StartCoroutine(CountdownRoutine(countdownStartValue, delayBetweenCountdown));
            #endregion
            //-----------------------------------------------------------------------------------------------------------------------------------------
        }

        void SetStartAfterCountdown()
        {
            //-----------------------------------------------------------------------------------------------------------------------------------------
            // START AFTER COUNTDOWN
            #region START AFTER COUNTDOWN
            // Set how many players are active
            SetPlayers(playersActive);

            // Start the coroutine to move the camera
            StartCoroutine(MoveCameraCoroutine());

            // Start the coroutine to move the camera map
            StartCoroutine(MoveCameraMapCoroutine());
            #endregion
            //-----------------------------------------------------------------------------------------------------------------------------------------
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
                for (int i = 0; i < playersActive; i++)
                {
                    PUpdates(i);
                }

                // Set the Level to false
                for (int i = 0; i < currentlevelSandwitch.Length; i++)
                {
                    OutsideBound(currentlevelSandwitch[i], eachLayerSize / 2, true);
                }

                if (playersFinishPlace.Count >= playersActive)
                {
                    allPlayerFinishLevel = true;
                }

                foreach (var pEach in playerEach)
                {
                    if (pEach.playerController.transform.position.y < _instaFinishLine.position.y)
                    { OutsideBound(pEach.playerController.transform, 0, false, () => SetPlayerAfterFinish(pEach)); }
                }

                if (_instaFinishLine.position.y >= (inGameCamera.position.y - vertical) && seeFinishLine == false) 
                {
                    foreach (var pEach in playerEach)
                    {
                        pEach.playerController.seeFinishLine = true;
                    }

                    seeFinishLine = true;
                }
            }
        }

        void SetPlayerAfterFinish(PlayerCustom pEach)
        {
            pEach.playerController.isAllowedToMove = false;
            if (pEach.playerController.characterMovement.ReturnCheckIsMoving() == false) 
            { pEach.playerAlive = false; pEach.playerController.transform.gameObject.SetActive(false); pEach.playerController.transform.position = Vector3.zero; }
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

        IEnumerator MoveCameraMapCoroutine()
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = inGameMapCam.transform.position;

            while (elapsedTime < levelDuration)
            {
                elapsedTime += Time.fixedDeltaTime;
                inGameMapCam.transform.position = Vector3.Lerp(startingPosition, new Vector3(0, endGoal + MapCamClamp, startingPosition.z), elapsedTime / levelDuration);
                yield return new WaitForFixedUpdate();
            }

            // Ensure the camera reaches exactly the target position
            inGameMapCam.transform.position = new Vector3(0, endGoal + MapCamClamp, startingPosition.z);
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

        public void OutsideBound(Transform objectBound, float objectBoundPositionOffset, bool SideY, Action applyAction = null)
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

            if (pointToCheckTop.y > outerTop && SideY == true)
            {
                // The point the outer boundaries
                objectBound.gameObject.SetActive(false);
            }
            if (pointToCheckDown.y > outerBottom && pointToCheckTop.y < outerTop && SideY == true)
            {
                // The point the outer boundaries
                objectBound.gameObject.SetActive(true);
            }

            if (objectBound.position.x < outerLeft && SideY == false || objectBound.position.x > outerRight && SideY == false)
            {
                applyAction?.Invoke();
            }
        }

        Vector3 SnapToGrid(Vector3 player)
        {
            // Convert world position to cell position and back to snap to the grid
            player = grid.CellToWorld(grid.WorldToCell(player));
            return player;
        }

        #region
        //Ray[] GetGridColumns(Transform origin, float range, int count)
        //{
        //    Ray[] rays = new Ray[count];
        //    float spacing = range / (count - 1);
        //    float start = -range / 2f;
        //    for (int i = 0; i < count; i++)
        //    {
        //        float currentX = start + i * spacing;
        //        Vector3 ori = new Vector3(currentX, 0, 0) / 1.5f + origin.position;
        //        Vector3 oriSnap = SnapToGrid(ori);
        //        rays[i].origin = new Vector3(oriSnap.x, ori.y, ori.z);
        //        rays[i].direction = -origin.up; // Columns extend vertically, so we use up
        //    }
        //    return rays;
        //}
        #endregion

        Ray[] GetGridColumns(Transform origin, float range, int count)
        {
            if (count < 1)
            {
                // Return an empty array or handle error as preferred
                return new Ray[0];
            }

            Ray[] rays = new Ray[count];

            if (count == 1)
            {
                // If count is 1, set the single ray directly at the origin's position
                Vector3 oriSnap = SnapToGrid(origin.position);
                rays[0].origin = new Vector3(oriSnap.x, origin.position.y, origin.position.z);
                rays[0].direction = -origin.up;
            }
            else if (count > 1)
            {
                float spacing = range / (count - 1);
                float start = -range / 2f;
                for (int i = 0; i < count; i++)
                {
                    float currentX = start + i * spacing;
                    Vector3 ori = new Vector3(currentX, 0, 0) / 1.5f + origin.position;
                    Vector3 oriSnap = SnapToGrid(ori);
                    rays[i].origin = new Vector3(oriSnap.x, ori.y, ori.z);
                    rays[i].direction = -origin.up; // Columns extend vertically, so we use up
                }
            }

            return rays;
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

            // START LINE

            Gizmos.color = Color.yellow;
            Vector3 startPos = new Vector3(0.5f, inGameCamera.position.y + playersStartPosition, inGameCamera.position.z);
            Vector3 startPositionSnap = SnapToGrid(startPos);
            startPosition = new Vector3(0, startPositionSnap.y, startPos.z);
            Gizmos.DrawLine(startPosition + new Vector3(-horizontal, 0), startPosition + new Vector3(horizontal, 0));

            //---------------------------------------------------------------------------------------

            // RESPAWN LINE

            Gizmos.color = Color.red;
            Vector3 respawnPos = new Vector3(0.5f, inGameCamera.position.y + playersRespawnPosition, inGameCamera.position.z);
            Vector3 respawnPositionSnap = SnapToGrid(respawnPos);
            respawnPosition = new Vector3(0, respawnPositionSnap.y, respawnPos.z);
            Gizmos.DrawLine(respawnPosition + new Vector3(-horizontal, 0), respawnPosition + new Vector3(horizontal, 0));

            //---------------------------------------------------------------------------------------

            // DEATH LINE

            Gizmos.color = Color.blue;
            deathPosition = new Vector3(0, inGameCamera.position.y + playersDeathPosition, inGameCamera.position.z);
            Gizmos.DrawLine(deathPosition + new Vector3(-horizontal, 0), deathPosition + new Vector3(horizontal, 0));

            //---------------------------------------------------------------------------------------

            // FINISH LINE

            Gizmos.color = Color.white;
            Vector3 finishPos = new Vector3(0.5f, inGameCamera.position.y + playersFinishPosition, inGameCamera.position.z);
            Vector3 finishPositionSnap = SnapToGrid(finishPos);
            Vector3 finishPosition = new Vector3(0, finishPositionSnap.y, finishPos.z);
            Gizmos.DrawLine(finishPosition + new Vector3(-horizontal, 0), finishPosition + new Vector3(horizontal, 0));

            //---------------------------------------------------------------------------------------

            // OUT OF SIDE BOUNDS LINE

            Gizmos.color = Color.blue;

            Vector3 boundSidePositionLeft = new Vector3(-playersSideBoundPosition, inGameCamera.position.y, inGameCamera.position.z);
            Gizmos.DrawLine(boundSidePositionLeft + new Vector3(0, -vertical), boundSidePositionLeft + new Vector3(0, vertical));

            Vector3 boundSidePositionRight = new Vector3(playersSideBoundPosition, inGameCamera.position.y, inGameCamera.position.z);
            Gizmos.DrawLine(boundSidePositionRight + new Vector3(0, -vertical), boundSidePositionRight + new Vector3(0, vertical));

            // OUT OF BOTTOM BOUNDS LINE

            Gizmos.color = Color.blue;
            Vector3 boundBottomPos = new Vector3(0.5f, inGameCamera.position.y + playersBottomBoundPosition, inGameCamera.position.z);
            Vector3 boundBottomPositionSnap = SnapToGrid(boundBottomPos);
            boundBottomPosition = new Vector3(0, boundBottomPositionSnap.y, boundBottomPos.z);
            Gizmos.DrawLine(boundBottomPosition + new Vector3(-horizontal, 0), boundBottomPosition + new Vector3(horizontal, 0));

            //---------------------------------------------------------------------------------------

            // GRID COLUMN LINE

            if (cColumns > 0)
            {
                foreach (var r in GetGridColumns(inGameCamera.transform, range, cColumns)) // Drawing columns
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(new Vector3(r.origin.x, r.origin.y + vertical, r.origin.z), new Vector3(r.origin.x, r.origin.y - vertical, r.origin.z));
                }
            }
        }

    }
}
