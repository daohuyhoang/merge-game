using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMergeHandler : MonoBehaviour
{
    public static UnitMergeHandler Instance { get; private set; }

    [SerializeField] GameObject mergeEffectPrefab;
    [SerializeField] private AudioClip mergeSound;
    [SerializeField] private UnitMergeUICard mergeUICard;
    [SerializeField] private Canvas uiCanvas;

    private Dictionary<Unit.UnitTypeEnum, int> highestMergedLevel = new Dictionary<Unit.UnitTypeEnum, int>();
    
    private string TEAM_TAG = "Player";
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public bool TryMergeUnits(Unit unitA, Unit unitB)
    {
        if (unitA == unitB) return false;
        if (unitA.UnitType == unitB.UnitType && unitA.UnitLevel == unitB.UnitLevel)
        {
            if (ObjectPool.Instance.HasNextLevel(unitA.UnitType, unitA.UnitLevel))
            {
                MergeUnits(unitA, unitB);
                audioSource.PlayOneShot(mergeSound);
                return true;
            }
        }
        return false;
    }

    private void MergeUnits(Unit unitA, Unit unitB)
    {
        Vector3 mergePosition = unitB.transform.position;
        int newLevel = unitA.UnitLevel + 1;
        Quaternion mergeRotation = Quaternion.Euler(0, 180, 0);
        GameObject newUnitObject = ObjectPool.Instance.SpawnFromPool(unitA.UnitType, newLevel, mergePosition, mergeRotation);
        if (newUnitObject != null)
        {
            Unit newUnit = newUnitObject.GetComponent<Unit>();
            newUnit.UnitLevel = newLevel;
            newUnit.tag = TEAM_TAG;
            newUnit.UpdateStats();
            Unit.UnitTypeEnum type = newUnit.UnitType;
            
            if (!highestMergedLevel.ContainsKey(type))
            {
                highestMergedLevel[type] = 0;
            }

            if (newLevel > highestMergedLevel[type])
            {
                highestMergedLevel[type] = newLevel;
                DisplayMergeUI(newUnit);
            }
            
            UnitHealthBar healthBar = newUnit.GetComponentInChildren<UnitHealthBar>();
            healthBar.SetHealthBarColor();
            newUnit.CurrentTile = unitB.CurrentTile;
            unitB.CurrentTile.SetUnit(newUnit);

            if (unitA.CurrentTile != null)
            {
                unitA.CurrentTile.SetUnit(null);
                unitA.CurrentTile.CanSpawn = true;
            }
            ObjectPool.Instance.ReturnToPool(unitA.UnitType, unitA.gameObject);
            if (unitB.CurrentTile != null)
            {
                unitB.CurrentTile.SetUnit(newUnit);
                unitB.CurrentTile.CanSpawn = false;
            }
            ObjectPool.Instance.ReturnToPool(unitA.UnitType, unitB.gameObject);
            if (mergeEffectPrefab != null)
            {
                GameObject effect = Instantiate(mergeEffectPrefab, mergePosition, Quaternion.identity);
                Destroy(effect, 1f);
            }

            StartCoroutine(ScaleEffect(newUnitObject.transform));
        }
    }

    public static IEnumerator ScaleEffect(Transform targetTransform)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        Vector3 currentScale = targetTransform.localScale;

        Vector3 startScale = currentScale - new Vector3(0.2f, 0.2f, 0.2f);
        Vector3 midScale = currentScale + new Vector3(0.2f, 0.2f, 0.2f);
        Vector3 endScale = currentScale;

        while (elapsedTime < duration)
        {
            targetTransform.localScale = Vector3.Lerp(startScale, midScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetTransform.localScale = midScale;

        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            targetTransform.localScale = Vector3.Lerp(midScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetTransform.localScale = endScale;
    }
    
    private void DisplayMergeUI(Unit mergedUnit)
    {
        if (mergeUICard != null && uiCanvas != null)
        {
            Sprite unitSprite = mergedUnit.UnitData.spritesByLevel[mergedUnit.UnitLevel - 1];
            UnitMergeUICard cardInstance = Instantiate(mergeUICard);

            Vector3 screenPos = Camera.main.WorldToScreenPoint(mergedUnit.transform.position);
            cardInstance.transform.position = screenPos;

            cardInstance.DisplayUnitInfo(
                unitSprite,
                mergedUnit.UnitHealth.HP,
                mergedUnit.ATK,
                mergedUnit.UnitType.ToString(),
                mergedUnit.UnitLevel
            );
        }
        else
        {
            Debug.LogError("mergeUICard or uiCanvas is null!");
        }
    }
}