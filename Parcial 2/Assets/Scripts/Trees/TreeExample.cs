using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeExample : MonoBehaviour
{
    public bool shootRange;
    public bool lineOfSight;
    public bool ammoCheck;
    public bool enemyCheck;
    INode _root;
    private void Awake()
    {
        InitializedTree();
    }
    private void Update()
    {
    
            _root.Execute();
    }
    void InitializedTree()
    {

        //Actions
        //INode shoot = new ActionNode(()=>print("Shoot"));
        INode shoot = new ActionNode(ShootAction);
        INode chase = new ActionNode(() => print("Chase"));
        INode patrol = new ActionNode(() => print("Patrol"));
        INode reload = new ActionNode(() => print("Reload"));

        Dictionary<INode, int> itemsRandom = new Dictionary<INode, int>();
        itemsRandom[shoot] = 25;
        itemsRandom[chase] = 15;
        itemsRandom[patrol] = 5;
        itemsRandom[reload] = 1;

        INode random = new RandomNode(itemsRandom);

        //Questions
        INode qShootRange = new QuestionNode(ShootRange, random, chase);
        INode qLineOfSight = new QuestionNode(LineOfSight, qShootRange, patrol);
        INode qAmmoCheck = new QuestionNode(AmmoCheck, qLineOfSight, reload);
        INode qEnemyCheck = new QuestionNode(() => enemyCheck, qAmmoCheck, patrol);

        _root = qEnemyCheck;
    }
    bool AmmoCheck()
    {
        return ammoCheck;
    }
    bool LineOfSight()
    {
        return lineOfSight;
    }
    bool ShootRange()
    {
        return shootRange;
    }
    void ShootAction()
    {
        print("Shoot");
        //Debug.Log("Shoot");
    }
}
