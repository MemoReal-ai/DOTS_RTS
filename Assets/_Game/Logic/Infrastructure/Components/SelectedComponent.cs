using Unity.Entities;

namespace _Game.Logic.Infrastructure.Components
{
    public struct SelectedComponent : IComponentData
    {
        public bool Selected;

        public SelectedComponent(bool selected)
        {
            Selected = selected;
        }
    }
}