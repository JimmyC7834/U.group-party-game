using System.Collections;
using System.Collections.Generic;
using Game.Resource;
using UnityEngine;

namespace Game.Dataset
{
    [CreateAssetMenu(menuName = "Game/Dataset/Resource", fileName = "ResourceDataset")]
    public class ResourceDataset : DataSetSO<ResourceId, ResourceSO> { }
}