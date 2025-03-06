using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public interface ICharacter
    {
        void Move(UnityEngine.Vector2 direction, bool isSprinting);

        void Attack();

        void TakeDamage(int damage);

        void Die();

        int HP { get; set; }
        int Damage { get; set; }
        float MoveSpeed { get; set; }
        float SprintSpeed { get; set; }
        bool isDead { get; set; }
    }
}
