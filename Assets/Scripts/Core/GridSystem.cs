using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject archerPrefab;
    [SerializeField] private Button warriorButton;
    [SerializeField] private Button archerButton;
    [SerializeField] private TMP_Text warriorCostText;
    [SerializeField] private TMP_Text archerCostText;

    private string TEAM_TAG = "Player";

    private void Start()
    {
        warriorButton.onClick.AddListener(() => SpawnUnit(Unit.UnitTypeEnum.Warrior, TEAM_TAG));
        archerButton.onClick.AddListener(() => SpawnUnit(Unit.UnitTypeEnum.Archer, TEAM_TAG));

        UpdateCostTexts();
    }

    private void SpawnUnit(Unit.UnitTypeEnum unitType, string teamTag)
    {
        int spawnCost = CalculateSpawnCost(unitType);
        if (CoinManager.Instance.GetTotalCoin() < spawnCost && spawnCost != 0)
        {
            Debug.Log("Not enough coins to spawn unit!");
            return;
        }

        if (spawnCost > 0)
        {
            CoinManager.Instance.AddCoin(-spawnCost);
        }

        Tile[] spawnableTiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in spawnableTiles)
        {
            if (tile.CanSpawn)
            {
                Vector3 spawnPosition = tile.transform.position + Vector3.up * 1.7f;
                Quaternion rotation = Quaternion.Euler(0, 180, 0);
                GameObject unitObject = ObjectPool.Instance.SpawnFromPool(unitType, 1, spawnPosition, rotation);

                Unit unit = unitObject.GetComponent<Unit>();
                if (unit != null)
                {
                    unit.CurrentTile = tile;
                    tile.SetUnit(unit);
                    tile.CanSpawn = false;
                    unitObject.tag = teamTag;
                    UnitHealthBar healthBar = unitObject.GetComponentInChildren<UnitHealthBar>();
                    healthBar.SetHealthBarColor();
                }

                Unit otherUnit = tile.GetUnit();
                if (otherUnit != null && otherUnit != unit && 
                    otherUnit.UnitType == unit.UnitType && 
                    otherUnit.UnitLevel == unit.UnitLevel)
                {
                    UnitMergeHandler.Instance.TryMergeUnits(unit, otherUnit);
                }

                if (unitType == Unit.UnitTypeEnum.Warrior)
                {
                    GameDataManager.Instance.SpawnCountWarrior++;
                    GameDataManager.Instance.SpawnCostWarrior = spawnCost;
                }
                else if (unitType == Unit.UnitTypeEnum.Archer)
                {
                    GameDataManager.Instance.SpawnCountArcher++;
                    GameDataManager.Instance.SpawnCostArcher = spawnCost;
                }
                UpdateCostTexts();
                StartCoroutine(UnitMergeHandler.ScaleEffect(unitObject.transform));
                break;
            }
        }
    }

    private int CalculateSpawnCost(Unit.UnitTypeEnum unitType)
    {
        int spawnCount = (unitType == Unit.UnitTypeEnum.Warrior) ? GameDataManager.Instance.SpawnCountWarrior : GameDataManager.Instance.SpawnCountArcher;
        int previousCost = (unitType == Unit.UnitTypeEnum.Warrior) ? GameDataManager.Instance.SpawnCostWarrior : GameDataManager.Instance.SpawnCostArcher;

        if (spawnCount == 0)
        {
            return 0;
        }
        else if (spawnCount == 1)
        {
            return 100;
        }
        else
        {
            return Mathf.RoundToInt(previousCost * 1.1f);
        }
    }

    private void UpdateCostTexts()
    {
        int warriorCost = CalculateSpawnCost(Unit.UnitTypeEnum.Warrior);
        int archerCost = CalculateSpawnCost(Unit.UnitTypeEnum.Archer);

        warriorCostText.text = warriorCost == 0 ? "0" : $"{warriorCost}";
        archerCostText.text = archerCost == 0 ? "0" : $"{archerCost}";
    }
}