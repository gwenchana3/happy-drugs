namespace Util
{
	public class TimeBufferedValue
	{
		/// <summary>
		/// The time it takes for the value to deactivate itself</param>
		/// </summary>
		public readonly float Delay;

		/// <summary>
		/// The time it will take for the value to deactivate itself</param>
		/// </summary>
		float _currentDelay = 0;

		/// <summary>
		/// The current state of the value
		/// </summary>
		public bool Active
		{
			get
			{
				return _currentDelay > 0;
			}
			set
			{
				_currentDelay = value ? Delay : 0;
			}
		}

		/// <summary>
		/// Constructs a new TimeBufferedValue
		/// </summary>
		/// <param name="delay">The time it should take for the value to deactivate itself</param>
		public TimeBufferedValue(float delay)
		{
			this.Delay = delay;
		}

		/// <summary>
		/// Inform the value of the passing of time
		/// </summary>
		/// <param name="timeDelta">The time that has passed since the last update</param>
		public virtual void Update(float timeDelta)
		{
			if (_currentDelay >= timeDelta)
			{
				_currentDelay -= timeDelta;
			}
			else
			{
				_currentDelay = 0;
			}
		}
	}
}
