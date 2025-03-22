using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000004 RID: 4
public class EnemyAI : MonoBehaviour
{
	// Token: 0x06000006 RID: 6 RVA: 0x00002283 File Offset: 0x00000483
	private void Awake()
	{
		this.player = GameObject.Find("Player").transform;
		this.agent = base.GetComponent<NavMeshAgent>();
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000022A8 File Offset: 0x000004A8
	private void Update()
	{
		this.playerInSightRange = Physics.CheckSphere(base.transform.position, this.sightRange, this.whatIsPlayer);
		this.playerInAttackRange = Physics.CheckSphere(base.transform.position, this.attackRange, this.whatIsPlayer);
		if (!this.playerInSightRange && !this.playerInAttackRange)
		{
			this.Patroling();
		}
		if (this.playerInSightRange && !this.playerInAttackRange)
		{
			this.ChasePlayer();
		}
		if (this.playerInAttackRange && this.playerInSightRange)
		{
			this.AttackPlayer();
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002348 File Offset: 0x00000548
	private void Patroling()
	{
		if (!this.walkPointSet)
		{
			this.SearchWalkPoint();
		}
		if (this.walkPointSet)
		{
			this.agent.SetDestination(this.walkPoint);
		}
		if ((base.transform.position - this.walkPoint).magnitude < 1f)
		{
			this.walkPointSet = false;
		}
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000023AC File Offset: 0x000005AC
	private void SearchWalkPoint()
	{
		float num = Random.Range(-this.walkPointRange, this.walkPointRange);
		float num2 = Random.Range(-this.walkPointRange, this.walkPointRange);
		this.walkPoint = new Vector3(base.transform.position.x + num2, base.transform.position.y, base.transform.position.z + num);
		if (Physics.Raycast(this.walkPoint, -base.transform.up, 2f, this.whatIsGround))
		{
			this.walkPointSet = true;
		}
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002452 File Offset: 0x00000652
	private void ChasePlayer()
	{
		this.agent.SetDestination(this.player.position);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x0000246C File Offset: 0x0000066C
	private void AttackPlayer()
	{
		this.agent.SetDestination(base.transform.position);
		base.transform.LookAt(this.player);
		if (!this.alreadyAttacked)
		{
			Rigidbody component = Object.Instantiate<GameObject>(this.projectile, this.projectileSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
			component.AddForce(base.transform.forward * 32f, ForceMode.Impulse);
			component.AddForce(base.transform.up * 8f, ForceMode.Impulse);
			this.GunshotHitCheck();
			this.alreadyAttacked = true;
			base.Invoke("ResetAttack", this.timeBetweenAttacks);
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002520 File Offset: 0x00000720
	private void GunshotHitCheck()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(this.RayPoint.position, this.RayPoint.forward), out raycastHit, this.range) && raycastHit.collider.CompareTag("Player"))
		{
			raycastHit.collider.GetComponent<PlayerHealth>().TakeDamage(this.damage);
			Debug.DrawLine(base.transform.position, base.transform.forward, Color.green);
		}
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000025A1 File Offset: 0x000007A1
	private void ResetAttack()
	{
		this.alreadyAttacked = false;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000025AA File Offset: 0x000007AA
	public void TakeDamage(int damage)
	{
		this.health -= (float)damage;
		if (this.health <= 0f)
		{
			base.Invoke("DestroyEnemy", 0.5f);
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000025D8 File Offset: 0x000007D8
	private void DestroyEnemy()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000025E8 File Offset: 0x000007E8
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, this.attackRange);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.transform.position, this.sightRange);
	}

	// Token: 0x04000003 RID: 3
	public NavMeshAgent agent;

	// Token: 0x04000004 RID: 4
	public Transform player;

	// Token: 0x04000005 RID: 5
	public LayerMask whatIsGround;

	// Token: 0x04000006 RID: 6
	public LayerMask whatIsPlayer;

	// Token: 0x04000007 RID: 7
	public float health;

	// Token: 0x04000008 RID: 8
	public Vector3 walkPoint;

	// Token: 0x04000009 RID: 9
	private bool walkPointSet;

	// Token: 0x0400000A RID: 10
	public float walkPointRange;

	// Token: 0x0400000B RID: 11
	public float timeBetweenAttacks;

	// Token: 0x0400000C RID: 12
	private bool alreadyAttacked;

	// Token: 0x0400000D RID: 13
	public GameObject projectile;

	// Token: 0x0400000E RID: 14
	public Transform projectileSpawn;

	// Token: 0x0400000F RID: 15
	public float sightRange;

	// Token: 0x04000010 RID: 16
	public float attackRange;

	// Token: 0x04000011 RID: 17
	public bool playerInSightRange;

	// Token: 0x04000012 RID: 18
	public bool playerInAttackRange;

	// Token: 0x04000013 RID: 19
	public Transform RayPoint;

	// Token: 0x04000014 RID: 20
	[SerializeField]
	private float range = 250f;

	// Token: 0x04000015 RID: 21
	public int damage = 4;
}
