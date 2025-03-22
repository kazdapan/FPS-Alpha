using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000006 RID: 6
public class EnemyAiTutorial : MonoBehaviour
{
	// Token: 0x06000019 RID: 25 RVA: 0x000028AC File Offset: 0x00000AAC
	private void Awake()
	{
		this.player = GameObject.Find("PlayerObj").transform;
		this.agent = base.GetComponent<NavMeshAgent>();
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000028D0 File Offset: 0x00000AD0
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

	// Token: 0x0600001B RID: 27 RVA: 0x00002970 File Offset: 0x00000B70
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

	// Token: 0x0600001C RID: 28 RVA: 0x000029D4 File Offset: 0x00000BD4
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

	// Token: 0x0600001D RID: 29 RVA: 0x00002A7A File Offset: 0x00000C7A
	private void ChasePlayer()
	{
		this.agent.SetDestination(this.player.position);
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002A94 File Offset: 0x00000C94
	private void AttackPlayer()
	{
		this.agent.SetDestination(base.transform.position);
		base.transform.LookAt(this.player);
		if (!this.alreadyAttacked)
		{
			Rigidbody component = Object.Instantiate<GameObject>(this.projectile, base.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
			component.AddForce(base.transform.forward * 32f, ForceMode.Impulse);
			component.AddForce(base.transform.up * 8f, ForceMode.Impulse);
			this.alreadyAttacked = true;
			base.Invoke("ResetAttack", this.timeBetweenAttacks);
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002B40 File Offset: 0x00000D40
	private void ResetAttack()
	{
		this.alreadyAttacked = false;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002B49 File Offset: 0x00000D49
	public void TakeDamage(int damage)
	{
		this.health -= (float)damage;
		if (this.health <= 0f)
		{
			base.Invoke("DestroyEnemy", 0.5f);
		}
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002B77 File Offset: 0x00000D77
	private void DestroyEnemy()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002B84 File Offset: 0x00000D84
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, this.attackRange);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.transform.position, this.sightRange);
	}

	// Token: 0x04000022 RID: 34
	public NavMeshAgent agent;

	// Token: 0x04000023 RID: 35
	public Transform player;

	// Token: 0x04000024 RID: 36
	public LayerMask whatIsGround;

	// Token: 0x04000025 RID: 37
	public LayerMask whatIsPlayer;

	// Token: 0x04000026 RID: 38
	public float health;

	// Token: 0x04000027 RID: 39
	public Vector3 walkPoint;

	// Token: 0x04000028 RID: 40
	private bool walkPointSet;

	// Token: 0x04000029 RID: 41
	public float walkPointRange;

	// Token: 0x0400002A RID: 42
	public float timeBetweenAttacks;

	// Token: 0x0400002B RID: 43
	private bool alreadyAttacked;

	// Token: 0x0400002C RID: 44
	public GameObject projectile;

	// Token: 0x0400002D RID: 45
	public float sightRange;

	// Token: 0x0400002E RID: 46
	public float attackRange;

	// Token: 0x0400002F RID: 47
	public bool playerInSightRange;

	// Token: 0x04000030 RID: 48
	public bool playerInAttackRange;
}
