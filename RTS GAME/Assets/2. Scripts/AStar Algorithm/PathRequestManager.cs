using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class PathRequestManager : MonoBehaviour
{
    // ���� ��� ��û�� �����ϴ� ť
    Queue<PathResult> results = new Queue<PathResult>();

    static PathRequestManager instance;
    Pathfinding pathfinding;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    void Update()
    {
        // ��� ��û�� �ִ� ���
        if (results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                // ����� ��� ��û�� �ϳ��� ó��
                for (int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    // ��� ��û�� �޾� ��� Ž�� �����带 �����ϴ� �޼���
    public static void RequestPath(PathRequest request)
    {
        // ������ ���� �� ����
        ThreadStart threadStart = delegate {
            instance.pathfinding.FindPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }

    // ��� Ž���� �Ϸ�� ��θ� ť�� �����ϴ� �ݹ� �޼���
    public void FinishedProcessingPath(PathResult result)
    {
        // ��� ��û ť�� ��� ����
        lock (results)
        {
            results.Enqueue(result);
        }
    }
}
// ��� ��û�� ����� ��� ����ü
public struct PathResult
{
    public Vector3[] path; // ã�� ���
    public bool success; // ��� ã�Ⱑ �����ߴ��� ����
    public Action<Vector3[], bool> callback; // ��� ��û �Ϸ� �� ȣ���� �ݹ� �Լ�
    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}

// ��� ��û�� ��� ���� ����ü
public struct PathRequest
{
    public Vector3 pathStart; // ��� ������
    public Vector3 pathEnd; // ��� ����
    public Action<Vector3[], bool> callback; // ��� ã�� �Ϸ� �� ȣ���� �ݹ� �Լ�
    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
    {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
    }
}