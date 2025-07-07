using UnityEngine;

namespace _Game.Logic.Infrastructure.Authoring
{
    [DisallowMultipleComponent]//запрещает несколько  накинуть несколько скриптов на один компонент
    public class PrefabEntityAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
    }
}
