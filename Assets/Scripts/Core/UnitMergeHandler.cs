using UnityEngine;
using System.Collections;

public class UnitMergeHandler : MonoBehaviour
{
    public static UnitMergeHandler Instance;

    [SerializeField] GameObject mergeEffectPrefab;

    private string TEAM_TAG = "Player";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool TryMergeUnits(Unit unitA, Unit unitB)
    {
        if (unitA == unitB) return false;
        if (unitA.UnitType == unitB.UnitType && unitA.UnitLevel == unitB.UnitLevel)
        {
            if (ObjectPool.Instance.HasNextLevel(unitA.UnitType, unitA.UnitLevel))
            {
                MergeUnits(unitA, unitB);
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

        Vector3 startScale = new Vector3(1.3f, 1.3f, 1.3f);
        Vector3 midScale = new Vector3(1.7f, 1.7f, 1.7f);
        Vector3 endScale = new Vector3(1.5f, 1.5f, 1.5f);

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
}