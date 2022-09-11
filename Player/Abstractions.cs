using UnityEngine;
namespace Axis.Abstractions
{
    public class Abstractions { }//Empty class just for script name


    public abstract class StateClass<T> //template for state machine
    {


        public abstract void OnStateEnter(T obj);

        public abstract void OnStateExit();

        public virtual void OnStateParametersChange(T obj)
        {
            return;
        }

        public virtual void OnStateFixedUpdate()
        {
            return;
        }

        public abstract void OnStateUpdate();


    }

    public abstract class BaseEnemy : MonoBehaviour, IDamagable //base template for enemies
    {
        public delegate void BaseEnemyHealth(float amount);
        public event BaseEnemyHealth OnTakeDamage;
        public event BaseEnemyHealth OnDeath;
        public event BaseEnemyHealth OnHeal;

        [SerializeField] LayerMask attackOnLayers;

        protected void TookDamage() => OnTakeDamage?.Invoke(health);
        protected virtual void OnEntityDeath()
        {
            dead = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
            OnDeath?.Invoke(health);
        }

        protected void Healed() => OnHeal?.Invoke(health);
        //needs more


        [field: SerializeField]
        protected Statistics Stats { get; set; }

        protected float health;

        protected bool dead;

        [SerializeField]
        private float attackRange=2;


        public void RestoreHealth()
        {
            health = Stats.Health;
            OnHeal?.Invoke(health);
        }

        public virtual void TakeDamage(float damage, DamageType dType)
        {
            if (dead) return;
            float calculatedDamage = Mathf.Round(damage * ((100 / (100 + Stats.GetDefenseFromType(dType))) * 100)) / 100;

            health -= calculatedDamage;
            TookDamage();


            DamageDisplay.Create(transform.position, calculatedDamage, dType);

            if (health <= 0)
            {
                OnEntityDeath();
            }
        }

        public virtual void HealTarget(float health)
        {
            this.health += health;
            this.health = Mathf.Clamp(health, 0, MaxHealth);
            Healed();
        }

        public void AttackNearby(DamageType damageType)
        {
            var collidersNearby = Physics2D.OverlapCircleAll(transform.position, attackRange, attackOnLayers); //wstepnie tylko wykrywa circle
            if (collidersNearby.Length == 0) return;

            foreach (Collider2D col in collidersNearby)
            {
                var crit = Random.Range(1, 1.3f);
                col.GetComponent<IDamagable>()?.TakeDamage(Stats.GetAttackFromType(damageType)* crit, damageType);
            }
        }


        public float CurrentHealth =>health;
        public float MaxHealth => Stats.Health;

        
    }

    public interface IDamagable
    {
        void TakeDamage(float health, DamageType dType);
        void HealTarget(float health);
    }
    public interface IInteractable
    {
        void Interaction();
        string GetName();

        bool IsActive();
    }

    public interface ITargetable
    {
        //TBD
    }

    public interface ISaveable
    {
        object SaveState();

        void RestoreState(object obj);
    }
    
    public interface IEquipable
    {
        Statistics GetItemStats();
    }

    public class DialogueInteraction : UnityEngine.MonoBehaviour
    {
        protected static event System.Action<Dialogue> OnDialogueStart;

        public delegate void DialogueStates();
        public static DialogueStates OnDialogueEnd;

        protected static void EndDialogue() { OnDialogueEnd?.Invoke(); }


        public static void StartDialogue(Dialogue dialog) => OnDialogueStart?.Invoke(dialog);

    }

}

