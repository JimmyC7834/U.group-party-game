using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Dataset
{
    [CreateAssetMenu(menuName = "Game/Dataset/Enemy", fileName = "EnemyDataset")]
    public class EnemyDataset : DataSetSO<EnemyId, EnemySO> { }
}