using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceHolders : MonoBehaviour
{
    [SerializeField] private Evidence[] evidenceList;

    public Evidence[] EvidenceList()
    {
        return evidenceList;
    }
}
