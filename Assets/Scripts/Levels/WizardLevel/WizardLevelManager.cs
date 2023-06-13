using System.Collections.Generic;
using System.Linq;
using GameScene;
using GameScene.PlayerControl;
using Levels.SheepLevel;
using Levels.WizardLevel;
using SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

public class WizardLevelManager : LevelManager
{
    [SerializeField] private TilemapManager groundMap;

    [SerializeField] private GridAdapter grid;
    [SerializeField] private WizardMachineManager machineManager;
    [SerializeField] private WizardVortexManager vortexManager;
    [SerializeField] private WizardVioletTileManager violetTileManager;
    [SerializeField] private WizardYellowTileManager yellowTileManager;
    
    [SerializeField] private MovableGridEntity player;
    
    [SerializeField] private WizardMachine machinePrefab;
    [SerializeField] private WizardVioletTile violetTilePrefab;
    [SerializeField] private WizardYellowTile yellowTilePrefab;
    [SerializeField] private Vortex vortexPrefab;
    
    [SerializeField] private Vector2Int playerStartPosition;
    [SerializeField] private List<Vector2Int> machinePositions;
    [SerializeField] private Vector2Int vortexPosition;

    private bool _setup;
    private bool _hasWon;
    private bool _hasLost;

    private List<Vector2Int> possibleIndices;

    private void Start()
    {
        SetupLevel();
        CalculateGameStatus();
    }

    private void CalculateGameStatus()
    {
        possibleIndices = groundMap.GetIndices();
        possibleIndices.AddRange(yellowTileManager.GetActiveIndices());
        
        if (!possibleIndices.Contains(player.GetIndex()))
        {
            // Player is on unfeasible position! 
            player.BlendOut();
            _hasLost = true;
        }
        if (vortexManager.HasAt(player.GetIndex()))
        {
            player.BlendOut();
            _hasWon = true;
        }
    }

    public void SetupLevel()
    {
        ExchangeTileMapWithRealTiles();
        InitPlayer();
        InitAim();
        InitMachines();
        InitMirrorTiles();
        UpdateMirrorTiles();

        _setup = true;
    }

    private void InitPlayer()
    {
        player.InstantUpdatePosition(playerStartPosition, grid.GetPositionForIndex(playerStartPosition));
        player.StartShaking();
    }
    
    private void InitAim()
    {
        var vortex = Instantiate(vortexPrefab, vortexManager.transform);
        vortexManager.AddAt(vortex, vortexPosition);
        vortex.StartShaking();
    }
    
    private void InitMachines()
    {
        foreach (var machineIndex in machinePositions)
        {
            var machine = Instantiate(machinePrefab, machineManager.transform);
            machineManager.AddAt(machine, machineIndex);
        }
    }

    private void ExchangeTileMapWithRealTiles()
    {
        groundMap.gameObject.SetActive(false);
        groundMap.GetIndices().ForEach(index =>
        {
            var tile = Instantiate(violetTilePrefab, violetTileManager.transform);
            tile.SetSortOrder(-index.y);
            violetTileManager.AddAt(tile, index);
        });
    }
    
    private void InitMirrorTiles()
    {
        var numberOfTiles = groundMap.GetIndices().Count;
        for (int i = 0; i < numberOfTiles; i++)
        {
            var mirrorTile = Instantiate(yellowTilePrefab, yellowTileManager.transform);
            mirrorTile.BlendOutInstantly();
            yellowTileManager.AddAt(mirrorTile, new Vector2Int(0, 0));
        }
    }

    private void UpdateMirrorTiles()
    {
        var indicesForMirrorTiles = new HashSet<Vector2Int>();
        foreach (var wizardMachine in machineManager.GetEntities())
        {
            var machineIndex = wizardMachine.GetIndex();
            foreach (var groundIndex in groundMap.GetIndices())
            {
                if (wizardMachine.GetDirection() == WizardMachine.WizardMachineDirection.Horizontal)
                {
                    if ((groundIndex.y - machineIndex.y) >= 0)
                    {
                        var mirroredIndex = new Vector2Int(
                            groundIndex.x,
                            machineIndex.y - (groundIndex.y - machineIndex.y)
                        );
                        indicesForMirrorTiles.Add(mirroredIndex);
                    }
                    
                }
                else
                {
                    if ((groundIndex.x - machineIndex.x) <= 0)
                    {
                        var mirroredIndex = new Vector2Int(
                            machineIndex.x - (groundIndex.x - machineIndex.x),
                            groundIndex.y
                        );
                        indicesForMirrorTiles.Add(mirroredIndex);
                    }
                }
            }
        }
        
        // Remove all indices which have a regular tile already:
        indicesForMirrorTiles.RemoveWhere(index => groundMap.GetIndices().Contains(index));

        yellowTileManager.UpdateMirrorTileIndices(indicesForMirrorTiles);
        
    }

    public override void HandleUpdate()
    {
        if (_setup)
        {
            var move = InputManager.instance.GetMoveDirection();
            
            if (move != Vector2Int.zero)
            {
                HandlePlayerMove(move);
                CalculateGameStatus();
            }

            if (Input.GetKeyDown(KeyCode.E) &&
                GetHVNeighboringIndices(player.GetIndex()).Any(index => machineManager.HasAt(index)))
            {
                machineManager.GetEntities()[0].ToggleDirection();
                UpdateMirrorTiles();
                CalculateGameStatus();
            }
        }
    }

    public override bool HasWon()
    {
        return _hasWon;
    }

    public override bool HasLost()
    {
        return _hasLost;
    }

    private void HandlePlayerMove(Vector2Int move)
    {
        var currentIndex = player.GetIndex();
        var nextIndex = currentIndex + move;

        if (!possibleIndices.Contains(nextIndex))
        {
            // Moving is not possible
            return;
        }

        if (machineManager.HasAt(nextIndex))
        {
            var nextIndexForMachine = nextIndex + move;

            if (machineManager.HasAt(nextIndexForMachine) || !groundMap.GetIndices().Contains(nextIndexForMachine))
            {
                // Machine cannot be moved
                return;
            }
            
            var machineToMove = machineManager.GetAt(nextIndex);
            machineToMove.UpdatePosition(nextIndexForMachine, grid.GetPositionForIndex(nextIndexForMachine));
            UpdateMirrorTiles();
        }

        player.UpdatePosition(nextIndex, grid.GetPositionForIndex(nextIndex));
    }
    
    public List<Vector2Int> GetHVNeighboringIndices(Vector2Int index)
    {
        List<Vector2Int> rawList = new List<Vector2Int>
        {
            new (index.x, index.y - 1),
            new (index.x - 1, index.y),
            new (index.x + 1, index.y),
            new (index.x, index.y + 1),
        };

        return rawList;
    }
}