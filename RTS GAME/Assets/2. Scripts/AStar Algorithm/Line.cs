using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    // �������� ���Ⱚ
    const float verticalLineGradient = 1e5f;

    // ������ ���Ⱚ, y����, ���� ���� �� ��
    float gradient;
    float y_intercept;
    Vector2 pointOnLine_1;
    Vector2 pointOnLine_2;

    // ���������� �������Ⱚ
    float gradientPerpendicular;

    // ���������κ��� ������ ����ʿ� �ִ����� ��Ÿ���� bool ����
    bool approachSide;

    public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
    {
        float dx = pointOnLine.x - pointPerpendicularToLine.x;
        float dy = pointOnLine.y - pointPerpendicularToLine.y;

        // �������� ��� �������Ⱚ�� ���Ѵ�� �����Ѵ�
        if (dx == 0)
        {
            gradientPerpendicular = verticalLineGradient;
        }
        else
        {
            gradientPerpendicular = dy / dx;
        }

        // �������� ��� ������ ���Ⱚ�� ���Ѵ�� �����Ѵ�
        if (gradientPerpendicular == 0)
        {
            gradient = verticalLineGradient;
        }
        else
        {
            gradient = -1 / gradientPerpendicular;
        }

        // y���� �� ���
        y_intercept = pointOnLine.y - gradient * pointOnLine.x;

        // ���� ���� �� �� ����
        pointOnLine_1 = pointOnLine;
        pointOnLine_2 = pointOnLine + new Vector2(1, gradient);

        approachSide = false;
        approachSide = GetSide(pointPerpendicularToLine);
    }

    bool GetSide(Vector2 p)
    {
        // ���������κ��� �� P������ �Ÿ��� �̿��Ͽ� �� P�� ������ ����ʿ� �ִ��� Ȯ��
        return (p.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) > (p.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
    }

    // �� P�� ������ �Ѿ���� Ȯ���ϴ� �Լ�
    public bool HasCrossedLine(Vector2 p)
    {
        return GetSide(p) != approachSide;
    }

    // �� P�� ������ �Ÿ��� ��ȯ�ϴ� �Լ�
    public float DistanceFromPoint(Vector2 p)
    {
        float yInterceptPerpendicular = p.y - gradientPerpendicular * p.x;
        float intersectX = (yInterceptPerpendicular - y_intercept) / (gradient - gradientPerpendicular);
        float intersectY = gradient * intersectX + y_intercept;
        return Vector2.Distance(p, new Vector2(intersectX, intersectY));
    }

 

}