using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerIdle : FSMSingleton<PlayerIdle>,IFSMState<Player>
{
    #region Method
    void ChangeMove(Player player)
    {
        if (Input.GetButton("Fire1"))
        {           
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 10000f, 1 << LayerMask.NameToLayer("Click")))
                {
                    player.TargetTransform = null;
                    player.TargetPos = Vector3.zero;

                    if (hit.collider.CompareTag("Monster"))
                    {
                        for(int i =0; i < MonsterManager.Instance.GetFieldMonsterList().Count; i++)
                        {
                            if (hit.collider.transform == MonsterManager.Instance.GetFieldMonsterList()[i].transform)
                                player.TargetTransform = MonsterManager.Instance.GetFieldMonsterList()[i].transform;
                        }
                    }
                    else
                        player.TargetPos = hit.point;
                }

                if (player.TargetTransform != null)
                {
                    player.Direction = player.TargetTransform.position - player.transform.position;
                    player.Direction = new Vector3(player.Direction.x, 0f, player.Direction.z);
                }
                else
                {
                    player.Direction = player.TargetPos - player.transform.position;
                    player.Direction = new Vector3(player.Direction.x, 0f, player.Direction.z);
                }
            }
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    if (ItemManager.Instance.SearchItem(ItemManager.eItemType.Belonging, ItemManager.eWeaponType.Shoes))
        //    {
        //        player.IsRun = !player.IsRun;
        //    }
        //}

        if (player.Direction != Vector3.zero)
            player.ChangeState(PlayerMove.Instance);
    }

    void ChangeDefence(Player player)
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            if (player.Shield.activeSelf)
                player.ChangeState(PlayerDefence.Instance);
        }
    }

    void OpenBox(Player player)
    {
        ItemManager.Instance.RemoveTreasureBox(player.Treasure_Box, player);
    }
    #endregion

    public void Enter(Player player)
    {
        player.Direction = Vector3.zero;

        if (player.GetAnimController().GetAnimState() != PlayerAnimController.eAnimState.IDLE)
            player.GetAnimController().Play(PlayerAnimController.eAnimState.IDLE);        
    }

    public void Execute(Player player)
    {
        ChangeMove(player);
        ChangeDefence(player);
    }

    public void Exit(Player player)
    {

    }
}
