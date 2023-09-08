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
        [SerializeField] private LayerMask _obstaclesLayer;

        private float _speed;
        private float _minSpeed;
        private float _maxSpeed;

        private SwimZone _swimZone;
        private Vector3 _goalPosition;
        private List<FishMovement> _shoal;

        public Action GoalReached;
        public Action<FishMovement> Destroyed;

		public void Init(float minSpeed, float maxSpeed, SwimZone swimZone)
        {
            _minSpeed = minSpeed;
            _maxSpeed = maxSpeed;

            _speed = Random.Range(_minSpeed, _maxSpeed);
            _swimZone = swimZone;
		}

        public void SetShoal(List<FishMovement> fishes)
        {
            _shoal = fishes;
        }

		private void Update()
		{
            var goalDistance = Vector3.Distance(this.transform.position, _goalPosition);
			if (goalDistance < GoalReachDistance)
			{
                GoalReached?.Invoke();
			}

            Vector3 localAvoidance = CalculateShoalAvoidance();
            Vector3 direction = _goalPosition + localAvoidance - this.transform.position;

            _goalPosition = CalculateObstacleAvoidance(_goalPosition);
            BalanceHeight();

			if (Random.Range(0f, 1f) < 0.01f)
			{
				_speed = Random.Range(_minSpeed, _maxSpeed);
			}

			transform.Translate(0, 0, Time.deltaTime * _speed);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _rotationLerpParameter * Time.deltaTime);

            if (transform.position.y > _goalPosition.y)
			{
				transform.Translate(Vector3.down * 0.1f * Time.deltaTime);
			}
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
