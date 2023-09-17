using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FreshwaterFish {

	public class Shoal : MonoBehaviour
	{
        [SerializeField] private FishMovement fishPrefab;

        [Space]
        [SerializeField] private float _spawnRadius = 1f;
        [SerializeField] private float _swimRadius = 5f;
        [SerializeField] private int _fishAmount = 5;
        [SerializeField] private float _fishAdditionalScale = 1f;
        
        [Header("Goal Update Time")]
        [SerializeField] private float _minTimeBetweenUpdates = 7f;
        [SerializeField] private float _maxTimeBetweenUpdates = 15f;

        private float _nextGoalUpdateTime;
        private Vector3 _goal;

        private List<FishMovement> _allFish;
        private SwimZone _swimZone;

        public void InitFishSpawn(SwimZone swimZone)
        {
            _swimZone = swimZone;
			_goal = this.transform.position;
			_allFish = new List<FishMovement>();

			for (int i = 0; i < _fishAmount; i++)
            {
                var randomOffset = Random.insideUnitCircle * _spawnRadius;
				Vector3 pos = this.transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

                pos.y = _swimZone.GetSwimHeight();

				var newFish = Instantiate(fishPrefab, pos, Quaternion.identity);
                newFish.transform.parent = transform;
                newFish.transform.localScale *= _fishAdditionalScale;

                newFish.Init(_swimZone);
                newFish.SetGoal(transform.position);

                _allFish.Add(newFish);
			}

            foreach (var fish in _allFish)
            {
                fish.SetShoal(_allFish);
                fish.GoalReached += UpdateGoalForAllFishes;
                fish.Destroyed += OnFishDestroyed;
            }

			UpdateGoalForAllFishes();
		}

        private void Update()
        {
            if (Time.time >= _nextGoalUpdateTime)
            {
                UpdateGoalForAllFishes();
            }
        }

        private void OnFishDestroyed(FishMovement destroyedFish)
        {
            destroyedFish.GoalReached -= UpdateGoalForAllFishes;
            destroyedFish.Destroyed -= OnFishDestroyed;

            _allFish.Remove(destroyedFish);

            foreach (var fish in _allFish)
            {
                fish.SetShoal(_allFish);
            }
        }

        private void UpdateGoalForAllFishes()
        {
            _nextGoalUpdateTime = Time.time + Random.Range(_minTimeBetweenUpdates, _maxTimeBetweenUpdates);

            var randomOffset = Random.insideUnitCircle * _swimRadius;
            _goal = this.transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);
            _goal.y = _swimZone.GetSwimHeight();

			for (int i = 0; i < _allFish.Count; i++)
            {
                _goal = _swimZone.ClampHorizontalPosition(_goal);
				_allFish[i].SetGoal(_goal);
			}
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _spawnRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _swimRadius);
        }
    }
}
