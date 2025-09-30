using System.Collections.Generic;
using UnityEngine;

// [System.Serializable] 어트리뷰트는 이 클래스를 유니티 인스펙터 창에 노출시켜줍니다.
[System.Serializable]
public class FruitChance
{
    // [Tooltip("과일 프리팹")]
    public GameObject fruitPrefab; // 생성할 과일의 프리팹

    // [Tooltip("확률 가중치 (높을수록 잘 나옴)")]
    public float weight;           // 확률 가중치
}

public class FruitSpawner : MonoBehaviour
{
    [Header("과일 생성 확률 설정")]
    public List<FruitChance> fruitChances;

    private float totalWeight;

    // 게임이 시작될 때 한 번만 호출됩니다.
    void Awake()
    {
        // 총 가중치를 미리 계산해 둡니다.
        CalculateTotalWeight();
    }

    // 총 가중치를 계산하는 함수
    void CalculateTotalWeight()
    {
        totalWeight = 0;
        foreach (var chance in fruitChances)
        {
            totalWeight += chance.weight;
        }
    }

    // 설정된 확률에 따라 랜덤한 과일 프리팹을 반환하는 함수
    public GameObject GetRandomFruitPrefab()
    {
        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0;

        foreach (var chance in fruitChances)
        {
            cumulativeWeight += chance.weight;
            if (randomValue <= cumulativeWeight)
            {
                return chance.fruitPrefab; // 당첨! 이 과일 프리팹을 반환
            }
        }

        return null; // 혹시 모를 예외 상황 방지
    }

    // --- 테스트용 코드 ---
    // 스페이스바를 누르면 랜덤 과일을 (0, 10, 0) 위치에 생성합니다.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject randomFruit = GetRandomFruitPrefab();
            if (randomFruit != null)
            {
                Instantiate(randomFruit, new Vector3(0, 10, 0), Quaternion.identity);
                Debug.Log($"생성된 과일: {randomFruit.name}");
            }
        }
    }
}