using System.Collections.Generic;
using UnityEngine;

public class NPCRelationshipManager : MonoBehaviour
{
    public static NPCRelationshipManager Instance;

    private Dictionary<string, int> npcAffection = new Dictionary<string, int>();
    private HashSet<string> triedAffectionOption = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public int GetAffection(string npcId)
    {
        return npcAffection.ContainsKey(npcId) ? npcAffection[npcId] : 0;
    }

    public void IncreaseAffection(string npcId, int amount)
    {
        if (!npcAffection.ContainsKey(npcId))
            npcAffection[npcId] = 0;

        npcAffection[npcId] += amount;
    }

    public void DecreaseAffection(string npcId, int amount)
    {
        if (!npcAffection.ContainsKey(npcId))
            npcAffection[npcId] = 0;

        npcAffection[npcId] = Mathf.Max(0, npcAffection[npcId] - amount);
    }

    public bool HasTriedAffectionOption(string npcId)
    {
        return triedAffectionOption.Contains(npcId);
    }

    public void MarkTriedAffectionOption(string npcId)
    {
        triedAffectionOption.Add(npcId);
    }
}
