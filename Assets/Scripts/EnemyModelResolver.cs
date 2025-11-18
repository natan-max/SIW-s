using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyModelResolver : MonoBehaviour
{
    public Entry[] Settings;

    private Dictionary<WaypointList, Entry> _map;
    
    [Serializable]
    public class Entry
    {
        public WaypointList Point;
        public GameObject Robert;
        public GameObject Engine;
        public GameObject PaperMan;

        public GameObject GetModel(EnemyId id)
        {
            return id switch
            {
                EnemyId.Engine => Engine,
                EnemyId.Robert => Robert,
                EnemyId.PaperMan => PaperMan
            };
        }
    }

    private void Awake()
    {
        _map = Settings.ToDictionary(x => x.Point, x => x);
    }

    public GameObject GetModel(WaypointList point, EnemyId id)
    {
        return _map[point].GetModel(id);
    }
}
