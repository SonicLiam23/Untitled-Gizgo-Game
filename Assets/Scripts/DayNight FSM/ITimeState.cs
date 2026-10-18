using UnityEngine;

// put anything specific to timestate here
public interface ITimeState : IState
{

    //I ASSUME THIS CLASS IS FOR VALUES LIKE THIS THAT GET SHARED BETWEEN THE STATES??
    
    float TempRate { get; }
    float StateDuration { get; }
}
