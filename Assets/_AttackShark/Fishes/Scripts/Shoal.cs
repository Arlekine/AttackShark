using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FreshwaterFish {

	public class Shoal : MonoBehaviour
	{
        [SerializeField] private FishMovement fishPrefab;
        [SerializeField] private SwimZone _swimZone;

        [Space]
        [SerializeField] private Vector3 _spawnLimits = new Vector3(1, 1, 1);
        [SerializeField] private Vector3 _swimLimits = new Vector3(5, 5, 5);
        [SerializeField] private int _fishAmount = 5;
        
        [Header("Goal Update Time")]
        [SerializeField] private float _minTimeBetweenUpdates = 7f;
        [SerializeField] private float _maxTimeBetweenUpdates = 15f;

        private Vector3 _goal;
        public List<FishMovement> _allFish;
        private float _nextGoalUpdateTime;

		private void Start()
        {
            _swimZone = GetComponent<SwimZone>();
			_goal = this.transform.position;
			_allFish = new List<FishMovement>();

			for (int i = 0; i < _fishAmount; i++)
			{
				Vector3 pos = this.transform.position + new Vector3(Random.Range(-_spawnLimits.x, _spawnLimits.x), 0, Random.Range(-_spawnLimits.z, _spawnLimits.z));

                pos.y = _swimZone.ClampYForPosition(pos);

				var newFish = Instantiate(fishPrefab, pos, Quaternion.identity);

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

            _goal = this.transform.position + new Vector3(Random.Range(-_swimLimits.x, _swimLimits.x), 0, Random.Range(-_swimLimits.z, _swimLimits.z));
            _goal.y = _swimZone.GetRandomYForPosition(_goal);

			for (int i = 0; i < _allFish.Count; i++)
			{
				_allFish[i].SetGoal(_goal);
			}
			
		}
	}
}
