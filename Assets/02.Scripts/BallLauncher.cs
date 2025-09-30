using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [Header("발사 설정")]
    [Tooltip("공이 생성될 위치. 이 오브젝트의 위치와 방향을 따릅니다.")]
    public Transform spawnPoint;

    [Tooltip("이전에 만든 FruitSpawner 스크립트를 연결하세요.")]
    public FruitSpawner fruitSpawner;

    [Tooltip("공을 발사할 힘의 크기")]
    public float launchForce = 500f;

    private GameObject currentBall;      // 현재 생성되어 발사를 대기중인 공
    private Rigidbody currentBallRigidbody; // 대기중인 공의 Rigidbody 컴포넌트

    // 게임 시작 시 첫 번째 공을 생성
    void Start()
    {
        SpawnNextBall();
    }

    // 매 프레임마다 입력을 확인합니다.
    void Update()
    {
        // 스페이스바를 눌렀고, 발사할 공이 대기중일 때
        if (Input.GetKeyDown(KeyCode.Space) && currentBall != null)
        {
            LaunchBall();
        }
    }

    // 다음 공을 생성하고 발사를 준비하는 함수
    void SpawnNextBall()
    {
        // FruitSpawner를 이용해 랜덤 과일 프리팹을 가져옴
        GameObject ballPrefab = fruitSpawner.GetRandomFruitPrefab();
        if (ballPrefab == null) return;

        // spawnPoint의 위치와 회전값에 맞춰 새로운 공을 생성
        currentBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        
        // 생성된 공의 Rigidbody 컴포넌트를 가져옵니다.
        currentBallRigidbody = currentBall.GetComponent<Rigidbody>();

        // 물리 효과 스위치를 끕니다 (중력 무시, 공중에 고정)
        currentBallRigidbody.isKinematic = true;
    }

    // 현재 공을 발사하는 함수
    void LaunchBall()
    {
        // 물리 효과 스위치를 다시 키기 (중력 적용 시작)
        currentBallRigidbody.isKinematic = false;

        // spawnPoint의 정면 방향으로 힘을 가해 발사
        // ForceMode.Impulse는 순간적으로 강력한 힘을 가하는 방식
        currentBallRigidbody.AddForce(spawnPoint.forward * launchForce, ForceMode.Impulse);

        // 발사된 공에 대한 참조를 비워서, 다시 발사할 수 없도록
        currentBall = null;
        currentBallRigidbody = null;

        // 다음 공을 바로 생성 (원한다면 코루틴으로 딜레이를 줄 수 있습니다)
        SpawnNextBall();
    }
}

