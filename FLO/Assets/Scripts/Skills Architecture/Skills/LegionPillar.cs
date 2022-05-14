using Skills;
using UnityEngine;

public class LegionPillar : ISkill
{
    private int _contactPoint;
    private int[] _contactPoints = new int[] {1, 2, 3, 4, 5};
    
    public void Use(GameObject parent)
    {
        int pillarSpawnPoint = BinarySearchForTargetContact(_contactPoints, 5, _contactPoint);
    }

    public void Tick()
    {
    }

    public Sprite Icon { get; set; }
    public SlotType SlotType { get; set; }

    private void SetContactPoint(int contactPoint)
    {
        _contactPoint = contactPoint;
    }

    private int BinarySearchForTargetContact(int[] array, int elements, int target)
    {
        int left = 0;
        int right = elements - 1;
        while (left <= right)
        {
            int mid = (int)Mathf.Floor((left + right) / 2);
            if (target == array[mid])
                return mid;
            else if (target < array[mid])
                right = mid - 1;
            else if (target < array[mid])
                left = mid + 1;
        }

        return -1;
    }
}