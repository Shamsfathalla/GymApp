using System;
using System.Collections.Generic;

[Serializable]
public class SetData // Represents a single set of an exercise
{
    public int reps;
    public float weight;
}

[Serializable]
public class HistorySession // Represents a past workout session for an exercise
{
    public string date; 
    public List<SetData> sets = new List<SetData>(); // List of sets performed in this session
}

[Serializable]
public class ExerciseProgressData  // Represents the progress of a specific exercise, including current sets and history
{
    public string exerciseName; 
    public List<SetData> currentSets = new List<SetData>(); //  List of sets in the current workout session for this exercise
    public List<HistorySession> history = new List<HistorySession>(); //  List of past workout sessions for this exercise
}

[Serializable]
public class WorkoutData // Represents a workout routine
{
    public string id;
    public string name;
    public int exerciseCount;
    public List<Dictionary<string, object>> exercises;
}

[Serializable]
public class ExerciseData // Represents the data for a specific exercise
{
    public string name; 
}