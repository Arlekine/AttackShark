using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FreshwaterFish 
{
    public class FishMovement : MonoBehaviour
    {
        public const float GoalReachDistance = 0.3f;
        public const float LocalAvoidanceDistance = 1f;
        public const float RayCastOffset = 0.1f;

		[SerializeField] private float _rotationLerpParameter = 1f;
        [SerializeField] private float _size = 1f;
        [SerializeField] private float _fishLevel = 1f;
        [SerializeField] private float _runawayDistance = 10f;
        [SerializeField] private LayerMask _obstaclesLayer;
        [SerializeField] private RunAwayTrigger _runAwayTrigger;

        [Header("Speed")]
        [Range(0f, 5f)] [SerializeField] private float _minSpeed = 0.1f;
        [Range(0f, 5f)] [SerializeField] private float _maxSpeed = 0.2f;

        private float _speed;

        private Rigidbody _rigidbody;
        private SwimZone _swimZone;
        private Vector3 _goalPosition;
        private List<FishMovement> _shoal;
        private List<FishHazard> _currentHazards = new List<FishHazard>();
        private List<FishCollider> _currentColliders = new List<FishCollider>();

        public Action GoalReached;
        public Action<FishMovement> Destroyed;

        public float FishLevel => _fishLevel;

        public void Init(SwimZone swimZone)
        {
            var progress = Mathf.InverseLerp(0, 4, _size);
            var mod = Mathf.Lerp(1, 2, progress);

            _speed = Random.Range(_minSpeed * mod, _maxSpeed * mod);
            _swimZone = swimZone;
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void SetShoal(List<FishMovement> fishes)
        {
            _shoal = fishes;
        }

		private void FixedUpdate()
		{
            var goalDistance = Vector3.Distance(this.transform.position, _goalPosition);
			if (goalDistance < GoalReachDistance)
			{
                GoalReached?.Invoke();
			}

            Vector3 localAvoidance = CalculateShoalAvoidance();

            //_goalPosition = CalculateObstacleAvoidance(_goalPosition);

            var additionalSpeed = 0f;
            
            if (HasRealHazard())
            {
                var hazardsCenter = GetHazardCenter();
                var directionFromHazards = (transform.position - hazardsCenter).normalized;
                _goalPosition = transform.position + directionFromHazards * _runawayDistance;
                _goalPosition.y = _swimZone.ClampYForPosition(_goalPosition);

                additionalSpeed = _speed;
            }

            //BalanceHeight();

            Vector3 direction = _goalPosition + localAvoidance - this.transform.position;

            _rigidbody.MovePosition(transform.position + transform.forward * Time.deltaTime * (_speed + additionalSpeed));
            _rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _rotationLerpParameter * Time.deltaTime));

            /*transform.Translate(0, 0, Time.deltaTime * (_speed + additionalSpeed));
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _rotationLerpParameter * Time.deltaTime);*/

            /*if (transform.position.y > _goalPosition.y)
			{
				transform.Translate(Vector3.down * 0.1f * Time.deltaTime);
			}*/
		}

        private void OnEnable()
        {
            _runAwayTrigger.TriggerEnter += AddHazard;
            _runAwayTrigger.TriggerExit += RemoveHazard;
        }

        private void OnDisable()
        {
            _runAwayTrigger.TriggerEnter -= AddHazard;
            _runAwayTrigger.TriggerExit -= RemoveHazard;
        }

        private void AddHazard(FishHazard hazard)
        {
            _currentHazards.Add(hazard);
        }

        private void RemoveHazard(FishHazard hazard)
        {
            _currentHazards.Remove(hazard);
        }

        private Vector3 GetHazardCenter()
        {
            var totalX = 0f;
            var totalY = 0f;
            var totalZ = 0f;

            foreach (var unit in _currentHazards)
            {
                totalX += unit.transform.position.x;
                totalY += unit.transform.position.y;
                totalZ += unit.transform.position.z;
            }
            var centerX = totalX / _currentHazards.Count;
            var centerY = totalY / _currentHazards.Count;
            var centerZ = totalZ / _currentHazards.Count;

            return new Vector3(centerX, centerY, centerZ);
        }

        private bool HasRealHazard()
        {
            foreach (var currentHazard in _currentHazards)
            {
                if (currentHazard.HazardLevel >= _fishLevel)
                    return true;
            }

            return false;
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke(this);
        }

        private Vector3 CalculateShoalAvoidance()
        {
            var localAvoidance = Vector3.zero;
            foreach (FishMovement fish in _shoal)
            {
                if (fish != this)
                {
                    var distanceToFish = Vector3.Distance(fish.transform.position, this.transform.position);

                    if (distanceToFish < LocalAvoidanceDistance)
                    {
                        localAvoidance += (this.transform.position - fish.transform.position);
                    }
                }
            }

            return localAvoidance;
        }

        private Vector3 CalculateObstacleAvoidance(Vector3 goal)
        {
            var raycastCheckPoint = this.transform.position + new Vector3(0, -RayCastOffset, 0);

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 left = transform.TransformDirection(Vector3.left);
            Vector3 right = transform.TransformDirection(Vector3.right);

            if (Physics.Raycast(transform.position, forward, _size, _obstaclesLayer))
            {
                goal = this.transform.position - this.transform.forward * 2;
            }

            if (Physics.Raycast(raycastCheckPoint, right, _size / 4, _obstaclesLayer))
            {
                goal = this.transform.position - this.transform.right * -2;
            }

            if (Physics.Raycast(raycastCheckPoint, left, _size / 4, _obstaclesLayer))
            {
                goal = this.transform.position - this.transform.right * 2;
            }

            return goal;
        }

        private void BalanceHeight()
        {
            if (transform.position.y < _swimZone.GetMinHeightForPosition(transform.position))
            {
                transform.Translate(Vector3.up * 0.3f * Time.deltaTime);
            }

            if (transform.position.y > _swimZone.GetMaxHeightForPosition(transform.position))
            {
                transform.Translate(Vector3.down * 0.3f * Time.deltaTime);
            }
        }

        public void SetGoal(Vector3 position)
		{
			_goalPosition = position;
        }
    }	
}