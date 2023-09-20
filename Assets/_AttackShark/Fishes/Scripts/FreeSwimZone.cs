using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FreshwaterFish
{
    public class FreeSwimZone : MonoBehaviour
    {
        [Serializable]
        private class FishVariant
        {
            [SerializeField] private FishData _fish;
            [SerializeField] private int _fishAmount = 5;

            public FishData Fish => _fish;
            public int FishAmount => _fishAmount;
        }

        [Space]
        [SerializeField] private CicleSwimZone _swimZone;
        [SerializeField] private FishVariant[] _fishVariants;
        [SerializeField] private float _fishAdditionalScale = 1f;
        [SerializeField] private float _spawnBordersOffset = 1f;
        [SerializeField] private float _spawnCenterOffset = 3f;

        [Header("Goal Update Time")]
        [SerializeField] private float _minTimeBetweenUpdates = 7f;
        [SerializeField] private float _maxTimeBetweenUpdates = 15f;

        private float _nextGoalUpdateTime;

        private List<FishMovement> _allFish;

        public void InitFishSpawn()
        {
            _allFish = new List<FishMovement>();

            foreach (var fishVariant in _fishVariants)
            {
                for (int i = 0; i < fishVariant.FishAmount; i++)
                {
                    var randomOffset = Random.insideUnitCircle * (_swimZone.ZoneRadius - _spawnBordersOffset);

                    while (Vector3.Distance(randomOffset, transform.position) <= _spawnCenterOffset)
                    {
                        randomOffset = Random.insideUnitCircle * (_swimZone.ZoneRadius - _spawnBordersOffset);
                    }

                    Vector3 pos = this.transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

                    pos.y = _swimZone.GetSwimHeight();

                    var newFish = fishVariant.Fish.CreateFish(transform).FishMovement;
                    newFish.transform.position = pos;
                    newFish.transform.localScale *= _fishAdditionalScale;

                    newFish.Init(_swimZone);

                    _allFish.Add(newFish);
                }
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

            for (int i = 0; i < _allFish.Count; i++)
            {
                var randomOffset = Random.insideUnitCircle * _swimZone.ZoneRadius;
                var goal = this.transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);
                goal.y = _swimZone.GetSwimHeight();

                goal = _swimZone.ClampHorizontalPosition(goal);
                _allFish[i].SetGoal(goal);
            }
        }

        private void OnDrawGizmos()
        {
            if (_swimZone == null)
                return;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_swimZone.transform.position, _swimZone.ZoneRadius - _spawnBordersOffset);
            Gizmos.DrawWireSphere(_swimZone.transform.position, _spawnCenterOffset);
        }

    }
}