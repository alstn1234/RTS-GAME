using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    // �ּ� ��� ������Ʈ �ð�
    const float minPathUpdateTime = .2f;
    // ��� ������Ʈ �Ÿ� �Ӱ�ġ
    const float pathUpdateMoveThreshold = .5f;

    // ��ǥ ��ġ
    public Transform target;
    // �̵� �ӵ�
    public float speed = 4;
    // ȸ�� �ӵ�
    public float turnSpeed = 3;
    // ȸ�� �Ÿ� �Ӱ�ġ
    public float turnDst = 5;
    // ���� �Ÿ� �Ӱ�ġ
    public float stoppingDst = 10;
    // ��� ���� �̵� ������ ����
    bool followingPath = true;

    // ��� ����
    Path path;

    // ��� �̵� ���� �޼ҵ�
    public void StopMethod()
    {
        followingPath = false;
    }

    // ��� �̵� ���� �޼ҵ�
    public void StartMethod()
    {
        followingPath = true;
        StartCoroutine(UpdatePath());
    }

    // ��� ��û ��� �ݹ� �Լ�
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            // ��� ��ü ����
            path = new Path(waypoints, transform.position, turnDst, stoppingDst);

            // ��� �̵� �ڷ�ƾ ���� �� ����
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    // ��� ������Ʈ �ڷ�ƾ
    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }

        // �ʱ� ��� ��û
        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);

            // ��ǥ ��ġ�� �̵��� ��쿡�� ��� ��û
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath()
    {
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]); // ù ��° ��� ������ �ٶ󺸵��� ȸ��

        float speedPercent = 1;

        while (followingPath) // ��θ� ���� �̵��ϴ� ���� �ݺ�
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z); // ������ ��ġ�� 2D ���ͷ� ��ȯ

            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D)) // ���� ��� �������� �̵��ϴ� ���� �ݺ�
            {
                if (pathIndex == path.finishLineIndex) // ����� ������ �������� ������ ���
                {
                    followingPath = false; // ��� �̵� ����
                    break;
                }
                else // ���� ��� �������� �̵�
                {
                    pathIndex++;
                }
            }

            if (followingPath) // ��� �̵��� ���� ���� ���
            {
                if (pathIndex >= path.slowDownIndex && stoppingDst > 0) // ��θ� ���� �̵��ϴٰ� ���� �Ÿ� �̳��� ������ ��� �ӵ� ����
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst); // �ӵ� ���� ���� ���
                    if (speedPercent < 0.01f) // �ӵ��� ���� 0�� �Ǿ��� ���
                    {
                        followingPath = false; // ��� �̵� ����
                    }
                }

                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position); // ���� ��� ������ �ٶ󺸵��� ȸ��
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed); // ȸ��
                transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self); // �̵�
            }

            yield return null; // ���� �����ӱ��� ���

        }
    }

}