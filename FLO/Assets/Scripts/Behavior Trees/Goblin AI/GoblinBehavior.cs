using System.Collections.Generic;
using GoblinBehaviorTree;
using GoblinStateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace GoblinBehaviorTree
{
//the guard behavior tree in and of itself
//Tree sets up nodes with SetUpTree()
    public class GoblinBehavior : Tree
    {
        [SerializeField] private float _detectionRadius = 5f;
        [SerializeField] private float _guardRadius = 2f;
        [SerializeField] private float _attackDelay = 2f;
        [SerializeField] private float _returnHomeTime = 4f;
        [SerializeField] private float _homeRadius = 10f;

        public NavMeshAgent NavMeshAgent { get; set; }
        public Player Player { get; set; }
        public Entity Entity { get; set; }

        public float DetectionRadius => _detectionRadius;
        public Vector3 InitialPosition { get; set; }
        public float AttackDelay => _attackDelay;
        public float ReturnHomeTime => _returnHomeTime;
        private bool IsHome => Vector3.Distance(Entity.transform.position, Entity.InitialPosition) <= _homeRadius;
        public float DistanceToPlayer => Vector3.Distance(NavMeshAgent.transform.position, Player.transform.position);
        public bool Invulnerable => false;
        public float GuardRadius => _guardRadius;
        public GoblinBehavior Instance { get; set; }

        public bool AttackPhase { get; set; }

        public float StunTime = .5f;
        private InTransitionEntity _inTransition;

        public bool Land { get; set; }
        public float FallTime => Entity.FallTime;
        public float HomeRadius => _homeRadius;

        protected override void Start()
        {
            Instance = this;
            Entity = GetComponent<Entity>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Player = FindObjectOfType<Player>();
            InitialPosition = transform.position;
            base.Start();
        }

        public static float speed = 2f;
        public static float fovRange = 6f;
        public static float attackRange = 1f;

        protected override Node SetupTree()
        {
            //this determines the list of priorities for your actions
            Node root = new Selector(
                new List<Node> //create a selector node that chooses between sequence and task patrol
                {
                    new Sequence(new List<Node>
                    {
                        new CheckTargetInAttackRange(transform), //if this returns success, goes to next behavior
                        new Attack(transform),
                    }),

                    new Sequence(new List<Node>
                    {
                        new DetectTarget(transform),
                        new Chase(transform),
                    }),
                    new Patrol(Instance), //patrolling is the fallback action
                });

            return root;
        }
    }
}