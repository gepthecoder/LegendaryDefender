using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : MonoBehaviour
{
    private Controller controller;

    void Start()
    {
        controller = GetComponentInParent<Controller>();
    }

    public void ShootThatArrowBoi()
    {
        Debug.Log("Arrow shot booi");
        controller.canShoot = true;
    }

    public void DeliverPunch()
    {
        controller.canPunch = true;
    }

    public void StopSkill01()
    {
        SkillManager.instance.bSKILL_01_ACTIVATED = false;
        Debug.Log("Stopped skill");
    }

    public void StopSkill02()
    {
        SkillManager.instance.bSKILL_02_ACTIVATED = false;
    }

}
