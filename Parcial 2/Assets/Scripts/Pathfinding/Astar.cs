using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar<T>
{
    public List<T> GetPath(T start, Func<T, bool> isSatisfied, Func<T, List<T>> getNeighbours, Func<T, T, float> getCost, Func<T, float> heuristic, int watchdog = 500)
    {
        PriorityQueue<T> pending = new PriorityQueue<T>();
        HashSet<T> visited = new HashSet<T>();
        Dictionary<T, T> parents = new Dictionary<T, T>();
        Dictionary<T, float> cost = new Dictionary<T, float>();
        pending.Enqueue(start, 0);
        cost[start] = 0;
        while (!pending.IsEmpty || watchdog <= 0)
        {
            //Debug.Log("AStar");
            watchdog--;
            T curr = pending.Dequeue();
            if (isSatisfied(curr))
            {
                return GeneratePath(curr, parents);
            }
            else
            {
                visited.Add(curr);
                List<T> neighbours = getNeighbours(curr);
                foreach (var neigh in neighbours)
                {
                    if (visited.Contains(neigh)) continue;
                    var neighCost = cost[curr] + getCost(curr, neigh);
                    if (cost.ContainsKey(neigh) && cost[neigh] <= neighCost) continue;
                    cost[neigh] = neighCost;
                    parents[neigh] = curr;
                    pending.Enqueue(neigh, neighCost + heuristic(neigh));
                }
            }
        }
        return new List<T>();
    }
    List<T> GeneratePath(T end, Dictionary<T, T> parents)
    {
        List<T> path = new List<T>();
        path.Add(end);
        while (parents.ContainsKey(path[path.Count - 1]))
        {
            path.Add(parents[path[path.Count - 1]]);
        }
        path.Reverse();
        return path;
    }
}
