using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNode : INode
{
    Roulette<INode> _roulette = new Roulette<INode>();
    Dictionary<INode, int> _items;
    public RandomNode(Dictionary<INode, int> items)
    {
        _items = items;
    }
    public void Execute()
    {
        var node = _roulette.Run(_items);
        if (node != null)
        {
            node.Execute();
        }
    }
}
