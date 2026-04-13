# Gamified Gym App

## About the Project
The Gamified Gym App is a mobile application that turns fitness tracking into a game. Traditional fitness apps are often static, rely on long lists of buttons, and can feel like boring spreadsheets. This project fixes these issues by providing an interactive experience where users control a character in a 2D retro pixel art world. The goal is to make logging workouts and tracking progress feel like playing a video game, which helps keep users motivated and consistent with their fitness goals.

## Features
* **Account Management:** Users can sign up and log in securely, ensuring their data and progress are never lost.
* **Interactive Navigation:** Instead of clicking through menus, users use a digital joystick to move their character around a 2D virtual gym map. For example, to check chest exercises, the player physically navigates to the bench press.
* **Workout Creation:** Users can organize their week by visiting the "Chalkboard" station to create and name custom workout routines.
* **Fitness Tracking:** Users can easily record their sets, reps, and weights for specific exercises. 
* **Exercise History & Database:** Users can review past statistics and view chronological logs of previous sets for specific exercises, as well as learn how to perform new movements.

## How It Works
The application merges a 2D game environment with database-driven UI menus:

1. **Movement & Exploration:** The player uses an on-screen joystick to walk around the gym. The physics movement is handled dynamically by Unity's 2D physics engine.
2. **Station Interaction:** When the player physically stands inside the trigger zone of a station (like the Chalkboard) and taps on it, the system recognizes the collision and opens a UI overlay menu.
3. **Data Logging:** Inside these menus, users can type in their reps and weights, add exercises to a workout, or view their past logs. UI elements dynamically populate data and handle their own listeners to prevent memory leaks and ghost clicks.
4. **Cloud Syncing:** When the user clicks "Save", the app asynchronously formats the data and pushes it to Firebase. This asynchronous background operation ensures the game doesn't freeze or lag while downloading exercises or saving workouts.

## Technology Stack
* **Game Engine:** Unity
* **Language:** C#
* **Backend & Database:** Firebase Authentication (for user logins and sessions) and Firestore (for dynamic data storage of workouts, exercises, and history).

## Installation and Setup

To download and run this project, you will need to use **Unity version 6.3 LTS (6000.3.6f1)**.

1. **Download the Project:** Clone this repository or download the source code to your local machine.
2. **Install Unity:** Ensure you have Unity Hub installed. Install Unity Editor version **6.3 LTS (6000.3.6f1)**.
3. **Open the Project:**
   * Open Unity Hub.
   * Click on **Add** and select the downloaded project folder.
   * Launch the project using Unity 6000.3.6f1.
4. **Dependencies:** The project relies on Firebase packages (Authentication and Firestore). Ensure these are resolved in the Unity Package Manager. The game runs an initial check to verify Firebase compatibility on startup.
5. **Running the App:** Open the authentication or main gym scene in the Unity Editor and press Play. Alternatively, you can build the project to a mobile device to test touch controls and functions directly.

## Video Demonstration
Watch a video demonstration of the app in action here: 
https://drive.google.com/drive/folders/10ha4WPEC5nLovX92CDq2vYYpSZ_Nj9sg?usp=sharing
