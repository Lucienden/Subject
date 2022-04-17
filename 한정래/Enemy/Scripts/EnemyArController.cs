using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArController : MonoBehaviour
{
    [SerializeField]
    private int numberOfBullet =  30;

    private Vector3 targetPosition;

    private EnemyAnimatorController enemyAnimatorController;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        enemyAnimatorController = GetComponentInParent<EnemyAnimatorController>();
        lineRenderer = GetComponent<LineRenderer>();
    }
    public void SetUp()
    {
        numberOfBullet--;

        Vector3 direction = (targetPosition - transform.position).normalized;
        Ray ray = new Ray(transform.position + transform.up, direction);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log("¸ÂÀº Æ®·»½ºÆû: " + hit.transform.name);
        }

        if (numberOfBullet == 0)
        {
            if (enemyAnimatorController)
            {
                enemyAnimatorController.OnReload();
                numberOfBullet = 30;
            }
        }
        Debug.Log("³²Àº ÃÑ¾Ë ¼ö: " + numberOfBullet);
    }

    public void BulletLine(Vector3 targetPosition)
    {
        this.targetPosition = new Vector3(targetPosition.x, targetPosition.y - 0.001f, targetPosition.z);
        
        StartCoroutine("SetBulletLine");
    }

    private IEnumerator SetBulletLine()
    {
        lineRenderer.enabled = true;
        lineRenderer.startWidth = 0.02f;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.endWidth = 0.001f;
        lineRenderer.SetPosition(1, this.targetPosition);

        yield return new WaitForSeconds(1.5f);

        lineRenderer.enabled = false;
    }
}
