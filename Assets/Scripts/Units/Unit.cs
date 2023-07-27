using System.Collections.Generic;
using Buildings;
using Helpers;
using UnityEngine;
using UnityEngine.AI;
using Zenject;


// Base class for all Unit. It will handle movement order given through the UserControl script.
// It require a NavMeshAgent to navigate the scene.
namespace Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Unit : MonoBehaviour,
        UIMainScene.IUIInfoContent
    {
        [SerializeField] private GameObject ringDecal;
        [SerializeField] private ProgressBar healthBar;
        [SerializeField] private FaceCamera faceCamera;
        

        private const int MaxHealth = 100;
        public float Speed = 3;
        protected Building Target;

        private NavMeshAgent agent;
        private ColorSaver colorSaver;
        private Selector selector;
        private Vector3 destination;
        private float health;

        protected void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = Speed;
            agent.acceleration = 999;
            agent.angularSpeed = 999;
            health = MaxHealth;
            
        }

        [Inject]
        private void Construct(ColorSaver colorSaverRef, Selector selectorRef)
        {
            selector = selectorRef;
            colorSaver = colorSaverRef;
        }

        public void OnTakeDamage(int damage)
        {
            health -= damage;
            healthBar.SetProgress(health, 3);
            
            if (health < 0)
            {
                OnDied();
                agent.enabled = false;
            }
        }

        private void OnDied()
        {
            Destroy(gameObject, 1f);
        }

        private void Start()
        {
            if (colorSaver != null)
            {
                SetColor(colorSaver.TeamColor);
            }
            

            selector.AddNewUnit(this);
        }

        public void SetupHealthBar(Camera gameCamera)
        {
            faceCamera.camera = gameCamera.transform;
        }

        void SetColor(Color c)
        {
            var colorHandler = GetComponentInChildren<ColorHandler>();
            if (colorHandler != null)
            {
                colorHandler.SetColor(c);
            }
        }

        private void Update()
        {
            if (Target != null)
            {
                DestinationReached(Target.transform.position);
            }
            else if (destination != Vector3.zero)
            {
                DestinationReached(destination);
            }
        }

        private void DestinationReached(Vector3 destinationRef)
        {
            float distance = Vector3.Distance(destinationRef, transform.position);
            if (distance < 2.0f)
            {
                agent.isStopped = true;
                agent.ResetPath();
                BuildingInRange();
            }
        }

        public virtual void GoTo(Building target)
        {
            Target = target;
            destination = Vector3.zero;
            if (Target != null)
            {
                agent.SetDestination(Target.transform.position);
                agent.isStopped = false;
            }
        }

        public virtual void GoTo(Vector3 position)
        {
            //we don't have a target anymore if we order to go to a random point.
            Target = null;
            destination = position;
            agent.SetDestination(position);
            agent.isStopped = false;
        }

        /*public bool DestinationReached(Vector3 actualPosition)
        {
            //  because takes some time to update the remainingDistance and will return a wrong value
            if (Agent.pathPending)
            {
                return Vector3.Distance(actualPosition, Agent.pathEndPosition) <= Agent.stoppingDistance;
            }
            else
            {
                return (Agent.remainingDistance <= Agent.stoppingDistance);
            }
        }*/


        /// <summary>
        /// Override this function to implement what should happen when in range of its target.
        /// Note that this is called every frame the current target is in range, not only the first time we get in range! 
        /// </summary>
        protected virtual void BuildingInRange()
        {
        }

        //Implementing the IUIInfoContent interface so the UI know it should display the UI when this is clicked on.
        //Implementation of all the functions are empty as default, but they are set as virtual so subclass units can
        //override them 
        public virtual string GetName()
        {
            return "Unit";
        }

        public virtual string GetData()
        {
            return "";
        }

        public virtual void GetContent(ref List<Building.InventoryEntry> content)
        {
        }

        public void OnSelected()
        {
            ringDecal.SetActive(true);
        }

        public void OnDeselected()
        {
            ringDecal.SetActive(false);
        }
    }
}