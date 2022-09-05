using System;
using Game.Dataset;
using Game.Resource;
using UnityEngine;

namespace Game.Data
{
    public enum RecipeId
    {
        GunTurret,
        Count,
    }
    
    [CreateAssetMenu(menuName = "Game/DataEntry/Recipe")]
    public class RecipeSO : DataEntrySO<RecipeId>
    {
        [Serializable]
        public struct Ingredient
        {
            public ResourceId resourceType;
            public int amount;
        }

        public Ingredient[] ingredients = default;

        public bool Craftable(int[] resourceCount)
        {
            for (int i = 0; i < ingredients.Length; i++)
            {
                // check the amount of needed ingredients
                if (resourceCount[(int) ingredients[i].resourceType] < ingredients[i].amount)
                {
                    return false;
                }
            }

            return true;
        }

        public void ConsumeIngredients(int[] resourceCount)
        {
            for (int i = 0; i < ingredients.Length; i++)
                resourceCount[(int) ingredients[i].resourceType] -= ingredients[i].amount;
        }
    }
}