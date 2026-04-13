# Gamified Gym App

## About the Project
[cite_start]The Gamified Gym App is a mobile application that turns fitness tracking into a game[cite: 42]. [cite_start]Traditional fitness apps are often static, rely on long lists of buttons, and can feel like boring spreadsheets[cite: 58, 60]. [cite_start]This project fixes these issues by providing an interactive experience where users control a character in a 2D retro pixel art world[cite: 42, 43]. [cite_start]The goal is to make logging workouts and tracking progress feel like playing a video game, which helps keep users motivated and consistent with their fitness goals[cite: 44, 45].

## Features
* [cite_start]**Account Management:** Users can sign up and log in securely, ensuring their data and progress are never lost[cite: 70, 71].
* [cite_start]**Interactive Navigation:** Instead of clicking through menus, users use a digital joystick to move their character around a 2D virtual gym map[cite: 73, 74, 97]. [cite_start]For example, to check chest exercises, the player physically navigates to the bench press[cite: 75].
* [cite_start]**Workout Creation:** Users can organize their week by visiting the "Chalkboard" station to create and name custom workout routines[cite: 79, 97].
* [cite_start]**Fitness Tracking:** Users can easily record their sets, reps, and weights for specific exercises[cite: 77]. 
* [cite_start]**Exercise History & Database:** Users can review past statistics and view chronological logs of previous sets for specific exercises, as well as learn how to perform new movements[cite: 78, 100].

## How It Works
[cite_start]The application merges a 2D game environment with database-driven UI menus[cite: 139, 142]:

1. [cite_start]**Movement & Exploration:** The player uses an on-screen joystick to walk around the gym[cite: 167, 168]. [cite_start]The physics movement is handled dynamically by Unity's 2D physics engine[cite: 146, 182].
2. [cite_start]**Station Interaction:** When the player physically stands inside the trigger zone of a station (like the Chalkboard) and taps on it, the system recognizes the collision and opens a UI overlay menu[cite: 150, 178, 184].
3. [cite_start]**Data Logging:** Inside these menus, users can type in their reps and weights, add exercises to a workout, or view their past logs[cite: 178]. [cite_start]UI elements dynamically populate data and handle their own listeners to prevent ghost clicks[cite: 197, 198].
4. [cite_start]**Cloud Syncing:** When the user clicks "Save", the app asynchronously formats the data and pushes it to Firebase[cite: 176, 178]. [cite_start]This asynchronous background operation ensures the game doesn't freeze or lag while downloading exercises or saving workouts[cite: 176, 202].

## Technology Stack
* [cite_start]**Game Engine:** Unity [cite: 51]
* [cite_start]**Language:** C# [cite: 52]
* [cite_start]**Backend & Database:** Firebase Authentication (for user logins and sessions) and Firestore (for dynamic data storage of workouts, exercises, and history)[cite: 170, 171].

## Installation and Setup

To download and run this project, you will need to use **Unity version 6.3 LTS (6000.3.6f1)**.

1. **Download the Project:** Clone this repository or download the source code to your local machine.
2. **Install Unity:** Ensure you have Unity Hub installed. Install Unity Editor version **6.3 LTS (6000.3.6f1)**.
3. **Open the Project:**
   * Open Unity Hub.
   * Click on **Add** and select the downloaded project folder.
   * Launch the project using Unity 6000.3.6f1.
4. **Dependencies:** The project relies on Firebase packages (Authentication and Firestore). Ensure these are resolved in the Unity Package Manager. [cite_start]The game runs an initial check (`CheckAndFixDependenciesAsync()`) to verify Firebase compatibility on startup[cite: 186].
5. **Running the App:** Open the authentication or main gym scene in the Unity Editor and press Play. [cite_start]Alternatively, you can build the project to a mobile device to test touch controls and functions directly[cite: 49].

## Video Demonstration
Watch a video demonstration of the app in action here: 
https://drive.google.com/drive/folders/10ha4WPEC5nLovX92CDq2vYYpSZ_Nj9sg?usp=sharing
