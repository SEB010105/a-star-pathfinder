using System.Collections;
using UnityEngine;

namespace Movement
{
    public abstract class Movement
    {
        protected GameObject Actor { get; }
        public bool Moving { get; protected set; }

        protected Movement(GameObject actor)
        {
            Actor = actor;
        }

        public abstract IEnumerator Move(Vector3 to);
    }

    public class Basic : Movement
    {
        public Basic(GameObject actor) : base(actor)
        {
        }
        
        public override IEnumerator Move(Vector3 to)
        {
            Actor.transform.position = to;
            yield return null;
        }
    }
    
    public class Smooth : Movement
    {
        private readonly int _smoothness;
        private readonly Actor _actorComponent;

        public Smooth(GameObject actor, int smoothness) : base(actor)
        {
            _smoothness = smoothness;
            _actorComponent = Actor.GetComponent<Actor>();
        }

        public override IEnumerator Move(Vector3 to)
        {
            Moving = true;
            
            var delayBetweenSteps = _actorComponent.GetMoveDelay() / _smoothness;
            var step = (to - Actor.transform.position) * _actorComponent.GetCurrentSpeed() / _smoothness;
            var index = 0;

            var iterations = _smoothness / _actorComponent.GetCurrentSpeed();
            
            while (index < iterations)
            {
                Actor.transform.position += step;
                yield return new WaitForSeconds(delayBetweenSteps);
                index++;
            }

            Actor.transform.position = to;

            Moving = false;
        }
    }
}