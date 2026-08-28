using UnityEngine;

namespace Scripts.Core
{
    public interface IWeapon
    {
        void StartAttack();
        void StopAttack();
        void Reload();
    }
}
