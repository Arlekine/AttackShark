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

		[Header("Fish Settings")]
		[Range(0f, 5f)][SerializeField] private float _minSpeed = 0.1f;
		[Range(0f, 5f)][SerializeField] private float _maxSpeed = 0.2f;

        private Vector3 _goal;
        public List<FishMovement> _allFish;

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

                newFish.Init(_minSpeed, _maxSpeed, _swimZone);
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
			_goal = this.transform.position + new Vector3(Random.Range(-_swimLimits.x, _swimLimits.x), 0, Random.Range(-_swimLimits.z, _swimLimits.z));
            _goal.y = _swimZone.GetRandomYForPosition(_goal);

			for (int i = 0; i < _allFish.Count; i++)
			{
				_allFish[i].SetGoal(_goal);
			}
			
		}
	}
}
