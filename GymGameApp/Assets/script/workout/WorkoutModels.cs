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
    public List<SetData> sets = new();
}

[Serializable]
public class ExerciseProgressData  // Represents the progress of a specific exercise, including current sets and history
{
    public string exerciseName;
    public List<SetData> currentSets = new();
    public List<HistorySession> history = new();
}

[Serializable]
public class WorkoutData // Represents a workout routine, including its name, the number of exercises, and the list of exercises with their details
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