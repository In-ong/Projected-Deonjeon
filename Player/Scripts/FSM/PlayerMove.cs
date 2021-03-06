﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : FSMSingleton<PlayerMove>, IFSMState<Player>
{
    #region Field
    float m_speed;
    #endregion

    #region Property
    public float Speed { get { return m_speed; } set { m_speed = value; } }
    #endregion

    #region Method
    void ChangeAttack(Player player)
    {
        if (player.Sword.activeSelf)
        {
            if (player.NavMesh.velocity.sqrMagnitude >= 0.2f * 0.2f && player.NavMesh.remainingDistance <= player.SwordAttackSight)
            {
                player.NavMesh.isStopped = true;
                player.NavMesh.velocity = Vector3.zero;

                player.ChangeState(PlayerAttack.Instance);
            }
        }
        else if(player.Bow.activeSelf)
        {
            if (player.NavMesh.velocity.sqrMagnitude >= 0.2f * 0.2f && player.NavMesh.remainingDistance <= player.SwordAttackSight)
            {
                player.NavMesh.isStopped = true;
                player.NavMesh.velocity = Vector3.zero;

                ItemManager.Instance.ShotArrow(player);
            }
            
        }
        else if(!player.Sword.activeSelf || !player.Bow.activeSelf)
        {
            if (player.NavMesh.velocity.sqrMagnitude >= 0.2f * 0.2f && player.NavMesh.remainingDistance <= player.SwordAttackSight)
            {
                player.NavMesh.isStopped = true;
                player.NavMesh.velocity = Vector3.zero;

                player.ChangeState(PlayerIdle.Instance);
            }
        }
    }

    void MovePos(Player player)
    {
        if (Input.GetButton("Fire1"))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 10000f, 1 << LayerMask.NameToLayer("Click")))
                {
                    player.Target = null;
                    player.TargetPos = Vector3.zero;
                    if (player.ItemController != null)
                        player.ItemController = null;
                    if (player.Treasure_Box != null)
                        player.Treasure_Box = null;
                    if (player.IsBoxOpen)
                        player.IsBoxOpen = false;
                    if (player.GetItem)
                        player.GetItem = false;

                    if (hit.collider.CompareTag("Monster"))
                    {
                        for (int i = 0; i < MonsterManager.Instance.GetFieldMonsterList().Count; i++)
                        {
                            if (hit.collider.transform == MonsterManager.Instance.GetFieldMonsterList()[i].transform)
                                player.Target = MonsterManager.Instance.GetFieldMonsterList()[i];
                        }
                    }
                    else if (hit.collider.CompareTag("Item"))
                    {
                        var item = hit.collider.gameObject.GetComponent<ItemController>();
                        if (item.Type == ItemManager.eItemType.Treasure_Box)
                        {
                            var box = hit.collider.gameObject.GetComponent<Treasure_Box>();

                            player.IsBoxOpen = true;
                            player.TargetPos = item.transform.position;
                            if (player.Treasure_Box == null)
                                player.Treasure_Box = box;
                        }
                        else if (item.Category == ItemManager.eCategory.Consume && item.Type != ItemManager.eItemType.Treasure_Box)
                        {
                            player.GetItem = true;
                            player.TargetPos = item.transform.position;
                            if (player.ItemController == null)
                                player.ItemController = item;
                        }
                    }
                    else
                        player.TargetPos = hit.point;
                }
            }
        }
    }
    #endregion

    #region Coroutine
    IEnumerator Coroutine_OnItem(Player player)
    {
        yield return new WaitForSeconds(0.5f);
        if(player.IsBoxOpen)
        {
            ItemManager.Instance.RemoveTreasureBox(player.Treasure_Box, player);            
        }
    }
    #endregion

    public void Enter(Player player)
    {
        if (!player.IsRun)
        {
            m_speed = player.WalkSpeed;

            if (player.GetAnimController().GetAnimState() != PlayerAnimController.eAnimState.WALK)
                player.GetAnimController().Play(PlayerAnimController.eAnimState.WALK);
        }
        else
        {
            m_speed = player.WalkSpeed * player.RunRate;

            if (player.GetAnimController().GetAnimState() != PlayerAnimController.eAnimState.RUN)
                player.GetAnimController().Play(PlayerAnimController.eAnimState.RUN);
        }
    }

    public void Execute(Player player)
    {
        MovePos(player);

        if(player.Target != null )
        {
            player.Direction = player.Target.transform.position - player.transform.position;
            player.Direction = new Vector3(player.Direction.x, 0f, player.Direction.z);
            //player.Direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            //var destination = Vector3.Distance(player.transform.position, player.TargetTransform.position);

            if (player.Direction != Vector3.zero)
            {
                Quaternion dir = Quaternion.LookRotation(player.Direction.normalized);

                player.transform.rotation = dir;
            }

            //if (player.Direction.sqrMagnitude <= player.NavMesh.stoppingDistance * player.NavMesh.stoppingDistance)
            //    player.ChangeState(PlayerIdle.Instance);

            //var moveVector = new Vector3(player.Direction.x, 0f, player.Direction.z);

            player.NavMesh.SetDestination(player.Target.transform.position);

            if(!player.Target.Death)
                ChangeAttack(player);


        }
        else
        {
            player.Direction = player.TargetPos - player.transform.position;
            player.Direction = new Vector3(player.Direction.x, 0f, player.Direction.z);
            //player.Direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            //var destination = Vector3.Distance(player.transform.position, player.TargetPos);

            if (player.Direction != Vector3.zero)
            {
                Quaternion dir = Quaternion.LookRotation(player.Direction.normalized);

                player.transform.rotation = dir;
            }

            //if (player.Direction.sqrMagnitude <= player.NavMesh.stoppingDistance * player.NavMesh.stoppingDistance)
            //    player.ChangeState(PlayerIdle.Instance);

            //var moveVector = new Vector3(player.Direction.x, 0f, player.Direction.z);

            player.NavMesh.SetDestination(player.TargetPos);

            if (!player.IsBoxOpen && !player.GetItem)
            {
                if (player.NavMesh.velocity.sqrMagnitude >= 0.2f * 0.2f && player.NavMesh.remainingDistance <= 0.3f)
                {
                    player.NavMesh.isStopped = true;
                    player.NavMesh.velocity = Vector3.zero;

                    player.ChangeState(PlayerIdle.Instance);
                }
            }
            else if (player.IsBoxOpen)
            {
                if (player.NavMesh.velocity.sqrMagnitude >= 0.2f * 0.2f && player.NavMesh.remainingDistance <= 1.3f)
                {
                    player.NavMesh.isStopped = true;
                    player.NavMesh.velocity = Vector3.zero;

                    StopCoroutine(Coroutine_OnItem(player));
                    StartCoroutine(Coroutine_OnItem(player));

                    player.ChangeState(PlayerIdle.Instance);
                }
                #region Ray를 통한 아이템 확인
                //RaycastHit rayHit = new RaycastHit();
                //if(Physics.Raycast(player.transform.position, player.Direction.normalized, out rayHit, 1.4f, 1 << LayerMask.NameToLayer("Click")))
                //{
                //    if(rayHit.collider.tag.Equals("Item"))
                //    {
                //        StopCoroutine(Coroutine_OnItem(player));
                //        StartCoroutine(Coroutine_OnItem(player));
                //    }
                //}
                #endregion
            }
            else if(player.GetItem)
            {
                if (player.NavMesh.velocity.sqrMagnitude >= 0.2f * 0.2f && player.NavMesh.remainingDistance <= 0.3f)
                {
                    player.NavMesh.isStopped = true;
                    player.NavMesh.velocity = Vector3.zero;

                    ItemManager.Instance.RemoveConsumeItem(player.ItemController, player);
                    player.ChangeState(PlayerIdle.Instance);
                }
            }
        }
        
    }

    public void Exit(Player player)
    {
        player.NavMesh.ResetPath();

        player.Direction = Vector3.zero;
        m_speed = 0f;
    }
}
